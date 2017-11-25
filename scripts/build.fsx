open System.Runtime.InteropServices
#r "../packages/FAKE/tools/FakeLib.dll"

open Fake
open System

let webServerPath = "../src/api/" |> FullName
let webAppPath = "../src/app/" |> FullName
let webAppBuildPath = "../src/app/dist/" |> FullName
let buildPath = "./build" |> FullName

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

Target "Clean" ( fun _ -> 
    CleanDirs [buildPath; webAppBuildPath]
)

Target "BuildWebServer" (fun _ ->
    let buildArgs = "build -o " + buildPath
    runDotnet webServerPath buildArgs
)

Target "InstallWebApp" (fun _ -> 
    runYarn webAppPath ""
)

Target "BuildWebApp" (fun _ ->
    runYarn webAppPath "build"
    CopyDir (buildPath + "/app") webAppBuildPath allFiles
)

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
    ==> "BuildWebServer"
    ==> "InstallWebApp"
    ==> "BuildWebApp"
    ==> "Build"

"InstallWebApp"
    ==> "Run"


RunTargetOrDefault "Build"