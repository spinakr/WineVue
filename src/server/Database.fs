module Database

open Domain
open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Table

let getDefault userName = async {
    printfn "Getting default wine list"
    let wineList = 
        { UserName = userName
          Wines = 
            [ { Name = "TestWine"
                Fruit = "Chardonnay"
                Price = "100" };
              { Name = "TestWine2"
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

let getWineListFromDB connection userName = async {
    let! results = async {
        let! table = getWinesTable connection
        let query = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userName)           
        return! table.ExecuteQuerySegmentedAsync(TableQuery(FilterString = query), null) |> Async.AwaitTask }
    return
        { UserName = userName
          Wines =
            [ for result in results -> 
                { Name = result.Properties.["Name"].StringValue
                  Price = string result.Properties.["Price"].StringValue
                  Fruit = string result.Properties.["Fruit"].StringValue } ] } 
}

                     
let getWineList connection userName = 
    match connection with 
    | DefaultConnection -> getDefault userName
    | AzureConnection conn -> getWineListFromDB conn userName


