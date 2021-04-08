module Wines

// NOTE: add `() : ...` so that the expression is not only evaluated once
let getAllWines () =
    let wines = Db.getAllWines ()
    wines
    
    