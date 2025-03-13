using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.RequestModel
{
    public class SurveyAnswerRequest
    {
        public class CreateSurveyAnswerRequest
        {
            public string Content { get; set; }
            public int Point { get; set; }
<<<<<<< HEAD
=======
            public Guid QuestionId { get; set; }
>>>>>>> develop
        }
        public class UpdateSurveyAnswerRequest
        {
            public string Content { get; set; }
            public int Point { get; set; }
<<<<<<< HEAD
=======
            public Guid QuestionId { get; set; }
>>>>>>> develop
            public bool? IsDelete { get; set; }
        }
    }

}
