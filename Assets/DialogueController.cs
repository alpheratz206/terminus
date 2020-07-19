using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    #region Singleton
    public static DialogueController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Editor

    public GameObject dialogueUI;
    public GameObject dialogueOption;

    #endregion

    #region UI

    private GameObject header;
    private GameObject optionsList;

    #endregion

    #region Logic

    private List<DialogueNode> currentOptions
        = new List<DialogueNode>();

    #endregion

    private void Start()
    {
        header = dialogueUI.transform.GetChild(0).gameObject;
        optionsList = dialogueUI.transform.GetChild(1).gameObject;
    }

    public void BeginDialogue(DialogueTree dialogue)
    {
        header.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.Name;

        currentOptions.AddRange(dialogue.Dialogue);

        UpdateUI();

        dialogueUI.SetActive(true);
    }

    private void UpdateUI()
    {
        //remove existing options

        foreach(var option in currentOptions)
        {
            var optionUI = Instantiate(dialogueOption);

            optionUI.transform.SetParent(optionsList.transform);
            optionUI.GetComponentInChildren<TextMeshProUGUI>().text = option.Prompt;
            optionUI.GetComponent<Button>().onClick.AddListener(() => EnterNode(option));
        }
    }

    private void EnterNode(DialogueNode node)
    {
        Debug.Log("Entering new node!");
    }
}
