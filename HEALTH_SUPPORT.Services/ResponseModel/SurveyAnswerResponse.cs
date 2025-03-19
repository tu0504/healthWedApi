using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.ResponseModel
{
    public class SurveyAnswerResponse
    {
        public class GetSurveyAnswerModel
        {
            public Guid Id { get; set; }
            public string Content { get; set; }
            public int Point { get; set; }
            public DateTimeOffset CreateAt { get; set; }
            public DateTimeOffset? ModifiedAt { get; set; }
            public bool IsDelete { get; set; }
            public List<Guid> QuestionIds { get; set; }
        }
    }
}
