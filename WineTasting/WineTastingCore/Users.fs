module Users

open Saturn
open Giraffe
open System.Security.Claims
open Types

// snippet from https://gist.github.com/Krzysztof-Cieslak/5b53a1ff47edf5d323d788cce4913934
let matchUpUsers : HttpHandler = fun next ctx ->
    // A real implementation would match up user identities with something stored in a database
    let isAdmin =
        ctx.User.Claims |> Seq.exists (fun claim ->
            // NOTE: `claim.Type` must match the mapped type in Config.fs
            claim.Issuer = "GitHub" && claim.Type = "fullName" && claim.Value = "Patrick Drechsler")
    if isAdmin then
        ctx.User.AddIdentity(ClaimsIdentity([Claim(ClaimTypes.Role, "Admin", ClaimValueTypes.String, "MyApplication")]))
    next ctx

let createUser (name : FullName option) (githubUserName : GitHubName) : User = 
    {
        GithubUserName = githubUserName
        Name           = name
    }

let saveUser : HttpHandler = fun next ctx ->
    
    let claims = ctx.User.Claims

    let claimsMap =
        claims
        |> Seq.map (fun claim -> (claim.Type, claim.Value))
        |> Map.ofSeq  
    
    let usernameOpt = 
        claimsMap 
            |> Map.tryFind "fullName"
            |> Option.map createFullName
    
    let githubnameOpt = 
        claimsMap 
            |> Map.tryFind "githubUsername" 
            |> Option.map createGitHubName

    Option.map (createUser usernameOpt) githubnameOpt 
    |> Option.map Db.saveUser 
    |> ignore
    
    next ctx

let loggedIn = pipeline {
    requires_authentication (Auth.challenge "GitHub")
    plug matchUpUsers
    plug saveUser
}

let error: HttpHandler = RequestErrors.forbidden (text "Must be admin")

let isAdmin = pipeline {
    plug loggedIn
    requires_role "Admin" error
}

