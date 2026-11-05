using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, 
                            IBeginDragHandler,
                            IDragHandler,
                            IEndDragHandler,
                            IDropHandler
{
    // Slot variables
    private RectTransform slotRect;
    private Image slotImage;

    // Item variables
    public int itemAmount;
    private Pickupable item;

    // UI variables
    private Image itemImage;
    private Image dragImage;
    private TextMeshProUGUI itemAmountText;

    public Pickupable GetItem()
    {
        return item;
    }

    public int GetItemAmount()
    {
        return itemAmount;
    }

    public string GetItemName()
    {
        return item.name;
    }

    public int GetMaxItemAmount()
    {
        return item.maxAmount;
    }

    public void SetItem(Pickupable newItem)
    {
        // Add only 1 of item by default
        SetItemAndAmount(newItem, 1);
    }

    public void SetItemAmount(int newItemAmount)
    {
        itemAmount = newItemAmount;
        UpdateItemAmountDisplay();
    }

    public void SetItemAndAmount(Pickupable newItem, int newItemAmount)
    {
        // Avoid repetition by calling RemoveItem() if none is passed in
        if (newItem == null)
        {
            RemoveItem();
            return;
        }

        // Avoid null errors
        if (itemImage == null)
        {
            Debug.Log("Error: Inventory slot itemImage has not been created");
            return;
        }

        // Set item and sprites
        item = newItem;
        itemAmount = newItemAmount;

        itemImage.sprite = item.sprite;
        itemImage.enabled = item.sprite != null;
        UpdateItemAmountDisplay();
    }

    public void AddItemAmount(int amount)
    {
        itemAmount += amount;
        UpdateItemAmountDisplay();
    }

    public void RemoveItem()
    {
        // Avoid null errors
        if (itemImage == null)
        {
            Debug.Log("Error: Inventory slot itemImage has not been created");
            return;
        }
        
        // Remove item and sprites
        item = null;
        itemAmount = 0;

        itemImage.sprite = null;
        itemImage.enabled = false;
        UpdateItemAmountDisplay();
    }

    public void RemoveItemAmount(int amount)
    {
        if (amount >= itemAmount)
        {
            RemoveItem();
            return;
        }

        itemAmount -= amount;
        UpdateItemAmountDisplay();
    }


    public void CreateSlot(float width)
    {
        // Create slot
        slotRect = GetComponent<RectTransform>();

        if (slotRect == null)
        {
            slotRect = gameObject.AddComponent<RectTransform>();
        }

        // Set slot size
        slotRect.sizeDelta = new Vector2(width, width);
        slotImage = gameObject.AddComponent<Image>();
        slotImage.color = Color.slateGray;
        slotImage.raycastTarget = true;

        // Create item
        CreateItem(width);
        CreateItemAmountDisplay(width);

        // Add button functionality
        Button button = gameObject.AddComponent<Button>();
    }

    public void CreateItem(float width)
    {
        // Create the item sprite Image
        GameObject itemObject = new GameObject("ItemSprite");
        itemObject.transform.SetParent(transform, false);
        itemImage = itemObject.AddComponent<Image>();

        // Set the item location
        RectTransform itemRect = itemObject.GetComponent<RectTransform>();

        itemRect.anchorMin = new Vector2(0.5f, 0.5f);
        itemRect.anchorMax = new Vector2(0.5f, 0.5f);
        itemRect.pivot = new Vector2(0.5f, 0.5f);

        itemRect.anchoredPosition = Vector2.zero;

        // Format the item
        itemRect.sizeDelta = new Vector2(width * 0.8f, width * 0.8f);
        itemImage.preserveAspect = true;
        itemImage.raycastTarget = false;

        itemImage.enabled = false;
    }

    public void CreateItemAmountDisplay(float width)
    {
        // Create text object
        GameObject amountObject = new GameObject(
            "ItemAmount",
            typeof(RectTransform)
        );

        amountObject.transform.SetParent(transform, false);

        itemAmountText = amountObject.AddComponent<TextMeshProUGUI>();

        // Set text location
        RectTransform amountRect = amountObject.GetComponent<RectTransform>();

        amountRect.anchorMin = new Vector2(1f, 0f);
        amountRect.anchorMax = new Vector2(1f, 0f);
        amountRect.pivot = new Vector2(1f, 0f);

        amountRect.anchoredPosition = new Vector2(-2f, 2f);
        amountRect.sizeDelta = new Vector2(width * 0.5f, width * 0.4f);

        // Set text format
        itemAmountText.fontSize = 14;
        itemAmountText.alignment = TextAlignmentOptions.BottomRight;
        itemAmountText.color = Color.white;
        itemAmountText.raycastTarget = false;

        itemAmountText.text = "";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Pickupable item = GetItem();

        // Only allow dragging if this slot contains an item
        if (item == null || item.sprite == null)
            return;

        // Create a temporary image that follows the cursor
        GameObject dragObject = new GameObject(
            "DraggedItem",
            typeof(RectTransform),
            typeof(CanvasGroup),
            typeof(Image)
        );

        dragObject.transform.SetParent(
            GetComponentInParent<Canvas>().transform,
            false
        );

        dragImage = dragObject.GetComponent<Image>();
        dragImage.sprite = item.sprite;
        dragImage.preserveAspect = true;
        dragImage.raycastTarget = false;
        dragObject.transform.SetAsLastSibling();

        RectTransform rect = dragObject.GetComponent<RectTransform>();
        rect.sizeDelta = GetComponent<RectTransform>().sizeDelta;
        rect.position = eventData.position;
    }

    private void UpdateItemAmountDisplay()
    {
        if (itemAmountText == null)
            return;

        // Only display amounts greater than 1
        itemAmountText.text = itemAmount > 1 ? itemAmount.ToString() : "";
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the item as its being dragged
        if (dragImage == null)
            return;

        dragImage.rectTransform.position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Identify the slot where the drag began
        GameObject draggedObject = eventData.pointerDrag;

        if (draggedObject == null) {
            return;
        }

        // Swap items
        InventorySlot from = draggedObject.GetComponent<InventorySlot>();

        if (from == null || from == this) {
            return;
        }

        Inventory.Instance.MoveOrSwapItems(from, this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Release item
        if (dragImage != null)
        {
            Destroy(dragImage.gameObject);
            dragImage = null;
        }
    }
}