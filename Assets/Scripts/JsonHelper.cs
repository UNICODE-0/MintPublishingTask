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

    [Serializable]
    private class Wrapper<T> {

        public T[] data;
    }
}
