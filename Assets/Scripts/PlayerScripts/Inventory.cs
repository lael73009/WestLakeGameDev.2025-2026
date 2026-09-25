using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private InventorySlot[] inventory;
    float slotWidth = 40f;
    private GameObject slotContainer;

    void Awake()
    {
        // Create canvas
        GameObject canvasObject = new GameObject("InventoryCanvas");

        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        // Create inventory box
        inventory = new InventorySlot[10];
        slotContainer = CreateInventoryBox(canvasObject, inventory.Length);

        // Create inventory slots
        CreateInventorySlots();
    }

    private GameObject CreateInventoryBox(GameObject parent, int slots)
    {
        // Create inventory box
        GameObject inventoryBoxObject =
            new GameObject("InventoryBox", typeof(RectTransform));

        inventoryBoxObject.transform.SetParent(parent.transform, false);

        // Set box look
        Image inventoryBox = inventoryBoxObject.AddComponent<Image>();
        inventoryBox.color = Color.darkMagenta;
        inventoryBox.raycastTarget = false;

        // Set box location
        RectTransform boxRect = inventoryBoxObject.GetComponent<RectTransform>();

        boxRect.anchorMin = new Vector2(0f, 1f);
        boxRect.anchorMax = new Vector2(0f, 1f);
        boxRect.pivot = new Vector2(0f, 1f);

        boxRect.anchoredPosition = new Vector2(10f, -10f);

        // Set slot look
        float slotSize = slotWidth;
        float spacing = 5f;
        float padding = 5f;

        float width =
            (slotSize * slots) +
            (spacing * (slots - 1)) +
            (padding * 2);

        float height =
            slotSize +
            (padding * 2);

        boxRect.sizeDelta = new Vector2(width, height);

        // Set slot arrangement and location
        HorizontalLayoutGroup layout = 
            inventoryBoxObject.AddComponent<HorizontalLayoutGroup>();

        layout.spacing = spacing;
        layout.padding = new RectOffset(
            (int)padding,
            (int)padding,
            (int)padding,
            (int)padding
        );

        layout.childAlignment = TextAnchor.MiddleCenter;

        layout.childControlWidth = false;
        layout.childControlHeight = false;

        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;

        return inventoryBoxObject;
    }

    private void CreateInventorySlots()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            // Create slot object
            GameObject slotObject = new GameObject("InventorySlot_" + i, typeof(RectTransform));
            slotObject.transform.SetParent(slotContainer.transform, false);

            // Draw slot
            InventorySlot slot =
                slotObject.AddComponent<InventorySlot>();
            inventory[i] = slot;
            slot.CreateSlot(slotWidth);
        }
    }
}