using HEALTH_SUPPORT.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.ResponseModel
{
    public class SurveyAnswerRecordResponse
    {
        public class SurveyAnswerRecordResponseModel
        {
            public Guid Id { get; set; }
            public Guid AccountSurveyId { get; set; }
            public Guid SurveyQuestionId { get; set; }
            public SurveyQuestionResponse.GetSurveyQuestionModel SurveyQuestion { get; set; }
            public Guid SurveyAnswerId { get; set; }
            public SurveyAnswerResponse.GetSurveyAnswerModel SurveyAnswer { get; set; }
            public bool IsDeleted { get; set; }
        }
    }
}
