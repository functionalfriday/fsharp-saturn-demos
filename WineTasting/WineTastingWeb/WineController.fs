module Controllers

open System
open Microsoft.AspNetCore.Http
open Saturn
open Saturn.ControllerHelpers.Controller
open Wines
open Types
open FSharp.Control.Tasks

let createId () = Guid.NewGuid()

// Sadly, we can't use anonymous types: We MUST use a real type for Saturn's `getForm` function
[<CLIMutable>]
type WineNameForm = { WineName : string }

let mycreate (ctx: HttpContext) =
    let githubUsernameClaim = ctx.User.Claims |> Seq.find (fun x -> x.Type = "githubUsername")
    
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
    create (fun ctx -> mycreate ctx |> Controller.json ctx) //Create a user // this is the actual POST command
    show (fun ctx id -> (sprintf "Show handler version 1 - %i" id) |> Controller.text ctx) //Show details of a user
    //edit (fun ctx id -> (sprintf "Edit handler version 1 - %i" id) |> Controller.text ctx)  //Edit a user // "Show form for update"
    update (fun ctx id -> (sprintf "Update handler version 1 - %i" id) |> Controller.text ctx)  //Update a user// this is the actual PUT command
}