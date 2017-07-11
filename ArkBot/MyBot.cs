using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using RconSharp;
using System.Threading;
using RconSharp.Net45;

namespace ArkBot
{
    class MyBot
    {
        DiscordClient discord;
        CommandService commands;
        // create an instance of the socket. In this case i've used the .Net 4.5 object defined in the project
        INetworkSocket socket = new RconSocket(); //this is for center map
        INetworkSocket socketB = new RconSocket(); //this is for scorched map
        // create the RconMessenger instance and inject the socket
        RconMessenger messenger;
        RconMessenger messengerB;

        Channel centerChannel;
        Channel scorchedChannel;
        Channel generalChannel;

        public MyBot()
        {
            //setup logging system for bot console
            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            //setup how commands are called in chat
            discord.UsingCommands(x =>
            {
                x.PrefixChar = '!'; //prefix chars must be encased in '' not ""
                x.AllowMentionPrefix = true; //allows users to address the bot directly instead of using command prefix
            });
            
            //grab command service so we can start defining commands
            commands = discord.GetService<CommandService>();

            //register commands
            RegisterBasicCommands();
            RegisterSpawnCommands();
            RegisterShadowIsleCommands();
            RegisterZoneCommands();
            RegisterEngageCommands();
            RegisterRconCommands();
            //this allows the bot to connect via token to the shadow isle discord server
            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MjI3ODkzOTIyODU5NDUwMzY4.CsNGNA.4fikLPFr-2Vncyuy5majAZ0AxmQ", TokenType.Bot);
            });
            
        }

        private void RegisterEngageCommands()
        {
            commands.CreateCommand("grab") //grabs chat channels in discord for game chat output
                .Do(async (e) =>
                {
                    centerChannel = discord.GetChannel(227947411371196423);
                    scorchedChannel = discord.GetChannel(227947476852801537);
                    generalChannel = discord.GetChannel(225048568577130498);
                    await scorchedChannel.SendMessage("ArkBot linked ...");
                    await centerChannel.SendMessage("ArkBot linked ...");
                });
            commands.CreateCommand("engage")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Rcon engaged for center map ...");
                    Thread t = new Thread(engageCenter);
                    t.Start();
                    await e.Channel.SendMessage("Rcon engaged for scorched map ...");
                    Thread tB = new Thread(engageScorched);
                    tB.Start();
                    await generalChannel.SendTTSMessage("ArkBot online");
                });            
        }

        private void RegisterBasicCommands()
        {
            commands.CreateCommand("help")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage($"{e.User.Name}" + "Please check #Arkbot_commands for bot commands. :grinning:");
                });
            commands.CreateCommand("rules")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Server Rules:\n1)No cheats or exploits.\n2)No raiding noobs unless they raid you first\n3)No preventing the use of obelisks.");
                });
            Console.WriteLine("help commands loaded ...");
        }

        private void RegisterSpawnCommands()
        {
            commands.CreateCommand("spawn center player")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/TheCenterSpawns.png");
                    await e.Channel.SendMessage("Player spawn locations for TheCenter map.");
                });
            commands.CreateCommand("spawn scorched player")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/ScorchedEarthSpawns.png");
                    await e.Channel.SendMessage("Player spawn locations for ScorchedEarth map.");
                });
            Console.WriteLine("spawn commands loaded ...");
        }

        private void RegisterShadowIsleCommands()
        {
            commands.CreateCommand("pvparena")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/PvpArenaOutside.png");
                    await e.Channel.SendFile("images/PvpArenaWaitingRoom.png");
                    await e.Channel.SendMessage("Please do not damage the arena. The turrets are to kill wild dinos only so players are kept safe during events. :grinning:");
                });
        }

        private void RegisterZoneCommands()
        {
            commands.CreateCommand("listzones")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Please pick a server:\n!listzones center\n!listzones scorched");
                });
            //start of center zones
            commands.CreateCommand("listzones center")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("The zone commands for The Center are:\n!halfburntisland\n!lavaisland\n!scorchedisland\n!eastbubble\n!easternislets\n!trench\n!tropicalislandnorth\n!tropicalislandsouth\n!junglesmid\n!junglesnorth\n!junglessouth\n!secludedisland\n!skullisland\n!soutbubble\n!underworld\n!floatingisland\n!edgeoftheworld\n!northsnowymountain\n!penguinpond\n!snowygrasslands\n!southsnowymountain\n!thebridge\n!redwoods\n");
                });
            commands.CreateCommand("easternislets")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterEasternIslets.png");
                });
            commands.CreateCommand("eastbubble")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterEastUnderwaterBubble.png");
                });
            commands.CreateCommand("edgeoftheworld")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterEdgeOfWorld.png");
                });
            commands.CreateCommand("floatingisland")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterFloatingIsland.png");
                });
            commands.CreateCommand("halfburntisland")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterHalfBurntIsland.png");
                });
            commands.CreateCommand("junglesmid")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterJunglesMid.png");
                });
            commands.CreateCommand("junglesnorth")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterJunglesNorth.png");
                });
            commands.CreateCommand("junglessouth")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterJunglesSouth.png");
                });
            commands.CreateCommand("lavaisland")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterLavaBiome.png");
                });
            commands.CreateCommand("northsnowymountain")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterNorthSnowyMountain.png");
                });
            commands.CreateCommand("penguinpond")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterPenguinPond.png");
                });
            commands.CreateCommand("redwoods")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterRedwoodForests.png");
                });
            commands.CreateCommand("scorchedisland")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterScorchedIsland.png");
                });
            commands.CreateCommand("secludedisland")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterSecludedIsland.png");
                });
            commands.CreateCommand("trench")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterSETrench.png");
                });
            commands.CreateCommand("skullisland")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterSkullIsland.png");
                });
            commands.CreateCommand("snowygrasslands")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterSnowyGrasslands.png");
                });
            commands.CreateCommand("southsnowymountain")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterSouthSnowyMountain.png");
                });
            commands.CreateCommand("southbubble")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterSouthUnderwaterBubble.png");
                });
            commands.CreateCommand("thebridge")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterTheBridge.png");
                });
            commands.CreateCommand("tropicalislandnorth")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterTropicalIslandNorth.png");
                });
            commands.CreateCommand("tropicalislandsouth")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterTropicalIslandSouth.png");
                });
            commands.CreateCommand("underworld")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("images/CenterUndergroundWorld.png");
                });
            //start of scorched zones
            commands.CreateCommand("listzones scorched")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage($"I'm sorry {e.User.Name}, but this command is not yet finished. Please try again later.");
                });
        }

        private async void engageCenter()
        {
            //rcon socket setup and connection

             messenger = new RconMessenger(socket);
            // initiate the connection with the remote server
            bool isConnected = await messenger.ConnectAsync("167.114.101.67", 18017);

            // try to authenticate with your supersecretpassword (... obviously this is my hackerproof key, you should use yours)
            bool authenticated = await messenger.AuthenticateAsync("qwe123123");
            if (authenticated)
            {
                await messenger.ExecuteCommandAsync("ServerChat ArkBot online ...");
                await centerChannel.SendMessage("Thread linked ...");

                int counter = 0;
                int keepalive = 0;
                string chat = "";
                while (true)
                {
                    if(counter == 10000)
                    {
                        chat = await messenger.ExecuteCommandAsync("GetGameLog");
                        if (chat.StartsWith("Server received, But no response!!"))
                        {

                        }
                        else if (chat.StartsWith("Keep Alive"))
                        {
                            keepalive++;
                        }
                        else if (chat.Contains("!discord"))
                        {
                            await messenger.ExecuteCommandAsync("ServerChat ArkBot: Our discord server is at https://discord.gg/XpmmBm2");
                        }
                        else
                        {
                            if (chat.Length > 2000)
                            {
                                string chatA = chat.Substring(0, 2000);
                                string chatB = chat.Substring(2000, chat.Length);
                                await centerChannel.SendMessage(chatA);
                                await centerChannel.SendMessage(chatB);
                                chat = "";
                            }
                            else
                            {
                                await centerChannel.SendMessage(chat);
                            }
                        }
                        counter = 0;
                        chat = "";
                        if(keepalive == 5)
                        {
                            await generalChannel.SendTTSMessage("ArkBot Suicide");
                            Environment.Exit(3);
                        }
                    }
                    counter++;
                    chat = "";
                }
            }
            else
            {
                Console.WriteLine("Rcon connection to center map failed ...");
            }

            //make thread to constantly pole center map chat and send it to in_game_chat_center channel
        }

        private async void engageScorched()
        {
            //rcon socket setup and connection

            messengerB = new RconMessenger(socketB);
            // initiate the connection with the remote server
            bool isConnectedB = await messengerB.ConnectAsync("167.114.101.67", 18027);

            // try to authenticate with your supersecretpassword (... obviously this is my hackerproof key, you shoul use yours)
            bool authenticatedB = await messengerB.AuthenticateAsync("qwe123123");
            if (authenticatedB)
            {
                await messengerB.ExecuteCommandAsync("ServerChat ArkBot online ...");
                await scorchedChannel.SendMessage("Thread linked ...");

                int counter = 0;
                int keepalive = 0;
                string chat = "";
                while (true)
                {
                    if (counter == 10000)
                    {
                        chat = await messengerB.ExecuteCommandAsync("GetGameLog");
                        if (chat.StartsWith("Server received, But no response!!"))
                        {

                        }
                        else if (chat.StartsWith("Keep Alive"))
                        {
                            keepalive++;
                        }
                        else
                        {
                            if (chat.Length > 2000)
                            {
                                string chatA = chat.Substring(0, 2000);
                                string chatB = chat.Substring(2000, chat.Length);
                                await scorchedChannel.SendMessage(chatA);
                                await scorchedChannel.SendMessage(chatB);
                                chat = "";
                            }else if (chat.Contains("!discord"))
                            {
                                await messengerB.ExecuteCommandAsync("ServerChat Our discord server is at https://discord.gg/XpmmBm2");
                            }else
                            {
                                await scorchedChannel.SendMessage(chat);
                            }
                        }
                        counter = 0;
                        chat = "";
                        if (keepalive == 5)
                        {
                            await generalChannel.SendTTSMessage("ArkBot Suicide");
                            Environment.Exit(4);
                        }
                    }
                    counter++;
                    chat = "";
                }
            }
            else
            {
                Console.WriteLine("Rcon connection to scorched map failed ...");
            }

            //make thread to constantly pole center map chat and send it to in_game_chat_scorched channel
        }

        private void RegisterRconCommands()
        {
            commands.CreateCommand("players")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Players on TheCenter map:\n" + await messenger.ExecuteCommandAsync("ListPlayers") + "\n\nPlayers on ScorchedEarth map:\n" + await messengerB.ExecuteCommandAsync("ListPlayers"));                    
                });
            commands.CreateCommand("saveworlds")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("saving center ... " + await messenger.ExecuteCommandAsync("SaveWorld"));
                    await e.Channel.SendMessage("saving scorched ... " + await messengerB.ExecuteCommandAsync("SaveWorld"));
                });
            commands.CreateCommand("daytime center")
                .Do(async (e) =>
                {
                    await messenger.ExecuteCommandAsync("SetTimeOfDay 08:30:00");
                    await e.Channel.SendMessage("Center time set to 08:30:00");
                });
            commands.CreateCommand("daytime scorched")
                .Do(async (e) =>
                {
                    await messengerB.ExecuteCommandAsync("SetTimeOfDay 08:30:00");
                    await e.Channel.SendMessage("Scorched Earth time set to 08:30:00");
                });
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
