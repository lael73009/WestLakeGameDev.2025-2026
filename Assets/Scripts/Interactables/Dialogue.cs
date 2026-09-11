using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;

public class Dialogue
{
    // Text
    private DialogueText text;
    // Choices
    private DialogueChoices choice;
    private TaskCompletionSource<int> playerChoice;

    public Dialogue(DialogueText newText, DialogueChoices newChoice)
    {
        text = newText;
        choice = newChoice;

        Debug.Log("SUBSCRIBING TO CHOICE EVENT");

        choice.OnDialogueChoiceSelected += SelectChoice;
        text.OnDialogueTextFinished += DisplayChoices;
    }

    public void SetScript(string[] newScript)
    {
        text.SetScript(newScript);
    }

    public void SetChoices((string, int)[] newChoices)
    {
        choice.SetChoices(newChoices);
    }

    public void SetChoices(string[] texts, int[] newIndexes)
    {
        choice.SetChoices(texts, newIndexes);
    }

    public void SetDialogueText(DialogueText newText)
    {
        text = newText;
    }

    public void SetDialogueChoices(DialogueChoices newChoice)
    {
        choice = newChoice;
    }

    public DialogueText GetDialogueText()
    {
        return text;
    }

    public DialogueChoices GetDialogueChoices()
    {
        return choice;
    }

    public void StartDialogue()
    {
        text.StartDialogue();
    }

    public Task<int> ChooseNextDialogue()
    {
        playerChoice = new TaskCompletionSource<int>();

        Debug.Log("playerChoice is null: " + (playerChoice == null));
        return playerChoice.Task;
    }

    public void SelectChoice(int nextDialogue)
    {
        Debug.Log("1. SelectChoice: " + nextDialogue);

        if (playerChoice == null)
        {
            Debug.LogError("ERROR: playerChoice is null when a choice was selected!");
            return;
        }

        text.choicesAreActive = false;
        text.EndDialogueText();
        choice.HideChoices();

        TaskCompletionSource<int> currentChoice = playerChoice;
        playerChoice = null;
        currentChoice.TrySetResult(nextDialogue);
    }

    public void DisplayChoices()
    {
        Debug.Log("=== DISPLAY CHOICES ===");
        Debug.Log("Current choices:");

        foreach (var c in choice.GetChoices())
        {
            Debug.Log("Choice: " + c.Item1 + " -> " + c.Item2);
        }

        text.choicesAreActive = true;
        choice.DisplayChoices();
    }
}