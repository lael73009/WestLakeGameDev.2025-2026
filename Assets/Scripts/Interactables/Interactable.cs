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
        // TO DO: MAKE FILTER INTO ONLY THE PLAYER LAYER
        filter = new ContactFilter2D().NoFilter();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CheckCollision()
    {
        // Ensure collider is filled in
        if (collider == null)
        {
            Debug.LogError("Error: Interactible" + gameObject.name + "needs a Collider2D component to interact");
            return;
        }

        // Get all collisions
        collidedObjects.Clear();
        collider.Overlap(filter, collidedObjects);

        // Do something for each collision
        foreach(var collidedObject in collidedObjects)
        {
            // Only collide if player is pressing E and the player isn't actively interacting with something (for now, same as dialogue)
            // NOTE: MAY NEED TO CHANGE "IS DIALOGUE ACTIVE CHECK" TO SOMETHING MORE ENCOMPASSING OF ALL INTERACTIBLES FOR FUTURE COMPLEX SCENARIOS
            if (!(collidedObject == null) && Input.GetKeyDown(KeyCode.E) && !DialogueController.Instance.IsDialogueActive) {
                OnCollision(collidedObject);
            }
        }
    }

     abstract public void OnCollision(Collider2D collidedObject);
}