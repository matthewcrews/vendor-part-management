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

type Description = Description of string

type AmazonItem = {
    Asin : Asin
    Description : Description
    AlternateIds : AlternateId list
}

type InventoryItem = {
    InventoryId : InventoryId
    Description : Description
    AlternateIds : AlternateId list
}

type UpcMap = Map<UPC, InventoryItem list>
type EanMap = Map<EAN, InventoryItem list>
type AsinMap = Map<Asin, InventoryItem list>
type GtinMap = Map<GTIN, InventoryItem list>

type VendorPartPrice = {
    UnitOfMeasure : UnitOfMeasure
    UnitCost : UnitCost
}

type VendorPart = {
    VendorPartNumber : VendorPartNumber
    Description : Description
    Status : Status
    PrePakMin : PrePakMin
    Price : VendorPartPrice
    AlternateIds : AlternateId list
}

type Vendor = {
    VendorId : VendorId
    Description : Description
    Catalog : VendorPart list
}

type UpcMatcher = UpcMap -> VendorPart -> InventoryItem list
type EanMatcher = EanMap -> VendorPart -> InventoryItem list
type GtinMatcher = GtinMap -> VendorPart -> InventoryItem list
type AsinMatcher = AsinMap -> VendorPart -> InventoryItem list

