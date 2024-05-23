using Azure.AI.OpenAI;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Text.Json;

namespace FunctionCallingFeature.Functions.Weather
{
    public class GetCapitalFunction
    {
        public const string FunctionName = "get_capital";

        static public ChatCompletionsFunctionToolDefinition GetFunctionDefinition()
        {
            return new ChatCompletionsFunctionToolDefinition()
            {
                Name = FunctionName,
                Description = "Get the capital of the location",
                Parameters = BinaryData.FromObjectAsJson(
                new
                {
                    Type = "object",
                    Properties = new
                    {
                        Location = new
                        {
                            Type = "string",
                            Description = "The country, e.g. Italy",
                        }
                    },
                    Required = new[] { "location" },
                },
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
            };
        }

        [KernelFunction("get_capital")]
        [Description("Get the capital of the location")]
        static public string GetCapital(string location)
        {
            return "Tokyo";
        }
    }
}
