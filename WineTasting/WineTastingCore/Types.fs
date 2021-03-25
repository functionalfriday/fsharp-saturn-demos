module Types

type GitHubName =
    GitHubName of string
let createGitHubName s = GitHubName s
let getGitHubNameValue (GitHubName s) = s

type FullName =
    FullName of string
let createFullName s = FullName s
let getFullNameValue (FullName s) = s

type WineId =
    WineId of System.Guid
let createWineId s = WineId s
let getWineIdValue (WineId s) = s
let getWineIdString (wineId : WineId) = (getWineIdValue wineId).ToString()

type WineName =
    WineName of string
let createWineName s = WineName s
let getWineNameValue (WineName s) = s

// Assumption: `GithubUserName` is unique -> can be our "primary key"
type User = {
    GithubUserName : GitHubName
    Name : FullName option
}

type UserForDb = {
    GithubUserName : string
    Name : string
}

type Wine = {
    WineId : WineId
    Name : WineName
    CreatedByGithubUserName : GitHubName
}

[<CLIMutable>]
type WineForDb = {
    WineId : string
    Name : string
    CreatedByGithubUserName : string
}
