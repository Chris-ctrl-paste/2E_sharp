using System.Linq;
using System.Reflection;
using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Discord;






namespace discordbot.Modules
{
    public class Moderation : ModuleBase
    {
     

        [Command("purge")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
            public async Task purge(int amount) {
                
                var messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
                await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);

                var message = await Context.Channel.SendMessageAsync($"{messages.Count()} messages deleted successfully. ");
                await Task.Delay(10000);
                await message.DeleteAsync();
            }
       

           



    }
}