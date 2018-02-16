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
    - New
    - AssignedAsin
6. AssignedAsin
    - AssignedStockItem
7. ProfitAnalyzed
    - Synced
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

This would be a search of possible matches by UPC, EAN, or GTIN. The `StockItems` that do match get added to a `PossibleMatch list`. If there are no StockItem Matches, then it returns an empty list.

### PossibleStockitem -> NeedingAsin

This transition can occur two different ways. The first is that the Item has no potential Stockitem matches. If that is the case, then the item is automatically moved to the `NeedingAsin` state. This can also occur if the user decides that none of the existing `StockItems` is a valid match. When this happens, the potential matches are removed and the items status is updated to `NeedingAsin`.

### PossibleStockItem -> AssignedStockItem

The user looks at the list of possible `StockItem list` and selects the one they believe to be the best match. When the selection is made, it is persisted to the data store.

### NeedingAsin -> AwaitingAsinMatch

This takes the list of Vendor Parts which need an Asin and searches for possible matches. If there are possible matches then the Vendor Part is moved from `NeedingAsin` to `AwaitingAsinMatch`. If no possible matches are found, then the part remains in the NeedingAsin state.

### AwaitingAsinMatch -> AssignedAsin

When a Vendor Part has a list of possible Asins, the user will need to select which one is the correct one. When the user selects the correct Asin, the the Part moved to the `AssignedAsin` state.

### AssignedAsin -> ProfitAnalyzed

While the Part is in the `AssignedAsin` state the system will continually try to draw information in order to perform a profitability analysis. Once it does have enough information, it will compute a `PriceFloor` number. When this happens the Part moved to the `ProfitAnalyzed` state.

### ProfitAnalyzed -> CheckForMatch

The system will continually monitor if the profitability of the Part is high enough to justify whether to migrate it into the Replenishment system. From here it would move to `CheckForMatch` state to ensure that another Part has not already made it into the Replenish service.

### CheckForMatch -> Synced

At the point it is, an InventoryId will be assigned and the data will be imported. The part will move to the `Synced` state.

### Synced -> Unsynced

Should a change be made to a Part that was in the `Synced` state, it will be moved back to the `Unsynced` state to indicate that it needs to be synced with the Replenishment platform.

### Unsynced -> Synced

A regular task will look for parts in the `Unsynced` state to sync them with the Replenishment service. When this happens the item moves back to the `Synced` state.

### NoAsinMatches -> New

Periodically a user can decide to move Parts from the `NoAsinMatch` state back to the `New` state so that the Parts can be re-analyzed.

### AwaitingAsinMatch -> New

Should a user decide that none of the `Asin`s are good matches, then they can decide to send the Part back to the `New` state so that it can be analyzed again.

### ProfitAnalyzed -> ProfitAnalyzed

Part in the `ProfitAnalyzed` state will have their numbers continually updated as new information comes in. This will look like a transition back to the same state.