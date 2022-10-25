# Requirements

## Functional requirements

- Receives path to a local git repository should collect commits with author names and dates.
- Should run in two modes - `CommitFrequency` & `CommitAuthor`
- `CommitFrequency` produces a textual output which lists the number of commits per day.
- `CommitAuthor` produces a textual output which lists the number of commits per author per day.
- Repository is set up with a Github Actions Workflow to run unit tests and builds `GitInsight` on all pushes and pull requests to `main`.

## Non-functional requirements

- Command Line Interface
- Running as `CommitFrequency` or `CommitAuthor` should be decided using CLI switches.
- `CommitFrequency` and `CommitAuthor` both writes output to `stdout`
- `GitInsight` should be developed using test-driven development.
- The application should be as simple as possible.

## Design decisions

- When running without a switch, `GitInsight` assumes user wants to use `CommitFrequency` by default.
