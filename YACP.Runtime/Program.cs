using Manganese.Text;
using YACP;
using YACP.Models;

var cmd = "/s -n jack -a 19 -g 3.6 -q";
var cmd2 = "/s -n rose -a 17 -g 2.7 -q";
var cmd3 = "/s -n captain -g 3.7 -q";
var cmd4 = "/s james bond";

var e1 = cmd.Parse<Student>();
var e2 = cmd2.Parse<Student>();
var e3 = cmd3.Parse<Student>();
var e4 = cmd4.Parse<Student>();

Console.WriteLine(e1.ToJsonString());
Console.WriteLine(e2.ToJsonString());
Console.WriteLine(e3.ToJsonString());
Console.WriteLine(e4.ToJsonString());

[Command("s")]
class Student
{
    [Argument("-n", IsDefault = true)]
    public List<string> Name { get; set; }

    [Argument("-a")]
    public int? Age { get; set; }

    [Argument("-g")]
    public double GPA { get; set; }

    [Argument("-q")]
    public bool IsQualified { get; set; }
}