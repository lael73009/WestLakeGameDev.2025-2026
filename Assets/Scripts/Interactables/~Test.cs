using UnityEngine;

public class test : interactable
{
    public override string Name { get; set; } = "test";

    public override void Interact()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
    }
}