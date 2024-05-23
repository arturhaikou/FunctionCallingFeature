using FunctionCallingFeature.Models.Responses;

namespace FunctionCallingFeature.Models.Amazon
{
    public class GetWeatherAmazonFunction
    {
        public const string Name = "get_weather";

        public static object GetFunctionDefinition()
        {
            return new
            {
                Name = Name,
                Description = "Get the current weather in a given location",
                input_schema = new
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
                            Description = "The unit of temperature, either \"Celsius\" or \"Fahrenheit\". The default value is \"Celsius\"."
                        }
                    },
                    Required = new[] { "location" },
                }
            };
        }

        static public WeatherResponse GetWeather(string location, string unit)
        {
            return new WeatherResponse { Temperature = 31, Unit = unit };
        }
    }
}
