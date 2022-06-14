using System.Collections.Generic;

namespace DiiBot.Models
{
    public class ApiResponse
    {
        /// <summary>
        /// The answer of the question if exist.
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// The user to mention if needed.
        /// </summary>
        public List<string> Mentions { get; set; }
    }
}
