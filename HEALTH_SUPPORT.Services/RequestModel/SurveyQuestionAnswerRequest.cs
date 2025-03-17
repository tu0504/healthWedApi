using HEALTH_SUPPORT.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.RequestModel
{
    public class SurveyQuestionAnswerRequest
    {
        public class AddSurveyQuestionAnswer
        {
            public Guid SurveyAnswersId { get; set; }
            public Guid SurveyQuestionsId { get; set; }
        }
    }
}
