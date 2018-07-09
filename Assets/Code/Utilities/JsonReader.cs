using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonReader {
    
    public static void SerializeAndWrite(object obj, string fileName)
    {
        System.IO.File.WriteAllText(fileName, JsonUtility.ToJson(obj, true));
    }

    public static T ReadAndDeserialize<T>(string fileName)
    {
        try
        {
            return JsonUtility.FromJson<T>(System.IO.File.ReadAllText(fileName));
        }
        catch
        {
            return default(T);
        }
    }
    

}
