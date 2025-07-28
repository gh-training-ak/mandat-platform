# ADR 0001: Use Azure Container Apps rather than AKS

- **Status:** Accepted
- **Date:** 2025-03-14
- **Deciders:** Platform team

## Context

The API is a single stateless service with a predictable traffic shape: a weekday peak between
16:00 and 21:00 when students search for mentors, and very little overnight. We need
autoscaling, zero-downtime deployments and a private link to Azure SQL.

We already hold AKS experience in the team, and the Terraform in `infra/terraform` provisions an
AKS cluster for a separate workload.

## Decision

Deploy the API to **Azure Container Apps**, not AKS.

## Consequences

Good:

- No cluster to patch, upgrade or right-size
- Scale to zero in dev and test, which removes most of the non-production bill
- Revisions give us blue-green for free

Less good:

- We give up fine-grained scheduling and custom admission control
- If we later need sidecars beyond what Container Apps supports, we migrate

We accept the trade. Revisit if the service count passes roughly ten, at which point a shared
cluster starts to pay for itself.
