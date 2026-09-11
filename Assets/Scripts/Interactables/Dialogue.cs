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
        Debug.Log("Waiting for player choice...");
        playerChoice = new TaskCompletionSource<int>();

        return playerChoice.Task;
    }

    public void SelectChoice(int nextDialogue)
    {
        Debug.Log("Player chose branch " + nextDialogue);

        text.EndDialogueText();
        choice.HideChoices();
        if (playerChoice != null)
        {
            playerChoice.TrySetResult(nextDialogue);
            playerChoice = null;
        }
    }

    public void DisplayChoices()
    {
        Debug.Log("Displaying choices");
        choice.DisplayChoices();
    }
}