module Database

open Domain
open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Table

let getDefault userName = async {
    printfn "Getting default wine list"
    let wineList = 
        { UserName = userName
          Wines = 
            [ { Id = "12321344"
                Name = "TestWine"
                Country = "France"
                Area = "Bordeaux"
                Type = "White"
                Fruit = "Chardonnay"
                Price = "100" };
              { Id = "123213123"
                Name = "TestWine2"
                Type = "White"
                Country = "France"
                Area = "Bordeaux"
                Fruit = "Pinor Noir"
                Price = "200" }]
        }
    return wineList
}

type AzureConnection = {connectionString: string; tableName: string}

type StorageConnection = 
    | AzureConnection of AzureConnection
    | DefaultConnection

let getWinesTable connection = async {
    let client = (CloudStorageAccount.Parse connection.connectionString).CreateCloudTableClient()
    let table = client.GetTableReference connection.tableName
    let rec createTableSafe() = async {
        try
        do! table.CreateIfNotExistsAsync() |> Async.AwaitTask |> Async.Ignore
        with _ ->
            do! Async.Sleep 5000
            return! createTableSafe() }
            
    do! createTableSafe()
    return table }

let convertWineType (wineString: string) = 
    let redWineStrings = ["Rødvin"]
    let whiteWineStrings = ["Hvitvin"]
    let sparklingWineStrings = ["Champagne, brut"; "Musserende vin"]

    match wineString with
    | wineString when (redWineStrings |> List.contains wineString) -> "red"
    | wineString when (whiteWineStrings |> List.contains wineString) -> "white"
    | wineString when (sparklingWineStrings |> List.contains wineString) -> "sparkling"
    | _ -> "unknown"


let getWineListFromDB connection userName = async {
    let! results = async {
        let! table = getWinesTable connection
        let query = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userName)           
        return! table.ExecuteQuerySegmentedAsync(TableQuery(FilterString = query), null) |> Async.AwaitTask }
    return
        { UserName = userName
          Wines =
            [ for result in results -> 
                { Id = result.Properties.["VinmonopoletId"].StringValue
                  Name = result.Properties.["Name"].StringValue
                  Price = string result.Properties.["Price"].StringValue
                  Country = string result.Properties.["Country"].StringValue
                  Area = string result.Properties.["Area"].StringValue
                  Type = convertWineType result.Properties.["Type"].StringValue
                  Fruit = string result.Properties.["Fruit"].StringValue } ] } 
}

                     
let getWineList connection userName = 
    match connection with 
    | DefaultConnection -> getDefault userName
    | AzureConnection conn -> getWineListFromDB conn userName


