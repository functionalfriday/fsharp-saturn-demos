// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open Saturn
open Microsoft.Extensions.Logging
open Config

let endpointPipe = pipeline {
    plug head
    plug requestId
}

let configureLog = fun (logConfig : ILoggingBuilder) ->
    logConfig.SetMinimumLevel(LogLevel.Information) |> ignore

let app : Microsoft.Extensions.Hosting.IHostBuilder = application {
    pipe_through endpointPipe
    url config.url
    use_router (Router.browserRouter)
    use_developer_exceptions
    memory_cache
    
    /// TODO Where is the root?
    /// This works when project is started from location of *.fsproj file,
    /// but fails when started from location of *.sln file
    use_static "static" 
    
    /// TODO Howto include logging in other parts of the application?
    logging configureLog

    force_ssl
        
    use_github_oauth config.github.clientId config.github.clientSecret config.github.callbackPath config.claimActions.claims 
}

[<EntryPoint>]
let main argv =
    run app
    0 // return an integer exit code