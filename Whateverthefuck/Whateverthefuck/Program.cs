using System;
using System.Threading.Tasks;
using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Discord.Net.WebSockets;

namespace Whateverthefuck
{
    class Program
    {
        public CommandService commands;
        public DiscordSocketClient client;
        public IServiceProvider services;
        static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();

        public async Task Start()
        {
            commands = new CommandService();
            client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 1000
            });
            client.Log += Logger;
            commands = new CommandService();
            await InstallCommands();
            await client.LoginAsync(TokenType.Bot, "MzY1ODIwOTAzNDExNjc5MjMz.DLj4yg.00Ex2 - 5C7i0eFuZW4iAIlzmMuXo");
            await client.StartAsync();
            await Task.Delay(-1);
        }

        public async Task InstallCommands()
        {
            IServiceCollection map = new ServiceCollection();

            services = map.BuildServiceProvider();

            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
            client.MessageReceived += HandleCommand;
        }

        public async Task HandleCommand(SocketMessage messageParam)
        {

            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            int argPos = 0;
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) return;
            var context = new CommandContext(client, message);

            var result = await commands.ExecuteAsync(context, argPos, services);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }

        private static Task Logger(LogMessage message)
        {
            var cc = Console.ForegroundColor;
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message}");
            Console.ForegroundColor = cc;
            return Task.CompletedTask;
        }
    }
}
