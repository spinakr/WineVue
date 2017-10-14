namespace Domain

type Wine =
    { 
        Name : string
        Fruit : string
        Price : string
    }

type WineList =
    {
        UserName : string
        Wines : Wine list
    }
