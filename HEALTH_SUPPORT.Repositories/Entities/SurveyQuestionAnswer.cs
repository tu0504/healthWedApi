using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HEALTH_SUPPORT.Repositories.Abstraction;

namespace HEALTH_SUPPORT.Repositories.Entities
{
    public class SurveyQuestionAnswer : Entity<Guid>
    {
        public Guid SurveyAnswersId { get; set; }
        public SurveyAnswer SurveyAnswer { get; set; }

        public Guid SurveyQuestionsId { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }
    }
}
