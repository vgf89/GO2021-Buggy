using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstructionCanvasController : MonoBehaviour
{
    [SerializeField]
    public List<TextMeshProUGUI> textMeshLists;

    [SerializeField] Button[] buttons;
    bool switchedTexts;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        switchedTexts = false;
    }

    private void OnEnable()
    {
        textMeshLists[0].enabled = true;
        textMeshLists[1].enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchTextMeshes()
    {
        for (int i = 0; i < textMeshLists.Count; i++)
            textMeshLists[i].enabled = !textMeshLists[i].enabled;

        switchedTexts = !switchedTexts;

        if (switchedTexts)
            buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Back";
        else
            buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Next";
    }


}
