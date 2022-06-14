// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using DiiBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DiiBot
{
    public class DiiBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            string api = "http://localhost:5025/api/Questions";

            Mention[] m = turnContext.Activity.GetMentions();
            var messageText = turnContext.Activity.Text.Replace("\n", "");
            for (int i = 0; i < m.Length; i++)
            {
                if (turnContext.Activity.MentionsId(m[i].Mentioned.Id))
                {
                    //Bot is in the @mention list.
                    //The below example will strip the bot name out of the message, so you can parse it as if it wasn't included. Note that the Text object will contain the full bot name, if applicable.
                    if (m[i].Text != null)
                        messageText = messageText.Replace(m[i].Text, "");
                }
            }

            List<string> tags = new List<string>();

            string[] hey = messageText.Split('#');
            for (int i = 1; i < hey.Length; i++)
            {
                string tag = hey[i].Split(' ')[0];
                tags.Add(tag);
                messageText = messageText.Replace($"#{tag}", "");
            }

            HttpClient http = new HttpClient();
            TeamsMessage input = new TeamsMessage() { Message = messageText,  Tags = tags };
            var json = JsonConvert.SerializeObject(input);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await http.PostAsync(api, content);
            string result = response.Content.ReadAsStringAsync().Result;
            ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);
            var replyActivity = MessageFactory.Text($"{apiResponse.Answer}");
            if (apiResponse.Mentions != null)
            {     
                var members = await TeamsInfo.GetMembersAsync(turnContext);
                var mem = members.Where(m => m.AadObjectId == apiResponse.Mentions[0]).First();
                Mention mention = new Mention() { Mentioned = mem, Text = $"<at>{XmlConvert.EncodeName(mem.Name)}</at>" };
                replyActivity.Text = $"{apiResponse.Answer} : {mention.Text}";
                replyActivity.Entities = new List<Entity> { mention };
            }
            await turnContext.SendActivityAsync(replyActivity, cancellationToken);
        }
    }
}
