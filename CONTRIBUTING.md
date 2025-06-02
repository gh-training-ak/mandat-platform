# Contributing

Thanks for helping out. This page is short on purpose.

## Getting set up

```bash
git clone https://github.com/gh-training-ak/mandat-platform.git
cd mandat-platform
dotnet restore
cd web && npm ci
```

You need the .NET 8 SDK, Node 20 or newer, and Docker if you want the local database.

## Branching

We use the GitHub flow. Branch from `main`, open a pull request, merge, delete the branch.

| Prefix | Use it for |
| --- | --- |
| `feature/` | New behaviour |
| `fix/` | Bug fixes |
| `chore/` | Dependencies, tooling, formatting |
| `docs/` | Documentation only |
| `infra/` | Anything under `infra/` or `deploy/` |

## Commit messages

We follow [Conventional Commits](https://www.conventionalcommits.org/). The release notes are
generated from them, so the prefix matters.

```
feat(api): add distance filter to mentor search
fix(web): stop the map re-centring on every keystroke
chore(deps): bump Microsoft.EntityFrameworkCore to 8.0.8
```

## Before you open a pull request

- [ ] `dotnet test` passes
- [ ] `npm run lint` passes
- [ ] You added a test for the behaviour you changed
- [ ] No secrets, connection strings or personal data in the diff

## Review

Every pull request needs one approval. Anything touching `/infra`, `/deploy` or
`/db/migrations` additionally needs a review from the owners listed in
[CODEOWNERS](.github/CODEOWNERS).
