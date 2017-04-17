using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using EmojiBot.Utilities;

namespace EmojiBot
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

                Activity reply = null;
                if (activity.Attachments.Count > 0)
                {

                    String emotionstext = "No emotions";
                    double dblexpression = 0.0;
                    System.Text.StringBuilder emotionsCollection = new System.Text.StringBuilder();

                    EmotionResult[] emotionResult = await Utilities.EmojiUtilities.CheckEmotion(activity.Attachments[0].ContentUrl);

                    if (emotionResult.Count() >0)
                    {
                        emotionsCollection.Append("Anger");
                        emotionsCollection.Append(emotionResult[0].scores.anger);
                        emotionsCollection.AppendLine();

                        emotionsCollection.Append("contempt");
                        emotionsCollection.Append(emotionResult[0].scores.contempt);
                        emotionsCollection.AppendLine();

                        emotionsCollection.Append("disgust");
                        emotionsCollection.Append(emotionResult[0].scores.disgust);
                        emotionsCollection.AppendLine();

                        emotionsCollection.Append("fear");
                        emotionsCollection.Append(emotionResult[0].scores.fear);
                        emotionsCollection.AppendLine();

                        emotionsCollection.Append("happiness");
                        emotionsCollection.Append(emotionResult[0].scores.happiness);
                        emotionsCollection.AppendLine();

                        emotionsCollection.Append("neutral");
                        emotionsCollection.Append(emotionResult[0].scores.neutral);
                        emotionsCollection.AppendLine();

                        emotionsCollection.Append("sadness");
                        emotionsCollection.Append(emotionResult[0].scores.sadness);
                        emotionsCollection.AppendLine();

                        emotionsCollection.Append("surprise");
                        emotionsCollection.Append(emotionResult[0].scores.surprise);
                        emotionsCollection.AppendLine();


                        if (emotionResult[0].scores.anger > 0)
                        {
                            dblexpression = emotionResult[0].scores.anger;
                            emotionstext = "anger";
                        }
                        if (emotionResult[0].scores.contempt > dblexpression)
                        {
                            emotionstext = "contempt";
                            dblexpression = emotionResult[0].scores.contempt;
                        }
                                              
                        if (emotionResult[0].scores.disgust > dblexpression)
                        {
                            emotionstext = "disgust";
                            dblexpression = emotionResult[0].scores.disgust;
                        }
                        if (emotionResult[0].scores.fear > dblexpression)
                        {
                            emotionstext = "fear";
                            dblexpression = emotionResult[0].scores.fear;
                        }

                        if (emotionResult[0].scores.happiness > dblexpression)
                        {
                            emotionstext = "happiness";
                            dblexpression = emotionResult[0].scores.happiness;
                        }

                        if (emotionResult[0].scores.neutral > dblexpression)
                        {
                            emotionstext = "neutral";
                            dblexpression = emotionResult[0].scores.neutral;
                        }

                        if (emotionResult[0].scores.sadness > dblexpression)
                        {
                            emotionstext = "sadness";
                            dblexpression = emotionResult[0].scores.sadness;
                        }
                        else if (emotionResult[0].scores.surprise > dblexpression)
                        {
                            emotionstext = "surprise";
                            dblexpression = emotionResult[0].scores.surprise;
                        }
                                                                       
                    }
                    else
                    {
                        emotionstext= "No images found";
                    }

                    emotionsCollection.AppendLine();
                    emotionsCollection.Append("------------------------");
                    emotionsCollection.Append(emotionstext); 

                    reply = activity.CreateReply(emotionsCollection.ToString());
                  }
                else
                {
                    reply = activity.CreateReply("Please add images");
                }
                
                // return our reply to the user
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
    }

 }