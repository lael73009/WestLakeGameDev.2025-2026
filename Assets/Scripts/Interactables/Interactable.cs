using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private Collider2D collider;
    private ContactFilter2D filter;
    private List<Collider2D> collidedObjects = new List<Collider2D>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        collider = GetComponent<Collider2D>();
        filter = new ContactFilter2D().NoFilter();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CheckCollision()
    {
        collidedObjects.Clear();
        collider.Overlap(filter, collidedObjects);

        foreach(var collidedObject in collidedObjects)
        {
            // Only collide if player is pressing E and the player isn't actively interacting with something (for now, same as dialogue)
            // NOTE: MAY NEED TO CHANGE "IS DIALOGUE ACTIVE CHECK" TO SOMETHING MORE ENCOMPASSING OF ALL INTERACTIBLES FOR FUTURE COMPLEX SCENARIOS
            if (Input.GetKeyDown(KeyCode.E) && !DialogueManager.Instance.IsDialogueActive) {
                OnCollision(collidedObject);
            }
        }
    }

     abstract public void OnCollision(Collider2D collidedObject);
}