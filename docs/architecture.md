# Architecture

## How a mentor search request flows

```mermaid
sequenceDiagram
    autonumber
    actor S as Student
    participant W as Angular SPA
    participant G as Azure Front Door
    participant A as Mandat.Api
    participant C as Redis
    participant D as Azure SQL

    S->>W: Filters by subject and distance
    W->>G: GET /api/mentors?subject=Mathematics
    G->>A: Forwarded request
    A->>C: Cache lookup
    alt Cache hit
        C-->>A: Cached page
    else Cache miss
        A->>D: SELECT with paging
        D-->>A: Rows
        A->>C: Store for 60 seconds
    end
    A-->>W: 200 with results
    W-->>S: Ranked list plus map pins
```

## Deployment topology

```mermaid
flowchart TB
    subgraph GH["GitHub"]
        PR["Pull request"]
        CI["CI workflow"]
        CD["CD workflow"]
        GHCR["ghcr.io registry"]
    end

    subgraph AZ["Azure"]
        subgraph DEV["Dev"]
            D1["Container App"]
            D2[("SQL Basic")]
        end
        subgraph TST["Test"]
            T1["Container App"]
            T2[("SQL S0")]
        end
        subgraph PRD["Production"]
            P1["Container App, 3 to 20 replicas"]
            P2[("SQL S3, zone redundant")]
        end
    end

    PR --> CI
    CI -->|"tests, CodeQL, Trivy"| CD
    CD --> GHCR
    GHCR --> D1
    D1 -->|"auto"| T1
    T1 -->|"manual approval"| P1
    D1 --- D2
    T1 --- T2
    P1 --- P2
```

## Domain model

```mermaid
erDiagram
    MENTOR ||--o{ ANNOUNCEMENT : publishes
    MENTOR ||--o{ MATCH_REQUEST : receives
    STUDENT ||--o{ MATCH_REQUEST : sends
    MENTOR ||--o{ REVIEW : "is reviewed in"
    STUDENT ||--o{ REVIEW : writes

    MENTOR {
        uuid Id PK
        string DisplayName
        string Email UK
        decimal HourlyRate
        bool AcceptsOnline
        datetime DeletedAt "null when active"
    }
    STUDENT {
        uuid Id PK
        string DisplayName
        string Email UK
        int SchoolYear
    }
    MATCH_REQUEST {
        uuid Id PK
        uuid StudentId FK
        uuid MentorId FK
        tinyint Status "0 pending, 1 accepted, 2 rejected"
    }
    REVIEW {
        uuid Id PK
        int Score "1 to 5"
        string Comment
    }
```

## The layers, and what is allowed to reference what

```mermaid
graph LR
    API["Mandat.Api<br/><i>controllers, auth, Swagger</i>"]
    APP["Mandat.Application<br/><i>use cases, DTOs</i>"]
    DOM["Mandat.Domain<br/><i>entities, rules</i>"]
    INF["Mandat.Infrastructure<br/><i>EF Core, repositories</i>"]

    API --> APP
    API --> INF
    INF --> APP
    APP --> DOM
    INF --> DOM

    classDef core fill:#dafbe1,stroke:#1a7f37
    classDef edge fill:#ddf4ff,stroke:#0969da
    class DOM,APP core
    class API,INF edge
```

`Mandat.Domain` references nothing. If you find yourself wanting to add a package reference to
it, the logic probably belongs in `Mandat.Application` instead.
