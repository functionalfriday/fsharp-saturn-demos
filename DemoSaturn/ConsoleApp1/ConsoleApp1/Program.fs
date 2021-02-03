// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open Saturn
open Giraffe
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.CookiePolicy
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Authentication.OAuth
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
    // use_cookie_policy (cpo)

    logging configureLog

    force_ssl

    use_github_oauth "21ac75fb5fc9aea4385b" "05eff61cbd57b24e02bce9cbfa771f2d177bd814" "/signin-github" [("login", "githubUsername"); ("name", "fullName")]
}



[<EntryPoint>]
let main argv =
    run app
    0 // return an integer exit code