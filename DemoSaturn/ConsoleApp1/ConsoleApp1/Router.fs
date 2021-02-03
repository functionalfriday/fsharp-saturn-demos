module Router

open Saturn
open Giraffe.Core
open UserViews
open Users

let browser = pipeline {
    plug acceptHtml
    plug putSecureBrowserHeaders
    plug fetchSession
    set_header "x-pipeline-type" "Browser"
}

let defaultView = router {
    get "/" (htmlView Hello.Views.index)
    get "/index.html" (redirectTo false "/")
    get "/default.html" (redirectTo false "/")
    get "/signin-github" (redirectTo false "/members-only")
}

let loggedInView = router {
    pipe_through loggedIn

    get "" (htmlView UserPage.layout)
    get "/admin" (isAdmin >=> htmlView AdminPage.layout)
}

let browserRouter = router {
    //not_found_handler (htmlView NotFound.layout) //Use the default 404 webpage
    pipe_through browser //Use the default browser pipeline
    forward "" defaultView //Use the default view
    forward "/members-only" loggedInView
}


let appRouter = router {
    forward "" browserRouter
}