module UserViews

open Giraffe.GiraffeViewEngine
open System.Collections.Generic
open Types

module AdminPage =
    let view = [
        h1 [] [rawText "I'm admin"]
    ]
    let layout = h1 [] [rawText "admin/admin"]

module UserPage =
    let view = 
        let wines : IEnumerable<Wine> = Wines.getAllWines ()

        let oneWine wine = tr [] [rawText "wine!"]

        [
            h1 [] [rawText "All wines"]
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
        ]

    let layout =
        html [] [
            head [] [
                link [_rel "stylesheet"; _href "assets/mycustom.css"]                
            ]
            body [] view    
        ]
