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
    private RectTransform _slotRect;
    private Image _slotImage;

    // Item variables
    public int _itemAmount;
    private Pickupable _item;

    // UI variables
    private Image _itemImage;
    private Image _dragImage;
    private TextMeshProUGUI _itemAmountText;

    public Pickupable GetItem()
    {
        return _item;
    }

    public int GetItemAmount()
    {
        return _itemAmount;
    }

    public string GetItemName()
    {
        return _item._name;
    }

    public int GetMaxItemAmount()
    {
        return _item._maxAmount;
    }

    public void SetItem(Pickupable newItem)
    {
        // Add only 1 of item by default
        SetItemAndAmount(newItem, 1);
    }

    public void SetItemAmount(int newItemAmount)
    {
        _itemAmount = newItemAmount;
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
        if (_itemImage == null)
        {
            Debug.Log("Error: Inventory slot itemImage has not been created");
            return;
        }

        // Set item and sprites
        _item = newItem;
        _itemAmount = newItemAmount;

        _itemImage.sprite = _item._sprite;
        _itemImage.enabled = _item._sprite != null;
        UpdateItemAmountDisplay();
    }

    public void AddItemAmount(int amount)
    {
        _itemAmount += amount;
        UpdateItemAmountDisplay();
    }

    public void RemoveItem()
    {
        // Avoid null errors
        if (_itemImage == null)
        {
            Debug.Log("Error: Inventory slot itemImage has not been created");
            return;
        }
        
        // Remove item and sprites
        _item = null;
        _itemAmount = 0;

        _itemImage.sprite = null;
        _itemImage.enabled = false;
        UpdateItemAmountDisplay();
    }

    public void RemoveItemAmount(int amount)
    {
        if (amount >= _itemAmount)
        {
            RemoveItem();
            return;
        }

        _itemAmount -= amount;
        UpdateItemAmountDisplay();
    }


    public void CreateSlot(float width)
    {
        // Create slot
        _slotRect = GetComponent<RectTransform>();

        if (_slotRect == null)
        {
            _slotRect = gameObject.AddComponent<RectTransform>();
        }

        // Set slot size
        _slotRect.sizeDelta = new Vector2(width, width);
        _slotImage = gameObject.AddComponent<Image>();
        _slotImage.color = Color.slateGray;
        _slotImage.raycastTarget = true;

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
        _itemImage = itemObject.AddComponent<Image>();

        // Set the item location
        RectTransform itemRect = itemObject.GetComponent<RectTransform>();

        itemRect.anchorMin = new Vector2(0.5f, 0.5f);
        itemRect.anchorMax = new Vector2(0.5f, 0.5f);
        itemRect.pivot = new Vector2(0.5f, 0.5f);

        itemRect.anchoredPosition = Vector2.zero;

        // Format the item
        itemRect.sizeDelta = new Vector2(width * 0.8f, width * 0.8f);
        _itemImage.preserveAspect = true;
        _itemImage.raycastTarget = false;

        _itemImage.enabled = false;
    }

    public void CreateItemAmountDisplay(float width)
    {
        // Create text object
        GameObject amountObject = new GameObject(
            "ItemAmount",
            typeof(RectTransform)
        );

        amountObject.transform.SetParent(transform, false);

        _itemAmountText = amountObject.AddComponent<TextMeshProUGUI>();

        // Set text location
        RectTransform amountRect = amountObject.GetComponent<RectTransform>();

        amountRect.anchorMin = new Vector2(1f, 0f);
        amountRect.anchorMax = new Vector2(1f, 0f);
        amountRect.pivot = new Vector2(1f, 0f);

        amountRect.anchoredPosition = new Vector2(-2f, 2f);
        amountRect.sizeDelta = new Vector2(width * 0.5f, width * 0.4f);

        // Set text format
        _itemAmountText.fontSize = 14;
        _itemAmountText.alignment = TextAlignmentOptions.BottomRight;
        _itemAmountText.color = Color.white;
        _itemAmountText.raycastTarget = false;

        _itemAmountText.text = "";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Pickupable item = GetItem();

        // Only allow dragging if this slot contains an item
        if (item == null || item._sprite == null)
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

        _dragImage = dragObject.GetComponent<Image>();
        _dragImage.sprite = item._sprite;
        _dragImage.preserveAspect = true;
        _dragImage.raycastTarget = false;
        dragObject.transform.SetAsLastSibling();

        RectTransform rect = dragObject.GetComponent<RectTransform>();
        rect.sizeDelta = GetComponent<RectTransform>().sizeDelta;
        rect.position = eventData.position;
    }

    private void UpdateItemAmountDisplay()
    {
        if (_itemAmountText == null)
            return;

        // Only display amounts greater than 1
        _itemAmountText.text = _itemAmount > 1 ? _itemAmount.ToString() : "";
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the item as its being dragged
        if (_dragImage == null)
            return;

        _dragImage.rectTransform.position = eventData.position;
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

        Inventory._Instance.MoveOrSwapItems(from, this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Release item
        if (_dragImage != null)
        {
            Destroy(_dragImage.gameObject);
            _dragImage = null;
        }
    }
}