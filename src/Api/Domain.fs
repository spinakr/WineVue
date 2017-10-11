namespace Domain

type Wine =
    { 
        Name : string
        Fruit : string
        Price : int
    }

type WineList =
    {
        UserName : string
        Wines : Wine list
    }
