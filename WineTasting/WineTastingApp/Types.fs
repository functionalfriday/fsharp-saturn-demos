module Types

type GitHubName =
    GitHubName of string
let createGitHubName s = GitHubName s
let getGitHubNameValue (GitHubName s) = s

type FullName =
    FullName of string
let createFullName s = FullName s
let getFullNameValue (FullName s) = s


/// Assumption: `GithubUserName` is unique across github -> can be our "primary key"
type User = {
    GithubUserName : GitHubName
    Name : FullName option
}

type UserForDb = {
    GithubUserName : string
    Name : string
}
