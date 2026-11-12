using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private Collider2D _collider;
    private ContactFilter2D _filter;
    private List<Collider2D> _collidedObjects = new List<Collider2D>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        _collider = GetComponent<Collider2D>();
        // TO DO: MAKE FILTER INTO ONLY THE PLAYER LAYER
        _filter = new ContactFilter2D().NoFilter();
    }

    public void CheckCollision()
    {
        // Ensure collider is filled in
        if (_collider == null)
        {
            Debug.LogError("Error: Interactible" + gameObject.name + "needs a Collider2D component to interact");
            return;
        }

        // Get all collisions
        _collidedObjects.Clear();
        _collider.Overlap(_filter, _collidedObjects);

        // Do something for each collision
        foreach(var collidedObject in _collidedObjects)
        {
            // Only collide if player is pressing E and the player isn't actively interacting with something (for now, same as dialogue)
            // NOTE: MAY NEED TO CHANGE "IS DIALOGUE ACTIVE CHECK" TO SOMETHING MORE ENCOMPASSING OF ALL INTERACTIBLES FOR FUTURE COMPLEX SCENARIOS
            if (!(collidedObject == null) && Input.GetKeyDown(KeyCode.E) && !DialogueController._Instance._IsDialogueActive) {
                OnCollision(collidedObject);
            }
        }
    }

     abstract public void OnCollision(Collider2D collidedObject);
}