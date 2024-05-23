using Chat.Models;

namespace Chat.Services
{
    public interface IAssistantService
    {
        Task<string> PerformActionAsync(UserRequest request);
    }
}
