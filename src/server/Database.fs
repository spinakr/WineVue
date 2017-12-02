module Database

open System.IO
open Domain
open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Table

let convertWineType (wineString: string) = 
    let redWineStrings = ["Rødvin"]
    let whiteWineStrings = ["Hvitvin"]
    let sparklingWineStrings = ["Champagne, brut"; "Musserende vin"; "Champagne"]

    match wineString with
    | wineString when (redWineStrings |> List.contains wineString) -> "red"
    | wineString when (whiteWineStrings |> List.contains wineString) -> "white"
    | wineString when (sparklingWineStrings |> List.contains wineString) -> "sparkling"
    | _ -> "unknown"

let filePath = "./demo-data/demo-wines.csv"

let wines = 
    File.ReadAllLines(filePath)
    |> Array.map (fun s -> s.Split(';')) 
    |> fun file -> file.[1..]
    |> Array.map(fun line -> 
        {Id=line.[1]; Name=line.[2]; Country=line.[8]; Area=line.[9]; Type=convertWineType line.[6]; Fruit="90% Cabernet Sauvignon, 10% Cabernet Franc"; Price=line.[11]; Producer=line.[7]})
    |> Array.toList

let getDefault userName = async {
    let wineList = 
        { UserName = userName
          Wines = wines
        }
    return wineList
}

let getWineDefault id = async {
    let wine = wines |> Seq.find (fun x -> (x.Id = id))
    return wine
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

let mapWineToEntity (result: DynamicTableEntity) = 
    { Id = result.Properties.["VinmonopoletId"].StringValue
      Name = result.Properties.["Name"].StringValue
      Price = string result.Properties.["Price"].StringValue
      Country = string result.Properties.["Country"].StringValue
      Area = string result.Properties.["Area"].StringValue
      Type = convertWineType result.Properties.["Type"].StringValue
      Producer = convertWineType result.Properties.["Producer"].StringValue
      Fruit = string result.Properties.["Fruit"].StringValue } 

let mapWinesToEntity (results: TableQuerySegment) = 
    [ for result in results ->  mapWineToEntity result ]


let getWineFromDB connection id = async {
    let! results = async {
        let! table = getWinesTable connection
        let query = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id)           
        return! table.ExecuteQuerySegmentedAsync(TableQuery(FilterString = query), null) |> Async.AwaitTask }
    return mapWineToEntity (Seq.exactlyOne results)
}

let getWineListFromDB connection userName = async {
    let! results = async {
        let! table = getWinesTable connection
        let query = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userName)           
        return! table.ExecuteQuerySegmentedAsync(TableQuery(FilterString = query), null) |> Async.AwaitTask }
    return 
        { UserName = userName
          Wines = mapWinesToEntity results } 
}

                     
let getWineList connection userName = 
    match connection with 
    | DefaultConnection -> getDefault userName
    | AzureConnection conn -> getWineListFromDB conn userName

let getWine connection id = 
    match connection with 
    | DefaultConnection -> getWineDefault id
    | AzureConnection conn -> getWineFromDB conn id

