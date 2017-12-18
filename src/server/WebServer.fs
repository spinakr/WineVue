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
                pathRegex @"/(.*)\.(css|png|gif|jpg|js|map|ico|svg)" >=> Files.browseHome
                pathRegex @"/assets/fonts/(.*)\.(woff|woff2|ttf)" >=> Files.browseHome
                
                pathScan "/api/comments/%s" (fun (id) -> 
                 context (fun c -> Comments.getComment connectionString id c))

                path "/api/wines/inventory" >=> Wines.getAllWines connectionString "instock"
                path "/api/wines/wishlist" >=> Wines.getAllWines connectionString "shoppinglist"
                path "/api/wines/archive" >=> Wines.getAllWines connectionString "archive"
                pathScan "/api/wines/%s" (fun (id) -> 
                 context (fun c -> Wines.getWine connectionString c id))
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
            bindings = [ HttpBinding.createSimple HTTP "0.0.0.0" 8080 ]
        }
    
    startWebServer config app
