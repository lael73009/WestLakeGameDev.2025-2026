using UnityEngine;

public abstract class interactable : MonoBehaviour
{

    public abstract string Name {get; set;}
    public abstract void Interact();

}