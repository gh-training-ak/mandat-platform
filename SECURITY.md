# Security policy

## Supported versions

| Version | Supported |
| --- | --- |
| 2.4.x | Yes |
| 2.3.x | Security fixes only, until 31 March 2026 |
| Below 2.3 | No |

## Reporting a vulnerability

**Please do not open a public issue.**

Use [private vulnerability reporting](https://github.com/gh-training-ak/mandat-platform/security/advisories/new)
instead. We aim to acknowledge within two working days and to ship a fix or a mitigation within
ten.

## What we run on every pull request

| Control | Tool |
| --- | --- |
| Static analysis | CodeQL, on every push and pull request to `main` |
| Dependency alerts | Dependabot, grouped weekly |
| Secret scanning | Push protection, enabled org-wide |
| Container scanning | Trivy, on every image build |
| Infrastructure linting | `tfsec` and `bicep lint` |
