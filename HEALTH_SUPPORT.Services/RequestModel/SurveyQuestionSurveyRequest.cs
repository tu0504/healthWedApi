using HEALTH_SUPPORT.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.RequestModel
{
    public class SurveyQuestionSurveyRequest
    {
        public class AddSurveyQuestionSurvey
        {
            public Guid SurveyQuestionsId { get; set; }
            public Guid SurveysId { get; set; }
        }
    }
}
