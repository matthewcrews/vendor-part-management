module VendorDomain

type VendorId = VendorId of string
type VendorPartNumber = VendorPartNumber of string
type UnitCost = UnitCost of decimal
type UnitOfMeasure = UnitOfMeasure of string
type PrePakMin = PrePakMin of decimal
type Status =
    | Active
    | InActive
type InventoryId = InventoryId of string
type Asin = Asin of string
type UPC = UPC of string
type EAN = EAN of string
type GTIN = GTIN of string
type AlternateId =
    | Asin of Asin
    | UPC of UPC
    | EAN of EAN
    | GTIN of GTIN

type PriceFloor = PriceFloor of decimal

type Description = Description of string

type AmazonItem = {
    Asin : Asin
    Description : Description
    AlternateIds : AlternateId list
}

type StockItem = {
    InventoryId : InventoryId
    Description : Description
    AlternateIds : AlternateId list
}

type UpcMap = Map<UPC, StockItem list>
type EanMap = Map<EAN, StockItem list>
type AsinMap = Map<Asin, StockItem list>
type GtinMap = Map<GTIN, StockItem list>

type VendorPartPrice = {
    UnitOfMeasure : UnitOfMeasure
    UnitCost : UnitCost
}

type PartData = {
    VendorPartNumber : VendorPartNumber
    Description : Description
    Status : Status
    PrePakMin : PrePakMin
    Price : VendorPartPrice
    AlternateIds : AlternateId list
}

type PossibleStockItem = {
    PartData : PartData
    StockItems : StockItem list
}

type AwaitingAsinMatch = {
    PartData : PartData
    Asins : Asin list
}

type AssignedAsin = {
    PartData : PartData
    Asin : Asin
}

type ProfitAnalyzed = {
    PartData : PartData
    Asin : Asin
    PriceFloor : PriceFloor
}

type Unsynced = {
    PartData : PartData
    InventoryId : InventoryId
}

type Synced = Unsynced

type PartState =
| NewPart of PartData
| PossibleStockItem of PossibleStockItem
| NeedingAsinMaches of PartData
| NoAsinMatches of PartData
| AwaitingAsinMatch of AwaitingAsinMatch
| AssignedAsin of AssignedAsin
| ProfitAnalyzed of ProfitAnalyzed
| Unsynced of Unsynced
| Synced of Synced


type VendorPart = {
    VendorPartNumber : VendorPartNumber
    State : PartState
}

type Vendor = {
    VendorId : VendorId
    Description : Description
    Catalog : VendorPart list
}

// type UpcMatcher = UpcMap -> PartData -> InventoryItem list
// type EanMatcher = EanMap -> PartData -> InventoryItem list
// type GtinMatcher = GtinMap -> PartData -> InventoryItem list
// type AsinMatcher = AsinMap -> PartData -> InventoryItem list

