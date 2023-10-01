using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class JsonHelper 
{
    public static List<T> ReadListFromJSON<T>(string path) 
    {
        string JsonString = ReadFile(path);

        if(string.IsNullOrEmpty(JsonString) || JsonString == "{}") 
        {
            return new List<T>();
        }

        List<T> Data = JsonHelper.FromJson<T>(JsonString).ToList();

        return Data;
    }
    public static List<T> ReadListFromJSONString<T>(string jsonString) 
    {
        if(string.IsNullOrEmpty(jsonString) || jsonString == "{}") 
        {
            return new List<T>();
        }

        List<T> Data = JsonHelper.FromJson<T>(jsonString).ToList();

        return Data;
    }
    public static T ReadFromJSON<T>(string path) 
    {
        string JsonString = ReadFile(path);

        if(string.IsNullOrEmpty(JsonString) || JsonString == "{}") 
        {
            return default(T);
        }

        T Data = JsonUtility.FromJson<T>(JsonString);

        return Data;
    }
    private static string ReadFile(string Path) 
    {
        if(File.Exists(Path)) 
        {
            using (StreamReader reader = new StreamReader(Path)) 
            {
                return reader.ReadToEnd();
            }
        } else return String.Empty;
    }
    public static void SaveToJSON<T>(List<T> Data, string path) 
    {
        string content = JsonHelper.ToJson<T>(Data.ToArray());
        File.WriteAllText(path, content);
        //WriteFile(path, content);
    }
    public static void SaveToJSON<T>(T Data, string path) 
    {
        string content = JsonUtility.ToJson(Data);
        WriteFile(path, content);
    }
    private static T[] FromJson<T>(string json) 
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.data;
    }

    private static string ToJson<T>(T[] array) 
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.data = array;
        return JsonUtility.ToJson(wrapper, true);
    }
    private static void WriteFile(string path, string data, FileMode mode = FileMode.Create) 
    {
        Debug.Log(data);
        File.WriteAllText(path, data);
        // FileStream fileStream = new FileStream(path, mode, FileAccess.Write);
        // using(StreamWriter writer = new StreamWriter(fileStream)) 
        // {
        //     writer.Write(data);
        // }
    }

    [Serializable]
    private class Wrapper<T> {

        public T[] data;
    }
}
