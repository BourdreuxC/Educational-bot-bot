// -----------------------------------------------------------------------
// <copyright file="QuestionsController.cs" company="DIIAGE">
// Copyright (c) DIIAGE 2022. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace EducationalTeamsBotApi.WebApi.Controllers
{
    using EducationalTeamsBotApi.Application.Questions.Commands.AskQuestion;
    using EducationalTeamsBotApi.Application.Questions.Queries.GetAllQuestionsQuery;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Bot.Schema;
    //test
    /// <summary>
    /// Controller allowing to interact with questions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ApiBaseController
    {
        /// <summary>
        /// Get the list of questions.
        /// </summary>
        /// <returns>All Questions.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetQuestions()
        {
            try
            {
                var questions = await this.Mediator.Send(new GetAllQuestionsQuery());
                return this.Ok(questions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Answer a given question.
        /// </summary>
        /// <param name="activity">the question asked.</param>
        /// <returns>the answer.</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task QuestionAsked([FromBody] Activity activity)
        {
            try
            {
               await this.Mediator.Send(new AskQuestionCommand { Activity = activity });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
