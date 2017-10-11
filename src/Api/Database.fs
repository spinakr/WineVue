module Database

open Domain

let getWineList username = 
    let wineList = 
        { UserName = "Anders"
          Wines = 
            [ { Name = "TestWine"
                Fruit = "Chardonnay"
                Price = 1 } ]
    }
    wineList




