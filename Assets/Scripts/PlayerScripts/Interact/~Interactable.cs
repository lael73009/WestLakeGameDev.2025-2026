using UnityEngine;

public abstract class interactable : MonoBehaviour
{

    public abstract string _Name {get; set;}
    public abstract void Interact();

}