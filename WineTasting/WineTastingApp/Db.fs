module Db

open System
open System.Collections.Generic
open System.Data.SQLite
open Dapper

type User = {
    UserId : string
    GithubId : string
}

let createDb =
    // Initialize connectionstring
    let databaseFilename = "app.sqlite"
    let connectionStringFile = sprintf "Data Source=%s;Version=3;" databaseFilename  

    // Create database
    SQLiteConnection.CreateFile(databaseFilename)

    // Open connection
    let connection = new SQLiteConnection(connectionStringFile)
    connection.Open()

    // Create table structure
    let structureSql =
        "create table Users (UserId text, GithubId text)"

    let structureCommand = new SQLiteCommand(structureSql, connection)
    structureCommand.ExecuteNonQuery() |> ignore
    
    let insertDummyUser =
        "insert into Users(UserId, GithubId) " + 
        "values (@userId, @githubId)"
         
    [{ UserId = "123"; GithubId = "456" }]
    |> List.map (fun x -> connection.Execute(insertDummyUser, x))
    |> List.sum
    |> (fun recordsAdded -> printfn "Records added  : %d" recordsAdded)
    