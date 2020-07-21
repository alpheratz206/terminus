using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject dialogueResponse;
    public GameObject scrollBar;

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
        optionsList = dialogueUI.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
    }

    public void BeginDialogue(DialogueTree dialogue)
    {
        header.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.Name;

        if(dialogue?.Dialogue == null)
        {
            Debug.Log(dialogue.NoDialogueGreeting);
            return;
        }

        currentOptions.AddRange(dialogue.Dialogue);

        RefreshUI();

        dialogueUI.SetActive(true);
    }

    public void EndDialogue()
    {
        dialogueUI.SetActive(false);

        currentOptions.Clear();

        ClearDialogueHistory();
    }

    private void RefreshUI()
    {
        int i = 1;

        foreach(var option in currentOptions)
        {
            var optionUI = Instantiate(dialogueOption);

            optionUI.tag = "DialogueOption";
            optionUI.transform.SetParent(optionsList.transform);
            optionUI.GetComponentInChildren<TextMeshProUGUI>().text = $"{i++}. {option.Prompt}";
            optionUI.GetComponent<Button>().onClick.AddListener(() => EnterNode(option));
        }

        scrollBar.GetComponent<Scrollbar>().value = 0;
    }

    private void EnterNode(DialogueNode node)
    {
        foreach (Transform child in optionsList.transform)
        {
            if (child.gameObject.tag == "DialogueOption")
                Destroy(child.gameObject);
        }

        var optionSelectedUI = Instantiate(dialogueResponse);

        optionSelectedUI.transform.SetParent(optionsList.transform);
        optionSelectedUI.GetComponentInChildren<TextMeshProUGUI>().text = node.Prompt;
        optionSelectedUI.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Italic;

        StartCoroutine(SpeakDialogue(node));
    }

    private IEnumerator SpeakDialogue(DialogueNode node)
    {
        foreach(var line in node.Lines)
        { 
            scrollBar.GetComponent<Scrollbar>().value = 0;

            var responseUI = Instantiate(dialogueResponse);

            responseUI.transform.SetParent(optionsList.transform);
            responseUI.GetComponentInChildren<TextMeshProUGUI>().text = line.Text;

            yield return new WaitForSeconds(line.Length);
        }

        BranchDialogue(node);
    }

    private void BranchDialogue(DialogueNode node)
    {
        node.ExecuteAction();

        if(node.Dialogue != null)
        {
            currentOptions = currentOptions.Where(x => x.Persist).ToList();
            currentOptions.AddRange(node.Dialogue);
        }

        RefreshUI();
    }

    private void ClearDialogueHistory()
    {
        foreach (Transform child in optionsList.transform)
            Destroy(child.gameObject);
    }
}
