using Chat.Models;

namespace Chat.Services
{
    public class SemanticKernelAssistantService : IAssistantService
    {
        private readonly IEshopApi _api;

        public SemanticKernelAssistantService(IEshopApi api) => _api = api;

        public async Task<string> PerformActionAsync(UserRequest request)
        {
            return await _api.PerformKernelAssistantActionAsync(request);
        }
    }
}
