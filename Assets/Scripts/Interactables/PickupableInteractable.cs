using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;


public class PickupableInteractable : Interactable
{
    private Pickupable item;
    private Sprite sprite; 

    void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError(gameObject.name + " needs a SpriteRenderer.");
            return;
        }

        sprite = spriteRenderer.sprite;
        item = new Pickupable();
        item.sprite = sprite;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();

        Debug.Log("Inventory: " + Inventory.Instance);
        Debug.Log("Item: " + item);
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
    }

    override public void OnCollision(Collider2D collidedObject)
    {
        Inventory.Instance.AddItem(item);
        Destroy(gameObject);
    }
}