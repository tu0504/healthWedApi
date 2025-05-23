﻿using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface ISurveyService
    {
        Task<List<SurveyResponse.GetSurveyModel>> GetSurveys();
        Task<SurveyResponse.GetSurveyDetailsModel?> GetSurveyById(Guid id);
        Task<SurveyResponse.GetSurveyDetailsModel?> GetByIdDelete(Guid id);
        Task AddSurvey(string userID, SurveyRequest.CreateSurveyRequest model);
        Task UpdateSurvey(Guid id, SurveyRequest.UpdateSurveyRequest model);
        Task RemoveSurvey(Guid id);
    }
}
