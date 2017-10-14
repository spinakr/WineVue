module Api.Wines

open Suave
open Suave.Filters
open Newtonsoft.Json



let getAllWines connectionString (ctx: HttpContext)= async {
    printfn "Get all wines invoked!"
    let! wineList = Database.getWineList connectionString "IngridAnders"
    return! Successful.OK (JsonConvert.SerializeObject(wineList)) ctx
}

let addNewWine (ctx: HttpContext) = async {
    printfn "Add new wine invoked!"
    return! Successful.OK "" ctx
}
