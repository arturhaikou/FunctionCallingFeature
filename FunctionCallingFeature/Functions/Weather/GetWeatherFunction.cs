using Azure.AI.OpenAI;
using FunctionCallingFeature.Models.Responses;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Text.Json;

namespace FunctionCallingFeature.Functions.Weather
{
    public class GetWeatherFunction
    {
        public const string FunctionName = "get_weather";

        static public ChatCompletionsFunctionToolDefinition GetFunctionDefinition()
        {
            return new ChatCompletionsFunctionToolDefinition()
            {
                Name = FunctionName,
                Description = "Get the current weather in a given location.",
                Parameters = BinaryData.FromObjectAsJson(
                new
                {
                    Type = "object",
                    Properties = new
                    {
                        Location = new
                        {
                            Type = "string",
                            Description = "The city and state, e.g. San Francisco, CA",
                        },
                        Unit = new
                        {
                            Type = "string",
                            Enum = new[] { "Celsius", "Fahrenheit" },
                        }
                    },
                    Required = new[] { "location" },
                },
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
            };
        }

        [KernelFunction("get_weather")]
        [Description("Get the current weather in a given location")]
        static public WeatherResponse GetWeather(string location, string unit)
        {
            // Can be any logic to retrieve the temperature for any locations.
            // For simplifying it returns 31 for every request
            return new WeatherResponse { Temperature = 31, Unit = unit };
        }
    }
}
