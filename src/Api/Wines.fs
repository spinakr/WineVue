module Api.Wines

open Suave
open Suave.Filters
open Newtonsoft.Json


let mutable wines = ["Wine1"; "Wine4"]

let getAllWines = 
    Successful.OK (JsonConvert.SerializeObject(wines))

let addNewWine = 
    List.append wines ["NewWine"] |> ignore
    Successful.OK ""
