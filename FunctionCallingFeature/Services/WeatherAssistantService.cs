using Azure.AI.OpenAI;
using Azure.Core;
using FunctionCallingFeature.Functions.Weather;
using FunctionCallingFeature.Models.Requests;
using FunctionCallingFeature.Models.Responses;
using FunctionCallingFeature.Services.Interfaces;
using System.Text.Json;

namespace FunctionCallingFeature.Services
{
    public class WeatherAssistantService : IAssistantService
    {
        private readonly IConfiguration _configuration;
        public WeatherAssistantService(IConfiguration configuration) => _configuration = configuration;

        public async Task<string> PerformActionAsync(string input)
        {
            // Initialize OpenAI client and options.
            var model = "gpt-3.5-turbo";
            var openAIKey = _configuration.GetValue<string>("API_KEY");
            var client = new OpenAIClient(openAIKey);

            var chatCompletionsOptions = new ChatCompletionsOptions
            {
                DeploymentName = model,
                Messages = { new ChatRequestUserMessage(input) },
                Temperature = 0.0f,
                Tools = { GetWeatherFunction.GetFunctionDefinition(), GetCapitalFunction.GetFunctionDefinition() }
            };

            var response = await client.GetChatCompletionsAsync(chatCompletionsOptions);
            var choice = response.Value.Choices.First();

            while (choice.FinishReason == CompletionsFinishReason.ToolCalls)
            {
                chatCompletionsOptions.Messages.Add(new ChatRequestAssistantMessage(choice.Message));
                PerformAction(choice, chatCompletionsOptions);
                response = await client.GetChatCompletionsAsync(chatCompletionsOptions);
                choice = response.Value.Choices.First();
            }

            return choice.Message.Content;
        }

        private void PerformAction(ChatChoice choice, ChatCompletionsOptions chatCompletionsOptions)
        {
            var chatCompletionsFunctionToolCall = choice.Message.ToolCalls[0] as ChatCompletionsFunctionToolCall;
            switch (chatCompletionsFunctionToolCall.Name)
            {
                case GetWeatherFunction.FunctionName:
                    {
                        CallFunction<WeatherRequest, WeatherResponse>(chatCompletionsFunctionToolCall, chatCompletionsOptions, request => GetWeatherFunction.GetWeather(request.Location, request.Unit));
                        break;
                    }
                case GetCapitalFunction.FunctionName:
                    {
                        var getCapitalRequest = JsonSerializer.Deserialize<GetCapitalRequest>(chatCompletionsFunctionToolCall.Arguments);
                        var capital = GetCapitalFunction.GetCapital(getCapitalRequest.Location);
                        var functionMessage = new ChatRequestToolMessage(JsonSerializer.Serialize(capital), chatCompletionsFunctionToolCall.Id);
                        chatCompletionsOptions.Messages.Add(functionMessage);

                        // CallFunction<GetCapitalRequest, string>(chatCompletionsFunctionToolCall, chatCompletionsOptions, request => GetCapitalFunction.GetCapital(request.Location));
                        break;
                    }
            }
        }

        private void CallFunction<Tin, Tout>(ChatCompletionsFunctionToolCall tool, ChatCompletionsOptions chatCompletionsOptions, Func<Tin, Tout> func)
        {
            var functionInput = JsonSerializer.Deserialize<Tin>(tool.Arguments);
            var functionOutput = func(functionInput);
            var functionMessage = new ChatRequestToolMessage(JsonSerializer.Serialize(functionOutput), tool.Id);
            chatCompletionsOptions.Messages.Add(functionMessage);
        }
    }
}
