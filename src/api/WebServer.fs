module Api.WebServer

open Suave
open Suave.Filters
open Suave.Operators
open System.IO

let start connectionString =
    let app = 
        choose [
            GET >=> choose [
                path "/" >=> Files.browseFileHome "index.html"
                pathRegex @"/(.*)\.(css|png|gif|jpg|js|map)" >=> Files.browseHome
                path "/api/wines/inventory" >=> Wines.getAllWines connectionString
                path "/api/wines/wishlist" >=> Wines.getAllWines connectionString
                path "/api/wines/archive" >=> Wines.getAllWines connectionString
            ]
            POST >=> choose [
                path "/api/wines/" >=> Wines.addNewWine
            ]
        ]

    let appPath =
        let devPath = Path.Combine("..","app", "dist")
        let prodPath = Path.Combine(".","app")
        if Directory.Exists devPath
        then (devPath |> Path.GetFullPath)
        else (prodPath |> Path.GetFullPath)


    let config =
        { defaultConfig with 
            homeFolder = Some appPath
            bindings = [ HttpBinding.createSimple HTTP "0.0.0.0" 8083 ]
        }
    
    startWebServer config app