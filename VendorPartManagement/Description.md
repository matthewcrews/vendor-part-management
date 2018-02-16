# How this should work

- Take a list of Vendor Parts
- Filter the list into two: Existing Parts and New Vendor Parts

## With Existing Parts

- Update the pricing for the Existing parts

## With New Parts

- Save the parts into the Vendor Part Table
- Filter the new parts into three sets: No Asin, No InventoryId
- Run a InvetoryItem match task which will lookup potential matches for the VendorParts. If there are potential matches then it will be stored in a VendorPartStockItemMatch table. These items will now wait until a use selects which StockItem it matches or says, there are no matches.


---

**Note** This model should be able to work with Vendor Parts getting and Asin and without getting an Asin.

---

## Possible Vendor Part States

---

1. \<State>
    - \<Possible Next State>

---

1. New
    - PossibleStockItem
2. PossibleStockItem
    - AssignedStockItem
    - NeedingAsinMatches
3. NeedingAsinMatches
    - PossibleAsin
4. NoAsinMatches
    - New
5. AwaitingAsinMatch
    - NeedingAsinMatches
    - AssignedAsin
6. AssignedAsin
    - AssignedStockItem
7. ProfitAnalyzed
    - Unsynced
    - ProfitAnalyzed
8. Unsynced
    - Synced
9. Synced
    - Unsynced

### Possible Initial States

- New
- AssignedAsin
- AssignedStockItem

## State Transition Functions

### New -> PossibleStockItem

This would be a search of possible matches by UPC, EAN, or GTIN. The StockItems that do match get added to a PossibleMatch List. If there are no StockItem Matches, the it returns an empty list.

### PossibleStockitem -> NeedingAsin

This transition can occur two different ways. The first is that the Item has no potential Stockitem matches. If that is the case, then the item is automatically moved to the `NeedingAsin` state. This can also occur if the user decides that none of the existing `StockItems` is a valid match. When this happens, the potential matches are removed and the items status is updated to `NeedingAsin`.

### NeedingAsin -> PossibleAsinMatch

This takes the list of Vendor Parts which need an Asin and searches for possible matches. If there are possible matches then the Vendor Part is moved from NeedingAsin to PossibleAsinMatch. If no possible matches are found, then the part remains in the NeedingAsin state.

### PossibleStockItem -> AssignedStockItem

The user looks at the list of possible StockItems and selects the one they believe to be the best match.