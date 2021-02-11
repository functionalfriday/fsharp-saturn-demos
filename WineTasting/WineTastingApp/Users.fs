module Users

open Config
open Microsoft.Extensions.Logging
open Saturn
open Giraffe
open System.Security.Claims

// snippet from https://gist.github.com/Krzysztof-Cieslak/5b53a1ff47edf5d323d788cce4913934
let matchUpUsers : HttpHandler = fun next ctx ->
    // A real implementation would match up user identities with something stored in a database
    let isAdmin =
        ctx.User.Claims |> Seq.exists (fun claim ->
            // NOTE: `claim.Type` must match the mapped type in Config.fs
            let logger = ctx.GetLogger("FOOLogger")
            logger.Log(LogLevel.Error, "xxxxxxxxxxxxxxxxxxxxxxxxx")
            claim.Issuer = "GitHub" && claim.Type = "fullName" && claim.Value = "Patrick Drechsler")
    if isAdmin then
        ctx.User.AddIdentity(ClaimsIdentity([Claim(ClaimTypes.Role, "Admin", ClaimValueTypes.String, "MyApplication")]))
    next ctx

let loggedIn = pipeline {
    requires_authentication (Auth.challenge "GitHub")
    plug matchUpUsers
}

let error: HttpHandler = RequestErrors.forbidden (text "Must be admin")

let isAdmin = pipeline {
    plug loggedIn
    requires_role "Admin" error
}