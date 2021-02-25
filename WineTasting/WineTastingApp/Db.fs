module Db

open System.Data.SQLite
open Dapper

/// Assumption: `GithubUserName` is unique across github -> can be our "primary key"
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
         
    [{ GithubUserName = "homer"; Name = "Homer Simpson" }]
    |> List.map (fun x -> connection.Execute(insertDummyUser, x))
    |> List.sum
    |> (fun recordsAdded -> printfn "Records added  : %d" recordsAdded)
    connection.Close()
    
let isKnownUser (connection : SQLiteConnection)  githubName : bool =
    
    let sql = "select count(*) from Users where GithubUserName = @githubName"
    let cnt = connection.ExecuteScalar<int>(sql, {| githubName = githubName |})
    cnt > 0
    

let save (user : User) : unit =
    
    connection.Open()
  
    if isKnownUser connection user.GithubUserName then
        ()
    else

        let insertUser =
            "insert into Users(GithubUserName, Name) " + 
            "values (@GithubUserName, @Name)"

        connection.Execute (insertUser, user) |> ignore

    connection.Close()
