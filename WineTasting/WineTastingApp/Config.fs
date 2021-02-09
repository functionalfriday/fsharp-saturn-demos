module Config

/// Settings from Github
type GithubConfig = {
    clientId : string
    clientSecret : string
    callbackPath : string
}

type ClaimActions = {
    /// This defines the mapping between
    /// 
    ///   Claims "properties" provided by the OAuth counterpart (here: GitHub) and
    ///   property names we want to use internally.
    /// 
    /// Example:
    /// 
    /// GitHub "login" -> Our property name "githubUsername"
    /// GitHub "name" -> Our property name "fullName"
    /// 
    /// See ASP.NET `ClaimActions.MapJsonKey` for other examples.
    ///
    /// IMHO this mapping is optional as long as the OAuth provider (here: GitHub) uses
    /// standard names.
    /// TODO Clarify if claims mapping is really needed for our use-case
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