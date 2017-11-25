module Api.Program

[<EntryPoint>]
let main args =
    let args = Array.toList args
    let connectionString = 
        args 
        |> List.tryFind(fun arg -> arg.StartsWith "DefaultEndpointsProtocol=")
        |> Option.map(Database.AzureConnection)

    WebServer.start connectionString
    0

