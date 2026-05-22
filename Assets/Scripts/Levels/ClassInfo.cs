using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ClassInfo
{
    public List<JObject> classes = new();
    private static ClassInfo theInstance;
    public static ClassInfo Instance {  get 
        {
            if (theInstance == null)
                theInstance = new ClassInfo();
            return theInstance; 
        } 
    }

    private ClassInfo()
    {
        IEnumerable<JToken> classTokens = JObject.Parse(File.ReadAllText("./Assets/Resources/classes.json"))
           .Children()
           .ToList();
        foreach (JObject playerClass in classTokens)
            classes.Add(playerClass);
    }
}