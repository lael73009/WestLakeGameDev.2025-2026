using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Dialogue
{
    // Text
    private DialogueText text;

    // Choices
    private DialogueChoice choice;


    public Dialogue(DialogueText newText, DialogueChoice newChoice)
    {
        text = newText;
        choice = newChoice;
    }

    public void SetScript(string[] newScript)
    {
        text.SetScript(newScript);
    }

    public void SetChoices(string[] newChoices, int[] newIndexes, int[] newEndIndexes)
    {
        choice.SetChoices(newChoices, newIndexes, newEndIndexes);
    }

    public void SetDialogueText(DialogueText newText)
    {
        text = newText;
    }

    public void SetDialogueChoices(DialogueChoice newChoice)
    {
        choice = newChoice;
    }

    public DialogueText GetDialogueText()
    {
        return text;
    }

    public DialogueChoice GetDialogueChoices()
    {
        return choice;
    }

    public void StartDialogue()
    {
        if (choice != null)
        {
            text.isWaiting = true;
        }

        text.StartDialogue();

        if (text.GetIndex() == text.GetScript().Length - 1)
        {
            DisplayChoices();
        }
    }

    public void DisplayChoices()
    {
        choice.DisplayChoices();
    }
}