namespace Domain

type Comment = 
    {
        VinmonopoletId: string
        Occation: string
        ConsumptionDate: string
        Note: string
    }

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
        Occation: string
        ConsumptionDate: string
        Note: string
    }

type WineList =
    {
        UserName : string
        Wines : Wine list
    }
