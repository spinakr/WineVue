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
            |None -> Some (Environment.GetEnvironmentVariable("WINEVUE_AZURE_CONNECTION")))
    
    let tableName = 
        args 
        |> List.tryFind(fun arg -> arg.StartsWith "AzureTableName=")
        |> Option.map(fun arg ->
                arg.Substring "AzureTableName=".Length)
        |> (fun n -> 
            match n with 
            |Some name -> name
            |None -> "winestest")

    let connection = 
        match connectionString with
        | Some conn -> (AzureConnection {connectionString=conn; tableName=tableName})
        | None -> DefaultConnection

    WebServer.start connection
    0

