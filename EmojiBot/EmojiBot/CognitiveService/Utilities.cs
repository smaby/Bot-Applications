using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.ProjectOxford.Emotion; 

namespace EmojiBot.Utilities
{
    public class EmojiUtilities
    {

        private static readonly EmotionServiceClient emotionClientService = new EmotionServiceClient("f18b29b86cbe49e993bdf2db5a70d622");
        /// <summary>
        /// Method to get the emotions depending on the images shared
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async Task<EmotionResult[]> CheckEmotion(string query)
        {
                     
            HttpClient client = new HttpClient();
            var  QueryString = HttpUtility.ParseQueryString(string.Empty);
            string responsestring = string.Empty;

            // Key from Microsoft cognitive serivice for emotion API. Key should be generated from the Azure portal. 
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "{f18b29b86cbe49e993bdf2db5a70d622}");

            //API for the Emoji provided by Congitive service of MS
            string cognitiveEmotionAPI = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize?" + QueryString;

            //var uri = string.Format(cognitiveEmotionAPI, QueryString);

            HttpResponseMessage response = null;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(query);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(cognitiveEmotionAPI, content).ConfigureAwait(false)  ;
            }

            responsestring = await response.Content.ReadAsStringAsync();
            EmotionResult[] faces = JsonConvert.DeserializeObject<EmotionResult[]>(responsestring);

            return faces;
        }
    }

    public class scores
    {
        public double anger { get; set; }
        public double contempt { get; set; }
        public double disgust { get; set; }
        public double fear { get; set; }
        public double happiness { get; set; }
        public double neutral { get; set; }
        public double sadness { get; set; }
        public double surprise { get; set; }
    }

    public class EmotionResult
    {
        public scores scores { get; set; }
    }
}