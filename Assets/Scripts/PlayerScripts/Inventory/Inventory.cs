using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Inventory : MonoBehaviour
{
    private DialogueData inventoryFullDialogueData;

    public static Inventory Instance;
    
    private InventorySlot[] inventory;
    float slotWidth = 40f;
    private GameObject slotContainer;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // Create an instance of the inventory
        GameObject inventoryObject = new GameObject("Inventory");
        inventoryObject.AddComponent<Inventory>();
    }

    void Awake()
    {
        // Create instance
        Instance = this;

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

    void Start()
    {
        (string[] script, (string, int)[] choices)[] data = new (string[], (string, int)[])[]
        {
            (new string[] {"There's no more space in your pockets."}, 
            new (string, int)[] {})
        };

        inventoryFullDialogueData = new DialogueData(data);
    }

    void Update()
    {
        // TODO: REMOVE AFTER TESTING
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddItem(CreateTestSprite());
        }
    }

    private GameObject CreateInventoryBox(GameObject parent, int slots)
    {
        // Create inventory box
        GameObject inventoryBoxObject = new GameObject("InventoryBox", typeof(RectTransform));
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

        // Set slot location
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
        // Loop through entire inventory size
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

    private int FindNextOpenSlot()
    {
        // Loop through all slots to find an open one
        for (int i=0; i<inventory.Length; i++)
        {
            if (inventory[i].GetItem() == null)
            {
                return i;
            }
        }

        // Returns -1 if all slots are filled
        return -1;
    }

    // Returns slot item is added to
    public int AddItem(Pickupable item)
    {
        // Add item at open slot
        int slot = FindNextOpenSlot();
        if (slot == -1)
        {
            DialogueController.Instance.StartDialogue(inventoryFullDialogueData);
            return -1;
        }

        inventory[FindNextOpenSlot()].SetItem(item);
        return slot;
    }

    // TODO: REMOVE AFTER TESTING
    private Pickupable CreateTestSprite()
    {
        // Find file in resources folder
        Sprite sprite = Resources.Load<Sprite>("Sprites/Placeholders/Rosier");

        // Assign Sprite
        return new Pickupable
        {
            sprite = sprite
        };
    }
}