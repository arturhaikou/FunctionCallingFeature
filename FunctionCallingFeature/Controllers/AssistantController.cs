using FunctionCallingFeature.Models.Requests;
using FunctionCallingFeature.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace FunctionCallingFeature.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssistantController : ControllerBase
    {
        private readonly Kernel _kernel;
        private static ChatHistory _chatHistory = new ChatHistory("You're the eshop assistant that can help users to show and buy products. Generate response based on the existed products in the catalog. Don't create new products");

        public AssistantController(Kernel kernel) => _kernel = kernel;

        [HttpPost("weather")]
        public async Task<IActionResult> PerformWeatherAction([FromKeyedServices("weatherAssistant")] IAssistantService service, [FromBody] UserRequest request)
        {
            var result = await service.PerformActionAsync(request.Input);
            return Ok(result);
        }

        [HttpPost("eshop")]
        public async Task<IActionResult> PerformEShopAction([FromKeyedServices("eshopAssistant")] IAssistantService service, [FromBody] UserRequest request)
        {
            var result = await service.PerformActionAsync(request.Input);
            return Ok(result);
        }

        [HttpPost("amazon")]
        public async Task<IActionResult> PerformAmazonAction([FromKeyedServices("amazonAssistant")] IAssistantService service, [FromBody] UserRequest request)
        {
            var result = await service.PerformActionAsync(request.Input);
            return Ok(result);
        }

        [HttpPost("kernel")]
        public async Task<IActionResult> PerformGeneralAction([FromBody] UserRequest request)
        {
            _chatHistory.AddUserMessage(request.Input);
            var completion = _kernel.GetRequiredService<IChatCompletionService>();
            OpenAIPromptExecutionSettings settings = new OpenAIPromptExecutionSettings()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                Temperature = 0
            };

            var result = await completion.GetChatMessageContentAsync(_chatHistory, settings, _kernel);

            _chatHistory.AddAssistantMessage(result.Content);
            return Ok(result.Content);
        }
    }
}
