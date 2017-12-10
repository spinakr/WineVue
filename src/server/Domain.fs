namespace Domain

type Wine =
    { 
        Id: string
        VinmonopoletId: string
        Status: string
        Name : string
        Fruit : string
        Type: string
        Country: string
        Area: string
        Price : string
        Producer : string
    }

type WineList =
    {
        UserName : string
        Wines : Wine list
    }
