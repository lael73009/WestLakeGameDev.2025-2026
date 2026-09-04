using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    // Allow other objects to access the general class without a specific identifier
    public static DialogueController Instance;
    private GameObject dialogueContainer;

    private Dialogue dialogue;
    
    public bool IsDialogueActive = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // Create an instance of the manager
        GameObject controllerObject = new GameObject("DialogueController");
        controllerObject.AddComponent<DialogueController>();
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

        // Create UI Objects
        dialogueContainer = CreateDialogueBox(canvasObject);
        TextMeshProUGUI dialogueTextObject = CreateDialogueText(dialogueContainer);
        GameObject dialogueChoicesObject = CreateDialogueChoices(dialogueContainer);

        // Create text
        DialogueText dialogueText = dialogueContainer.AddComponent<DialogueText>();
        dialogueText.SetTextComponent(dialogueTextObject);
        dialogueContainer.SetActive(false);

        // Create choices 
        DialogueChoice dialogueChoice = dialogueContainer.AddComponent<DialogueChoice>();
        dialogueChoice.SetChoiceContainer(dialogueChoicesObject);
        dialogueChoice.SetFont(dialogueTextObject.font);

        // Set dialogue
        dialogue = new Dialogue(dialogueText, dialogueChoice);
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
        boxRect.anchorMax = new Vector2(1f, 0f);
        boxRect.offsetMin = new Vector2(10f, 10);
        boxRect.offsetMax = new Vector2(-10f, 210f);

        return dialogueBoxObject;
    }

    // TO DO: MAKE CHANGEABLE PARAMATERS
    private  TextMeshProUGUI CreateDialogueText(GameObject parent)
    {
        // Create text object
        GameObject textObject = new GameObject("DialogueText");
        textObject.transform.SetParent(parent.transform);
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();

        // Set text look (size, color, ect)
        text.fontSize = 24f;
        text.color = Color.white;
        text.raycastTarget = false;

        // Set text location
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0f, 0);
        textRect.anchorMax = new Vector2(1f, 1f);
        textRect.offsetMin = new Vector2(10f, 10f);
        textRect.offsetMax = new Vector2(-10f, -10f);

        return text;
    }

    private GameObject CreateDialogueChoices(GameObject parent)
    {
        // Create choices object
        GameObject choicesObject = new GameObject("DialogueChoices");
        choicesObject.transform.SetParent(parent.transform);

        // Set layout (listed vertically and at bottom)
        VerticalLayoutGroup layout = choicesObject.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 5f;
        layout.childAlignment = TextAnchor.LowerLeft;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        ContentSizeFitter fitter = choicesObject.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        // Set choices location
        RectTransform choicesRect = choicesObject.GetComponent<RectTransform>();

        choicesRect.anchorMin = new Vector2(0f, 0f);
        choicesRect.anchorMax = new Vector2(1f, 0f);
        choicesRect.pivot = new Vector2(0.5f, 0f);
        choicesRect.offsetMin = new Vector2(10f, 0f);
        choicesRect.offsetMax = new Vector2(-10f, 0f);
        choicesRect.anchoredPosition = new Vector2(0f, 10f);

        return choicesObject;
    }

/*    private void ConvertDataToDialogue(DialogueData data)
    {
        for (int i=0; )
    }
*/
    public void StartDialogue(DialogueData data)
    {

        int currentSeq = 0; // TODO: THIS IS A CONTROLLED MEMBER VARIALBE
        //foreach (string[] seq in data.script) {
        dialogue.GetDialogueText().SetScript(data.script[currentSeq]);
            //dialogue.SetDialogueChoices(data.choice);
        //}
        dialogue.StartDialogue();

    }

    public void StartDialogueTest()
    {
        string[][] script = new string[][] {new string[] {"Hello, I am an NPC! Here to be non-playable and a character!", 
                                "Not much else though... Anyhoo, how are you?", "EEEEEEEQQQQQ"},
                                new string[] {"Great! It's a nice outside, isn't it---you should go!",
                                "Aw, darn. Well, tomorrow's a new day! Maybe get some time outside, hang out with friends..."}};
        string[][] choices = new string[][] {new string[]{"Good", "Bad"}, 
                                            new string[]{"2", "3"}};
        DialogueData testDialogueData = new DialogueData(script, choices);
    

        StartDialogue(testDialogueData);
    }

    public void EndDialogue()
    {
        // Allow other objects to read diaogue
        IsDialogueActive = false;
    }

    /*
    public void ChooseDialogue(int nextIndex, int nextEndIndex)
    {
        //dialogue.ContinueFromChoice(nextIndex, nextEndIndex);
    }

    // TO DO: REMOVE, TESTING METHOD
    public void DisplayPresetChoices()
    {
        DisplayChoices(new string[] { "Good", "Bad" }, new int[] { 2, 3 }, new int[] { 3, 4 });
    }
    */
}