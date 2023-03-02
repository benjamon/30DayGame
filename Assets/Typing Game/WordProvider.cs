using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class WordProvider : HashSet<string>
{
    List<string> ordered;
    List<string>[] orderedByLength;
    HashSet<string>[] groupedByLength;
    public int LinesLoaded;

    public WordProvider(string path, int maxLength = 10)
    {
        LoadWords(path, maxLength);
    }

    void LoadWords(string path, int maxLength)
    {
        orderedByLength = new List<string>[maxLength];
        groupedByLength = new HashSet<string>[maxLength];
        for (int i = 0; i < maxLength; i++)
        {
            orderedByLength[i] = new List<string>();
            groupedByLength[i] = new HashSet<string>();
        }
        ordered = new List<string>();
        using (var reader = new StreamReader(Application.streamingAssetsPath + "/" + path))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                for (int i = 2; i < line.Length; i++)
                {
                    if (i - 1 > maxLength || line[i] == '-')
                        break;
                    if (line[i] == '"')
                    {
                        string s = line.Substring(1, i - 1);
                        this.Add(s);
                        ordered.Add(s);
                        orderedByLength[i-2].Add(s);
                        groupedByLength[i-2].Add(s);
                        LinesLoaded++;
                        break;
                    }
                }
            }
        }
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