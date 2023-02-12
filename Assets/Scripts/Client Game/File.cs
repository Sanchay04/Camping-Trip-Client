//Sanchay Ravindiran 2020

/*
    Handles all file operations needed throughout the
    game. Collects specified server ports, server addresses,
    and player highscore entries from within files in the
    specialdata folder located in the project directory.
*/

using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class File
{
    public static void Wipe()
    {
        string[] defaultScores = Enumerable.Repeat("Guy 0", 10).ToArray();
        string[] blankLines = { "0" };

        System.IO.File.WriteAllLines(PathTo("score"), defaultScores);
        System.IO.File.WriteAllLines(PathTo("address"), blankLines);
        System.IO.File.WriteAllLines(PathTo("port"), blankLines);
    }

    public static Entry[] Score(string name, float score)
    {
        string[] currentScores = System.IO.File.ReadAllLines(PathTo("score"));
        List<Entry> tempEntries = new List<Entry>();

        for (int i = 0; i < currentScores.Length; i++)
        {
            string[] separatedLine = currentScores[i].Split(' ');

            Entry entry = new Entry
            {
                name = separatedLine[0],
                score = float.Parse(separatedLine[1])
            };

            tempEntries.Add(entry);
        }

        Entry ownEntry = new Entry
        {
            name = name,
            score = score
        };

        tempEntries.Add(ownEntry);

        tempEntries = tempEntries.OrderByDescending(o => o.score).ToList();
        tempEntries.Reverse();
        tempEntries.RemoveAt(0);

        for (int i = 0; i < tempEntries.Count; i++)
        {
            currentScores[i] = string.Format("{0} {1}", tempEntries[i].name, tempEntries[i].score);
        }

        System.IO.File.WriteAllLines(PathTo("score"), currentScores);

        return tempEntries.ToArray();
    }

    public static string Address()
    {
        string[] lines = System.IO.File.ReadAllLines(PathTo("address"));

        return lines[0];
    }

    public static int Port()
    {
        string[] lines = System.IO.File.ReadAllLines(PathTo("port"));
        int result = 0;

        int.TryParse(lines[0], out result);

        return result;
    }

    private static string PathTo(string component)
    {
        return Application.dataPath + "/specialdata/" + component + ".txt";
    }
}
