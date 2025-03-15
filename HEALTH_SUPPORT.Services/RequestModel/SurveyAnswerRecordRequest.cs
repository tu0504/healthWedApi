using HEALTH_SUPPORT.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.RequestModel
{
    public class SurveyAnswerRecordRequest
    {
        public class AddSurveyAnswerRecordRequest
        {
            public Guid AccountSurveyId { get; set; }
            public List<AddQuestionAndAnswerRequest> QuestionAndAnswerRequests { get; set; } = new();
        }

        public class AddQuestionAndAnswerRequest
        {
            public Guid SurveyQuestionId { get; set; }
            public Guid SurveyAnswerId { get; set; }
        }
    }
}
