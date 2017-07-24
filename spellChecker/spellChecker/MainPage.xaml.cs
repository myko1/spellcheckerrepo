using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Net;
using System.Collections.Generic;
using System.Linq;

namespace spellChecker
{
    public partial class MainPage : ContentPage
    {


        public class Spellc
        {
            public string Offset
            {
                get;
                set;
            }
            public string token
            {
                get;
                set;
            }
            public string Suggestion
            {
                get;
                set;
            }
        }
        public class Spellcol
        {
            public Spellc spco
            {
                get;
                set;
            }
        }
       
        private async void OnClick(object sender)
        {
            var sp = await spellcheck();
            for (int i = 0; i < sp.Count(); i++)
            {
                Spellc sc = sp.ElementAt(i);
                s.Text = sc.Suggestion;
            }
        }
        async Task<IEnumerable<Spellc>> spellcheck()
        {
            List<Spellc> spelc = new List<Spellc>();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "6daabdf7ea5a4d56baa2bacfbd950ca5");
            string text = text1.Text;
            string mode = "proof";
            string mkt = "en-us";
            var SpellEndPoint = "https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/?";
            var result = await client.GetAsync(string.Format("{0}text={1}&mode={2}&mkt={3}", SpellEndPoint, text, mode, mkt));
            result.EnsureSuccessStatusCode();
            var json = await result.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(json);
            foreach (int item in data)
            {

                spelc.Add(new Spellc
                {
                     
                    Offset = "Offset : " + data.flaggedTokens[item].offset,
                    token = "Wrong Word : " + data.flaggedTokens[item].token,
                    Suggestion = "Spelling Suggestion : " + data.flaggedTokens[item].suggestions[0].suggestion
                });
            }
            return spelc;
        }

    }
}
