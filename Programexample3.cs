using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

#region общие элементы для 2-х заданий по строкам должны быть в базовом классе
abstract class Task
{
    protected string text = "No text here yet";
    // для десериализации обязательно прописывайте свойства для всех полей
    protected int _res;
    public string Text
    {
        get => text;
        protected set => text = value;
    }
    public int Res
    {
        get => _res;
        protected set => _res = value;
    }
    public Task(string text)
    {
        this.text = text;
    }
    public override string ToString()
    {
        return _res.ToString();
    }
}
#endregion 
#region помечайте конструктор, который будет использоваться для десериализации (конструкторов может быть несколько в классе)
class Task1 : Task
{
    [JsonConstructor]
    public Task1(string text) : base(text)
    {
        Do_The_Task(text, out _res);
    }
    private void Do_The_Task(string text, out int res)
    {
        string[] words = text.Split(new char[] { ' ' });
        res = 0;
        foreach (string word in words)
        {
            // Проверяем, содержит ли слово гласные буквы
            bool hasVowel = false;
            foreach (char letter in word.ToLower())
            {
                if ("аоиеёыиэюяу".Contains(letter)) // Проверяем, является ли буква гласной
                {
                    hasVowel = true;
                    break;
                }
            }

            // Если слово не содержит гласных букв и имеет длину 1, увеличиваем счетчик
            if (!hasVowel && word.Length == 1)
            {
                res++;
            }
        }
    }
    public override string ToString()
    {
        return _res.ToString() + " Количество cоюзов и предлогов";
    }
}
    class Task2 : Task
{
    [JsonConstructor]
    public Task2(string text) : base(text)
    {
        Do_The_Task(text, out _res);
    }

    private void Do_The_Task(string text, out int _res)
    {
        string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        _res = 0;
        int MostLongest = 0;
        string MostLongestWordd = "";
        foreach (var word in words)
        {
            if (word.Length > MostLongest)
            {
                MostLongest = word.Length;
                MostLongestWordd = word;

            }
        }
        Console.WriteLine("Cамое длинное слово = " + MostLongestWordd);
        _res = MostLongestWordd.Length;
    }
    public override string ToString()
    {
        return _res.ToString() + " = Кол-во букв в самом длинном слове";
    }
}
#endregion
class JsonIO
{
    #region для тех, кто хочет максимум, используйте обобщение:
    public static void Write<T>(T obj, string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            JsonSerializer.Serialize(fs, obj); // преобразовать данные(куда, что)
        }
    }
    public static T Read<T>(string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            return JsonSerializer.Deserialize<T>(fs); // преобразовать данные<во что>(откуда)
        }
        return default(T);
    }
    #endregion

    #region для тех, кто запутался с обобщением, можно так (но это -1 балл):
    public static void Write1(Task1 obj, string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            JsonSerializer.Serialize(fs, obj);
        }
    }
    public static Task1 Read1(string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            return JsonSerializer.Deserialize<Task1>(fs);
        }
        return null;
    }
    public static void Write2(Task2 obj, string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            JsonSerializer.Serialize(fs, obj);
        }
    }
    public static Task2 Read2(string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            return JsonSerializer.Deserialize<Task2>(fs);
        }
        return null;
    }
    #endregion
}
class Program
{
    static void Main()
    {
        #region создаете массив заданий (даже, если сделали всего одно) (2 задания по 5 баллов)
        Task[] tasks = {
            new Task1("В то время как неэффективное ручное установление цен в магазинах сети влияет на создание неактуальных цен на продукцию в магазинах и увеличение недовольства покупателей"),       // решаете 1е задание
            new Task2("В то время как неэффективноееееее ручное установление цен в магазинах сети влияет на создание неактуальных цен на продукцию в магазинах и увеличение недовольства покупателей")    // решаете 2е задание
        };
        Console.WriteLine(tasks[0]);
        Console.WriteLine(tasks[1]);
        #endregion

        #region 3е задание на работу с папками и файлами (5 баллов)
        string path = @"/Users/anastasiasacik/Projects/пример кр 2"; // исходную папку ищем в компьютере
        string folderName = "Solution"; // если нужно создать подпапку
        path = Path.Combine(path, folderName);
        if (!Directory.Exists(path))    // создать отсутствующую подпапку
        {
            Directory.CreateDirectory(path);
        }
        string fileName1 = "cw_2_task_1.json"; // имена файлов
        string fileName2 = "cw_2_task_2.json";

        fileName1 = Path.Combine(path, fileName1);
        fileName2 = Path.Combine(path, fileName2);
        #endregion

        #region 4е задание на JSON сериализацию (5 баллов)
        if (!File.Exists(fileName2)) // создаем файл, если его нет
        {
            JsonIO.Write<Task1>(tasks[0] as Task1, fileName1);  // можно так приводить к нужному типу
            JsonIO.Write<Task2>((Task2)tasks[1], fileName2);    // а можно так
        }
        else // читаем файл (если меняли логику заданий, то удалите старые файлы!)
        {
            var t1 = JsonIO.Read<Task1>(fileName1);
            var t2 = JsonIO.Read<Task2>(fileName2);
            Console.WriteLine(t1);
            Console.WriteLine(t2);
        }
        #endregion
    }
}