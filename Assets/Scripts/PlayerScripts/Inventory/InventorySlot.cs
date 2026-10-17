using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    private Pickupable item;
    private Image itemImage;
    private Image dragImage;

    public Pickupable GetItem()
    {
        return item;
    }

    public void SetItem(Pickupable newItem)
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

        itemImage.sprite = item.sprite;
        itemImage.enabled = item.sprite != null;
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

        itemImage.sprite = null;
        itemImage.enabled = false;
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