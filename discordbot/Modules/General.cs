using System.Linq;
using System.Reflection;

using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Discord;






namespace discordbot.Modules
{
    public class General : ModuleBase
    {
        [Command("ping")]
        public async Task ping() {
            await Context.Channel.SendMessageAsync("pong");
        }

        [Command("info")]
        public async Task info(SocketGuildUser user = null) {

            if(user == null) {

            

            var builder = new EmbedBuilder()
                .WithThumbnailUrl(Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl())
                .WithDescription($"You will see some information about this user: {Context.User.Username} ")
                .WithColor(new Color(33, 180, 255))
                .AddField("User ID", Context.User.Username, true)
                .AddField("Descriminator", Context.User.Discriminator, true)
                .AddField("Created at", Context.User.CreatedAt.ToString("dd/MM/yyyy"), true)
                .AddField("Joined at", (Context.User as SocketGuildUser).JoinedAt.Value.ToString("dd/mm/yyyy"), true)
                .AddField("Roles", string.Join(" ",(Context.User as SocketGuildUser).Roles.Select(x => x.Mention)))
                .WithCurrentTimestamp();

            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);    
            } else {
            var builder = new EmbedBuilder()
                .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                .WithDescription($"You will see some information about this user: {Context.User.Username} ")
                .WithColor(new Color(33, 180, 255))
                .AddField("User ID", user.Username, true)
                .AddField("Descriminator", user.Discriminator, true)
                .AddField("Created at", user.CreatedAt.ToString("dd/MM/yyyy"), true)
                .AddField("Joined at", user.JoinedAt.Value.ToString("dd/MM/yyyy"), true)
                .AddField("Roles", string.Join(" ",user.Roles.Select(x => x.Mention)))
                .WithCurrentTimestamp();

            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);    
            }
            

        } 

        [Command("purge")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
            public async Task purge(int amount) {
                
                var messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
                await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);

                var message = await Context.Channel.SendMessageAsync($"{messages.Count()} messages deleted successfully. ");
                await Task.Delay(10000);
                await message.DeleteAsync();
            }
       

            [Command("sinfo")]
            public async Task Server() {
                var builder = new EmbedBuilder()
                    .WithThumbnailUrl(Context.Guild.IconUrl)
                    .WithDescription("in this message you can find nice information about the current server")
                    .WithTitle($"{Context.Guild.Name} information" )
                       .WithColor(new Color(33, 180, 255))
                       .AddField("Created at", Context.Guild.CreatedAt.ToString("dd/MM/yyyy"), true)
                       .AddField("Membercount", (Context.Guild as SocketGuild).MemberCount + "members", true)
                       .AddField("online users", (Context.Guild as SocketGuild).Users.Where(x => x.Status != UserStatus.Offline).Count() + " Members", true);

                       var embed = builder.Build();
                        await Context.Channel.SendMessageAsync(null, false, embed);  

            }



    }
}