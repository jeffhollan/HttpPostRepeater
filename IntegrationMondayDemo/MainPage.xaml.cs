using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IntegrationMondayDemo
{
    /// <summary>
    /// Simple Windows 10 app to push an Azure Logic App a bunch of times with a JSON Packet
    /// </summary>
    public sealed partial class MainPage : Page
    {
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
            for (int x = 0; x < 1000; x++)
            {
                var upload = new Upload { name = "ManualTrigger", outputs = new Upload.Outputs { name = "Jeff Hollan", id = Guid.NewGuid(), timestamp = DateTime.UtcNow, prioritize = false, value = 1.1 } };
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
            public Guid id { get; set; }
            public string name { get; set; }
            public DateTime timestamp { get; set; }
            public double value { get; set; }

            public bool prioritize { get; set; }
        }
    }


}
