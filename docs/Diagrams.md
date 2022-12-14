# Diagrams

## Package Diagram showing architecture of program 

```plantuml
@startuml

skinparam componentStyle rectangle

package "GitInsight.BlazorApp" as app {
    [Program] as app.prog
}

package "GitInsight.RestAPI" as rest {
    [Program] as api.prog
}

    [app.prog] ..> [api.prog]
       
package "GitInsight.Infrastructure" as inf {
    [GithubApiController] as gitcontr
    [PersistentStorageController] as pscontr
    [DBRepositoryPersistentStorage] as dbrp
    [DBCommitPersistentStorage] as dbcp
    [PersistentStorageContext] as psc
    [DBRepository] as dbr
    [DBCommit] as dbc
}
    [api.prog] ..> [gitcontr]
    [api.prog] ..> [pscontr]
    [pscontr] ..> [psc] 
    [pscontr] ..> [dbrp]
    [pscontr] ..> [dbcp] 
    [dbrp] ..> [dbcp] 
    [dbrp] ..> [dbr]
    [dbcp] ..> [dbc]


package "GitInsight.Core" as core {
    [AuthorCommitDTO] as adto
    [CommitCountDTO] as ccdto
    [DBCommitDTO] as dbcdto
    [DBRepositoryDTO] as dbrdto
    [ForkDTO] as fdto
    [Response] as resp
}

rest ..> core
inf ..> core


@enduml
```

![Package Diagram](https://www.plantuml.com/plantuml/svg/XLD1JiCm4Bpx5LOVa2UWfgb0FL4LRYW7rtLJgubtjTv85DI_y1PUXkFK2T0KSlBEZ6VizMRkMH6tJIs4Frg7YbGZDJG8pZX-ubDj91dDobMr4GALFghAoDc3vPNpjZfmjgZL6r2EE9FAIuKetuKCtykQe0fsvGNECDJYF6soCPxpzIfQK5XFUzZaqPF9j9HPTjULjYyZ6xx9f9NRa_9ChUQMJ0oqRf_oWl3GRdEq1JWcg6j3CRkoh3laSYf3tdgE7G8A2R_KwB-9bukDGV0sA4zN6wDyjoLCsWAQnl8jdSO_3c1UEQLV9wUWdpd9LfmZFhRmyoEsD5sxdARHZ-nOZMGf9vdklkKDHcFQ55N_uYayAaPrAQRdh83Bc-kmI8-Ujtm0wZkoV7xiXst7qBCzNa3h1bBhnAPE3bJ83jnr_qUU1luUw9Zm_O1kZ4TmtaIOGj7DU_mdmkNYkKKOsB4GOcxShctgBm00)


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
