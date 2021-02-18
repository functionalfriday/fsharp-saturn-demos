module Db

open System
open System.Collections.Generic
open System.Data.SQLite
open Dapper

type User = {
    UserId : string
    GithubId : string
    Name : string
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
        "create table Users (UserId text, GithubId text, Name text)"

    let structureCommand = new SQLiteCommand(structureSql, connection)
    structureCommand.ExecuteNonQuery() |> ignore
    
    let insertDummyUser =
        "insert into Users(UserId, GithubId, Name) " + 
        "values (@userId, @githubId, @name)"
         
    [{ UserId = "123"; GithubId = "456"; Name = "Homer Simpson" }]
    |> List.map (fun x -> connection.Execute(insertDummyUser, x))
    |> List.sum
    |> (fun recordsAdded -> printfn "Records added  : %d" recordsAdded)
    