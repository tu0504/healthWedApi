using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.Implementations
{
    public class GoogleMeetService : IGoogleMeetService
    {
        private static readonly string[] Scopes = { CalendarService.Scope.Calendar };
        private const string ApplicationName = "Google Meet API";
        private CalendarService GetCalendarService()
        {
            using (var stream = new FileStream("google-credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;

                return new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
            }
        }

        public async Task<string> CreateGoogleMeetEvent(GoogleMeetRequest request)
        {
            var service = GetCalendarService();

            Event newEvent = new Event()
            {
                Summary = request.Summary,
                Description = request.Description,
                Start = new EventDateTime()
                {
                    DateTime = request.StartTime,
                    TimeZone = "Asia/Ho_Chi_Minh"
                },
                End = new EventDateTime()
                {
                    DateTime = request.EndTime,
                    TimeZone = "Asia/Ho_Chi_Minh"
                },
                ConferenceData = new ConferenceData()
                {
                    CreateRequest = new CreateConferenceRequest()
                    {
                        RequestId = Guid.NewGuid().ToString(),
                        ConferenceSolutionKey = new ConferenceSolutionKey()
                        {
                            Type = "hangoutsMeet"
                        }
                    }
                },
                Attendees = request.Attendees?.ConvertAll(email => new EventAttendee() { Email = email })
            };

            EventsResource.InsertRequest insertRequest = service.Events.Insert(newEvent, "primary");
            insertRequest.ConferenceDataVersion = 1;
            Event createdEvent = await insertRequest.ExecuteAsync();

            return createdEvent.HangoutLink;
        }
    }
}
