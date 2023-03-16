
using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

var f = new F().Get();
var timer = new Stopwatch();

//Самописный сериализатор
timer.Start();
for (var i = 0; i < 100000; i++)
{
    var csv = Obj2CSV(f);
    var obj = CSV2Obj<F>(csv);
}
timer.Stop();
var time1 = timer.ElapsedMilliseconds;
Console.WriteLine($"CSV time:{time1} ms {Obj2CSV(f)}");


//Newtonsoft json сериализатор
timer.Start();
for (var i = 0; i < 100000; i++)
{
    var json = JsonConvert.SerializeObject(f);
     var obj = JsonConvert.DeserializeObject<F>(json);
}
timer.Stop();
var time2 = timer.ElapsedMilliseconds;
Console.WriteLine($"json time:{time2} ms {JsonConvert.SerializeObject(f)}");

if(time1 > time2)
    Console.WriteLine($"json быстрее чем CSV на {time1-time2} ms");
else
    Console.WriteLine($"CSV быстрее чем json на {time2 - time1} ms");



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