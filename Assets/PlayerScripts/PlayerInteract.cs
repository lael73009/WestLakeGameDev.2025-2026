using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            
            Interact();
        }
    }

    public void Interact(interactable interactable)
    {
        interactable.Interact();
    }
}