using UnityEngine;
using TMPro;
using System.Collections;
public class DialogueText : MonoBehaviour
{
    // Display
    public TextMeshProUGUI textComponent;

    private string[] script;
    private float textSpeed = 0.05f;
    private int index; 
    public bool isWaiting = true;

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetMouseButtonDown(0))
        {
            // Ensure script exists
            if (script == null)
            {
                return;
            }
            Debug.Log("[Setting Text] " + this.GetHashCode() + " " + (textComponent == null) + " [" + index + "] " + script[index]);
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

    public void SetText(string text)
    {
        // Set a specific text for certian instance
        textComponent.text = text;
    }

    public void SetTextComponent(TextMeshProUGUI text)
    {
        // Chamge text
        textComponent = text;
        textComponent.text = string.Empty;
    }

    public int GetIndex()
    {
        // Give index
        return index;
    }

    public string GetCurrentText()
    {
        // Give current displayed text
        return textComponent.text;
    }

    public string[] GetScript()
    {
        return script;
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
        Debug.Log("Typing : " + script[index]);
        foreach (char c in script[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        // Reset to move to the next line
        if (index < script.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else if (isWaiting)
        {
            Debug.Log("Waiting...");
        }
        // Otherwise, finish and reset
        else
        {
            gameObject.SetActive(false);
            DialogueController.Instance.EndDialogue();

            textComponent.text = string.Empty;
            script = null;
            index = 0;
        }
    }
}