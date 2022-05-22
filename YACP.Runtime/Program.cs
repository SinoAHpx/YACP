using System.Reactive.Linq;
using System.Reflection;
using Manganese.Text;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Sessions;
using Mirai.Net.Utils.Scaffolds;
using Newtonsoft.Json;
using YACP;
using YACP.Models;
using YACP.Utils;

var bot = new MiraiBot
{
    Address = "localhost:8080",
    QQ = "1590454991",
    VerifyKey = "1145141919810"
};

await bot.LaunchAsync();

bot.MessageReceived
    .OfType<GroupMessageReceiver>()
    .Subscribe(x =>
    {
        if (!x.MessageChain.GetPlainMessage().StartsWith("/img"))
        {
            return;
        }
        var cmd = x.MessageChain.ToArray()[1..].Select(m =>
        {
            if (m is PlainMessage plainMessage)
            {
                return plainMessage.Text;
            }

            return m.ToJsonString();
        }).JoinToString(" ");

        var entity = cmd.Parse<MyClass>();

        x.SendMessageAsync(JsonConvert.DeserializeObject<ImageMessage>(entity.Image));
    });

Console.WriteLine("Launched");

Console.ReadLine();

[Command("img", Prefix = "/")]
class MyClass
{
    [Argument("-i")]
    public string Image { get; set; }
}

// var cmd = "/help -c awkldj kwld -v ";
//
// var e = cmd.Parse<Help>();
// e.ToJsonString().Out();
//
// var complexCmd =
//     "/c -n AHpx -p C:\\CodeBase\\Environments\\flutter -t 27 -o 0.5 -ps Zhang3 Li4 Wang5 Zhao6 -s 12 15 17 14 -f";
// var entity = complexCmd.Parse<CreateMode>();
//
// entity.ToJsonString().Out();
//
// [Command("help", "h", Description = "helping you", Prefix = "/")]
// class Help
// {
//     [Argument("-v", "--verbose", Description = "verbose help")]
//     public bool IsVerbose { get; set; }
//
//     [Argument("-c", "--command", Description = "command help")]
//     public string Command { get; set; }
// }
//
// [Command("c", "create")]
// class CreateMode
// {
//     [Argument("-n", "--name")] public string Name { get; set; }
//
//     [Argument("-p", "--path")] public string Path { get; set; }
//
//     [Argument("-t", "--template")] public int Template { get; set; }
//
//     [Argument("-o", "--offset")] public double Offset { get; set; }
//
//     [Argument("-ps", "--players")] public List<string> Players { get; set; }
//
//     [Argument("-s", "--spec")] public List<int> Specifications { get; set; }
//
//     [Argument("-f", "--finished")] public bool IsFinished { get; set; }
// }

static class MainUtils
{
    public static T Out<T>(this T t, string appendix = "")
    {
        Console.WriteLine($"{appendix} {t}");
        return t;
    }
}