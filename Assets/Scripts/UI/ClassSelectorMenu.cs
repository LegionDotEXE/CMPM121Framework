using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelectorMenu : MonoBehaviour
{
    public GameObject playerClassUI;
    public GameObject difficultySelector;

    private TextMeshProUGUI messageText;
    private bool classChosen = false;

    void Start()
    {
        // hide difficulty until class is picked
        if (difficultySelector != null)
            difficultySelector.SetActive(false);

        // show class panel
        if (playerClassUI != null)
            playerClassUI.SetActive(true);

        CreateMenu();
    }

    void Update()
    {
        // once game starts, hide class panel
        if (GameManager.Instance.state != GameManager.GameState.PREGAME)
        {
            if (playerClassUI != null)
                playerClassUI.SetActive(false);
        }
    }

    void CreateMenu()
    {
        // title text
        GameObject title = new GameObject("Title");
        title.transform.SetParent(playerClassUI.transform, false);
        TextMeshProUGUI titleText = title.AddComponent<TextMeshProUGUI>();
        titleText.text = "Choose your class";
        titleText.fontSize = 28;
        titleText.color = Color.black;
        titleText.alignment = TextAlignmentOptions.Center;
        RectTransform titleRect = title.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.2f, 0.7f);
        titleRect.anchorMax = new Vector2(0.8f, 0.9f);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;

        // confirmation message
        GameObject message = new GameObject("Message");
        message.transform.SetParent(playerClassUI.transform, false);
        messageText = message.AddComponent<TextMeshProUGUI>();
        messageText.fontSize = 20;
        messageText.color = Color.black;
        messageText.alignment = TextAlignmentOptions.Center;
        RectTransform msgRect = message.GetComponent<RectTransform>();
        msgRect.anchorMin = new Vector2(0.2f, 0.55f);
        msgRect.anchorMax = new Vector2(0.8f, 0.68f);
        msgRect.offsetMin = Vector2.zero;
        msgRect.offsetMax = Vector2.zero;

        // one button per class
        List<string> classes = new List<string>();
        foreach (var pClass in ClassInfo.Instance.classes)
            classes.Add(((Newtonsoft.Json.Linq.JProperty)pClass).Name);
        for (int i = 0; i < classes.Count; i++)
        {
            string className = classes[i];
            float xMin = 0.1f + i * 0.28f;
            float xMax = xMin + 0.24f;

            GameObject btn = new GameObject("Btn_" + className);
            btn.transform.SetParent(playerClassUI.transform, false);

            Image img = btn.AddComponent<Image>();
            img.color = new Color(0.75f, 0.85f, 1f);
            Button button = btn.AddComponent<Button>();

            RectTransform rt = btn.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(xMin, 0.35f);
            rt.anchorMax = new Vector2(xMax, 0.53f);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            GameObject labelObj = new GameObject("Label");
            labelObj.transform.SetParent(btn.transform, false);
            TextMeshProUGUI label = labelObj.AddComponent<TextMeshProUGUI>();
            label.text = className;
            label.fontSize = 22;
            label.color = Color.black;
            label.alignment = TextAlignmentOptions.Center;
            RectTransform labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;

            string captured = className;
            button.onClick.AddListener(() => SetClass(captured));
        }
    }

    public void SetClass(string className)
    {
        classChosen = true;
        ClassInfo.Instance.selectedClass = className;

        // change player sprite using SpriteRenderer
        JToken classValue = ((JProperty)ClassInfo.Instance.GetClass(className)).Value;
        if (classValue["sprite"] != null)
        {
            int spriteID = classValue["sprite"].ToObject<int>();
            SpriteRenderer sr = GameManager.Instance.player.GetComponentInChildren<SpriteRenderer>();
            if (sr == null)
                Debug.LogError("SpriteRenderer not found on player");
            else
                sr.sprite = GameManager.Instance.playerSpriteManager.Get(spriteID);
        }

        //int spriteID = ((JProperty)ClassInfo.Instance.GetClass(className)).Value["sprite"].ToObject<int>();
        //Image sprite = GameManager.Instance.player.AddComponent<Image>();
        //GameManager.Instance.playerSpriteManager.PlaceSprite(spriteID, sprite);

        if (messageText != null)
            messageText.text = className + " selected!";

        if (playerClassUI != null)
            playerClassUI.SetActive(false);
        if (difficultySelector != null)
            difficultySelector.SetActive(true);
    }
}