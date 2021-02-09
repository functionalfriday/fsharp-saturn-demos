module Config

/// Settings from Github
type GithubConfig = {
    clientId : string
    clientSecret : string
    callbackPath : string
}

type ClaimActions = {
    /// This defines which properties from Github we (this application) want to have access to and how they are mapped.
    /// See ASP.NET `ClaimActions.MapJsonKey` for details. 
    claims : (string * string) list
}

type Config = {
    /// `url` should start with https, not http
    url : string
    github : GithubConfig
    claimActions : ClaimActions
}

let config = {
    url = "https://localhost:5000/"
    github = {
        clientId = "437816def3facd7c4684"
        clientSecret = "cf775fc2b347c2117519f980a373a19abe04face"
        callbackPath = "/signin-github"
    }
    claimActions = {
        claims = [("login", "githubUsername"); ("name", "fullName")]   
    }
}