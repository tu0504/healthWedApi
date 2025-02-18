using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HEALTH_SUPPORT.Repositories.Abstraction;

namespace HEALTH_SUPPORT.Repositories.Entities
{
    public class SurveyResults : Entity<Guid>
    {
        [Required]
        public int Score { get; set; }
        [Required]
        public string ResultDescription { get; set; }
        public Guid AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }

        public Guid SurveyTypeId { get; set; }
        [ForeignKey("SurveyTypeId")]
        public SurveyType SurveyType { get; set; }
    }
}
