using HEALTH_SUPPORT.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Repositories.Entities
{
    public class SurveyQuestionSurvey : Entity<Guid>
    {
        public Guid SurveyQuestionsId { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }

        public Guid SurveysId { get; set; }
        public Survey Survey { get; set; }
    }
}
