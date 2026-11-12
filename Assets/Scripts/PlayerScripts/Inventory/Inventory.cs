using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System;

public class Inventory : MonoBehaviour
{

    public static Inventory _Instance;
    private InventorySlot[] _inventory;
    private DialogueData _inventoryFullDialogueData;

    // UI variables
    private GameObject _canvasObject;
    private RectTransform _inventoryBoxRect;
    private GameObject[] _slotContainers;

    private Button _dropdownButton;
    private RectTransform _arrowRect;
    private Text _arrowText;

    private bool _isExpanded = false;  


    // Slot amount variables
    private int _rows = 2;
    private const int _slotsPerRow = 10;

    // Slot visual settings variables
    private float _slotWidth = 40f;
    private float _spacing = 5f;
    private int _padding = 5;
    private float _dropdownHeight = 24f;

    void Awake()
    {
        // Create instance
        if (_Instance != null && _Instance != this) {
            Destroy(this);
            return;
        }

        _Instance = this;

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

        SetupInventory();
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

        _inventoryFullDialogueData = new DialogueData(data);
    }


    // TODO: REMOVE AFTER TESTING
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_Instance != null) {
                _Instance.AddItem(CreateTestSprite());
            } else {
                Debug.LogError("Inventory.Instance is null!");
            }
        }
    
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (_Instance != null) {
                _Instance.AddInventoryRows(1);
            } else {
                Debug.LogError("Inventory.Instance is null!");
            }
        }
    }

    private void SetupInventory()
    {
        // Create canvas
        _canvasObject = CreateCanvas();

        // Create inventory 
        _inventory = new InventorySlot[_rows * _slotsPerRow];
        _slotContainers = new GameObject[_rows];

        // Create inventory UI
        CreateInventoryBox(_canvasObject);
        CreateInventorySlots();

        SetExpanded(_isExpanded);
    }

    private GameObject CreateCanvas()
    {
        // Create canvas object
        GameObject canvasObject = new GameObject("InventoryCanvas");

        // Configure canvas
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        return canvasObject;
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

        _inventoryBoxRect = box.GetComponent<RectTransform>();

        _inventoryBoxRect.anchorMin = new Vector2(0f, 1f);
        _inventoryBoxRect.anchorMax = new Vector2(0f, 1f);
        _inventoryBoxRect.pivot = new Vector2(0f, 1f);
        _inventoryBoxRect.anchoredPosition = new Vector2(10f, -10f);

        float rowWidth = _slotsPerRow * _slotWidth + (_slotsPerRow - 1) * _spacing;

        _inventoryBoxRect.sizeDelta = new Vector2(
            rowWidth + _padding * 2,
            _slotWidth + _padding * 2 + _dropdownHeight + _spacing
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

        // Dropdown button
        CreateDropdownButton(parent.transform, rowWidth);

        // Create rows
        for (int i=0; i<_rows; i++)
        {
            _slotContainers[i] = CreateSlotRow(
                box.transform,
                "ExtraSlotRow_" + i,
                rowWidth
            );

            // Hide all but the first row
            if (i != 0)
            {
                _slotContainers[i].SetActive(false);
            }
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
        _dropdownButton = buttonObject.GetComponent<Button>();
        _dropdownButton.transition = Selectable.Transition.None;

        // Position the button to the right of the inventory box
        buttonRect.anchorMin = new Vector2(0f, 1f);
        buttonRect.anchorMax = new Vector2(0f, 1f);
        buttonRect.pivot = new Vector2(0f, 1f);

        buttonRect.sizeDelta = new Vector2(_dropdownHeight, _dropdownHeight);

        buttonRect.anchoredPosition = new Vector2(
            10f + _inventoryBoxRect.sizeDelta.x + 5f,
            -10f - _padding
        );

        // Transparent background, but still clickable
        buttonImage.color = new Color(1f, 1f, 1f, 0f);
        buttonImage.raycastTarget = true;

        _dropdownButton.onClick.AddListener(ToggleExpanded);

        // Create the arrow text
        GameObject arrowObject = new GameObject(
            "Arrow",
            typeof(RectTransform),
            typeof(Text)
        );

        arrowObject.transform.SetParent(buttonObject.transform, false);

        _arrowRect = arrowObject.GetComponent<RectTransform>();
        _arrowRect.anchorMin = Vector2.zero;
        _arrowRect.anchorMax = Vector2.one;
        _arrowRect.offsetMin = Vector2.zero;
        _arrowRect.offsetMax = Vector2.zero;

        _arrowText = arrowObject.GetComponent<Text>();
        _arrowText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        _arrowText.fontSize = 16;
        _arrowText.text = "\u25BC";
        _arrowText.color = Color.white;
        _arrowText.alignment = TextAnchor.MiddleCenter;
        _arrowText.raycastTarget = false;
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
        rowRect.sizeDelta = new Vector2(rowWidth + _padding * 2, _slotWidth + _padding);

        // Set background
        Image background = row.GetComponent<Image>();
        background.color = Color.darkMagenta;
        background.raycastTarget = false;

        // Set layout
        HorizontalLayoutGroup layout = row.GetComponent<HorizontalLayoutGroup>();

        layout.spacing = _spacing;
        layout.padding = new RectOffset(_padding, _padding, _padding/2, _padding/2);
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
        for (int i=0; i<_slotContainers.Length; i++)
        {
            // Choose the correct row for this slot
            Transform row = _slotContainers[i].transform;

            for (int j=0; j<_slotsPerRow; j++) 
            {
                // Create slot object
                GameObject slotObject = new GameObject("InventorySlot_" + ((i*_slotsPerRow) + j), typeof(RectTransform));
                slotObject.transform.SetParent(row, false);

                InventorySlot slot = slotObject.AddComponent<InventorySlot>();

                _inventory[(i*_slotsPerRow) + j] = slot;

                slot.CreateSlot(_slotWidth);
            }
        }
    }

    private void ToggleExpanded()
    {
        SetExpanded(!_isExpanded);
    }

    private void SetExpanded(bool expanded)
    {
        _isExpanded = expanded;

        // Calculate visible rows
        int lastVisibleRow = expanded ? _rows - 1 : 0;

        // Toggle rows and reset padding
        _slotContainers[0].SetActive(true);
        for (int i = 0; i < _rows; i++) 
        {
            // Toggle rows
            _slotContainers[i].SetActive(i == 0 || expanded);

            // Reset padding
            HorizontalLayoutGroup layout = _slotContainers[i].GetComponent<HorizontalLayoutGroup>();

            layout.padding.top = _padding / 2;
            layout.padding.bottom = _padding / 2;
            if (i == 0) 
            {
                layout.padding.top = _padding;
            }
            if (i == lastVisibleRow)
            {
                layout.padding.bottom = _padding;
            }

            // Update row height
            RectTransform rowRect = _slotContainers[i].GetComponent<RectTransform>();

            rowRect.sizeDelta = new Vector2(
                rowRect.sizeDelta.x,
                _slotWidth + layout.padding.top + layout.padding.bottom
            );
        }

        // Update the arrow 
        if (_arrowRect != null) 
        {
            _arrowText.text = _isExpanded ? "\u25B2" : "\u25BC";
            _arrowText.gameObject.SetActive(true);
        }
    }

    private int FindNextOpenSlot(Pickupable item)
    {
        if (_inventory == null) 
        {
            Debug.LogError( $"Inventory array is not initialized on {gameObject.name}.", this);
            return -1;
        }

        // Loop through all slots to find an open one
        for (int i=0; i<_inventory.Length; i++)
        {
            if (_inventory[i].GetItem() == null || 
                (_inventory[i].GetItemAmount() < _inventory[i].GetMaxItemAmount() && _inventory[i].GetItemName() == item._name))
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
        // Check for open slots
        int slot = FindNextOpenSlot(item);
        if (slot == -1)
        {
            DialogueController._Instance.StartDialogue(_inventoryFullDialogueData);
            return -1;
        }

        // Check if the next slot is open or stackabble
        if (_inventory[slot].GetItem() == null)
        {
            // Add new item
            _inventory[slot].SetItem(item);
        }
        else 
        {
            // Stack item
            _inventory[slot].AddItemAmount(1);
        }

        return slot;
    }

    public void MoveOrSwapItems(InventorySlot from, InventorySlot to)
    {
        // Avoid null errors
        if (from == null || to == null || from == to)
        {
            return;
        }

        // Store items
        Pickupable draggedItem = from.GetItem();
        Pickupable targetItem = to.GetItem();
        int fromItemAmount = from.GetItemAmount();

        // Avoid more null errors
        if (draggedItem == null)
        {
            return;
        }

        // Check if stackable or swappable
        if (to.GetItem() != null && from.GetItemName() == to.GetItemName())
        {
            // Stack items, leaving extra items in from slot (choose either the amount in the from slot or the max amount to add to the to slot)
            int itemsToAdd = Math.Min(from.GetItemAmount(), to.GetMaxItemAmount() - to.GetItemAmount());

            to.AddItemAmount(itemsToAdd);
            from.RemoveItemAmount(itemsToAdd);
        }
        else
        {
            // Swap items
            from.SetItemAndAmount(targetItem, to.GetItemAmount());
            to.SetItemAndAmount(draggedItem, fromItemAmount);
        }
    }

    public void AddInventoryRows(int newRows)
    {
        // Store then replace old inventory
        _rows += newRows;
        InventorySlot[] oldInventory = _inventory;

        // Reset UI
        Destroy(_canvasObject);
        SetupInventory();

        // Restore items
        for (int i=0; i<oldInventory.Length; i++)
        {
            if (oldInventory[i].GetItem() != null)
            {
                _inventory[i].SetItemAndAmount(oldInventory[i].GetItem(), oldInventory[i].GetItemAmount());
            }
        }
    }

    // TODO: REMOVE AFTER TESTING
    private Pickupable CreateTestSprite()
    {
        // Find file in resources folder
        Sprite sprite = Resources.Load<Sprite>("Sprites/Placeholders/Rosier");

        // Assign Sprite
        return new Pickupable
        {
            _sprite = sprite,
            _name = "Rosier"
        };
    }
}