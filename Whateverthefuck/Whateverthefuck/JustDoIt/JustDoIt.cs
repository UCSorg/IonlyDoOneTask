using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.IO;

namespace Whateverthefuck.JustDoIt
{
    public class JustDoIt : ModuleBase
    {
        [Command("justdoit")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task justdoit()
        {
            var rolex = Context.Guild.Roles.First(x => x.Name == "Rank Not Determined");
            var usersAll = await Context.Guild.GetUsersAsync();
            if (rolex != null)
            {
                var m = await Context.Channel.SendMessageAsync("Working...");
                int users = 0;
                foreach (SocketGuildUser user in usersAll)
                {
                    var role = user.Roles.Where(x => x.Name.Contains("Champion") || x.Name.Contains("Diamond") || x.Name.Contains("Platinum") || x.Name.Contains("Gold") || x.Name.Contains("Silver") || x.Name.Contains("Bronze") || x.Name.Contains("Lurker") || x.Name.Contains("Bots"));
                    if(role.Count() != 0 && user.Roles.Where(y => y.Name == "Rank Not Determined").Count() != 0)
                    {
                        await user.RemoveRoleAsync(rolex);
                        await Task.Delay(2000);
                        users = users + 1;
                        await m.ModifyAsync(x => x.Content = $"Working... **{users}** users handled. This might take a while");
                    }
                    if(role.Count() == 0 && user.Roles.Where(y=>y.Name == "Rank Not Determined").Count() == 0)
                    {
                        await user.AddRoleAsync(rolex);
                        await Task.Delay(2000);
                        users = users + 1;
                        await m.ModifyAsync(x => x.Content = $"Working... **{users}** users handled. This might take a while");
                    }
                }
            }
        }
    }
}
