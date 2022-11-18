# Diagrams

## Class Diagram showing structure of program

```plantuml
@startuml

package "GitInsight.BlazorApp" as app {
    [Program] as app.prog
}

package "GitInsight.RestAPI" as rest {
    [Program] as api.prog
}

    [app.prog] --> [api.prog]
       
package "GitInsight.Infrastructure" as inf {
    [GithubApiController] as gitcontr
    [PersistentStorageController] as pscontr
    [DBRepositoryPersistentStorage] as dbrp
    [DBCommitPersistentStorage] as dbcp
    [PersistentStorageContext] as psc
    [DBRepository] as dbr
    [DBCommit] as dbc
}
    [api.prog] --> [gitcontr]
    [api.prog] --> [pscontr]
    [pscontr] --> [psc] 
    [pscontr] --> [dbrp]
    [pscontr] --> [dbcp] 
    [dbrp] --> [dbcp] 
    [dbrp] --> [dbr]
    [dbcp] --> [dbc]
 

package "GitInsight.Core" as core {
    [AuthorCommitDTO] as adto
    [CommitCountDTO] as ccdto
    [DBCommitDTO] as dbcdto
    [DBRepositoryDTO] as dbrdto
    [ForkDTO] as fdto
    [Response] as resp
}

rest --> core
inf --> core


@enduml
´´´

![Component Diagram](https://www.plantuml.com/plantuml/svg/XLDBJiCm4Dtx557txH5GqrIWhgWAEvI5cxgfrSIsnXE9ZteNpiB9c3YP6AY1h3BlFS_pVBpqA06xjX72oUeiQvNDxZHkZDVr2HUhHhvQo9sRPT9dqhdiJMJqFMt1rY3RSeGNZc9nIIVPAO_vTXDI0FsdSsZECP2SjCpcyniAHxuCB7r9fuqvWlG8NOKTg62epP7zI7ZgzhdJXJK8jcaK1EzQOzKZuwqKUErH6Nn02-JmG-ty5_5wjLFEUatAbwk3GNxOWsDjOTjMus-wolrn0VMCx7xjp4RVVJYjk0Gyjl3ZFRIKooxJjFEHZL4aoop9zFMcp_GLyg6W-XTdyw28QaBJYrROSUWL_V3KymvF5eQMh1_lXsqxe1tO0IziPoQogYQMMyaKcHDtFO38myJVMZWpVfnGUWlE6Z_C72Ze5puy2gekt5lGniP0YAKo1tgVdm00)


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
