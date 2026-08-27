using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // Allow other objects to access the general class without a specific identifier
    public static DialogueManager Instance;
    private Dialogue dialogue;
    public bool IsDialogueActive = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // Create an instance of the manager
        GameObject managerObject = new GameObject("DialogueManager");
        managerObject.AddComponent<DialogueManager>();
    }
    void Awake()
    {
        Instance = this;

        // Create canvas
        GameObject canvasObject = new GameObject("DialogueCanvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // Create UI Objects (canvas, text, box)
        GameObject dialogueBox = CreateDialogueBox(canvasObject);
        TextMeshProUGUI text = CreateText(dialogueBox);

        // Create dialogue
        dialogue = dialogueBox.AddComponent<Dialogue>();
        dialogue.SetTextComponent(text);
        dialogueBox.SetActive(false);
    }

    // TO DO: MAKE CHANGEABLE PARAMATERS
    private  TextMeshProUGUI CreateText(GameObject parent)
    {
        // Create text object
        GameObject textObject = new GameObject("DialogueText");
        textObject.transform.SetParent(parent.transform);
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();

        // Set text look (size, color, ect)
        text.fontSize = 24f;
        text.color = Color.white;

        // Set text location
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0f, 0);
        textRect.anchorMax = new Vector2(1f, 1f);
        textRect.offsetMin = new Vector2(10f, 10f);
        textRect.offsetMax = new Vector2(-10f, -10f);

        return text;
    }

    private GameObject CreateDialogueBox(GameObject parent)
    {
        // Create box object
        GameObject dialogueBoxObject = new GameObject("DialogueBox");
        dialogueBoxObject.transform.SetParent(parent.transform);

        // Set box look (color, etc)
        UnityEngine.UI.Image dialogueBox = dialogueBoxObject.AddComponent<UnityEngine.UI.Image>();
        dialogueBox.color = Color.darkMagenta;

        // Set box location
        RectTransform boxRect = dialogueBoxObject.GetComponent<RectTransform>();
        boxRect.anchorMin = new Vector2(0f, 0f);
        boxRect.anchorMax = new Vector2(1f, 1f);
        boxRect.offsetMin = new Vector2(10f, 10);
        boxRect.offsetMax = new Vector2(-10f, -300f);

        return dialogueBoxObject;
    }

    /* NOTE: USE FOR BUG CHECKING
    void Start()
    {
        StartDialogue(new string[] {"Hi, how are you? I am pretty good. Yeah, I get that. Huh? You didn't say anything... I guess I'm hearing things. Maybe I should get that checked out.", "Oh! Sorry, I'm getting a call- *begins talking to imaginary phone*"});
    }
    */

    public void StartDialogue(string[] script)
    {
        IsDialogueActive = true;

        dialogue.SetScript(script);
        dialogue.StartDialogue();
    }

    public void EndDialogue()
    {
        IsDialogueActive = false;
    }
}