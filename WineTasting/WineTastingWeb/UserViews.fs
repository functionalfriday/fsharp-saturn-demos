module UserViews

open Giraffe.GiraffeViewEngine
open Types

module AdminPage =
    let view = [
        h1 [] [rawText "I'm admin"]
    ]
    let layout = h1 [] [rawText "admin/admin"]

module UserPage =
    let view = 
        let wines : Wine seq = Wines.getAllWines ()

        let oneWine wine = tr [] [rawText "wine!"]

        [
            h1 [] [rawText "All existing wines"]
            table [] [
                thead [] [
                    tr [] [th [] [rawText "first column"]]
                ]
                tbody [] (
                        wines
                        |> List.ofSeq
                        |> List.map oneWine
                    )
            ]

            form [_action "members-only/wine"; _method "POST"] [
                label [_for "wine-name"] [rawText "Wine Name"]
                input [_id "wine-name"; _type "text"; _required; _name "wine-name"]

                button [_type "submit"] [rawText "Save wine"]
            ]
        ]

    let layout =
        html [] [
            head [] [
                link [_rel "stylesheet"; _href "assets/mycustom.css"]                
            ]
            body [] view    
        ]
