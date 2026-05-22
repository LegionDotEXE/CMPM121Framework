using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ClassInfo
{
    public List<JToken> classes; 
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
        classes = JToken.Parse(File.ReadAllText("./Assets/Resources/classes.json")).Children().Values().ToList();
    }
}