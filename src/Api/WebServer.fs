module Api.WebServer

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.Writers

let start =
    let app = 
        choose [
            GET >=> choose [
                path "/api/wines" >=> Wines.getAllWines
            ]
            POST >=> choose [
                path "/api/wines/" >=> Wines.addNewWine
            ]
        ]
    let config =
        { defaultConfig with 
            bindings = [ HttpBinding.createSimple HTTP "0.0.0.0" 8083 ]
        }
    
    startWebServer config app
