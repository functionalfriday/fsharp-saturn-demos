module Controllers

open System
open Microsoft.AspNetCore.Http
open Saturn
open Saturn.ControllerHelpers.Controller
open Wines
open Types
open FSharp.Control.Tasks

// NOTE: add unit `()` as input parameter to ensure that each call creates a new random Guid.
let createId () = Guid.NewGuid()

// NOTE: Sadly, we can't use anonymous types: We MUST use a real type for Saturn's `getForm` function
[<CLIMutable>]
type WineNameForm = { WineName : string }

let createNewWineFromContext (ctx: HttpContext) =
    // NOTE: There is probably a more elegant way to get the current user's GithubUsername ;-)
    let githubUsernameClaim = ctx.User.Claims |> Seq.find (fun x -> x.Type = "githubUsername")
    
    // Workflow:
    //
    // 1. We need to access the content of the HTML form using `getForm`.
    //    `getForm` is provided by `Saturn.ControllerHelpers.Controller`
    //
    // 2. We have to use the computational expression ("CE") `task`
    //    (located in namespace `FSharp.Control.Tasks`) to wrap the asynchronous call:
    //      - `let!`: corresponds to "await", and only continues if successful 
    //      - `return`:  wraps the return value
    task {
        let! formValue = getForm<WineNameForm> ctx
        return createWine {
            Wine.WineId = WineId (createId ())
            Name = WineName formValue.WineName
            CreatedByGithubUserName = GitHubName githubUsernameClaim.Value
        }
    }

let wineController = controller {
    index (fun ctx -> "Index handler version 1" |> Controller.text ctx) //View list of users
    //add (fun ctx -> "Add handler version 1" |> Controller.text ctx) //Add a user // "Show form for create"
    create (fun ctx -> createNewWineFromContext ctx |> Controller.json ctx) //Create a user // this is the actual POST command
    show (fun ctx id -> $"Show handler version 1 - %i{id}" |> Controller.text ctx) //Show details of a user
    //edit (fun ctx id -> (sprintf "Edit handler version 1 - %i" id) |> Controller.text ctx)  //Edit a user // "Show form for update"
    update (fun ctx id -> $"Update handler version 1 - %i{id}" |> Controller.text ctx)  //Update a user// this is the actual PUT command
}