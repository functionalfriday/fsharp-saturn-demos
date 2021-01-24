// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open Saturn
open Giraffe

let app = application {
    use_router (Router.browserRouter)
}

[<EntryPoint>]
let main argv =
    run app
    0 // return an integer exit code