using System;
using System.Collections.Generic;
using System.Linq;

namespace EnglishTester.Scripts
{
public class ProcessSentences
{
    private static readonly List<Tuple<string, string>> RawSentences = new();
    private static readonly List<Tuple<string, string>> Sentences = new();
    private static readonly List<string> _verbs = new();

    private static readonly string Path = Environment.CurrentDirectory + "\\Resources";
    
    private static void VerbLoader()
    {
        var verbsPath = $"{Path}\\Verbs.txt";
        var reader = new System.IO.StreamReader(verbsPath);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line == null || line.Contains('#')) continue;
            _verbs.Add(line);
        }
        reader.Close();
    }

    public static void Go()
    {
        VerbLoader();
        var verbsPath = $"{Path}\\rawSentences.txt";
        var reader = new System.IO.StreamReader(verbsPath);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line == null || line.Contains("#")) continue;
            var split = line.Split('|');
            RawSentences.Add(new Tuple<string, string>(split[0], split[1]));
        }

        var sentenceIndex = 0;
        foreach (var s in RawSentences)
        {
            var ss = s.Item1.Split(' ');
            var modifiedSentence = s.Item1;
            var noChanged = 0;
            
            foreach (var v in _verbs)
            {
                if (s.Item1.Contains(v))
                {
                    noChanged--;
                    var vs = v.Split(' ');

                    var indexFirst = ss.ToList().FindIndex(a => a.Contains(vs[0]));
                    var indexLast = ss.ToList().FindIndex(a => a.Contains(vs[vs.Length - 1]));

                    modifiedSentence = s.Item1.Replace(ss[indexFirst], $"[{ss[indexFirst]}")
                        .Replace(ss[indexLast], $"{ss[indexLast]}]");

                    // if (sentenceIndex == Sentences.Count - 1)
                    // {
                    //     Sentences[sentenceIndex] = new Tuple<string, string>(modifiedSentence, s.Item2);
                    // }
                    // else
                    // {
                    //     Sentences.Add(new Tuple<string, string>(modifiedSentence, s.Item2));
                    // }
                    Sentences.Add(new Tuple<string, string>(modifiedSentence, s.Item2));
                }
                else
                {
                    noChanged++;
                }
            }

            if (noChanged == _verbs.Count)
            {
                Sentences.Add(new Tuple<string, string>("s.Item1", "s.Item2"));
            }
            sentenceIndex++;
        }

        foreach (var VARIABLE in Sentences)
        {
            Console.WriteLine(VARIABLE.Item1);
        }
        System.IO.File.WriteAllLines($"{Path}\\sentences.txt", Sentences.Select(a => a.Item1 + "|" + a.Item2));
    }
}
}