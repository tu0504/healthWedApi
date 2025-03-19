using System;
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
        // Model to represent a Category (Id + Name)
        public record CategoryModel(Guid Id, string CategoryName);

        // Model to represent a Psychologist (Id + Name)
        public record PsychologistModel(Guid Id, string Name, string Specialization);

        // Response model for fetching category and psychologist lists
        public record GetSubscriptionFormData(
            List<CategoryModel> Categories,
            List<PsychologistModel> Psychologists
        );
    }
}
