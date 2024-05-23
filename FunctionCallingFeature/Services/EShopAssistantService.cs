using Azure.AI.OpenAI;
using FunctionCallingFeature.Functions.EShop.Cart;
using FunctionCallingFeature.Functions.EShop.Catalog;
using FunctionCallingFeature.Functions.EShop.Order;
using FunctionCallingFeature.Functions.EShop.Purchase;
using FunctionCallingFeature.Models.Requests.EShop;
using FunctionCallingFeature.Models.Responses.EShop;
using FunctionCallingFeature.Services.Interfaces;
using System.Text.Json;

namespace FunctionCallingFeature.Services
{
    public class EShopAssistantService : IAssistantService
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly ICatalogService _catalogService;
        private readonly IPurchaseService _purchaseService;

        public EShopAssistantService(IConfiguration configuration, IOrderService orderService, ICatalogService catalogService, ICartService cartService, IPurchaseService purchaseService) =>
            (_configuration, _orderService, _catalogService, _cartService, _purchaseService) = (configuration, orderService, catalogService, cartService, purchaseService);

        public async Task<string> PerformActionAsync(string input)
        {
            // Initialize AI client and options.
            var openAIKey = _configuration.GetValue<string>("API_KEY");
            var model = "gpt-3.5-turbo";
            var openAIClient = new OpenAIClient(openAIKey);
            var instruction = @"You are a helpful assistant that can access external functions. The responses from these function calls will be appended to this dialogue. Please provide responses based on the information from these function calls.";
            var systemMessage = new ChatRequestSystemMessage(instruction);
            var chatCompletionsOptions = new ChatCompletionsOptions
            {
                DeploymentName = model,
                Temperature = 0.0f,
                Messages = { systemMessage, new ChatRequestUserMessage(input) },
                Tools = { GetProductsFunction.GetFunctionDefinition(), CreateCartFunction.GetFunctionDefinition(), CreateOrderFunction.GetFunctionDefinition(), PurchaseOrderFunction.GetFunctionDefinition() }
            };

            var response = await openAIClient.GetChatCompletionsAsync(chatCompletionsOptions);
            var choice = response.Value.Choices.First();

            while (choice.FinishReason == CompletionsFinishReason.ToolCalls)
            {
                chatCompletionsOptions.Messages.Add(new ChatRequestAssistantMessage(choice.Message));
                var chatCompletionsFunctionToolCall = choice.Message.ToolCalls[0] as ChatCompletionsFunctionToolCall;
                switch (chatCompletionsFunctionToolCall.Name)
                {
                    case GetProductsFunction.FunctionName:
                        {
                            await CallFunction(chatCompletionsFunctionToolCall, chatCompletionsOptions, async () => await _catalogService.GetItemsAsync());
                            break;
                        }
                    case CreateCartFunction.FunctionName:
                        {
                            Func<CreateCartRequest, Task<CreateCartResponse>> func = async input =>
                            {
                                var id = await _cartService.CreateAsync(input.ProductIds);
                                return new CreateCartResponse { CartId = id };
                            };

                            await CallFunction(chatCompletionsFunctionToolCall, chatCompletionsOptions, func);
                            break;
                        }
                    case CreateOrderFunction.FunctionName:
                        {
                            Func<CreateOrderRequest, Task<CreateOrderResponse>> func = async input =>
                            {
                                var id = await _orderService.CreateAsync(input.CartId);
                                return new CreateOrderResponse { OrderId = id };
                            };

                            await CallFunction(chatCompletionsFunctionToolCall, chatCompletionsOptions, func);
                            break;
                        }
                    case PurchaseOrderFunction.FunctionName:
                        {
                            Func<PurchaseOrderRequest, Task<PurchaseOrderResponse>> func = async input =>
                            {
                                var isSuccess = await _purchaseService.BuyAsync(input.OrderId);
                                return new PurchaseOrderResponse { IsSuccess = isSuccess };
                            };

                            await CallFunction(chatCompletionsFunctionToolCall, chatCompletionsOptions, func);
                            break;
                        }

                }

                response = await openAIClient.GetChatCompletionsAsync(chatCompletionsOptions);
                choice = response.Value.Choices.First();
            }

            return choice.Message.Content;
        }

        private async Task CallFunction<Tin, Tout>(ChatCompletionsFunctionToolCall function, ChatCompletionsOptions chatCompletionsOptions, Func<Tin, Task<Tout>> func)
        {
            var argmt = function.Arguments;
            var functionInput = JsonSerializer.Deserialize<Tin>(argmt, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var functionOutput = await func(functionInput);
            var functionMessage = new ChatRequestToolMessage(JsonSerializer.Serialize(functionOutput), function.Id);
            chatCompletionsOptions.Messages.Add(functionMessage);
        }

        private async Task CallFunction<T>(ChatCompletionsFunctionToolCall function, ChatCompletionsOptions chatCompletionsOptions, Func<Task<T>> func)
        {
            var functionOutput = await func();
            var functionMessage = new ChatRequestToolMessage(JsonSerializer.Serialize(functionOutput), function.Id);
            chatCompletionsOptions.Messages.Add(functionMessage);
        }
    }
}
