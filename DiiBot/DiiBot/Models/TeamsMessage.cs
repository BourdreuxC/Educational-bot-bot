// <copyright file="TeamsMessage.cs" company="DiiBot">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiiBot.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Format the message sent to the API.
    /// </summary>
    public class TeamsMessage
    {
        /// <summary>
        /// Gets or sets the content of the request.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets if provided, the tags contained in the query.
        /// </summary>
        public List<string> Tags { get; set; }
    }
}
