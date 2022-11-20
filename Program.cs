using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyRedisApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        // static void Main(string[] args)
        // {
        //     Console.WriteLine("Hello, World!");
        //     var connectionString = "[cache-name].redis.cache.windows.net:6380,password=[password-here],ssl=True,abortConnect=False";
        //     var redisConnection = ConnectionMultiplexer.Connect(connectionString);

        //     IDatabase db = redisConnection.GetDatabase();
        //     bool wasSet = db.StringSet("favorite:flavor", "i-love-rocky-road");
            
        //     string value = db.StringGet("favorite:flavor");
        //     Console.WriteLine(value); // displays: ""i-love-rocky-road""

        //     var result = db.Execute("ping");
        //     Console.WriteLine(result.ToString()); // displays: "PONG"

        //     var result = await db.ExecuteAsync("client", "list");
        //     Console.WriteLine($"Type = {result.Type}\r\nResult = {result}");

        //     redisConnection.Dispose();
        //     redisConnection = null;

        // }
        static async Task Main(string[] args)
        {
            // The connection to the Azure Cache for Redis is managed by the ConnectionMultiplexer class.
            var connectionString = "redisconnectdemo.redis.cache.windows.net:6380,password=SapMJquQA3h0IpZt0J8nktPVgn6p4rOV8AzCaM4etEA=,ssl=True,abortConnect=False";
            using (var cache = ConnectionMultiplexer.Connect(connectionString))
            {
                IDatabase db = cache.GetDatabase();

                // Snippet below executes a PING to test the server connection
                var result = await db.ExecuteAsync("ping");
                Console.WriteLine($"PING = {result.Type} : {result}");

                // Call StringSetAsync on the IDatabase object to set the key "test:key" to the value "100"
                bool setValue = await db.StringSetAsync("test:key", "100");
                Console.WriteLine($"SET: {setValue}");

                // StringGetAsync takes the key to retrieve and return the value
                string getValue = await db.StringGetAsync("test:key");
                Console.WriteLine($"GET: {getValue}");

                var stat = new GameStat("Soccer", new DateTime(2019, 7, 16), "Local Game", 
                    new[] { "Team 1", "Team 2" },
                    new[] { ("Team 1", 2), ("Team 2", 1) });

                string serializedValue = JsonConvert.SerializeObject(stat);
                bool added = db.StringSet("event:1950-world-cup", serializedValue);

                var result2 = db.StringGet("event:1950-world-cup");
                var getBack = Newtonsoft.Json.JsonConvert.DeserializeObject<GameStat>(result2.ToString());
                Console.WriteLine(getBack.Sport); // displays "Soccer"

            }
        }
    }
}

