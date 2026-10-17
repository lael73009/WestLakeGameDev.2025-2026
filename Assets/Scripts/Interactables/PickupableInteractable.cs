using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;


public class PickupableInteractable : Interactable
{
    private DialogueData dialogueData;
    private Pickupable item;
    private Sprite itemSprite; 
    public string itemName;

    void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError(gameObject.name + " needs a SpriteRenderer.");
            return;
        }

        itemSprite = spriteRenderer.sprite;
        item = new Pickupable();
        item.sprite = itemSprite;
        item.name = itemName;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();

        (string[] script, (string, int)[] choices)[] data = new (string[], (string, int)[])[]
        {
            (new string[] {"Its a " + itemName + ". Pick it up?"}, 
            new (string, int)[] {("Yes", -1), ("No", -2)})
        };

        dialogueData = new DialogueData(data);
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
    }

    override async public void OnCollision(Collider2D collidedObject)
    {
        DialogueController.Instance.StartDialogue(dialogueData);

        Task<int> choiceTask = DialogueController.Instance.ChooseNextDialogue();
        int choice = await choiceTask;

        DialogueController.Instance.EndDialogue();

        if (choice == -1)
        {
            Pickup();
        }
    }

    private void Pickup()
    {
        int slot = Inventory.Instance.AddItem(item);
        if (slot != -1)
        {
            Destroy(gameObject);
        }
    }
}