#r "../packages/FAKE/tools/FakeLib.dll"

open Fake
open System

let webServerPath = "../src/server/" |> FullName
let webAppPath = "../src/app/" |> FullName
let webAppBuildPath = "../src/app/dist/" |> FullName
let deployDir = "./deploy" |> FullName

let mutable dotnetExePath = "dotnet"

let runDotnet workingDir args =
    let result =
        ExecProcess (fun info ->
            info.FileName <- "dotnet"
            info.WorkingDirectory <- workingDir
            info.Arguments <- args) TimeSpan.MaxValue
    if result <> 0 then failwithf "dotnet %s failed" args

let runYarn workingDir args =
    let result =
        ExecProcess (fun info ->
            info.FileName <- "yarn"
            info.WorkingDirectory <- workingDir
            info.Arguments <- args) TimeSpan.MaxValue
    if result <> 0 then failwithf "yarn %s failed" args

//-----Tasks-----
Target "InstallDotNetCore" (fun _ ->
    dotnetExePath <- DotNetCli.InstallDotNetSDK "2.0.3"
)

Target "Clean" ( fun _ -> 
    CleanDirs [deployDir; webAppBuildPath]
)

Target "InstallApp" (fun _ -> 
    runYarn webAppPath ""
)

Target "BuildApp" (fun _ ->
    runYarn webAppPath "build"
)

Target "BuildServer" (fun _ ->
    runDotnet webServerPath "build"
)

Target "TestApp" (fun _ -> 
    ()
)

Target "TestServer" (fun _ -> 
    ()
)

Target "PublishApp" (fun _ -> 
    let buildArgs = "publish -c Release -o \"" + FullName deployDir + "\""
    runDotnet webServerPath buildArgs
    CopyDir (deployDir + "/app") webAppBuildPath allFiles
    CopyDir (deployDir + "/demo-data") (webServerPath + "/demo-data/") allFiles
)

let dockerUser = "sp1nakr"
let dockerImageName = "winevue"
Target "CreateDockerImage" (fun _ -> 
    if String.IsNullOrEmpty dockerUser then
        failwithf "docker username not given."
    if String.IsNullOrEmpty dockerImageName then
        failwithf "docker image Name not given."
    let result =
        ExecProcess (fun info ->
            info.FileName <- "docker"
            info.UseShellExecute <- false
            info.Arguments <- sprintf "build -t %s/%s ." dockerUser dockerImageName) TimeSpan.MaxValue
    if result <> 0 then failwith "Docker build failed"
)

//Run for development
Target "Run" (fun _ ->
    let runWebServer = async { runDotnet webServerPath "watch run" }
    let runWebApp = async { runYarn webAppPath "start" }
    let openBrowser = async {
        System.Threading.Thread.Sleep(5000)
        Diagnostics.Process.Start("http://localhost:4000/#") |> ignore }

    Async.Parallel [| runWebServer; runWebApp; openBrowser |]
    |> Async.RunSynchronously
    |> ignore
)

//-----Pipeline definition-----

Target "Build" DoNothing

"Clean"
    ==> "InstallDotNetCore"
    ==> "InstallApp"
    ==> "BuildApp"
    ==> "BuildServer"
    ==> "TestApp"
    ==> "TestServer"
    ==> "PublishApp"
    ==> "CreateDockerImage"

"InstallApp"
    ==> "Run"

"BuildServer"
    ==>"Build"


RunTargetOrDefault "Build"