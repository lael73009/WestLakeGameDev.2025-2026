// TODO: MERGE WITH DIALOGUECONTROLLER CLASS
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
    private DialogueChoices choices;
    private TaskCompletionSource<int> playerChoice;

    public Dialogue(DialogueText newText, DialogueChoices newChoice)
    {
        text = newText;
        choices = newChoice;

        // Add methods for the asyncs to call
        choices.OnDialogueChoiceSelected += SelectChoice;
        text.OnDialogueTextFinished += DisplayChoices;
    }

    public void SetScript(string[] newScript)
    {
        text.SetScript(newScript);
    }

    public void SetChoices((string, int)[] newChoices)
    {
        choices.SetChoices(newChoices);
    }

    public void SetChoices(string[] texts, int[] newIndexes)
    {
        choices.SetChoices(texts, newIndexes);
    }

    public void SetDialogueText(DialogueText newText)
    {
        text = newText;
    }

    public void SetDialogueChoices(DialogueChoices newChoice)
    {
        choices = newChoice;
    }

    public DialogueText GetDialogueText()
    {
        return text;
    }

    public DialogueChoices GetDialogueChoices()
    {
        return choices;
    }

    public void StartDialogue()
    {
        // Begin text
        Debug.Log("2. Dialogue method...");
        text.StartDialogue();
    }

    public Task<int> ChooseNextDialogue()
    {
        // Assign task
        playerChoice = new TaskCompletionSource<int>();
        return playerChoice.Task;
    }

    public void SelectChoice(int nextDialogue)
    {
        if (playerChoice == null)
        {
            Debug.LogError("ERROR: playerChoice is null when a choice was selected!");
            return;
        }

        // Remove this dialogue's elements
        text.choicesAreActive = false;
        text.EndDialogueText();
        choices.HideChoices();

        // Spit out choice
        TaskCompletionSource<int> currentChoice = playerChoice;
        playerChoice = null;
        currentChoice.TrySetResult(nextDialogue);
    }

    public void DisplayChoices()
    {
        // Show choices
        text.choicesAreActive = true;
        choices.DisplayChoices();
    }
}