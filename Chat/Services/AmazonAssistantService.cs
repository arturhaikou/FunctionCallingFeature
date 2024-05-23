using Chat.Models;

namespace Chat.Services
{
    public class AmazonAssistantService : IAssistantService
    {
        private readonly IEshopApi _api;
        public AmazonAssistantService(IEshopApi api) => _api = api;

        public async Task<string> PerformActionAsync(UserRequest request)
        {
            return await _api.PerformAmazonlAssistantActionAsync(request);
        }
    }
}
