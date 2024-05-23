using Chat.Models;

namespace Chat.Services
{
    public class EshopAssistantService : IAssistantService
    {
        private readonly IEshopApi _api;

        public EshopAssistantService(IEshopApi api) => _api = api;

        public async Task<string> PerformActionAsync(UserRequest request)
        {
            return await _api.PerformEshopAssistantActionAsync(request);
        }
    }
}
