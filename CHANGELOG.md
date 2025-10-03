# Changelog

All notable changes are recorded here. The format follows
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and the project uses
[semantic versioning](https://semver.org/).

## [Unreleased]

### Added
- Recurring schedule slots, behind the `Features__EnableScheduling` flag

## [2.0.0] - 2025-10-02

### Added
- Audit log for every write operation, with a queryable table
- Fixed window rate limiting on the public search endpoint
- Redis caching for mentor search, 60 second time to live

### Changed
- **Breaking.** `GET /api/mentors` now returns a paged envelope rather than a bare array
- Minimum supported .NET version raised to 8.0

### Fixed
- Sub-kilometre distances no longer round to zero (#71)

## [1.5.1] - 2025-09-25

### Fixed
- Distance rounding regression

## [1.5.0] - 2025-09-01

### Added
- Audit logging

## [1.4.0] - 2025-08-08

### Added
- Mentor search caching

## [1.0.0] - 2025-04-18

### Added
- First public release: mentors, students, announcements, match requests and reviews
