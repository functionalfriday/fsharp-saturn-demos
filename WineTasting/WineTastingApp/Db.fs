module Db

open System.Data.SQLite
open Dapper
open Microsoft.Extensions.Logging

type User = {
    GithubUserName : string
    Name : string
}

let databaseFilename = "app.sqlite"
let connectionStringFile = sprintf "Data Source=%s;Version=3;" databaseFilename  

// Create database
SQLiteConnection.CreateFile(databaseFilename)

// Open connection
let connection = new SQLiteConnection(connectionStringFile)


let createDb =
//    let connection = new SQLiteConnection(connectionStringFile)
    connection.Open()

    // Create table structure
    let structureSql =
        "create table Users (GithubUserName text, Name text)"

    let structureCommand = new SQLiteCommand(structureSql, connection)
    structureCommand.ExecuteNonQuery() |> ignore
    
    let insertDummyUser =
        "insert into Users(GithubUserName, Name) " + 
        "values (@githubUserName, @name)"
         
    [{ GithubUserName = "homer"; Name = "Homer Simpson" }]
    |> List.map (fun x -> connection.Execute(insertDummyUser, x))
    |> List.sum
    |> (fun recordsAdded -> printfn "Records added  : %d" recordsAdded)
    connection.Close()
    
let save (logger: ILogger) fullName ghName  : Result<string, string> =
    
    logger.LogInformation("starting save...")
    
    connection.Open()
    
    logger.LogInformation("1")
    
    let countUsersWithGithubName ghName : int =
        logger.LogInformation("2")
        let sql = "select count(*) from Users where GithubUserName = @ghName"
        logger.LogInformation("3")
        let cnt = connection.ExecuteScalar<int>(sql, {| ghName = ghName |})
        logger.LogInformation("4")
        cnt
    
    let isKnownUser = countUsersWithGithubName ghName > 0
    
    if isKnownUser then
        connection.Close()
        Ok "user already in db"
    else
        logger.LogInformation("5")
        let insertUser =
            "insert into Users(GithubUserName, Name) " + 
            "values (@ghName, @fullName)"
        logger.LogInformation("6")
        let i = connection.Execute (insertUser, {| ghName = ghName; fullName = fullName |})
        logger.LogInformation("7")
        connection.Close()
        Ok (i.ToString())
        
    