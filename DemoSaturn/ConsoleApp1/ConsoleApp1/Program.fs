// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open Saturn
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.CookiePolicy
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging

let endpointPipe = pipeline {
    plug head
    plug requestId
}

let  cpo: CookiePolicyOptions = 
    CookiePolicyOptions(
        HttpOnly = HttpOnlyPolicy.Always,
        Secure = CookieSecurePolicy.Always,
        MinimumSameSitePolicy = SameSiteMode.None
    )

let configureLog = fun (logConfig : ILoggingBuilder) ->
    logConfig.SetMinimumLevel(LogLevel.Trace) |> ignore

let app: Microsoft.Extensions.Hosting.IHostBuilder = application {
    pipe_through endpointPipe
    url "https://localhost:5000/"
    use_router (Router.browserRouter)
    use_developer_exceptions
    memory_cache
    
    logging configureLog

    force_ssl
    
    use_github_oauth "2dfd0060878f3db64d90" "df1465798ffc37075bf16f6a92c8688d818eaa36" "/signin-github" [("login", "githubUsername"); ("name", "fullName")]
}

[<EntryPoint>]
let main argv =
    run app
    0 // return an integer exit code