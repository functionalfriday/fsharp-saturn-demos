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
        clientId = "2dfd0060878f3db64d90"
        clientSecret = "df1465798ffc37075bf16f6a92c8688d818eaa36"
        callbackPath = "/signin-github"
    }
    claimActions = {
        claims = [("login", "githubUsername"); ("name", "fullName")]   
    }
}