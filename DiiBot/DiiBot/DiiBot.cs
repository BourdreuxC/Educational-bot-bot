// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using DiiBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DiiBot
{
    public class DiiBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            string api = "http://localhost:5025/api/Questions";
            HttpClient http = new HttpClient();
            TeamsMessage input = new TeamsMessage() { Message = turnContext.Activity.Text };
            var json = JsonConvert.SerializeObject(input);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await http.PostAsync(api, content);
            string result = response.Content.ReadAsStringAsync().Result;
            var replyText = $"{result}";
            var res = await turnContext.SendActivityAsync(MessageFactory.Text(replyText), cancellationToken);
            Console.WriteLine(res);
        }
    }
}
