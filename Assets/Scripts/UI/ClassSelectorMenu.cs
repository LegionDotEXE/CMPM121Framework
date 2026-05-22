using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClassSelectorMenu
{
    public GameObject playerClassUI;
    private TextMeshProUGUI buttonText;
    private TextMeshProUGUI messageText;
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
}