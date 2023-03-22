
using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Otus_HomeWork5.CSV;

var f = new F().Get();
var timer = new Stopwatch();

//Самописный сериализатор
timer.Start();
for (var i = 0; i < 100000; i++)
{
    var csv = CSVConver.SerializeObject(f);
    var obj = CSVConver.DeserializeObject<F>(csv);
}
timer.Stop();
var time1 = timer.ElapsedMilliseconds;
Console.WriteLine($"CSV time:{time1} ms {CSVConver.SerializeObject(f)}");


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