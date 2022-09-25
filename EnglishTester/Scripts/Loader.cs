using System;
using System.Collections.Generic;

namespace EnglishTester.Scripts
{
public static class Loader
{
    public static readonly List<Tuple<string, string>> Sentences = new ();

    private static readonly string Path = AppDomain.CurrentDomain.BaseDirectory + "\\Resources";
    public static void Setup()
    {
        SentencesLoader();
    }
    
    private static void SentencesLoader()
    {
        var verbsPath = $"{Path}\\Sentences.txt";
        var reader = new System.IO.StreamReader(verbsPath);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line == null || line.Contains('#')) continue;
            var split = line.Split('|');
            Sentences.Add(new Tuple<string, string>(split[0], split[1]));
        }
        reader.Close();
    }
}
}