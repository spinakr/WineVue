module Api.Program

open Database
open System

[<EntryPoint>]
let main args =
    let args = Array.toList args
    let connectionString = 
        args 
        |> List.tryFind(fun arg -> arg.StartsWith "DefaultEndpointsProtocol=")
        |> (fun x -> 
            match x with
            |Some _ -> x
            |None ->  match Environment.GetEnvironmentVariable("WINEVUE_AZURE_CONNECTION") with
                      | null -> None
                      | value -> Some value)
    
    let tableName = 
        match Environment.GetEnvironmentVariable("WINEVUE_TABLE_NAME") with
        | null -> "winestest"
        | value -> value

    let connection = 
        match connectionString with
        | Some conn -> (AzureConnection {connectionString=conn; tableName=tableName})
        | None -> DefaultConnection

    WebServer.start connection
    0

