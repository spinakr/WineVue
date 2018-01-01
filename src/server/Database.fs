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
        {Id=line.[1]; VinmonopoletId=line.[4]; Status=line.[3]; Name=line.[2]; Country=line.[8]; Area=line.[9]; Type=convertWineType line.[6]; Fruit="90% Cabernet Sauvignon, 10% Cabernet Franc"; Price=line.[11]; Producer=line.[7]; Occation=line.[17]; Note=line.[16]; ConsumptionDate=line.[15]})
    |> Array.toList

let getDefault userName status = async {
    let wineList = 
        { UserName = userName
          Wines = wines |> List.filter (fun x -> x.Status = status) 
        }
    return wineList
}

let getWineDefault id = async {
    let wine = wines |> Seq.find (fun x -> (x.Id = id))
    return wine
}

let getCommentsDefault vinmonopoletId = async {
    let comments = 
        wines 
        |> Seq.filter (fun x -> (x.VinmonopoletId = vinmonopoletId))
        |> Seq.map (fun wine -> {Id = wine.VinmonopoletId; Occation = wine.Occation; Note=wine.Note; ConsumptionDate=wine.ConsumptionDate})
    return comments
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

let getPropertyFromTableEntity propName (result: DynamicTableEntity) =
    match result.Properties.TryGetValue propName with
      | true, value -> value.StringValue
      | _           -> ""

let getDatePropertyFromTableEntity propName (result: DynamicTableEntity) =
    let stringValue = getPropertyFromTableEntity propName result 
    match System.DateTime.TryParse stringValue with
      | true, value -> value.ToString "dd.MM.yyyy"
      | _           -> ""

let mapWineToEntity (result: DynamicTableEntity) = 
    { Id = result.RowKey
      VinmonopoletId = result |> getPropertyFromTableEntity "VinmonopoletId"
      Name = result |> getPropertyFromTableEntity "Name"
      Price =  result |> getPropertyFromTableEntity "Price"
      Country = result |> getPropertyFromTableEntity "Country"
      Area =  result |> getPropertyFromTableEntity "Area"
      Type =  convertWineType (result |> getPropertyFromTableEntity "Type")
      Producer = result |> getPropertyFromTableEntity "Producer"
      Status =  result |> getPropertyFromTableEntity "Status"
      Occation = result |> getPropertyFromTableEntity "Occation"
      ConsumptionDate = result |> getPropertyFromTableEntity "ConsumptionDate"
      Note = result |> getPropertyFromTableEntity "Note" 
      Fruit = result |> getPropertyFromTableEntity "Fruit" } 


let mapWinesToEntity (results: TableQuerySegment) = 
    [ for result in results ->  mapWineToEntity result ]


let getWineFromDB connection id = async {
    let! results = async {
        let! table = getWinesTable connection
        let query = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id)           
        return! table.ExecuteQuerySegmentedAsync(TableQuery(FilterString = query), null) |> Async.AwaitTask }
    return mapWineToEntity (Seq.exactlyOne results)
}

let getWineListFromDB connection userName status = async {
    let! results = async {
        let! table = getWinesTable connection
        let query = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userName)           
        return! table.ExecuteQuerySegmentedAsync(TableQuery(FilterString = query), null) |> Async.AwaitTask }
    return 
        { UserName = userName
          Wines = (mapWinesToEntity results) |> List.filter (fun x -> x.Status = status) } 
}

let getCommentsFromDB connection vinmonopoletId = async {
    let! results = async {
        let! table = getWinesTable connection
        let query = TableQuery.GenerateFilterCondition("VinmonopoletId", QueryComparisons.Equal, vinmonopoletId)           
        return! table.ExecuteQuerySegmentedAsync(TableQuery(FilterString = query), null) |> Async.AwaitTask }
    return 
        results
        |> Seq.map (fun result -> 
            { Id = result.RowKey
              ConsumptionDate = result |> getDatePropertyFromTableEntity "ConsumptionDate"
              Note = result |> getPropertyFromTableEntity "Note"
              Occation = result |> getPropertyFromTableEntity "Occation" } )
        |> Seq.filter (fun x -> x.ConsumptionDate <> "")
}
                     
let getWineList connection userName status = 
    match connection with 
    | DefaultConnection -> getDefault userName status
    | AzureConnection conn -> getWineListFromDB conn userName status

let getWine connection id = 
    match connection with 
    | DefaultConnection -> getWineDefault id
    | AzureConnection conn -> getWineFromDB conn id

let getComments connection vinmonopoletId = 
    match connection with 
    | DefaultConnection -> getCommentsDefault vinmonopoletId
    | AzureConnection conn -> getCommentsFromDB conn vinmonopoletId
