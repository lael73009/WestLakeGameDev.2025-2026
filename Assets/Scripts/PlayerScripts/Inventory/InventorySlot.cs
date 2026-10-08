using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    // Slot variables
    private RectTransform slotRect;
    private Image slotImage;

    // Item variables
    //SpriteRenderer itemRenderer;
    private Pickupable item;
    private Image itemImage;

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
}