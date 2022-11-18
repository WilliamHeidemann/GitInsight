# Diagrams

## Class Diagram showing structure of program
```plantuml
@startuml
() "CommitMode" as commit
() "AuthorMode" as author
() "dotnet test" as dotnettest

package "GitInsight" {
  [GitCommitTracker] as gitinsight
  [LibGit2Sharp] as lb2s

  [GitInsight.Tests] as test

  () "MSSQL" as mssql
  () "MySQL inmemory" as mysql


  database "PersistantStorage" as db {
    [DbRepository]
    [DbCommit]
  }
}

test --> mysql
mysql --> db

gitinsight --> mssql
mssql --> db


dotnettest --> test
commit --> gitinsight
author --> gitinsight

db --> lb2s

gitinsight --> () output
@enduml
```

![Component Diagram](https://www.plantuml.com/plantuml/svg/NL7DIWCn4BxFKmnxqaCzz06aeY122-hs9ZqaJMY3cysQcO15-kvcChQLNWBVJ_9zCraGcymb3d1pYzrZYZ7mAZdVeI7SDjYKXyBxbC-AQR0fBl7e6TaJDqcnG839R3_DpcFt7FXbfB3RLyyF87vKGiFMkNfytiZLNU2WBh4iWwskURytUMhoOES4ebnUkrlN76gg9Y9AfrNVlorRcqZqDPpOGsKnZD77b0yg7qIMQywmiOPgrrUVAH2RaNjEkTRNiMmhNjEVxBkV4WMkZsnEb0uZy0X702c3izdzNq0x6tOMu3ocMhIYd1SBNBOed0wdto7u_wRqAwvPS5OetTbreYo3_JSNCs0gF1NkOE57L-Bm2m00)

## Architectural Diagram of REST Api

```mermaid
graph LR;
    A[Client] --- B[GET];
    B --> C[CONTROLLER <br/> PersistentStorageController];
    C --- D[GET<br/> POST<br/> PUT];
    D --> E[(DB <br/> PersistentStorage)];
    C --> |RESPONSE<br/> JSON OBJECT| A;
```

## Activity Diagram of GitInsight GET-operations 

NOT FINISHED!! Since JSON data not implemented yet

```mermaid
stateDiagram-v2
    state if_state <<choice>>
    [*] --> GET
    GET --> FindAllCommitsAsync
    FindAllCommitsAsync --> if_state
    if_state --> RepositoryNotFoundException: is not valid repo
    if_state --> CreateAsync : is valid repo

    state if_state2 <<choice>>
    POST_to_database : POST to database
    CreateAsync --> if_state2
    if_state2 --> POST_to_database : is not existing in DB
    if_state2 --> UpdateAsync : is already existing in DB

    PUT_to_DB : PUT to database
    UpdateAsync --> PUT_to_DB

    state if_state3 <<choice>>
    POST_to_database --> if_state3
    PUT_to_DB --> if_state3
    if_state3 --> FindAllCommitsByRepoId
    FindAllCommitsByRepoId --> JSON_data???
    
    JSON_data??? --> [*]
```

## Sequence diagram of GitInsight.BlazorApp running frequency mode
```mermaid
sequenceDiagram
    actor me
    participant blazor as GitInsight.BlazorApp
    participant rest as GitInsight.RestAPI
    participant infra as GitInsight.Infrastructure
    participant db as Database
    me->>+blazor: handleValidSubmit
    blazor->>+rest: isValid
    rest-->>-blazor: valid repo
    blazor-->>-me: valid repo
    me->>+blazor: onInizializeAsync
    blazor->>+rest: GET
    rest->>+infra: FindAllGithubCommits
    infra-->>-rest: cloned repo path
    rest->>+infra: GetFrequencyMode
    infra->>+db: FindAllCommitsByRepoId
    db-->>-infra: list of DBCommits
    infra-->>-rest: list of CommitCountDTO's
    rest-->>-blazor: list of CommitCountDTO's
    blazor-->>-me: render data on screen
```
