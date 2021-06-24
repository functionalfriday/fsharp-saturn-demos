module Wines

open System
open FSharpPlus
open FSharpPlus.Data
open Types

let getAllWines () : Wine seq =
    let convertWine (w : WineForDb) : Wine =
        {
            Wine.WineId = createWineId <| System.Guid.Parse(w.WineId)
            Name = createWineName w.Name
            CreatedByGithubUserName = createGitHubName w.CreatedByGithubUserName
        }

    Db.getAllWines ()
    |> Seq.map convertWine 

let createWineInDb (wine : Wine) =
    let wineForDb = {
        WineForDb.WineId = getWineIdString wine.WineId
        Name = getWineNameValue wine.Name
        CreatedByGithubUserName = getGitHubNameValue wine.CreatedByGithubUserName
    }
    Db.createWine wineForDb   

let parseAndImportWines (githubUserName: GitHubName) (wineNames:string) : Validation<string list, Wine list> =
    let lines = wineNames.Split (Environment.NewLine, StringSplitOptions.RemoveEmptyEntries) |> List.ofArray

    // Idee: Validieren jeder Zeile: Nicht länger als 3 Zeichen und enthält kein B
    
    // Rückgabe: Pro Zeile ein Ding oder ein Ding mit allen Zeilen?
    let hasLessThan3Chars (x:string) =
        if x.Length <= 3 then
            Success x
        else
            Failure ["ups: longer than 3"]

    let doesNotContainLetterB (x:string) =
        if x.Contains("B") then
            Failure ["ups: it has a B"]
        else
            Success x

    let validateName (wineName:string) : Validation<string list, string> =
        let makeStringWithTwoOpenParameters: Validation<string list, string -> string -> string> = (Success (fun s _ -> s))
        let makeStringWithOneOpenParameter = Validation.apply makeStringWithTwoOpenParameters (hasLessThan3Chars wineName)
        let validated = Validation.apply makeStringWithOneOpenParameter (doesNotContainLetterB wineName)

        // let y = id <!> hasLessThan3Chars wineName <* doesNotContainLetterB wineName
        validated

    let validateOne (wineName:string) : Validation<string list, Wine> =
        match (validateName wineName) with
        | Success name -> 
            createWine (createWineId (Guid.NewGuid ())) (createWineName name) (githubUserName)
            |> Success
        | Failure fs -> 
            Failure fs

    lines
    |> traverse validateOne
