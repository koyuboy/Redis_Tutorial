using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisClient.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RedisController : Controller
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _db;
    private readonly ISubscriber _sub;

    public RedisController(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _db = _redis.GetDatabase();
        _sub = _redis.GetSubscriber();
    }
    
    [HttpGet("ping")]
    public async Task<IActionResult> Ping()
    {
        var pong = await _db.PingAsync();

        return Ok(pong.ToString());
    }
    
    [HttpGet("getValue")]
    public async Task<IActionResult> GetValue(string key)
    {
        var value = await _db.StringGetAsync(key);

        return Ok(value.ToString());
    }
    
    [HttpPost("setValue")]
    public async Task<IActionResult> SetValue(string key, string value)
    {
        await _db.StringSetAsync(key, value);

        return Ok();
    }
    
    [HttpPost("publish")]
    public async Task<IActionResult> Publish(string channelName, string message)//run Pub_Sub project to subscribe channels and get messages.
    {
        await _sub.PublishAsync(channelName, message, CommandFlags.FireAndForget);
        
        return Ok();
    }
    
    [NonAction]
    [HttpGet("subscribe")]
    public async Task<IActionResult> Subscribe(string channelName) //run Pub_Sub project to subscribe channels and get messages.
    {
        string result = String.Empty;
        
        await _sub.SubscribeAsync(channelName, (channel, message) =>
        {
            result = message.ToString();
        });
        
        return Ok(result);
    }
    
}