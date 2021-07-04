using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public event EventHandler OnNextLine;

    private TextMeshProUGUI spearkerText;
    private TextMeshProUGUI dialogueText;
    private Button nextButton;
    private Button closeButton;

    private void Awake()
    {
        dialogueText = transform.Find("dialogue_text").GetComponent<TextMeshProUGUI>();
        
        nextButton = transform.Find("next_button_holder")
            .Find("next_button")
            .GetComponent<Button>();
        nextButton.onClick.AddListener(() => OnNextLine?.Invoke(this, EventArgs.Empty));

        closeButton = transform.Find("close_button_holder")
            .Find("close_button")
            .GetComponent<Button>();
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));

        gameObject.SetActive(false);
    }

    public void Display(DialogueNode dialogueNode)
    {
        gameObject.SetActive(true);

        dialogueText.text = dialogueNode.Text;

        if(dialogueNode.NextDialogueNodes.Count > 0)
            nextButton.gameObject.SetActive(true);
        else
            nextButton.gameObject.SetActive(false);
    }
}
