﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.ResponseModel
{
    public static class SubscriptionResponse
    {
        public record GetSubscriptionsModel(
            Guid Id,
            string SubscriptionName,
            string Description,
            float Price,
            int Duration,
            string CategoryName,
            string PsychologistName,
            string Purpose,
            string Criteria,
            string FocusGroup,
            string AssessmentTool
        );
    }
}
