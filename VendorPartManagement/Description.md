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
4. PossibleAsin
    - NeedingAsinMatches
    - AssignedAsin
5. AssignedAsin
    - AssignedStockItem
6. AssignedStockItem

### Possible Initial States

- New
- AssignedAsin
- AssignedStockItem

## State Transition Functions

### New -> PossibleStockItem

This would be a search of possible matches by UPC, EAN, or GTIN. The StockItems that do match get added to a PossibleMatch List. If there are no StockItem Matches, the it returns an empty list.

### PossibleStockitem -> NeedingAsin

This is a search on Amazon by UPC to find which Asins are possible matches. The input is the list of Vendor Parts which do not have any possible Stock Item Matches, or  The possible matches are stored in a PossibleAsin List.

### NeedingAsin -> PossibleAsinMatch

This takes the list of Vendor Parts which need an Asin and searches for possible matches. If there are possible matches then the Vendor Part is moved from NeedingAsin to PossibleAsinMatch. If no possible matches are found, then the part remains in the NeedingAsin state.

### PossibleStockItem -> AssignedStockItem

The user looks at the list of possible StockItems and selects the one they believe to be the best match.