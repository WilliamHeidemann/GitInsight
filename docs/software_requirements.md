## Functional requirements

- Receives repository identifier from GitHub, and should collect commits with author names and dates.
- Should clone the repository from GitHub.
- Has a front-end web-application.
- Shall expose a REST API.
- Front end should visualize `CommitFrequency`, `CommitAuthor` and `Forks` for a repository.
- Front end should visualize line additions, deletions and total changes per commit, per repository.
- Front end and REST API shall only be accessible to authenticated and authorized users.
- In case the repository already exists locally, the local repository should be updated.
- CLI tool should run in two modes - `CommitFrequency` & `CommitAuthor`
- `CommitFrequency` produces an output which lists the number of commits per day.
- `CommitAuthor` produces an output which lists the number of commits per author per day.
- The system should be able to produce a list of forks of a given repository.
- Repository is set up with a Github Actions Workflow to run unit tests and builds `GitInsight` on all pushes and pull requests to `main`.
- The system should store information about which repositories were analyzed at what state.
- If an already analyzed repository is re-analyzed, the stored information should be updated with the most recent results.
- If an already analyzed repository is to be re-analyzed, but the data is already up to date, the analysis should be skipped and results should be gathered from already stored data.

## Non-functional requirements

- Front-end web-application is written with .Net Blazor
- Front-end should visualize using the Radzen framework.
- Data about forks should be acquired using the GitHub REST API.
- REST API shall return analysis result via JSON objects.
- Security in frontend and REST API should be implemented using ASP .NET Core Identity.
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

- When running without a switch, `GitInsight` assumes user wants to use `CommitFrequency` by default in CLI tool.
