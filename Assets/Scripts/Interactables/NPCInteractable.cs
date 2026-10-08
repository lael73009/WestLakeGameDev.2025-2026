using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;


public class NPCInteractable : Interactable
{
    public DialogueData dialogueData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
    }

    override public void OnCollision(Collider2D collidedObject)
    {
        // Start dialogue
        DialogueController.Instance.StartDialogueTest();
        //Debug.Log("What is happening");
    }
}