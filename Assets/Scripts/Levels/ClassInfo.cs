using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ClassInfo
{
    public List<JToken> classes;
    public Dictionary<string, JToken> classData;
    public string selectedClass = "mage";

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

    private ClassInfo()
    {
        classes = JToken.Parse(File.ReadAllText("./Assets/Resources/classes.json")).Children().Values().ToList();
        JObject raw = JObject.Parse(File.ReadAllText("./Assets/Resources/classes.json"));
        classData = new Dictionary<string, JToken>();
        foreach (var entry in raw)
        {
            classData[entry.Key] = entry.Value;
        }
    }
}