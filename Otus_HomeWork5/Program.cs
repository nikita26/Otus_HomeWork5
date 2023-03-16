
using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection;
using Newtonsoft.Json;

var f = new F().Get();


var timer = new Stopwatch();

timer.Start();
for (var i = 0; i < 100000; i++)
{
    var csv = Obj2CSV(f);
    var obj = CSV2Obj<F>(csv);
}
timer.Stop();
Console.WriteLine($"CSV time:{timer.ElapsedMilliseconds} ms {Obj2CSV(f)}");


timer.Start();
for (var i = 0; i < 100000; i++)
{
    var json = JsonConvert.SerializeObject(f);
     var obj = JsonConvert.DeserializeObject<F>(json);
}
timer.Stop();
Console.WriteLine($"json time:{timer.ElapsedMilliseconds} ms {JsonConvert.SerializeObject(f)}");


//Объект в CSV строку
string Obj2CSV(object obj)
{
    List<string> result = new List<string>();

    var type = obj.GetType();
    var publicProps = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

    foreach (var p in publicProps)
    {
        var field = p.Name;
        var pValue = p.GetValue(obj);

        result.Add($"{p.Name}:{pValue}");
    }
    return String.Join(';',result);
};

//CSV строка в объект
T CSV2Obj<T>(string str) where T : new()
{
    var result = new T();
    var type = typeof(T);

    var fields = str.Split(';');
    foreach (var field in fields)
    {
        var data = field.Split(":");
        var prop = type.GetProperty(data[0], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (prop != null)
        {
            var t = Type.GetType(prop.PropertyType.FullName);
            if (t == typeof(double))
                data[1] = data[1].Replace(',', '.');
            var value = TypeDescriptor.GetConverter(t).ConvertFromInvariantString(data[1]);
            prop.SetValue(result, value);
        }
    }
    return result;
}

public class F
{
    [JsonProperty]
    public int I1 { get; set; }
    [JsonProperty]
    int i2 { get; set; }
    [JsonProperty]
    int i3 { get; set; }
    [JsonProperty]
    int i4 { get; set; }
    [JsonProperty]
    int i5 { get; set; }
    [JsonProperty]
    double f1 { get; set; }
    public F Get() => new F() { I1 = 11, i2 = 22, i3 = 33, i4 = 44, i5 = 55, f1 = 32.42 };
}