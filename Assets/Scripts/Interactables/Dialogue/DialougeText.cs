using UnityEngine;
using TMPro;
using System.Collections;
public class DialogueText : MonoBehaviour
{
    // Display
    public TextMeshProUGUI _textComponent;

    private string[] _script;
    private float _textSpeed = 0.05f;
    private int _index; 
    public bool _choicesAreActive = false;

    public System.Action _OnDialogueTextFinished;

    // Update is called once per frame
    void Update()
    {   
        // Keep text on screen while player chooses 
        if (_choicesAreActive)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            // Ensure script exists
            if (_script == null)
            {
                return;
            }
            if (_textComponent.text == _script[_index])
            {
                NextLine();
            }
            // If all lines have been printed, finish
            else
            {
                StopAllCoroutines();
                _textComponent.text = _script[_index];
            }
        }
    }

    public void SetScript(string[] newScript)
    {
        // Reset text
        _index = 0;
        _script = newScript;
    }

    public void SetText(string text)
    {
        // Set a specific text for certian instance
        _textComponent.text = text;
    }

    public void SetTextComponent(TextMeshProUGUI text)
    {
        // Chamge text
        _textComponent = text;
        _textComponent.text = string.Empty;
    }

    public int GetIndex()
    {
        // Give index
        return _index;
    }

    public string GetCurrentText()
    {
        // Give current displayed text
        return _textComponent.text;
    }

    public string[] GetScript()
    {
        return _script;
    }

    public void StartDialogue()
    {
        // Reset text
        gameObject.SetActive(true);
        _textComponent.text = string.Empty;
        _index = 0;

        // Begin typing
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        // Type line by each character
        foreach (char c in _script[_index].ToCharArray())
        {
            _textComponent.text += c;
            yield return new WaitForSeconds(_textSpeed);
        }
    }

    void NextLine()
    {
        // Reset to move to the next line
        if (_index < _script.Length - 1)
        {
            _index++;
            _textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        // Otherwise, finish and reset
        else
        {
            _OnDialogueTextFinished?.Invoke();
        }
    }

    public void EndDialogueText()
    {
        // Disappear and reset
        StopAllCoroutines();
        _textComponent.text = string.Empty;

        _script = null;
        _index = 0;
    }
}