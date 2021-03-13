using System.Net.Http;
using System.Net;
using System.Linq;
using System.Reflection;
using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Newtonsoft.Json.Linq;









namespace discordbot.Modules
{
    public class Entertainment : ModuleBase
    {

        [Command("meme")]
        [Alias("reddit")]
        public async Task meme(string subreddit = null) {
            var client = new HttpClient();
            var result = await client.GetStringAsync($"https://reddit.com/r/{subreddit ?? "memes"}/random.json?limit=1");
            if(!result.StartsWith("[")) {
                await Context.Channel.SendMessageAsync("This subreddit doesnt exist");
                return;
            }

            if(result.StartsWith("[locked]")) {
                return;
            }

            JArray arr = JArray.Parse(result);
            JObject post = JObject.Parse(arr[0]["data"]["children"][0]["data"].ToString());

         
           
           
           
            var builder = new EmbedBuilder()
                .WithImageUrl(post["url"].ToString())
                .WithColor(new Color(200, 180, 252))
                .WithTitle(post["title"].ToString())
                .WithUrl("https://reddit.com" + post["permalink"].ToString())
                .WithFooter($"🗨️ {post["num_comments"]}  ⬆️ {post["ups"] }");
            
            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);



        }

        
    }
}