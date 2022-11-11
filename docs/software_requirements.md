## Functional requirements

- Receives repository identifier from GitHub, and should collect commits with author names and dates.
- Should clone the repository from GitHub
- In case the repository already exists locally, the local repository should be updated.
- Should run in two modes - `CommitFrequency` & `CommitAuthor`
- `CommitFrequency` produces an output which lists the number of commits per day.
- `CommitAuthor` produces an output which lists the number of commits per author per day.
- Repository is set up with a Github Actions Workflow to run unit tests and builds `GitInsight` on all pushes and pull requests to `main`.
- The system should store information about which repositories were analyzed at what state.
- If an already analyzed repository is re-analyzed, the stored information should be updated with the most recent results.
- If an already analyzed repository is to be re-analyzed, but the data is already up to date, the analysis should be skipped and results should be gathered from already stored data.

## Non-functional requirements

- Web applicaqtion
- Shall expose a REST API
- GitHub identifier should be of the form `<github_user>/<repository_name>` or `<github_organization>/<repository_name>`
- Data about commits should be collected using `libgit2sharp`
- Running as `CommitFrequency` or `CommitAuthor` should be decided using CLI switches.
- `CommitFrequency` and `CommitAuthor` both writes output to `stdout`
- `GitInsight` should be developed using test-driven development.
- The application should be as simple as possible.
- Results from analyzing repositories should be stored in a database
- During development, recall design patterns that are relevant for the implementation.
- The REST API shall return the analysis results via JSON objects.

## Design decisions

- When running without a switch, `GitInsight` assumes user wants to use `CommitFrequency` by default.
