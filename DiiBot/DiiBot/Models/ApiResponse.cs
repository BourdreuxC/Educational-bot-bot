// <copyright file="ApiResponse.cs" company="DiiBot">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace DiiBot.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Format the response recieved.
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Gets or sets the answer of the question if exist.
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Gets or sets the users to mention if needed.
        /// </summary>
        public List<string> Mentions { get; set; }
    }
}
