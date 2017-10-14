module Api.Program

[<EntryPoint>]
let main args =
    let args = Array.toList args
    let connectionString = 
        args 
        |> List.tryFind(fun arg -> arg.StartsWith "DefaultEndpointsProtocol=")
        |> Option.map(fun arg -> Database.AzureConnection arg)

    WebServer.start connectionString
    0

