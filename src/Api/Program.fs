module Api.Program

[<EntryPoint>]
let main args =
    printfn "Web server started"

    WebServer.start
        
    0

