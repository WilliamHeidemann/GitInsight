## Functional requirements

- Receives path to a local git repository should collect commits with author names and dates.
- Should run in two modes - `CommitFrequency` & `CommitAuthor`
- `CommitFrequency` produces a textual output which lists the number of commits per day.
- `CommitAuthor` produces a textual output which lists the number of commits per author per day.
- Repository is set up with a Github Actions Workflow to run unit tests and builds `GitInsight` on all pushes and pull requests to `main`.
- The system should store information about which repositories were analyzed at what state.
- If an already analyzed repository is re-analyzed, the stored information should be updated with the most recent results.
- If an already analyzed repository is to be re-analyzed, but the data is already up to date, the analysis should be skipped and results should be gathered from already stored data.

## Non-functional requirements

- Command Line Interface
- Running as `CommitFrequency` or `CommitAuthor` should be decided using CLI switches.
- `CommitFrequency` and `CommitAuthor` both writes output to `stdout`
- `GitInsight` should be developed using test-driven development.
- The application should be as simple as possible.
- Results from analyzing repositories should be stored in a database
- During development, recall design patterns that are relevant for the implementation.
