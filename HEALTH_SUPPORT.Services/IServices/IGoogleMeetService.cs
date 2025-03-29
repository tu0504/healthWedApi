using HEALTH_SUPPORT.Services.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface IGoogleMeetService
    {
        Task<string> CreateGoogleMeetEvent(GoogleMeetRequest request);
    }
}
