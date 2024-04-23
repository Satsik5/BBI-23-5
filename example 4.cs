using System.Collections.Generic;
using System;
using System.ComponentModel.Design;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

abstract class Task
{
    protected string text = "";
    public string Text
    {
        get => text;
        protected set => text = value;
    }

    public virtual void Solution() { }
    public Task(string text)
    {
        this.text = text;
    }
}
class Task1 : Task
{
    private string centralWord;

    public string CentralWord
    {
        get => centralWord;
    }

    [JsonConstructor]
    public Task1(string text) : base(text)
    {
        centralWord = "";
    }

    public override void Solution()
    {
        string[] words = text.Split(" ,-!.:;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        int middleIndex = words.Length / 2;
        centralWord = words[middleIndex];
    }

    public override string ToString()
    {
        Solution();
        return centralWord;
    }
}
class Task2 : Task
{
    private int uniqueWordCount;

    public int UniqueWordCount
    {
        get => uniqueWordCount;
    }

    [JsonConstructor]
    public Task2(string text) : base(text)
    {
        uniqueWordCount = 0;
    }

    public override void Solution()
    {
        string[] words = text.Split(" ,-!.:;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        HashSet<string> uniqueWords = new HashSet<string>();

        foreach (var word in words)
        {
            if (!uniqueWords.Contains(word))
            {
                uniqueWords.Add(word);
            }
        }

        uniqueWordCount = uniqueWords.Count;
    }

    public override string ToString()
    {
        Solution();
        return uniqueWordCount.ToString();
    }
}
class JsonIO
{
    public static void Write<T>(T obj, string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            JsonSerializer.Serialize(fs, obj);
        }
    }
    public static T Read<T>(string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            return JsonSerializer.Deserialize<T>(fs);
        }
        return default(T);
    }
}
class Program
{
    static void Main()
    {
        string text = "Объектно ориентированное программиирование — методология на основе описания моделей и их взаимодействия. С джейсоном очень сложно разобраться. Ляля принципе мне нравится программировать, но часто возникают трудности. С ними я пытаюсь справиться. Сегодня я очень старалась.";
        Task[] tasks = {
            new Task1(text),
            new Task2(text)
        };
        Console.WriteLine(tasks[0]);
        Console.WriteLine(tasks[1]);

        string path = @"C:\Users\user\Desktop";
        string folderName = "Test";
        path = Path.Combine(path, folderName);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string fileName1 = "task_1.json";
        string fileName2 = "task_2.json";

        fileName1 = Path.Combine(path, fileName1);
        fileName2 = Path.Combine(path, fileName2);
        if (!File.Exists(fileName1))
        {
            JsonIO.Write<Task1>(tasks[0] as Task1, fileName1);
        }
        else
        {
            var t1 = JsonIO.Read<Task2>(fileName1);
            Console.WriteLine(t1);
        }
    }
}