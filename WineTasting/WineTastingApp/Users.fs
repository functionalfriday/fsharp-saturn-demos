module Users

open Saturn
open Giraffe
open System.Security.Claims

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

let saveUser : HttpHandler = fun next ctx ->
    let claims = ctx.User.Claims
    let claimsMap =
        claims
        |> Seq.map (fun claim -> (claim.Type, claim.Value))
        |> Map.ofSeq  
    
    let fullNameOpt = claimsMap |> Map.tryFind "fullName"
    let githubNameOpt = claimsMap |> Map.tryFind "githubUsername"
    let logger = ctx.GetLogger("FooLogger")
    
    // NOTE: error handling with dbResult possible...        
    let dbResult = Option.map2 (Db.save logger) fullNameOpt githubNameOpt  
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