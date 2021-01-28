namespace Hello

open Saturn
open Giraffe.ResponseWriters

module Controller =
    let indexAction =
        htmlView (Views.index)

    let aboutAction =
        htmlView (Views.about)
    
    let helloView = router {
        get "/" indexAction
        get "/about" aboutAction
    }