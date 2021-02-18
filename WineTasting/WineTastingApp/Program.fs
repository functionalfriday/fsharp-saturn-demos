// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open Db
open System
open System.IO
open Saturn
open Microsoft.Extensions.Logging
open Config
open FsConfig
open Microsoft.Extensions.Configuration

let appConfig =
    let b = ConfigurationBuilder()
    b.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build()
    |> AppConfig

let secretConfig =
    let b = ConfigurationBuilder()
    b.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("secrets.json")
        .Build()
    |> AppConfig

let config =
    let appSettings = appConfig.Get<AppSettings> ()
    match appSettings with
    | Ok x -> x
    | _ -> raise (Exception ("ups: could not parse config file"))

let secret =
    let appSettings = secretConfig.Get<SecretSettings> ()
    match appSettings with
    | Ok x -> x
    | Error e -> raise (Exception (e.ToString()))
    
let endpointPipe = pipeline {
    plug head
    plug requestId
}

let configureLog = fun (logConfig : ILoggingBuilder) ->
    logConfig.SetMinimumLevel(LogLevel.Information) |> ignore

let app : Microsoft.Extensions.Hosting.IHostBuilder = application {
    pipe_through endpointPipe
    url config.Url
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
        
    use_github_oauth
        config.MyConfigs.GitHub.ClientId
        secret.GithubSecret
        config.MyConfigs.GitHub.CallbackPath
        [("login", "githubUsername"); ("name", "fullName")]   
}


[<EntryPoint>]
let main argv =
//    Db.createDb
    run app
    0 // return an integer exit code