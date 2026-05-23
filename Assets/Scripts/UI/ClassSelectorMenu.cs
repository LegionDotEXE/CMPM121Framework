using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelectorMenu : MonoBehaviour
{
    public GameObject playerClassUI;
    public GameObject button;
    public string playerClass;
    private TextMeshProUGUI buttonText;
    private TextMeshProUGUI messageText;

    void Start()
    {
        buttonText = playerClassUI.GetComponentInChildren<TextMeshProUGUI>(true);
        GameObject message = new GameObject("Class Select");
        message.transform.SetParent(playerClassUI.transform, false);
        messageText = message.AddComponent<TextMeshProUGUI>();
        messageText.alignment = TextAlignmentOptions.Center;
        messageText.fontSize = 32;
        messageText.color = Color.black;

        RectTransform rect = messageText.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.15f, 0.55f);
        rect.anchorMax = new Vector2(0.85f, 0.85f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        ClassInfo.Instance.selectedClass = "mage";

        CreateMenu();

        playerClassUI.SetActive(false);
    }

    void Update()
    {
        if(playerClassUI.activeSelf == false && GameManager.Instance.state == GameManager.GameState.PREGAME)
            playerClassUI.SetActive(true);
        else if(GameManager.Instance.state != GameManager.GameState.PREGAME) playerClassUI.SetActive(false);

    }
    public void CreateMenu()
    {
        if (button == null) return;

        int i = 0;
        foreach (var entry in ClassInfo.Instance.classes)
        {
            string className = ((JProperty)entry).Name;
            GameObject selector = Instantiate(button, playerClassUI.transform);
            selector.transform.localPosition = new Vector3(0, 40 - i * 40, 0);

            TextMeshProUGUI label = selector.GetComponentInChildren<TextMeshProUGUI>();
            if (label != null) label.text = "Class: " + className;

            string captured = className;
            selector.GetComponent<Button>().onClick.RemoveAllListeners();
            selector.GetComponent<Button>().onClick.AddListener(() => SetClass(captured));
            i++;
        }
    }

    public void SetClass(string className)
    {
        ClassInfo.Instance.selectedClass = className;
        if (messageText != null)
            messageText.text = "Class: " + className + " selected!";
    }
}