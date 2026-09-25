using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private string itemName;
    private GameObject item;

    RectTransform slotRect;

    public void CreateSlot(float width)
    {
        // Create UI
        slotRect = GetComponent<RectTransform>();
        slotRect.sizeDelta = new Vector2(width, width);
        Image image = gameObject.AddComponent<Image>();

        image.color = Color.slateGray;
        image.raycastTarget = true;

        // Add button functionality
        Button button = gameObject.AddComponent<Button>();
    }
}