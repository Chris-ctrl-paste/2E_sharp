using System.Threading.Tasks;
using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Victoria;
using Victoria.Enums;
using Victoria.EventArgs;








namespace discordbot.Modules 
{
    public class Music : ModuleBase
    {
        private readonly LavaNode _lavaNode;
        
     
        public Music(LavaNode lavaNode  ) {
        _lavaNode = lavaNode;
       
        
}
        
        


        private async Task OnTrackEnded(TrackEndedEventArgs args) {
    if (!args.Reason.ShouldPlayNext()) {
        return;
    }

    var player = args.Player;
    if (!player.Queue.TryDequeue(out var queueable)) {
        await player.TextChannel.SendMessageAsync("Queue completed! Please add more tracks to rock n' roll!");
        return;
    }

    if (!(queueable is LavaTrack track)) {
        await player.TextChannel.SendMessageAsync("Next item in queue is not a track.");
        return;
    }

    await args.Player.PlayAsync(track);
    await args.Player.TextChannel.SendMessageAsync(
        $"{args.Reason}: {args.Track.Title}\nNow playing: {track.Title}");
}

[Command("Join")]
public async Task JoinAsync() {
    if (_lavaNode.HasPlayer(Context.Guild)) {
        await ReplyAsync("I'm already connected to a voice channel!");
        return;
    }

    var voiceState = Context.User as IVoiceState;
    if (voiceState?.VoiceChannel == null) {
        await ReplyAsync("You must be connected to a voice channel!");
        return;
    }

    try {
        await _lavaNode.JoinAsync(voiceState.VoiceChannel, Context.Channel as ITextChannel);
        await ReplyAsync($"Joined {voiceState.VoiceChannel.Name}!");
    }
    catch (Exception exception) {
        await ReplyAsync(exception.Message);
    }
}


[Command("Play")]
public async Task PlayAsync([Remainder] string searchQuery) {
    if (string.IsNullOrWhiteSpace(searchQuery)) {
        await ReplyAsync("Please provide search terms.");
        return;
    }

    if (!_lavaNode.HasPlayer(Context.Guild)) {
        await ReplyAsync("I'm not connected to a voice channel.");
        return;
    }

    var queries = searchQuery.Split(' ');
    foreach (var query in queries) {
        var searchResponse = await _lavaNode.SearchYouTubeAsync(query);
        if (searchResponse.LoadStatus == LoadStatus.LoadFailed ||
            searchResponse.LoadStatus == LoadStatus.NoMatches) {
            await ReplyAsync($"I wasn't able to find anything for `{query}`.");
            return;
        }

        var player = _lavaNode.GetPlayer(Context.Guild);

        if (player.PlayerState == PlayerState.Playing || player.PlayerState == PlayerState.Paused) {
            if (!string.IsNullOrWhiteSpace(searchResponse.Playlist.Name)) {
                foreach (var track in searchResponse.Tracks) {
                    player.Queue.Enqueue(track);
                }

                await ReplyAsync($"Enqueued {searchResponse.Tracks.Count} tracks.");
            }
            else {
                var track = searchResponse.Tracks[0];
                player.Queue.Enqueue(track);
                await ReplyAsync($"Enqueued: {track.Title}");
            }
        }
        else {
            var track = searchResponse.Tracks[0];

            if (!string.IsNullOrWhiteSpace(searchResponse.Playlist.Name)) {
                for (var i = 0; i < searchResponse.Tracks.Count; i++) {
                    if (i == 0) {
                        await player.PlayAsync(track);
                        await ReplyAsync($"Now Playing: {track.Title}");
                    }
                    else {
                        player.Queue.Enqueue(searchResponse.Tracks[i]);
                    }
                }

                await ReplyAsync($"Enqueued {searchResponse.Tracks.Count} tracks.");
            }
            else {
                await player.PlayAsync(track);
                await ReplyAsync($"Now Playing: {track.Title}");
            }
        }
    }
}



    }
}