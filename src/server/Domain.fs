namespace Domain

type Wine =
    { 
        Id: string
        Name : string
        Fruit : string
        Type: string
        Country: string
        Area: string
        Price : string
    }

type WineList =
    {
        UserName : string
        Wines : Wine list
    }