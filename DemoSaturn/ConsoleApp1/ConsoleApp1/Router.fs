module Router

open Microsoft.AspNetCore.Http
open Saturn
open Giraffe.Core
open Giraffe.ResponseWriters

let browser = pipeline {
    plug acceptHtml
    plug putSecureBrowserHeaders
//    plug fetchSession
    set_header "x-pipeline-type" "Browser"
}

let browserRouter = router {
    pipe_through browser
    forward "/hello" Hello.Controller.helloView
}

//let appRouter : HttpFunc -> HttpContext -> HttpFuncResult =
//    text "hello 12"
