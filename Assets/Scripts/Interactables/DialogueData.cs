using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogueData
{
    public string[][] script;

    // First row is the choice text, second is the script index that choice will go to 
    public string[][] choices;

    public DialogueData(string[][] newScript, string[][] newChoices)
    {
        script = newScript;

        // Should only have 2 rows equal in length --- TODO: ADD CHECK 
        choices = newChoices;
    }

    public void SetScript(string[][] newScript)
    {
        script = newScript;
    }

    public void SetChoices(string[][] newChoices)
    {
        choices = newChoices;
    }

    public string[][] GetScript()
    {
        return script;
    }

    public string[][] GetChoices()
    {
        return choices;
    }

    public string[] GetChoiceOptions()
    {
        return choices[0];
    }

    /* public int[] GetChoiceStartIdexes()
    {
        int[] choiceIndexes = new int[choices[1].Length];

        for (int i=0; i<choices[1].Length; i++)
        {
            choiceIndexes[i] = int.Parse(choiceIndexes[1][i]);
        }

        return choiceIndexes;
    } */
}