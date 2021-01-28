namespace Hello

open Saturn
open Giraffe.ResponseWriters

module Controller =
    let indexAction =
        htmlView (Views.index)

    let helloView = router {
        get "/" indexAction
    }