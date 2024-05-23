namespace FunctionCallingFeature.Services.Interfaces
{
    public interface IAssistantService
    {
        Task<string> PerformActionAsync(string input);
    }
}
