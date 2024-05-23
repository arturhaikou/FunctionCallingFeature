using Chat.Models;

namespace Chat.Services
{
    public class WeatherAssistantService : IAssistantService
    {
        private readonly IEshopApi _api;

        public WeatherAssistantService(IEshopApi api) => _api = api;

        public async Task<string> PerformActionAsync(UserRequest request)
        {
            return await _api.PerformWeatherAssistantActionAsync(request);
        }
    }
}
