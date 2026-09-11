using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;

public class DialogueData
{
    // TODO: MAKE THE SCRIPT MATRIX AND CHOICES MATRIX AN ARRAY TO ALLOY FOR DIALOGUE OPTIONS WITH NO CHOICES
    public string[][] script;

    // First row is the choice text, second is the script index that choice will go to 
    public (string text, int nextIndex)[][] choices;

    public DialogueData(string[][] newScript, (string, int)[][] newChoices)
    {
        script = newScript;

        // Should only have 2 rows equal in length --- TODO: ADD CHECK 
        choices = newChoices;
    }

    public void SetScript(string[][] newScript)
    {
        script = newScript;
    }

    public void SetChoices((string, int)[][] newChoices)
    {
        choices = newChoices;
    }

    public string[][] GetScript()
    {
        return script;
    }

    public (string, int)[][] GetChoices()
    {
        return choices;
    }

    public string[] GetChoicesTextsForRow(int index)
    {
        string[] texts = new string[choices[index].Length];

        for(int i=0; i<choices.Length; i++)
        {
            texts[i] = choices[index][i].Item1;
        }

        return texts;
    }

    public int[] GetChoicesStartIdexesForRow(int index)
    {
        int[] indexes = new int[choices[index].Length];

        for(int i=0; i<choices.Length; i++)
        {
            indexes[i] = choices[index][i].Item2;
        }

        return indexes;
    }

}