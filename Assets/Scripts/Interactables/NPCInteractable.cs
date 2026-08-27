using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class NPCInteractable : Interactable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        //dialogue.setScript(new string[] {"Hi,", "how are you"});
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
    }

    override public void OnCollision(Collider2D collidedObject)
    {
        DialogueManager.Instance.StartDialogue(new string[] {"Hello, I am an NPC! Here to be non-playable and a character!", "Not much else though..."});
    }
}
