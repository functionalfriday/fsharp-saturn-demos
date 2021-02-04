namespace Index

open Giraffe.GiraffeViewEngine

module Views =
  let index =
    div [] [
        h2 [] [rawText "Hello from Saturn!"]
        a [ _href "https://google.de" ] [ encodedText "todo" ]
    ]
    
  let about =
    div [] [
        h2 [] [rawText "About!"]
    ]