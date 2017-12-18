module Api.Comments

open Suave
open Newtonsoft.Json

let getComment connectionString vinmonopoletId (ctx: HttpContext) =
    let wineComments = (Database.getComments connectionString vinmonopoletId) |> Async.RunSynchronously
    Successful.OK (JsonConvert.SerializeObject(wineComments))
