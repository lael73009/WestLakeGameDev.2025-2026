using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class Inventory : MonoBehaviour
{

    public static Inventory Instance;
    private InventorySlot[] inventory;
    private DialogueData inventoryFullDialogueData;

    // UI Elements
    private RectTransform inventoryBoxRect;
    private GameObject[] slotContainers;

    private Button dropdownButton;
    private RectTransform arrowRect;
    private Text arrowText;

    private bool isExpanded = false;  


    // Slot amounts
    private int rows = 2;
    private const int slotsPerRow = 10;

    // Visual settings
    private float slotWidth = 40f;
    private float spacing = 5f;
    private int padding = 5;
    private float dropdownHeight = 24f;

    void Awake()
    {
        // Create instance
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }

        Instance = this;

        /*
        if (FindFirstObjectByType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject(
                "EventSystem",
                typeof(EventSystem),
                typeof(StandaloneInputModule)
            );
        }
        */

        // Create canvas
        GameObject canvasObject = new GameObject("InventoryCanvas");

        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        // Create inventory UI
        inventory = new InventorySlot[rows * slotsPerRow];
        slotContainers = new GameObject[rows];

        CreateInventoryBox(canvasObject);
        CreateInventorySlots();

        SetExpanded(false);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // Create an instance of the inventory
        if (FindFirstObjectByType<Inventory>() != null) {
            return;
        }

        GameObject inventoryObject = new GameObject("Inventory");
        inventoryObject.AddComponent<Inventory>();
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
            if (Instance != null) {
                Instance.AddItem(CreateTestSprite());
            } else {
                Debug.LogError("Inventory.Instance is null!");
            }
        }
    }

    private GameObject CreateInventoryBox(GameObject parent)
    {
        // Create inventory background
        GameObject box = new GameObject(
            "InventoryBox",
            typeof(RectTransform),
            typeof(VerticalLayoutGroup)
        );

        // Set box loaction
        box.transform.SetParent(parent.transform, false);

        inventoryBoxRect = box.GetComponent<RectTransform>();

        inventoryBoxRect.anchorMin = new Vector2(0f, 1f);
        inventoryBoxRect.anchorMax = new Vector2(0f, 1f);
        inventoryBoxRect.pivot = new Vector2(0f, 1f);
        inventoryBoxRect.anchoredPosition = new Vector2(10f, -10f);

        float rowWidth = slotsPerRow * slotWidth + (slotsPerRow - 1) * spacing;

        inventoryBoxRect.sizeDelta = new Vector2(
            rowWidth + padding * 2,
            slotWidth + padding * 2 + dropdownHeight + spacing
        );

        // Set box layout
        VerticalLayoutGroup verticalLayout = box.GetComponent<VerticalLayoutGroup>();

        verticalLayout.spacing = 0f;
        verticalLayout.padding = new RectOffset(0, 0, 0, 0);

        verticalLayout.childAlignment = TextAnchor.UpperLeft;
        verticalLayout.childControlWidth = false;
        verticalLayout.childControlHeight = false;
        verticalLayout.childForceExpandWidth = false;
        verticalLayout.childForceExpandHeight = false;

        // Set first row as always visible
        slotContainers[0] = CreateSlotRow(
            box.transform,
            "MainSlotRow",
            rowWidth
        );

        // Dropdown button
        CreateDropdownButton(parent.transform, rowWidth);

        // Second row: hidden until expanded

        for (int i=1; i<rows; i++)
        {
            slotContainers[i] = CreateSlotRow(
                box.transform,
                "ExtraSlotRow_" + i,
                rowWidth
            );

            slotContainers[i].SetActive(false);
        }
        
        return box;
    }

    
    private void CreateDropdownButton(Transform parent, float rowWidth)
    {
        // Create the clickable button
        GameObject buttonObject = new GameObject(
            "InventoryDropdownButton",
            typeof(RectTransform),
            typeof(Image),
            typeof(Button)
        );

        // Add button to the canvas
        buttonObject.transform.SetParent(parent, false);

        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();

        Image buttonImage = buttonObject.GetComponent<Image>();
        dropdownButton = buttonObject.GetComponent<Button>();
        dropdownButton.transition = Selectable.Transition.None;

        // Position the button to the right of the inventory box
        buttonRect.anchorMin = new Vector2(0f, 1f);
        buttonRect.anchorMax = new Vector2(0f, 1f);
        buttonRect.pivot = new Vector2(0f, 1f);

        buttonRect.sizeDelta = new Vector2(dropdownHeight, dropdownHeight);

        buttonRect.anchoredPosition = new Vector2(
            10f + inventoryBoxRect.sizeDelta.x + 5f,
            -10f - padding
        );

        // Transparent background, but still clickable
        buttonImage.color = new Color(1f, 1f, 1f, 0f);
        buttonImage.raycastTarget = true;

        dropdownButton.onClick.AddListener(ToggleExpanded);

        // Create the arrow text
        GameObject arrowObject = new GameObject(
            "Arrow",
            typeof(RectTransform),
            typeof(Text)
        );

        arrowObject.transform.SetParent(buttonObject.transform, false);

        arrowRect = arrowObject.GetComponent<RectTransform>();
        arrowRect.anchorMin = Vector2.zero;
        arrowRect.anchorMax = Vector2.one;
        arrowRect.offsetMin = Vector2.zero;
        arrowRect.offsetMax = Vector2.zero;

        arrowText = arrowObject.GetComponent<Text>();
        arrowText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        arrowText.fontSize = 16;
        arrowText.text = "\u25BC";
        arrowText.color = Color.white;
        arrowText.alignment = TextAnchor.MiddleCenter;
        arrowText.raycastTarget = false;
    }

    private GameObject CreateSlotRow(Transform parent, string rowName, float rowWidth)
    {
        GameObject row = new GameObject(
            rowName,
            typeof(RectTransform),
            typeof(Image),
            typeof(HorizontalLayoutGroup)
        );

        row.transform.SetParent(parent, false);

        RectTransform rowRect = row.GetComponent<RectTransform>();
        rowRect.sizeDelta = new Vector2(rowWidth + padding * 2, slotWidth + padding);

        // Set background
        Image background = row.GetComponent<Image>();
        background.color = Color.darkMagenta;
        background.raycastTarget = false;

        // Set layout
        HorizontalLayoutGroup layout = row.GetComponent<HorizontalLayoutGroup>();

        layout.spacing = spacing;
        layout.padding = new RectOffset(padding, padding, padding/2, padding/2);
        layout.childAlignment = TextAnchor.MiddleLeft;

        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;

        return row;
    }

    private void CreateInventorySlots()
    {
        // Loop through entire inventory size
        for (int i = 0; i < slotContainers.Length; i++)
        {
            // Choose the correct row for this slot
            Transform row = slotContainers[i].transform;

            for (int j = 0; j < slotsPerRow; j++) {
                // Create slot object
                GameObject slotObject = new GameObject("InventorySlot_" + i + "_" + j, typeof(RectTransform));
                slotObject.transform.SetParent(row, false);

                InventorySlot slot =
                    slotObject.AddComponent<InventorySlot>();

                inventory[(i*slotsPerRow) + j] = slot;

                slot.CreateSlot(slotWidth);
            }
        }
    }

    private void ToggleExpanded()
    {
        SetExpanded(!isExpanded);
    }

    private void SetExpanded(bool expanded)
    {
        isExpanded = expanded;

        // Calculate visible rows
        int lastVisibleRow = expanded ? rows - 1 : 0;

        // Toggle rows and reset padding
        slotContainers[0].SetActive(true);
        for (int i = 0; i < rows; i++) 
        {
            // Toggle rows
            slotContainers[i].SetActive(i == 0 || expanded);

            // Reset padding
            HorizontalLayoutGroup layout = slotContainers[i].GetComponent<HorizontalLayoutGroup>();

            layout.padding.top = padding / 2;
            layout.padding.bottom = padding / 2;
            if (i == 0) 
            {
                layout.padding.top = padding;
            }
            if (i == lastVisibleRow)
            {
                layout.padding.bottom = padding;
            }

            // Update row height
            RectTransform rowRect = slotContainers[i].GetComponent<RectTransform>();

            rowRect.sizeDelta = new Vector2(
                rowRect.sizeDelta.x,
                slotWidth + layout.padding.top + layout.padding.bottom
            );
        }

        // Update the arrow 
        if (arrowRect != null) 
        {
            arrowText.text = isExpanded ? "\u25B2" : "\u25BC";
            arrowText.gameObject.SetActive(true);
        }
    }

    private int FindNextOpenSlot()
    {
        if (inventory == null) 
        {
            Debug.LogError( $"Inventory array is not initialized on {gameObject.name}.", this);
            return -1;
        }

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

        inventory[slot].SetItem(item);
        return slot;
    }

    public void MoveOrSwapItems(InventorySlot from, InventorySlot to)
    {
        if (from == null || to == null || from == to)
        {
            return;
        }

        // Swap items
        Pickupable draggedItem = from.GetItem();
        Pickupable targetItem = to.GetItem();

        if (draggedItem == null)
        {
            return;
        }

        from.SetItem(targetItem);
        to.SetItem(draggedItem);
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