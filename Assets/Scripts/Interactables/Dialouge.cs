using UnityEngine;
using TMPro;
using System.Collections;
using System;
using NUnit.Framework.Interfaces;

public class Dialogue : MonoBehaviour
{

    public TextMeshProUGUI textComponent;

    private string[] script;
    private float textSpeed = 0.05f;
    private int index; 

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetMouseButtonDown(0))
        {
            // Move on to next line if player is clicking
            if (textComponent.text == script[index])
            {
                NextLine();
            }
            // If all lines have been printed, finish
            else
            {
                StopAllCoroutines();
                textComponent.text = script[index];
            }
        }
    }

    public void SetScript(string[] newScript)
    {
        // Reset text
        index = 0;
        script = newScript;
    }

    public void SetTextComponent(TextMeshProUGUI text)
    {
        textComponent = text;
        textComponent.text = string.Empty;
    }

    public void StartDialogue()
    {
        // Reset text
        gameObject.SetActive(true);
        textComponent.text = string.Empty;
        index = 0;

        // Begin typing
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        // Type line by each character
        foreach (char c in script[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine ()
    {
        // Reset to move to the next line
        if (index < script.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        // Otherwise, finish and reset
        else
        {
            gameObject.SetActive(false);
            DialogueManager.Instance.EndDialogue();

            textComponent.text = string.Empty;
            script = null;
            index = 0;
        }
    }
}