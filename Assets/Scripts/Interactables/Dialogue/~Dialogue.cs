// WARNING: THIS FILE IS NO LONGER IN USE
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;

public class Dialogue
{
    // Text
    private DialogueText _text;

    // Choices
    private DialogueChoices _choices;
    private TaskCompletionSource<int> _playerChoice;

    public Dialogue(DialogueText newText, DialogueChoices newChoice)
    {
        _text = newText;
        _choices = newChoice;

        // Add methods for the asyncs to call
        _choices._OnDialogueChoiceSelected += SelectChoice;
        _text._OnDialogueTextFinished += DisplayChoices;
    }

    public void SetScript(string[] newScript)
    {
        _text.SetScript(newScript);
    }

    public void SetChoices((string, int)[] newChoices)
    {
        _choices.SetChoices(newChoices);
    }

    public void SetChoices(string[] texts, int[] newIndexes)
    {
        _choices.SetChoices(texts, newIndexes);
    }

    public void SetDialogueText(DialogueText newText)
    {
        _text = newText;
    }

    public void SetDialogueChoices(DialogueChoices newChoice)
    {
        _choices = newChoice;
    }

    public DialogueText GetDialogueText()
    {
        return _text;
    }

    public DialogueChoices GetDialogueChoices()
    {
        return _choices;
    }

    public void StartDialogue()
    {
        // Begin text
        Debug.Log("2. Dialogue method...");
        _text.StartDialogue();
    }

    public Task<int> ChooseNextDialogue()
    {
        // Assign task
        _playerChoice = new TaskCompletionSource<int>();
        return _playerChoice.Task;
    }

    public void SelectChoice(int nextDialogue)
    {
        if (_playerChoice == null)
        {
            Debug.LogError("ERROR: playerChoice is null when a choice was selected!");
            return;
        }

        // Remove this dialogue's elements
        _text._choicesAreActive = false;
        _text.EndDialogueText();
        _choices.HideChoices();

        // Spit out choice
        TaskCompletionSource<int> currentChoice = _playerChoice;
        _playerChoice = null;
        currentChoice.TrySetResult(nextDialogue);
    }

    public void DisplayChoices()
    {
        // Show choices
        _text._choicesAreActive = true;
        _choices.DisplayChoices();
    }
}