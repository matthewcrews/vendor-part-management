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
    - NoAsinMatches
    - AwaitingAsinMatch
2. NoAsinMatches
    - New
3. AwaitingAsinMatch
    - New
    - AssignedAsin
4. AssignedAsin
    - ProfitAnalyzed
    - New
5. ProfitAnalyzed
    - CheckForStockItemMatch
    - ProfitAnalyzed
    - New
6. CheckForStockItemMatch
    - PossibleStockItem
    - Synced
7. PossibleStockItem
    - Synced
    - New
8. Unsynced
    - Synced
9. Synced
    - Unsynced

### Possible Initial States

- New
- AssignedAsin
- Unsynced

## State Transition Functions

### New -> NoAsinMatches

For all parts in the `New` state a process queries the possible Asin values and nothing is returned. When this occurs the Part is moved to the `NoAsinMatches` state.

### New -> AwaitingAsinMatch

For all parts in the `New` state a process queries for possible Asin matches by AlternateIds. A list of possible Asins if found and the Part is moved to the `AwaitingAsinMatch` state.

### AwaitingAsinMatch -> AssignedAsin

For all parts in the `AwaitingAsinMatch` state the user must select which `Asin` is the correct match. When the selection is made the Part is moved to the `AssignedAsin` state.

### AssignedAsin -> ProfitAnalyzed

While the Part is in the `AssignedAsin` state the system will continually try to draw information in order to perform a profitability analysis. Once it does have enough information, it will compute a `PriceFloor` number. When this happens the Part moved to the `ProfitAnalyzed` state.

### ProfitAnalyzed -> CheckForStockItemMatch

The system will continually monitor if the profitability of the Part is high enough to justify whether to migrate it into the Replenishment system. From here it would move to `CheckForMatch` state to ensure that another Part has not already made it into the Replenish service.

### CheckForStockItemMatch -> Synced

For parts in the `CheckForStockItemMatch` state, a process looks for possible matches to existing `StockItem`s. If not possible match is found then the part is assigned an `InventoryId` and is moved to the `Synced` status. This process also imports all the Part information necessary for creating the new `StockItem` in the Replenishment service.

### CheckForStockItemMatch -> PossibleStockItem

For parts in the `CheckForStockItemMatch` state, a process looks for possible matches to existing `StockItem`s. If possible matches are found then the item is moved to the `PossibleStockItem` status where it will wait for a User to confirm whether one of the `StockItem`s is a match.

### PossibleStockItem -> AwaitingCreate

If a part is in the `PossibleStockItem` status and the user decides that none of the potential matches are actually good, then they can decide to move the part to `AwaitingCreate` which tells the system to go ahead and create a new `StockItem` for the part.

### PossibleStockItem -> Unsynced

If a User decides that a part in the `PossibleStockItem` status does actually have a `StockItem` match then they can select that item. The `InventoryId` will be assigned to the part and it will be moved to the `Unsynced` status.

### AwaitingCreate -> Synced

A periodic task takes items in the `AwaitingCreate` status and creates a new corresponding `StockItem` for the Part. The Part is then moved to the `Synced` status.

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