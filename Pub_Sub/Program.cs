

using StackExchange.Redis;

var redis = ConnectionMultiplexer.Connect("localhost");
var db = redis.GetDatabase();
var sub = redis.GetSubscriber();

var channelName = "messages";
string input;

Console.WriteLine("You subscribed the " + channelName + " channel.");
Console.WriteLine("Type exit to stop program.");


while (true)
{
    sub.Subscribe(channelName, (channel, message) =>
    {
        Console.WriteLine(message.ToString());
    });
    
    input = Console.ReadLine();
    if (input == "exit")
    {
        return;
    }
}


