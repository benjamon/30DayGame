using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WordProvider : HashSet<string>
{
    public static WordProvider Instance { get; internal set; }

    List<string> ordered;
    List<string>[] orderedByLength;
    HashSet<string>[] groupedByLength;
    public int LinesLoaded;
    public int maxLength { get; }
    public int min = 1;

    [System.NonSerialized]
    static bool singletonBusy;
    public static IEnumerator InitSingleton(string fileName, int maxLength, int min = 1)
    {
        if (singletonBusy) yield break;
        singletonBusy = true;
        var wp = new WordProvider(maxLength);

        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
        wp.min = min;

        if (Application.platform == RuntimePlatform.Android)
        {
            using (var www = UnityWebRequest.Get(filePath))
            {
                www.SendWebRequest();
                Debug.Log("sending");
                while (!www.downloadHandler.isDone)
                    yield return null;
                Debug.Log("adding request success");
                string[] dict = www.downloadHandler.text.Split(System.Environment.NewLine);
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("adding words");
                    foreach (string s in dict)
                        wp.AddWord(s);
                }
                wp.IsSetup = true;
                Instance = wp;
                singletonBusy = false;
            }
        }
        else
        {
            Debug.Log("wrong plat lol");
            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string s = reader.ReadLine();
                        wp.AddWord(s);
                    }
                }
            } catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
            }
            wp.IsSetup = true;
            Instance = wp;
            singletonBusy = false;
        }
    }

    WordProvider(int maxLength)
    {
        this.maxLength = maxLength;
        orderedByLength = new List<string>[maxLength];
        groupedByLength = new HashSet<string>[maxLength];
        for (int i = 0; i < maxLength; i++)
        {
            orderedByLength[i] = new List<string>();
            groupedByLength[i] = new HashSet<string>();
        }
        ordered = new List<string>();
    }

    public WordProvider(string fileName, int maxLength = 10) : this(maxLength)
    {
        ReadDictionary(fileName);
    }

    public bool IsSetup;
    async void ReadDictionary(string fileName)
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
        if (Application.platform == RuntimePlatform.Android)
        {
            using (var www = UnityWebRequest.Get(filePath))
            {
                www.SendWebRequest();
                while (!www.downloadHandler.isDone)
                {
                    await Task.Delay(10);
                }
                string[] dict = www.downloadHandler.text.Split(System.Environment.NewLine);
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                } else
                {
                    foreach (string s in dict)
                        ProcessLine(s);
                    Debug.Log(Count + " total words");
                    for (int i = 0; i < maxLength; i++)
                    {
                        Debug.LogError(orderedByLength[i].Count + " total words length " + i);
                    }
                }
            }
        }
        else
        {
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    ProcessLine(reader.ReadLine());
                }
            }
        }
        IsSetup = true;
    }

    void ProcessLine(string line)
    {
        line = line.ToLower();
        for (int i = 2; i < line.Length; i++)
        {
            if (i - 1 > maxLength || line[i] == '-')
                break;
            if (line[i] == '"' || !IsValid(line[i]))
            {
                string s = line.Substring(1, i - 1);
            }
        }
    }

    void AddWord(string s)
    {
        s = s.Trim();
        if (s.Length < min || s.Length > maxLength)
            return;
        this.Add(s);
        ordered.Add(s);
        orderedByLength[s.Length - 1].Add(s);
        groupedByLength[s.Length-1].Add(s);
        LinesLoaded++;
    }

    bool IsValid(char c)
    {
        return c >= 'a' && c <= 'z';
    }

    public string GetRandom()
    {
        return ordered[Random.Range(0, ordered.Count)];
    }

    public string GetRandom(int size)
    {
        size = (int)Mathf.Clamp(size, 1, orderedByLength.Length);
        return orderedByLength[size-1][Random.Range(0, orderedByLength[size-1].Count)];
    }
}