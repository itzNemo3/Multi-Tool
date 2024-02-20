using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.IO;

namespace MultiTool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Multi-Tool";
            Console.Clear();
            Console.WriteLine("Welcome to Multi-Tool");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.logOption("1", "Nitro Gen");
            Console.logOption("2", "Nuker");
            Console.logOption("3", "Option 3");
            Console.logOption("0", "Exit");

            Console.log("Enter your choice: ", false);
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                Console.log("How Much Do You Want To Gen: ", false);
                string howmuch = Console.ReadLine();
                Console.log("Generating pls Wait");
                if (int.TryParse(howmuch, out int howMany))
                {
                    for (int i = 0; i < howMany; i++) { NitroGen.SendPostRequest().Wait(); }
                    Console.log($"Links Generated: {NitroGen.quantity}");
                    Console.log($"You can find the results in: {Environment.CurrentDirectory + "\\results.txt"}");
                }
            }
            if (choice == "2")
            {

            }
            else
            {
                Console.log("Invalid choice. Please try again.");
            }

            Console.WriteLine();
            Console.ReadKey();
            Console.Clear();
            Main(null);
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
        public static void Clear() { System.Console.Clear(); }
        public static void WriteLine() { System.Console.WriteLine(); }
        public static void WriteLine(string value) { System.Console.WriteLine(value); }
        public static void Write(string value) { System.Console.Write(value); }
        public static string ReadLine() { return System.Console.ReadLine(); }
        public static ConsoleKeyInfo ReadKey() { return System.Console.ReadKey(); }
    }
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

}
