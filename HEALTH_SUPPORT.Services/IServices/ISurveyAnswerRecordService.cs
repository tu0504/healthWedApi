using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface ISurveyAnswerRecordService
    {
        Task<List<SurveyAnswerRecordResponse.SurveyAnswerRecordResponseModel?>> GetSurveyAnswerRecordById(Guid id);
        Task AddSurveyAnswerRecord(SurveyAnswerRecordRequest.AddSurveyAnswerRecordRequest model);
        Task RemoveSurveyAnswerRecord(Guid id);
    }
}
