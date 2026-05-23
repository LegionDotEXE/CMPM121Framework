using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelectorMenu : MonoBehaviour
{
    public GameObject playerClassUI;
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
        List<JToken> classes = ClassInfo.Instance.classes;
        foreach (var c in classes)
        {
            string className = c.ToString();
            int index = className.IndexOf(":");
            GameObject nameDisplay = new GameObject(className.Substring(0, index));
            messageText = nameDisplay.GetComponent<TextMeshProUGUI>();
        }
    }

    public void SetClass(string className)
    {
        playerClass = className;
    }
}