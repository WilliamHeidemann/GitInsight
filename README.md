# GitInsight


## To launch GitInsight:

- Open a terminal.
- Type ```dotnet run --project GitInsight --repo-path="[Insert the path to the repo here]"```
- (Optional) Use the ```--author-mode``` flag after the repo-path to run it in author-mode.

## Setting up GitInsight

To setup the connection to the database:

From commandLine:

1. create variable 'password', a password you want or an autogenerated one like the one below:

    password=$(uuidgen)

2. create varibale 'database', the name of the database you want to use
    
    database="gitinsight"

3. create varibale connectionString
    
    connectionString="Host=localhost;Username=postgres;Password=$password;Database=$database"

4. init user secrets. Stand in the folder where the context is
   
    dotnet user-secrets init
    dotnet user-secrets set "ConnectionStrings:$database" "$connectionString"

5. run the database with docker
    docker run --name $database -e POSTGRES_PASSWORD=$password -d -p 5432:5432 postgres

*(det her burde ikke være nødvendigt, for det burde ligge der i forvejen) *
6. add migrations should say something and the done
    
    dotnet ef migrations add initialcreate

7. update the database according to the new migrations. Should also end with done
    
    dotnet ef database update

8. take a look a docker to see the id of the container running i.e the $containerId
    
    docker ps
