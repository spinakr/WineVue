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

type AzureConnection = 
    | AzureConnection of string

let getWinesTable (AzureConnection connectionString) = async {
    let client = (CloudStorageAccount.Parse connectionString).CreateCloudTableClient()
    let table = client.GetTableReference "winestest"
    let rec createTableSafe() = async {
        try
        do! table.CreateIfNotExistsAsync() |> Async.AwaitTask |> Async.Ignore
        with _ ->
            do! Async.Sleep 5000
            return! createTableSafe() }
            
    do! createTableSafe()
    return table }

let getWineListFromDB connectionString userName = async {
    let! results = async {
        let! table = getWinesTable connectionString
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

                     
let getWineList connectionString userName = 
    match connectionString with
        | Some cs -> getWineListFromDB cs userName
        | None -> getDefault userName


