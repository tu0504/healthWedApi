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
    public class SurveyResultRequest
    {
        public class AddSurveyResultRequest
        {
<<<<<<< HEAD
            public string ResultDescription { get; set; }
            public Guid SurveyId { get; set; }
            public int MaxScore { get; set; }
            public int MinScore { get; set; }
=======
            public List<Guid>? SurveyAnswerList { get; set; }
            public List<int>? ScoreList { get; set; }
            public string ResultDescription { get; set; }
            public Guid SurveyId { get; set; }
>>>>>>> develop
        }
        
        public class UpdateSurveyResultRequest
        {
<<<<<<< HEAD
            public int MaxScore { get; set; }
            public int MinScore { get; set; }
            public string ResultDescription { get; set; }
            public Guid SurveyId { get; set; }
            public bool? IsDelete { get; set; }

=======
            public int Score { get; set; }
            public string ResultDescription { get; set; }
            public Guid AccountSurveyId { get; set; }
            public Guid SurveyId { get; set; }
>>>>>>> develop
        }
    }
}
