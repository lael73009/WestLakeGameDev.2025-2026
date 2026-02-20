using UnityEngine;

public abstract class interactable : MonoBehaviour
{

    public abstract string name {get; set;}
    public abstract void Interact();

}