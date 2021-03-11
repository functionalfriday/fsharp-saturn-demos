module Db

open System.Data.SQLite
open Dapper
open Types

let databaseFilename = "app.sqlite"
let connectionStringFile = sprintf "Data Source=%s;Version=3;" databaseFilename  

// Create database
SQLiteConnection.CreateFile(databaseFilename)

// Open connection
let connection = new SQLiteConnection(connectionStringFile)

let createDb =
    connection.Open()

    // Create table structure
    // TODO Maybe add Primary Key attribute to `GithubUserName` column? Does Sqlite support this?
    let structureSql =
        "create table Users (GithubUserName text, Name text)"

    let structureCommand = new SQLiteCommand(structureSql, connection)
    structureCommand.ExecuteNonQuery() |> ignore
    
    let insertDummyUser =
        "insert into Users(GithubUserName, Name) " + 
        "values (@githubUserName, @name)"
         
    [{ UserForDb.GithubUserName = "homer"; Name = "Homer Simpson" }]
    |> List.map (fun x -> connection.Execute(insertDummyUser, x))
    |> List.sum
    |> (fun recordsAdded -> printfn "Records added  : %d" recordsAdded)
    connection.Close()
    
let isKnownUser (connection : SQLiteConnection) (GitHubName githubName) : bool =
    let sql = "select count(*) from Users where GithubUserName = @githubName"
    let cnt = connection.ExecuteScalar<int>(sql, {| githubName = githubName |})
    cnt > 0

let save (user : User) : unit =
    connection.Open()

    let { User.GithubUserName = githubUserName; Name = name } = user

    if isKnownUser connection githubUserName then
        ()
    else
        let insertUser =
            "insert into Users(GithubUserName, Name) " + 
            "values (@GithubUserName, @Name)"

        // TODO Code golf
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
