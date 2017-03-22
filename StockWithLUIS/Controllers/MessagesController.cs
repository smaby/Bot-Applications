using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using StockWithLUIS.JSON; 

namespace StockWithLUIS
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                //string result = await GetStockValue(activity.Text);     
                string result;
                
                //get the JSON class from the Method that converts the LUIS to the StockLUIS
                StockLUIS stockLUISData = await GetEnityFromLUIS(activity.Text);

                


                if ((stockLUISData.intents.Count() > 0)  && (stockLUISData.entities.Count() > 0))
                {
                    switch (stockLUISData.intents[0].intent)
                    {
                        case "None":
                        case "StockPrice":
                            result = await GetStockValue(stockLUISData.entities[0].entity);
                            break;
                        default:
                            result = "Please repeat your question...";
                            break;
                    }
                }
                else
                {
                    result = "Please check stock symbol you are looking for....";

                }

                // return our reply to the user
                Activity reply = activity.CreateReply(result);
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }


        /// <summary>
        /// Method without Talking with LUIS
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private async Task<String> GetStockValue(string symbol)
        {
            string result = await StockWithLUIS.Modules.YahooStockServiceBot.GetStockRateAsync(symbol);
            return result;
        }

        /// <summary>
        /// get Entity populated from luis
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static async Task<StockLUIS> GetEnityFromLUIS(string text)
        {
            text = Uri.EscapeDataString(text);
            StockLUIS stockLUISData = new StockLUIS();

            using (HttpClient client = new HttpClient())
            {
                string requestURL = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/c72c94ba-ffe5-4131-b69b-3095419621a8?subscription-key=eab03dbe739546539cb46ab2ebbd9339&verbose=true&q={0}";
                requestURL = string.Format(requestURL, text); 

                HttpResponseMessage responseMessage = await client.GetAsync(requestURL);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var JsonDataResponse = await responseMessage.Content.ReadAsStringAsync();
                    stockLUISData = JsonConvert.DeserializeObject<StockLUIS>(JsonDataResponse);

                }
            }
            return stockLUISData;
        }
    }
}