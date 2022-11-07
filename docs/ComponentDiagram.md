# Class Diagram showing structure of program

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
