module VendorDomain

type VendorId = VendorId of string
type VendorPartNumber = VendorPartNumber of string
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

type AmazonItem = {
    Asin : Asin
    AlternateIds : AlternateId list
}

type InventoryItem = {
    InventoryId : InventoryId
    AlternateIds : AlternateId list
}

type UpcMap = Map<UPC, InventoryItem list>
type EanMap = Map<EAN, InventoryItem list>
type AsinMap = Map<Asin, InventoryItem list>
type GtinMap = Map<GTIN, InventoryItem list>

type VendorPart = {
    VendorPartNumber : VendorPartNumber
    AlternateIds : AlternateId list
}

type Vendor = {
    VendorId : VendorId
    Catalog : VendorPart list
}