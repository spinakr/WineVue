module Api.Wines

open Suave
open Newtonsoft.Json

let getAllWines connectionString (ctx: HttpContext) = async {
    let! wineList = Database.getWineList connectionString "IngridAnders"
    return! Successful.OK (JsonConvert.SerializeObject(wineList)) ctx
}

let getWine connectionString (ctx: HttpContext) id = 
    let wine = (Database.getWine connectionString id) |> Async.RunSynchronously
    Successful.OK (JsonConvert.SerializeObject(wine))

let addNewWine (ctx: HttpContext) = async {
    printfn "Add new wine invoked!"
    return! Successful.OK "" ctx
}
