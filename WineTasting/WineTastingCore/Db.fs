module Db

open System
open System.Data.SQLite
open Dapper
open Types

let databaseFilename = "app.sqlite"
let baseFolder = "../../.." // TODO maybe replace with something like `AppDomain.CurrentDomain.BaseDirectory`?
let dbFile = $"%s{baseFolder}/%s{databaseFilename}"
let connectionStringFile = $"Data Source=%s{dbFile};Version=3;"  

// Create database
SQLiteConnection.CreateFile(dbFile)

// Open connection
let connection = new SQLiteConnection(connectionStringFile)

let createDb =
    connection.Open()

    // Create table structure
    // TODO Maybe add Primary Key attribute to `GithubUserName` column? Does Sqlite support this?
    let structureSql =
        "create table Users (GithubUserName text, Name text);
         create table Wines (WineId text, Name text, CreatedByGithubUserName text);"

    let structureCommand = new SQLiteCommand(structureSql, connection)
    structureCommand.ExecuteNonQuery() |> ignore
    
    let insertDummyUser =
        "insert into Users(GithubUserName, Name) " + 
        "values (@githubUserName, @name)"
         
    [{ UserForDb.GithubUserName = "homer"; Name = "Homer Simpson" }]
    |> List.map (fun x -> connection.Execute(insertDummyUser, x))
    |> List.sum
    |> (fun recordsAdded -> printfn $"Records added  : %d{recordsAdded}")
    connection.Close()
    
let isKnownUser (connection : SQLiteConnection) (GitHubName githubName) : bool =
    let sql = "select count(*) from Users where GithubUserName = @githubName"
    let cnt = connection.ExecuteScalar<int>(sql, {| githubName = githubName |})
    cnt > 0

let saveUser (user : User) : unit =
    connection.Open()

    let { User.GithubUserName = githubUserName; Name = name } = user

    if isKnownUser connection githubUserName then
        ()
    else
        let insertUser =
            "insert into Users(GithubUserName, Name) " + 
            "values (@GithubUserName, @Name)"

        let nameForDb =
            match name with
            | Some s -> getFullNameValue s
            | _ -> ""

        let userForDb = {
            UserForDb.GithubUserName = getGitHubNameValue githubUserName
            Name = nameForDb 
        }

        connection.Execute (insertUser, userForDb) |> ignore

    connection.Close()

(*
    +--------+----------+-------------------------+
    | WineId | WineName | CreatedByGithubUserName |
    +--------+----------+-------------------------+
*)
let isKnownWine (connection : SQLiteConnection) (wineId : string) : bool =
    let sql = "select count(*) from Wines where WineId = @wineId"
    let cnt = connection.ExecuteScalar<int>(sql, {| wineId = wineId |})
    cnt > 0

(*
    +--------+--------+----------------+
    | Rating | WineId | GithubUserName | 
    +--------+--------+----------------+
*)
let createWine (wine : WineForDb) : unit =
    connection.Open()

    if isKnownWine connection wine.WineId then
        ()
    else
        let insert =
            "insert into Wines(WineId, Name, CreatedByGithubUserName) " + 
            "values (@WineId, @Name, @CreatedByGithubUserName)"
        
        connection.Execute (insert, wine) |> ignore

    connection.Close()

let getAllWines () =
    let sql = "select WineId, Name, CreatedByGithubUserName from Wines"
    connection.Query<WineForDb> sql
