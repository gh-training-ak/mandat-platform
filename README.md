<div align="center">

<img src="https://user-images.githubusercontent.com/62501946/215290843-17a4d393-f25d-49d3-b266-630212e55e36.jpg" alt="Mandat logo" width="420" />

# I actually changed in this in the meantime

**An all-in-one mentor and student matchmaking platform.**
University students teach school students. The platform handles discovery, matching, scheduling and reputation.

<!-- Badges. These are live shields, they update themselves. -->

[![CI](https://github.com/gh-training-ak/mandat-platform/actions/workflows/ci.yml/badge.svg)](https://github.com/gh-training-ak/mandat-platform/actions/workflows/ci.yml)
[![CodeQL](https://github.com/gh-training-ak/mandat-platform/actions/workflows/codeql.yml/badge.svg)](https://github.com/gh-training-ak/mandat-platform/actions/workflows/codeql.yml)
[![Release](https://img.shields.io/github/v/release/gh-training-ak/mandat-platform?sort=semver)](https://github.com/gh-training-ak/mandat-platform/releases)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Open issues](https://img.shields.io/github/issues/gh-training-ak/mandat-platform)](https://github.com/gh-training-ak/mandat-platform/issues)

<sub>Built with</sub>

![Stack](https://skillicons.dev/icons?i=dotnet,cs,angular,typescript,azure,docker,kubernetes,terraform,githubactions)

</div>

---

> [!NOTE]
> This repository is used as a teaching example. Every tab is populated on purpose, so that each
> GitHub feature has something real to look at.

> [!TIP]
> Start with [docs/architecture.md](docs/architecture.md) if you want the diagrams rather than the prose.

> [!IMPORTANT]
> Database migrations in `db/migrations` are applied automatically on deploy to `test` and `prod`.
> Never edit a migration that has already shipped.

> [!WARNING]
> `infra/terraform` points at a shared remote state. Run `terraform plan` before anything else.

> [!CAUTION]
> Edited this in main
> cannot be undone.

---

## Table of contents

- [I actually changed in this in the meantime](#i-actually-changed-in-this-in-the-meantime)
  - [Table of contents](#table-of-contents)
  - [What it does](#what-it-does)
  - [Quick start](#quick-start)
  - [Architecture](#architecture)
  - [The team](#the-team)
  - [Project status](#project-status)
  - [API reference](#api-reference)
  - [Configuration](#configuration)
  - [Testing](#testing)
  - [Deployment](#deployment)
  - [Roadmap](#roadmap)
  - [FAQ](#faq)
  - [Appendix: markdown features used on this page](#appendix-markdown-features-used-on-this-page)

---

## What it does

A student signs up, filters for a mentor, and requests a slot. The mentor accepts or rejects.
Once matched, they agree a weekly timeslot, online or in person.

| Capability | Student | Mentor | Admin |
| :--- | :---: | :---: | :---: |
| Search and filter mentors | :white_check_mark: | :x: | :white_check_mark: |
| See locations on a map | :white_check_mark: | :x: | :white_check_mark: |
| Publish an announcement | :x: | :white_check_mark: | :white_check_mark: |
| Accept or reject a request | :x: | :white_check_mark: | :x: |
| Leave a review | :white_check_mark: | :white_check_mark: | :x: |
| Generate a meeting link | :x: | :white_check_mark: | :x: |
| Suspend an account | :x: | :x: | :white_check_mark: |

Filters available on search:
Edited this in main
- Session type, online or face to face
- Subject: maths, physics, chemistry, biology, computer science, English, history
- Location, and a radius in kilometres
- Minimum mentor rating
- Maximum hourly rate

---

## Quick start

<!-- Tabbed-feeling sections using collapsible details. -->

<details open>
<summary><b>Run everything with Docker Compose</b></summary>

```bash
git clone https://github.com/gh-training-ak/mandat-platform.git
cd mandat-platform
docker compose up --build
```

Then open <http://localhost:4200>. The API is on <http://localhost:8080> and Swagger at `/swagger`.

</details>

<details>
<summary><b>Run the API on its own</b></summary>

```bash
dotnet restore
dotnet run --project src/Mandat.Api
```

```console
$ dotnet run --project src/Mandat.Api
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7041
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

</details>

<details>
<summary><b>Run the front end on its own</b></summary>

```bash
cd web
npm ci
npm start
```

</details>

<details>
<summary><b>Prerequisites</b></summary>

| Tool | Minimum | Check with |
| --- | --- | --- |
| .NET SDK | 8.0.400 | `dotnet --version` |
| Node | 20.11 | `node --version` |
| Docker | 24 | `docker --version` |
| Terraform | 1.9 | `terraform version` |
| Azure CLI | 2.60 | `az version` |

</details>

---

## Architecture

```mermaid
flowchart LR
    subgraph Client
        A["Angular 18 SPA"]
    end

    subgraph Edge
        B["Azure Front Door"]
    end

    subgraph Services
        C["Mandat.Api<br/>ASP.NET Core 8"]
        D["Mandat.Application"]
        E["Mandat.Domain"]
        F["Mandat.Infrastructure"]
    end

    subgraph Data
        G[("Azure SQL")]
        H[("Redis")]
        I["Blob Storage"]
    end

    subgraph External
        J["Google Maps"]
        K["Zoom API"]
        L["Entra ID"]
    end

    A --> B --> C
    C --> D --> E
    C --> F --> E
    F --> G
    C --> H
    C --> I
    A --> J
    C --> K
    A --> L

    classDef core fill:#dafbe1,stroke:#1a7f37,color:#1a7f37
    classDef data fill:#ddf4ff,stroke:#0969da,color:#0969da
    classDef ext fill:#fff8c5,stroke:#9a6700,color:#9a6700
    class D,E core
    class G,H,I data
    class J,K,L ext
```

<details>
<summary><b>Request lifecycle, as a sequence</b></summary>

```mermaid
sequenceDiagram
    autonumber
    actor U as Student
    participant W as SPA
    participant A as API
    participant R as Redis
    participant S as SQL

    U->>W: Apply filters
    W->>A: GET /api/mentors?subject=Mathematics&maxHourlyRate=25
    A->>R: GET mentors:maths:25
    alt cached
        R-->>A: hit
    else not cached
        A->>S: SELECT TOP 20 ... ORDER BY Rating DESC
        S-->>A: rows
        A->>R: SETEX 60
    end
    A-->>W: 200 OK
    W-->>U: Results plus map pins
```

</details>

<details>
<summary><b>Branching model</b></summary>

```mermaid
gitGraph
    commit id: "init"
    commit id: "domain model"
    branch feature/mentor-search
    commit id: "search service"
    commit id: "controller"
    checkout main
    merge feature/mentor-search tag: "v1.0.0"
    branch feature/reviews
    commit id: "review entity"
    checkout main
    branch fix/distance-rounding
    commit id: "fix haversine"
    checkout main
    merge fix/distance-rounding
    checkout feature/reviews
    commit id: "rating average"
    checkout main
    merge feature/reviews tag: "v1.1.0"
```

</details>

<details>
<summary><b>Where the time goes in a release</b></summary>

```mermaid
pie showData title Pull requests by area, last 90 days
    "Backend" : 38
    "Front end" : 27
    "Infrastructure" : 14
    "Dependencies" : 19
    "Documentation" : 7
```

</details>

---

## The team

<table>
  <tr>
    <td align="center" width="160">
      <b>Dobre Talida</b><br />
      <sub>Product</sub><br />
      <sub><code>@talida</code></sub>
    </td>
    <td align="center" width="160">
      <b>Ion Alexandra</b><br />
      <sub>Front end</sub><br />
      <sub><code>@alexandra</code></sub>
    </td>
    <td align="center" width="160">
      <b>Kayed Amar</b><br />
      <sub>Platform</sub><br />
      <sub><code>@AmarKayed</code></sub>
    </td>
  </tr>
  <tr>
    <td align="center">
      <b>Necula Narcis</b><br />
      <sub>Backend</sub><br />
      <sub><code>@narcis</code></sub>
    </td>
    <td align="center">
      <b>Postolache Miruna</b><br />
      <sub>QA</sub><br />
      <sub><code>@miruna</code></sub>
    </td>
    <td align="center">
      <b>Predescu Denisa</b><br />
      <sub>Data</sub><br />
      <sub><code>@denisa</code></sub>
    </td>
  </tr>
</table>

Reviews are routed by [CODEOWNERS](.github/CODEOWNERS). Anything under `infra/` needs a platform review.

---

## Project status

Current sprint board: [Projects tab](../../projects).

```mermaid
gantt
    title Delivery plan
    dateFormat YYYY-MM-DD
    axisFormat %b
    excludes weekends

    section Foundations
    Domain model and schema      :done,    f1, 2025-01-13, 28d
    Authentication with JWT      :done,    f2, after f1, 21d

    section Matching
    Mentor search and filters    :done,    m1, 2025-03-10, 35d
    Map integration              :done,    m2, after m1, 21d
    Match requests               :done,    m3, after m2, 28d

    section Quality
    Reviews and ratings          :done,    q1, 2025-06-16, 21d
    Test coverage to 80 percent  :active,  q2, 2025-09-01, 60d

    section Platform
    Container Apps and Bicep     :done,    p1, 2025-05-05, 30d
    Multi-environment pipeline   :done,    p2, after p1, 25d
    Blue-green deployments       :         p3, 2026-01-12, 30d
```

Progress at a glance, using task lists:

- [x] Domain model and database schema
- [x] JWT authentication, with refresh tokens
- [x] Mentor search with paging and filters
- [x] Google Maps integration
- [x] Match request lifecycle
- [x] Reviews and average rating
- [x] CI with tests, CodeQL and container scanning
- [x] Three-environment deployment pipeline
- [ ] Scheduling with recurring slots
- [ ] In-app chat
- [ ] Assignment upload and download
- [ ] Payments

---

## API reference

Interactive docs are at `/swagger`. The short version:

| Method | Route | Auth | Description |
| :--- | :--- | :---: | :--- |
| `GET` | `/api/mentors` | Anonymous | Search mentors with filters and paging |
| `GET` | `/api/mentors/{id}` | Anonymous | A single mentor profile |
| `POST` | `/api/match-requests` | Student | Request to join a mentor's list |
| `PATCH` | `/api/match-requests/{id}` | Mentor | Accept or reject |
| `POST` | `/api/reviews` | Either | Leave a review after a session |
| `GET` | `/healthz` | Anonymous | Liveness and readiness |

<details>
<summary><b>Example request and response</b></summary>

```http
GET /api/mentors?subject=Mathematics&meetingType=Online&maxHourlyRate=25&page=1&pageSize=2 HTTP/1.1
Host: mandat.example.org
Accept: application/json
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

```json
[
  {
    "id": "6f2b1c4e-93a1-4b77-9f2a-1d0e5c8b7a31",
    "displayName": "Alexandra Ion",
    "hourlyRate": 22.50,
    "rating": 4.8,
    "distanceKm": null
  },
  {
    "id": "a1c3e5f7-2b4d-6e8f-0a1c-3e5f7b9d1c3e",
    "displayName": "Narcis Necula",
    "hourlyRate": 18.00,
    "rating": 4.6,
    "distanceKm": 3.2
  }
]
```

</details>

<details>
<summary><b>Error shape</b></summary>

Every error is an [RFC 7807](https://datatracker.ietf.org/doc/html/rfc7807) problem document.

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Mentor not found",
  "status": 404,
  "detail": "Mentor 6f2b1c4e-93a1-4b77-9f2a-1d0e5c8b7a31 was not found.",
  "traceId": "00-8f1c2d3e4a5b6c7d8e9f0a1b2c3d4e5f-1a2b3c4d5e6f7a8b-00"
}
```

</details>

---

## Configuration

Settings resolve in this order, last one wins:

1. `appsettings.json`
2. `appsettings.{Environment}.json`
3. Environment variables
4. Key Vault, in `test` and `prod` only

| Variable | Required | Default | Notes |
| --- | :---: | --- | --- |
| `ConnectionStrings__Mandat` | yes | none | Azure SQL connection string |
| `Redis__ConnectionString` | no | none | Caching is skipped when unset |
| `Jwt__Issuer` | yes | none | Must match the Entra ID tenant |
| `Jwt__Audience` | yes | none | |
| `Maps__ApiKey` | yes | none | Pulled from Key Vault in `test` and `prod` |
| `Features__EnableChat` | no | `false` | Feature flag, see the roadmap |

> [!WARNING]
> Never put a real connection string in `appsettings.json`. Secret scanning push protection is on
> for this repository and the push will be blocked.

---

## Testing

```bash
dotnet test                                  # unit and integration
dotnet test --collect:"XPlat Code Coverage"  # with coverage
cd web && npm run test                       # front end
```

Current coverage, as reported by the last CI run on `main`:

| Project | Line | Branch |
| --- | ---: | ---: |
| `Mandat.Domain` | 94% | 88% |
| `Mandat.Application` | 87% | 79% |
| `Mandat.Infrastructure` | 71% | 63% |
| `Mandat.Api` | 66% | 58% |
| **Total** | **79%** | **72%** |

The team target is 80% line coverage, tracked in [#142](../../issues/142).

---

## Deployment

Three environments, one pipeline, promotion by approval.

```mermaid
stateDiagram-v2
    [*] --> PullRequest
    PullRequest --> CI: opened
    CI --> Blocked: checks fail
    Blocked --> CI: push a fix
    CI --> Review: checks pass
    Review --> Main: approved and merged
    Main --> Dev: automatic
    Dev --> Test: automatic
    Test --> Production: manual approval
    Production --> [*]
```

| Environment | Trigger | Infrastructure | Approval |
| --- | --- | --- | :---: |
| `dev` | Merge to `main` | Container App, 1 replica, SQL Basic | none |
| `test` | After dev succeeds | Container App, 1 to 3 replicas, SQL S0 | none |
| `prod` | After test succeeds | Container App, 3 to 20 replicas, SQL S3 zone redundant | required |

Deploying a specific version by hand:

```bash
gh workflow run cd.yml -f environment=prod
gh run watch
```

---

## Roadmap

| Quarter | Theme | Status |
| --- | --- | --- |
| Q1 2026 | Scheduling with recurring slots | :construction: In progress |
| Q2 2026 | In-app chat and notifications | :calendar: Planned |
| Q3 2026 | Assignment upload and download | :calendar: Planned |
| Q4 2026 | Payments and mentor payouts | :thought_balloon: Idea |

---

## FAQ

<details>
<summary><b>Why Container Apps instead of AKS?</b></summary>

See [ADR 0001](docs/adr/0001-use-container-apps-over-aks.md). Short version: one stateless service,
predictable traffic, and nobody wants to patch a cluster for it.

</details>

<details>
<summary><b>Why is the rating a decimal and not a float?</b></summary>

Because money and ratings both get compared for equality, and floats lose. `decimal(10,2)` in SQL,
`decimal` in C#.

</details>

<details>
<summary><b>How do I add a new subject?</b></summary>

Add it to the `Subject` enum, add a migration that widens the check constraint, and update the
front-end filter list. All three, or the filter silently drops results.

```diff
  public enum Subject
  {
      Mathematics = 1,
      Physics = 2,
      Chemistry = 3,
      Biology = 4,
      ComputerScience = 5,
      English = 6,
-     History = 7
+     History = 7,
+     Geography = 8
  }
```

</details>

---

## Appendix: markdown features used on this page

Kept deliberately, as a reference.

| Feature | Example |
| --- | --- |
| Headings, six levels | `# .. ######` |
| Bold, italic, both | **bold**, *italic*, ***both*** |
| Strikethrough | ~~deprecated~~ |
| Inline code | `dotnet test` |
| Fenced code with language | ` ```csharp ` |
| Diff highlighting | ` ```diff ` |
| Console output | ` ```console ` |
| Tables with alignment | `:---`, `:---:`, `---:` |
| Task lists | `- [x]` |
| Collapsible sections | `<details><summary>` |
| Raw HTML | `<div align="center">`, `<table>`, `<sub>` |
| Alerts | `> [!NOTE]`, `[!TIP]`, `[!IMPORTANT]`, `[!WARNING]`, `[!CAUTION]` |
| Mermaid: flowchart | `flowchart LR` |
| Mermaid: sequence | `sequenceDiagram` |
| Mermaid: entity relationship | `erDiagram` |
| Mermaid: gantt | `gantt` |
| Mermaid: state | `stateDiagram-v2` |
| Mermaid: pie | `pie showData` |
| Mermaid: git graph | `gitGraph` |
| Emoji shortcodes | `:white_check_mark:` `:construction:` |
| Footnotes | See below[^1] |
| Autolinks | <https://github.com> |
| Relative links | [CONTRIBUTING.md](CONTRIBUTING.md) |
| Issue and PR references | `#142` becomes a link |
| Images with sizing | `<img width="420">` |
| Badges | shields.io and Actions status |
| Horizontal rules | `---` |
| Block quotes | `>` |
| Nested lists | ordered inside unordered |
| Definition-style tables | the configuration table above |
| Subscript and superscript | H<sub>2</sub>O, 10<sup>3</sup> |
| Keyboard keys | <kbd>Ctrl</kbd> + <kbd>Shift</kbd> + <kbd>P</kbd> |
| Mathematics | $E = mc^2$ and the block below |

Inline maths: the haversine distance between two points is $d = 2r\arcsin\left(\sqrt{\sin^2\left(\frac{\varphi_2-\varphi_1}{2}\right) + \cos\varphi_1\cos\varphi_2\sin^2\left(\frac{\lambda_2-\lambda_1}{2}\right)}\right)$.

Block maths:

$$
\text{rating}(m) = \frac{1}{|R_m|}\sum_{r \in R_m} \text{score}(r), \qquad \text{score}(r) \in \{1,2,3,4,5\}
$$

[^1]: Footnotes render at the bottom of the page and link both ways.

---

<div align="center">
<sub>Built by six students who wanted a better way to find a tutor. Licensed under MIT.</sub>
</div>
