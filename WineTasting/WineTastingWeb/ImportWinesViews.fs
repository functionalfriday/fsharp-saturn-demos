module ImportWinesViews

open Giraffe.GiraffeViewEngine

let view =
    div [] [
        h1 [] [rawText "IMPORT ALL THE WINE!"]

        form [
            _style "display: flex; flex-direction: column;"
            _action "bulk-import"
            _method "POST"] [
                label [_for "WineNames"] [rawText "Wine Names"]
                textarea [_id "WineNames"; _name "WineName"; _cols "80"; _rows "5"; _required;] []

                button [_type "submit"] [rawText "Import wines"]
            ]
    ]