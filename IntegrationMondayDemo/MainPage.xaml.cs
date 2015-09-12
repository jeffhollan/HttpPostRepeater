using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IntegrationMondayDemo
{
    /// <summary>
    /// Simple Windows 10 app to push an Azure Logic App a bunch of times with a JSON Packet
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string[] locations = { "China", "India", "United States", "Indonesia", "Brazil", "Pakistan", "Nigeria", "Bangladesh", "Russia", "Japan", "Mexico", "Phillipines", "Vietnam", "Ethiopia", "Egypt", "United Kingdom", "Germany", "Turkey", "Iran", "France", "Italy", "Australia", "New Zealand", "United States", "Canada", "Canada", "United Kingdom", "United Kingdom", "United Staes" };
        private string[] customers = { "Jeff", "Josh", "Stephen", "Priti", "Prashant", "Rachel", "Shyam", "Srivatsan", "Sameer", "Rajesh", "Jon", "Tony", "John", "Ping", "Lakshmi" };
        //Loading a Resources File that has the URL and AuthToken.  Doing this so I can store nice places like GitHub without compromising :)
        private string url;
        private string authtoken;
        public MainPage()
        {
            this.InitializeComponent();

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            url = loader.GetString("url");
            authtoken = loader.GetString("authtoken");

            text.Text = "";
        }

        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            for (int x = 0; x < 500; x++)
            {
                var upload = new Upload { name = "ManualTrigger", outputs = new Upload.Outputs { customer = customers[rnd.Next(0, customers.Length - 1)],  trackingId = Guid.NewGuid(), location = locations[rnd.Next(0, locations.Length - 1)] } };
                Debug.WriteLine(JsonConvert.SerializeObject(upload));
                await CallLogicApp(JsonConvert.SerializeObject(upload));
                
            }
        }

        private async Task<HttpResponseMessage> CallLogicApp(string stringcontent)
        {
            using (var client = new HttpClient())
            {
                HttpContent content = new StringContent(stringcontent);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authtoken);
                var response = await client.PostAsync(url, content);
                text.Text = text.Text + "\n" + response.ReasonPhrase;
                return response;
            }
        }
    }

    internal class Upload
    {
        public string name { get; set; }
        public Outputs outputs { get; set; }

        

        public class Outputs
        {
            
            public string customer { get; set; }

            public string location { get; set; }
            

            public Guid trackingId { get; set; }
        }

        
    }


}
