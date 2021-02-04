
module UserViews

open Giraffe.GiraffeViewEngine

module AdminPage =
    let view = [
        h1 [] [rawText "I'm admin"]
    ]
    let layout = h1 [] [rawText "admin/admin"]

module UserPage =
    let view = [
        h1 [] [rawText "I'm logged user"]
    ]
    let layout =
        html [] [
            head [] [
                link [_rel "stylesheet"; _href "assets/mycustom.css"]                
            ]
            body [] [
                h1 [] [rawText "admin/user"]    
            ]
        ]
