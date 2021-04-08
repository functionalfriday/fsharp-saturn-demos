module Wines

open Types
open System.Collections.Generic

let getAllWines () : IEnumerable<Wine> =
    let convertWine (w : WineForDb) : Wine =
        {
            Wine.WineId = createWineId <| System.Guid.Parse(w.WineId)
            Name = createWineName w.Name
            CreatedByGithubUserName = createGitHubName w.CreatedByGithubUserName
        }

    Db.getAllWines ()
    |> Seq.map convertWine 

let createWine (wine : Wine) =
    let wineForDb = {
        WineForDb.WineId = getWineIdString wine.WineId
        Name = getWineNameValue wine.Name
        CreatedByGithubUserName = getGitHubNameValue wine.CreatedByGithubUserName
    }
    Db.createWine wineForDb    