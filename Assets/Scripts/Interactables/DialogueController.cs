using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;

public class DialogueController : MonoBehaviour
{
    // Allow other objects to access the general class without a specific identifier
    public static DialogueController Instance;
    private GameObject dialogueContainer;

    // Text
    private DialogueText text;
    // Choices
    private DialogueChoices choices;

    private TaskCompletionSource<int> playerChoice;
    
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
        text = dialogueContainer.AddComponent<DialogueText>();
        text.SetTextComponent(dialogueTextObject);
        dialogueContainer.SetActive(false);

        // Create choices 
        choices = dialogueContainer.AddComponent<DialogueChoices>();
        choices.SetChoiceContainer(dialogueChoicesObject);
        choices.SetFont(dialogueTextObject.font);
        
        // Add methods for the asyncs to call
        choices.OnDialogueChoiceSelected += SelectChoice;
        text.OnDialogueTextFinished += DisplayChoices;
    }

    private GameObject CreateDialogueBox(GameObject parent)
    {
        // Create box object
        GameObject dialogueBoxObject = new GameObject("DialogueBox");
        dialogueBoxObject.transform.SetParent(parent.transform);

        // Set box look (color, etc)
        UnityEngine.UI.Image dialogueBox = dialogueBoxObject.AddComponent<UnityEngine.UI.Image>();
        dialogueBox.color = Color.darkMagenta;
        dialogueBox.raycastTarget = false;

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
    async void StartDialogue(DialogueData data)
    {
        int current = 0;
        int timeout = 0;
        IsDialogueActive = true;

        // Loop though all dialogue, changing index based of player choice
        while (current >= 0 && current < data.dialogueData.Length) {
            text.SetScript(data.dialogueData[current].Item1);

            // Make dialogue the current index in DialogueData
            choices.SetChoices(data.dialogueData[current].Item2);
            Task<int> choiceTask = ChooseNextDialogue();
            text.StartDialogue();

            // Make the index the next choice
            current = await choiceTask;

            // Timeout to avoid infinite loops freezing Unity
            timeout++;
            if (timeout > 5000) 
            {
                Debug.LogError("Error: Infinite loop occured when displaying dialogue.");
                break; 
            }
        }

        EndDialogue();
    }

    public void StartDialogueTest()
    {
        // Dummy data for ease of testing
        (string[] script, (string, int)[] choices)[] dialogueData = new (string[], (string, int)[])[]
        {
            (new string[] {"Hello, I am an NPC! Here to be non-playable and a character!", "Not much else though... Anyhoo, how are you?"}, 
            new (string, int)[] {("Sorry, repeat that?", 0), ("Good.", 1), ("Bad.", 3)}), 
            (new string[] {"Great! It's a nice outside, isn't it---you should go!"}, 
            new (string, int)[] {}),
            (new string[] {"Oh no, the code is going to the incorrect index! Better fix that!"}, 
            new (string, int)[] {}),
            (new string[] {"Aw, darn. Well, tomorrow's a new day! Maybe get some time outside, hang out with friends..."}, 
            new (string, int)[] {})
        };
        /* string[][] script = new string[][] {new string[] {"Hello, I am an NPC! Here to be non-playable and a character!", 
                                "Not much else though... Anyhoo, how are you?"},
                                new string[] {"Great! It's a nice outside, isn't it---you should go!"},
                                new string[] {"Oh no, the code is going to the incorrect index! Better fix that!"},
                                new string[] {"Aw, darn. Well, tomorrow's a new day! Maybe get some time outside, hang out with friends..."}};
        (string, int)[][] choices = new (string, int)[][] {new (string, int)[] {("Sorry, repeat that?", 0), ("Good.", 1), ("Bad.", 3)}, 
                                                            new (string, int)[]{("Goodbye", -1)},
                                                            new (string, int)[]{("Whoops, I'll get on it...", -1)},
                                                            new (string, int)[]{("Goodbye", -1)}};
        DialogueData testDialogueData = new DialogueData(script, choices); */
        DialogueData testDialogueData = new DialogueData(dialogueData);
    

        StartDialogue(testDialogueData);
    }

    public Task<int> ChooseNextDialogue()
    {
        // Assign task
        playerChoice = new TaskCompletionSource<int>();
        return playerChoice.Task;
    }

    public void SelectChoice(int nextDialogue)
    {
        // Protect against null errors
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


    public void EndDialogue()
    {
        // Remove dialogue box and allow another to be created
        IsDialogueActive = false;
        dialogueContainer.SetActive(false);
    }
}