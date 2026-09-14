using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;

public class DialogueData
{
    public (string[] script, (string, int)[] choices)[] dialogueData;

    //public string[][] script;
    //public (string text, int nextIndex)[][] choices;

    public DialogueData((string[] script, (string, int)[] choices)[] newDialogue)
    {
        dialogueData = newDialogue;
    }

    public DialogueData(string[][] newScript, (string, int)[][] newChoices)
    {
        // Ensure correct parallel arrays
        if (!(newScript.Length == newChoices.Length))
        {
            Debug.LogError("Error: mismatched script and choice lengths");
            return;
        }

        // Add each script and choice set to the dialogueData object one by one
        dialogueData = new (string[] script, (string, int)[] choices)[newScript.Length];
        for (int i=0; i<dialogueData.Length; i++)
        {
            dialogueData[i] = (newScript[i], newChoices[i]);
        }
    }

    public void setDialogue((string[] script, (string, int)[] choices)[] newDialogue)
    {
        dialogueData = newDialogue;
    }

    public void SetDialogue(string[][] newScript, (string, int)[][] newChoices)
    {
        // Ensure correct parallel arrays
        if (!(newScript.Length == newChoices.Length))
        {
            Debug.LogError("Error: mismatched script and choice lengths");
            return;
        }

        // Add each script and choice set to the dialogueData object one by one
        dialogueData = new (string[] script, (string, int)[] choices)[newScript.Length];
        for (int i=0; i<dialogueData.Length; i++)
        {
            dialogueData[i] = (newScript[i], newChoices[i]);
        }
    }

    public string[] GetScriptAtIndex(int row)
    {
        return dialogueData[row].Item1;
    }

    public (string, int)[] GetChoicesAtIndex(int row)
    {
        return dialogueData[row].Item2;
    }

    // WARNING: THIS METHOD HAS NOT BEEN TESTED
    public string[] GetChoicesTextsAtIndex(int index)
    {
        // Loops through the all choices at that index and collects the text 
        string[] texts = new string[dialogueData.Length];

        for(int i=0; i<dialogueData.Length; i++)
        {
            texts[i] = dialogueData[index].Item2[i].Item1;
        }

        return texts;
    }

    // WARNING: THIS METHOD HAS NOT BEEN TESTED
    public int[] GetChoicesNextIdexesAtIndex(int index)
    {
        // Loops through the all choices at that index and collects the next indexes 
        int[] indexes = new int[dialogueData.Length];

        for(int i=0; i<dialogueData.Length; i++)
        {
            indexes[i] = dialogueData[index].Item2[i].Item2;
        }

        return indexes;
    }

    // WARNING: THIS METHOD HAS NOT BEEN TESTED
    public string[][] GetAllScripts()
    {
        // Loops through and collects the scripts for each index
        string[][] scripts = new string[dialogueData.Length][];
        
        for (int i=0; i<dialogueData.Length; i++)
        {
            scripts[i] = GetScriptAtIndex(i);
        }

        return scripts;
    }

    // WARNING: THIS METHOD HAS NOT BEEN TESTED
    public (string, int)[][] GetAllChoices()
    {
        // Loops through and collects the choices for each index
        (string, int)[][] choices = new (string, int)[dialogueData.Length][];
        
        for (int i=0; i<dialogueData.Length; i++)
        {
            choices[i] = GetChoicesAtIndex(i);
        }

        return choices;
    }
}