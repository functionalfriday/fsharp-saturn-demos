namespace Hello

open Giraffe.ViewEngine
open Saturn

module Views =
  let index =
    div [] [
        h2 [] [rawText "Hello from Saturn!"]
    ]
    
  let about =
    div [] [
        h2 [] [rawText "About!"]
    ]