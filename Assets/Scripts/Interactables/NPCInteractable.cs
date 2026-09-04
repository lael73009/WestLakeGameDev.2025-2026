using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;


public class NPCInteractable : Interactable
{
    private string[] dialogueText = new string[] {"Hello, I am an NPC! Here to be non-playable and a character!", "Not much else though... Anyhoo, how are you?", "Great! It's a nice outside, isn't it---you should go!", "Aw, darn. Well, tomorrow's a new day! Maybe get some time outside, hang out with friends..."};
    public DialogueChoice[] dialogueChoices;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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