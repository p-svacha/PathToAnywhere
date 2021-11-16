using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NameGenerator
{
    private static List<string> CharacterNames;
    private static List<string> SettlementNames;

    public static void Init()
    {
        CharacterNames = GetFileNames("Assets/Resources/NameGenerationData/Firstnames.txt");
        SettlementNames = GetFileNames("Assets/Resources/NameGenerationData/Settlements.txt");
    }

    private static List<string> GetFileNames(string path)
    {
        List<string> entries = new List<string>();
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(path);
        while ((line = file.ReadLine()) != null) entries.Add(line);
        file.Close();
        return entries;
    }

    public static string GenerateName(NameGenerationType type)
    {
        if (type == NameGenerationType.Character) return RandomListElement(CharacterNames);
        if (type == NameGenerationType.Settlement) return RandomListElement(SettlementNames);
        throw new System.Exception("Name Generation Type not handled");
    }

    private static string RandomListElement(List<string> list)
    {
        return list[Random.Range(0, list.Count)];
    }
}
