using UnityEngine;

public class test : interactable
{
    public override string name { get; set; } = "test";

    public override void Interact()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
    }
}