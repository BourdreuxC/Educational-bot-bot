// <copyright file="DiiBot.cs" company="DiiBot">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiiBot
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using global::DiiBot.Models;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Teams;
    using Microsoft.Bot.Schema;
    using Newtonsoft.Json;

    /// <summary>
    /// Handle the interactions with the bot.
    /// </summary>
    public class DiiBot : ActivityHandler
    {
        /// <inheritdoc/>
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            string api = "http://localhost:5025/api/Questions";

            Mention[] m = turnContext.Activity.GetMentions();
            var messageText = turnContext.Activity.Text.Replace("\n", string.Empty);
            for (int i = 0; i < m.Length; i++)
            {
                if (turnContext.Activity.MentionsId(m[i].Mentioned.Id) && m[i].Text != null)
                {
                   messageText = messageText.Replace(m[i].Text, string.Empty);
                }
            }

            List<string> tags = new List<string>();

            string[] hey = messageText.Split('#');
            for (int i = 1; i < hey.Length; i++)
            {
                string tag = hey[i].Split(' ')[0];
                tags.Add(tag);
                messageText = messageText.Replace($"#{tag}", string.Empty);
            }

            HttpClient http = new HttpClient();
            TeamsMessage input = new TeamsMessage() { Message = messageText,  Tags = tags };
            var json = JsonConvert.SerializeObject(input);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await http.PostAsync(api, content);
            string result = response.Content.ReadAsStringAsync().Result;
            ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);
            var replyActivity = MessageFactory.Text($"{apiResponse.Answer}");
            if (apiResponse.Mentions.Count > 0)
            {
                var members = await TeamsInfo.GetMembersAsync(turnContext);
                var mem = members.First(m => m.AadObjectId == apiResponse.Mentions[0]);
                Mention mention = new Mention() { Mentioned = mem, Text = $"<at>{XmlConvert.EncodeName(mem.Name)}</at>" };
                replyActivity.Text = $"{apiResponse.Answer} : {mention.Text}";
                replyActivity.Entities = new List<Entity> { mention };
            }

            await turnContext.SendActivityAsync(replyActivity, cancellationToken);
        }
    }
}
