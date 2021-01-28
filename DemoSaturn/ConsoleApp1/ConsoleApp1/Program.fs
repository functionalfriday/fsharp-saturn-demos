// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open Saturn
open Giraffe

let endpointPipe = pipeline {
    plug head
    plug requestId
}

let app = application {
    pipe_through endpointPipe
    url "http://0.0.0.0:5000/"
    use_router (Router.browserRouter)
    memory_cache
    use_github_oauth "XX" "XX" "/signin-github" [("login", "githubUsername"); ("name", "fullName")]
}

[<EntryPoint>]
let main argv =
    run app
    0 // return an integer exit code