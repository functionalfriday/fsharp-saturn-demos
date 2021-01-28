namespace Hello

open Giraffe.GiraffeViewEngine
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