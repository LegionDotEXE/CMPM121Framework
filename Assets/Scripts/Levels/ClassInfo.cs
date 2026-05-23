using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ClassInfo
{
    public List<JToken> classes;
    public string selectedClass;
    private static ClassInfo theInstance;
    public static ClassInfo Instance
    {
        get
        {
            if (theInstance == null)
                theInstance = new ClassInfo();
            return theInstance;
        }
    }

    public JToken GetClass(string name)
    {
        foreach (var c in classes)
        {
            JProperty casterClass = (JProperty)c;
            if (name == casterClass.Name) return c;
        }
        return null;
    }

    private ClassInfo()
    {
        classes = JToken.Parse(File.ReadAllText("./Assets/Resources/classes.json")).Children().ToList();
        selectedClass = "mage";
    }
}