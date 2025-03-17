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
            public string ResultDescription { get; set; }
            public Guid SurveyId { get; set; }
            public int MaxScore { get; set; }
            public int MinScore { get; set; }
        }
        
        public class UpdateSurveyResultRequest
        {
            public int MaxScore { get; set; }
            public int MinScore { get; set; }
            public string ResultDescription { get; set; }
            public Guid SurveyId { get; set; }
            public bool? IsDelete { get; set; }
        }
    }
}
