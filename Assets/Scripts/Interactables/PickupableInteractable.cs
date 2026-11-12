using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;


public class PickupableInteractable : Interactable
{
    private DialogueData _dialogueData;
    private Pickupable _item;
    private Sprite _itemSprite; 
    public string _itemName;

    void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError(gameObject.name + " needs a SpriteRenderer.");
            return;
        }

        _itemSprite = spriteRenderer.sprite;
        _item = new Pickupable();
        _item._sprite = _itemSprite;
        _item._name = _itemName;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();

        (string[] script, (string, int)[] choices)[] data = new (string[], (string, int)[])[]
        {
            (new string[] {"Its a " + _itemName + ". Pick it up?"}, 
            new (string, int)[] {("Yes", -1), ("No", -2)})
        };

        _dialogueData = new DialogueData(data);
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
    }

    override async public void OnCollision(Collider2D collidedObject)
    {
        DialogueController._Instance.StartDialogue(_dialogueData);

        Task<int> choiceTask = DialogueController._Instance.ChooseNextDialogue();
        int choice = await choiceTask;

        DialogueController._Instance.EndDialogue();

        if (choice == -1)
        {
            Pickup();
        }
    }

    private void Pickup()
    {
        int slot = Inventory._Instance.AddItem(_item);
        if (slot != -1)
        {
            Destroy(gameObject);
        }
    }
}