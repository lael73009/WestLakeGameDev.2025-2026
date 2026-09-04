using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueChoice : MonoBehaviour
{
    // Display
    private GameObject choiceContainer;
    public TMP_FontAsset textFont;

    public string[] choices;
    public int[] nextDialogueIndexes;
    public int[] nextDialogueEndIndexes;

    public void SetChoices(string[] newChoices, int[] newIndexes, int[] newEndIndexes)
    {
        // Ensure all can be matched up
        if (!(newChoices.Length == newIndexes.Length && newIndexes.Length == newEndIndexes.Length))
        {
            Debug.LogError("Error: bad choice inputs---check array lengths");
            return;
        }

        // Set variables
        choices = newChoices;
        nextDialogueIndexes = newIndexes;
        nextDialogueEndIndexes = newEndIndexes;
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
            CreateChoiceButton(choices[i], i);
        }
        choiceContainer.SetActive(true);
    }

    private void CreateChoiceButton(string choiceText, int choiceIndex)
    {
        // Create button
        GameObject buttonObject = new GameObject("Choice_" + choiceIndex);
        buttonObject.transform.SetParent(choiceContainer.transform, false);

        Button button = buttonObject.AddComponent<Button>();
        Image image = buttonObject.AddComponent<Image>();
        image.color = Color.clear;
        image.raycastTarget = true;

        // Create text
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(buttonObject.transform, false);
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();

        text.font = textFont;
        text.text = choiceText;
        text.fontSize = 24f;
        text.color = Color.white;
        text.alignment = TextAlignmentOptions.Left;

        // Make the text fill the button
        RectTransform textRect = text.GetComponent<RectTransform>();

        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        LayoutElement layoutElement = buttonObject.AddComponent<LayoutElement>();
        layoutElement.preferredHeight = text.preferredHeight;

        // Set up choice
        int index = choiceIndex;
        button.onClick.AddListener(() => Choose(index));
    }

    private void Choose(int choice)
    {
        Debug.Log("Selected " + choice);

        //DialogueController.Instance.StartDialogue(nextDialogueIndexes[choice], nextDialogueEndIndexes[choice]);

        HideChoices();
    }

    public void HideChoices()
    {
        // Remove when done
        choiceContainer.SetActive(false);
    }
}
