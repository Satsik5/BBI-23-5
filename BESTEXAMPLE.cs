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
    private int answer;
    public int Answer
    {
        get => answer;
        protected set => answer = value;
    }
    [JsonConstructor]
    public Task1(string text) : base(text)
    {
        answer = 0;

    }
    public override void Solution()
    {
        string[] words = text.Split(" ,-!.:;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words)
        {
            if (word.Length == 1 && "вксВКС".Contains(word))
            {
                answer++;
            }
        }
    }

    public override string ToString()
    {
        Solution();
        return answer.ToString();
    }
}
class Task2 : Task
{
    private List<string> answer;
    public List<string> Answer
    {
        get => answer;
    }
    [JsonConstructor]
    public Task2(string text) : base(text)
    {
        answer = new List<string>();

    }
    public override void Solution()
    {
        int[] counter = new int[33];
        string checker = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        foreach (char i in text)
        {
            if (checker.Contains(i))
            {
                counter[i - 'А']++;
            }
        }
        char maxChar = ' ';
        int tmpMax = 0;
        for (int i = 0; i < 33; ++i)
        {
            if (counter[i] > tmpMax)
            {
                tmpMax = counter[i];
                maxChar = Convert.ToChar('А' + i);
            }
        }
        string[] words = text.Split(" ,-!.:;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words)
        {
            if (word[0] == maxChar)
            {
                answer.Add(word);
            }
        }
    }
    public override string ToString()
    {
        Solution();
        if (answer == null) return "";
        return string.Join(",", answer.ToArray()); ;
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
        string text = "Объектно ориентированное программиирование — методология на основе описания моделей и их взаимодействия. С джейсоном очень сложно разобраться. В принципе мне нравится программировать, но часто возникают трудности. С ними я пытаюсь справиться. Сегодня я очень старалась.";
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