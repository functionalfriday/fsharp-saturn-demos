module WineValidation

open System
open FSharpPlus
open FSharpPlus.Data

type Error =
    | MustNotBeEmpty
    | IsTooLong

type ErrorAndItem = (Error * string)


let isNotEmpty (s: string) : Validation<ErrorAndItem list, string> =
    if String.IsNullOrWhiteSpace s then
        Failure([ (MustNotBeEmpty, s) ])
    else
        Success s


let isNotTooLong (s: string) : Validation<ErrorAndItem list, string> =
    if length s > 80 then
        Failure([ (IsTooLong, s) ])
    else
        Success s

let validateLength (s: string) : Validation<ErrorAndItem list, string> = id <!> isNotEmpty s <* isNotTooLong s

let validateContent (s: String) : Validation<ErrorAndItem list, string> = Success s // todo

let validate (s: string) : Validation<ErrorAndItem list, string> =
    id <!> validateLength s <* validateContent s


let import (xs: string list) = map validate xs
let importT (xs: string list) = traverse validate xs
let importS (xs: string list) = sequence (map validate xs)
