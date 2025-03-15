using HEALTH_SUPPORT.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Repositories.Entities
{
    public class SurveyAnswerRecord : Entity<Guid>
    {
        [Required]
        public Guid SurveyQuestionId { get; set; }
        [ForeignKey("SurveyQuestionId")]
        public SurveyQuestion SurveyQuestion { get; set; }

        [Required]
        public Guid SurveyAnswerId { get; set; }
        [ForeignKey("SurveyAnswerId")]
        public SurveyAnswer SurveyAnswer { get; set; }
        [Required]
        public Guid AccountSurveyId { get; set; }
        [ForeignKey("AccountSurveyId")]
        public AccountSurvey Account { get; set; }

        public DateTimeOffset CreateAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
