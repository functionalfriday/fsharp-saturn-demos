module Controllers

open System
open Microsoft.AspNetCore.Http
open Saturn
open Wines
open Types

let createId = Guid.NewGuid()

let mycreate (ctx: HttpContext) wineName =
    let githubUsernameClaim = ctx.User.Claims |> Seq.find (fun x -> x.Type = "githubUsername")
    createWine {
        Wine.WineId = WineId createId
        Name = wineName
        CreatedByGithubUserName = GitHubName githubUsernameClaim.Value
    }


let wineController = controller {
    index (fun ctx -> "Index handler version 1" |> Controller.text ctx) //View list of users
    //add (fun ctx -> "Add handler version 1" |> Controller.text ctx) //Add a user // "Show form for create"
    create (fun ctx -> mycreate ctx (WineName "rotwein") |> Controller.json ctx) //Create a user // this is the actual POST command
    show (fun ctx id -> (sprintf "Show handler version 1 - %i" id) |> Controller.text ctx) //Show details of a user
    //edit (fun ctx id -> (sprintf "Edit handler version 1 - %i" id) |> Controller.text ctx)  //Edit a user // "Show form for update"
    update (fun ctx id -> (sprintf "Update handler version 1 - %i" id) |> Controller.text ctx)  //Update a user// this is the actual PUT command
}