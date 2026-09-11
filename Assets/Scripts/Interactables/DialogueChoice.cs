using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueChoices : MonoBehaviour
{
    public System.Action<int> OnDialogueChoiceSelected;

    // Display
    private GameObject choiceContainer;
    public TMP_FontAsset textFont;

    private (string text, int nextIndex)[] choices;

    public void SetChoices((string, int)[] newChoices)
    {
        choices = newChoices;
    }

    public void SetChoices(string[] newTexts, int[] newIndexes)
    {
        // Ensure all can be matched up
        if (!(newTexts.Length == newIndexes.Length))
        {
            Debug.LogError("Error: Mismatched choices array lengths. All input arrays should have the same length such that index i corresponds to the same choice on the text and nextIndex array.");
            return;
        }

        // Set variables
        choices = new (string, int)[newTexts.Length];
        for(int i=0; i<choices.Length; i++)
        {
            choices[i].Item1 = newTexts[i];
            choices[i].Item2 = newIndexes[i];
        }
    }

    public void SetChoiceContainer(GameObject Container)
    {
        // Set
        choiceContainer = Container;
    }

    public void SetFont(TMP_FontAsset font)
    {
        textFont = font;
    }

    public (string, int)[] GetChoices()
    {
        return choices;
    }

    public void DisplayChoices()
    {
        // Clear container 
        foreach (Transform child in choiceContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Set container to hold choices
        for (int i = 0; i < choices.Length; i++)
        {
            CreateChoiceButton(choices[i].Item1, choices[i].Item2);
        }

        choiceContainer.SetActive(true);
        //LayoutRebuilder.ForceRebuildLayoutImmediate(choiceContainer.GetComponent<RectTransform>());
    }

    private void CreateChoiceButton(string choiceText, int choiceIndex)
    {
        GameObject buttonObject = new GameObject("Choice_" + choiceIndex, typeof(RectTransform));
        buttonObject.transform.SetParent(choiceContainer.transform, false);

        // Button RectTransform
        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();

        buttonRect.anchorMin = new Vector2(0f, 0.5f);
        buttonRect.anchorMax = new Vector2(1f, 0.5f);
        buttonRect.pivot = new Vector2(0.5f, 0.5f);
        buttonRect.offsetMin = Vector2.zero;
        buttonRect.offsetMax = Vector2.zero;

        // Button
        Button button = buttonObject.AddComponent<Button>();

        // Invisible Image used for raycasting/click detection
        Image image = buttonObject.AddComponent<Image>();
        image.color = Color.clear;
        image.raycastTarget = true;

        // Choice text
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(buttonObject.transform, false);

        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();

        text.font = textFont;
        text.text = choiceText;
        text.fontSize = 24f;
        text.color = Color.white;
        text.alignment = TextAlignmentOptions.Left;
        text.raycastTarget = false;

        // Make text fill the button
        RectTransform textRect = textObject.GetComponent<RectTransform>();

        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        // Give the button a predictable height
        LayoutElement layoutElement = buttonObject.AddComponent<LayoutElement>();
        layoutElement.preferredHeight = 30f;
        layoutElement.minHeight = 30f;

        // Store the index locally so the lambda uses the correct choice
        button.onClick.AddListener(() =>
        {
            ChooseDialogue(choiceIndex);
        });
    }

    private void ChooseDialogue(int choiceIndex)
    {
        OnDialogueChoiceSelected?.Invoke(choiceIndex);
    }

    public void HideChoices()
    {
        // Remove when done
        choiceContainer.SetActive(false);
    }
}
