using Bot_Application2.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application2.Dialogs
{
    public class ConferenceBotDialog
    {
        public static readonly IDialog<string> dialog = Chain.PostToChain()
                .Select(msg => msg.Text)
                .Switch(
                     new RegexCase<IDialog<string>>(new Regex("^hi", RegexOptions.IgnoreCase), (context, txt) =>
                     {
                         return Chain.ContinueWith(new GreetingDialog(), AfterGreetingContinuation);
                     }),
                     new DefaultCase<string, IDialog<string>>((context, txt) =>
                     {
                         return Chain.ContinueWith(FormDialog.FromForm(ConferenceReservation.BuildForm, FormOptions.PromptInStart), AfterGreetingContinuation);
                     }))
         .Unwrap()
         .PostToUser();

        private async static Task<IDialog<string>> AfterGreetingContinuation(IBotContext context, IAwaitable<object> res)
        {
            var token = await res;
            var name = "User";
            context.UserData.TryGetValue<string>("Name", out name);
            return Chain.Return($"Thank you for using the Conference Room Reservation bot: {name}");
        }
    }


}