
module UserViews

open Giraffe.ViewEngine


module AdminPage =
    let view = [
        h1 [] [rawText "I'm admin"]
    ]
    let layout = h1 [] [rawText "admin/admin"]

module UserPage =
    let view = [
        h1 [] [rawText "I'm logged user"]
    ]
    let layout = h1 [] [rawText "admin/user"]