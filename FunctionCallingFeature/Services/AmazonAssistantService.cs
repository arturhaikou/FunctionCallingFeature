using Amazon.BedrockRuntime.Model;
using Amazon.BedrockRuntime;
using Amazon.Util;
using FunctionCallingFeature.Functions.Weather;
using FunctionCallingFeature.Services.Interfaces;
using FunctionCallingFeature.Models.Amazon;
using Newtonsoft.Json.Linq;
using FunctionCallingFeature.Models.Responses;
using Newtonsoft.Json;
using Amazon;
using FunctionCallingFeature.Models.Requests;

namespace FunctionCallingFeature.Services
{
    public class AmazonAssistantService : IAssistantService
    {
        private readonly IConfiguration _configuration;

        public AmazonAssistantService(IConfiguration configuration) => _configuration = configuration;
        public async Task<string> PerformActionAsync(string input)
        {
            AmazonBedrockRuntimeClient bedrockClient = new(_configuration.GetValue<string>("AWS:KeyId"), _configuration.GetValue<string>("AWS:SecretKey"), RegionEndpoint.USEast1);
            string prompt = $@"This is a task converting text into a function chain calls. We will first give the function schemas and then ask a question in text. You are asked to determine what function should call first to achieve user action. We will give output format and you generate a request object.
IMPORTANT: The output must be only a JSON object without any explanations. Make Sure the JSON is valid.
IMPORTANT: You should add only one function into the output.

Schema:
{JObject.FromObject(GetCapitalAmazonFunction.GetFunctionDefinition())}

Schema:
{JObject.FromObject(GetWeatherFunction.GetFunctionDefinition())}

output:
{JObject.FromObject(FunctionOutput.GetFunctionOutput())}
|


User: {input}


output:";
            string functionName = string.Empty;
            string capital = string.Empty;
            WeatherResponse weather = null;
            do
            {
                string payload = JObject.FromObject(new
                {
                    inputText = prompt,
                    textGenerationConfig = new
                    {
                        temperature = 0,
                        stopSequences = new List<string> { "|" },
                        topP = 1,
                        maxTokenCount = 8192
                    }
                }).ToString();
                InvokeModelResponse imResponse = await bedrockClient.InvokeModelAsync(new InvokeModelRequest()
                {
                    ModelId = "amazon.titan-text-express-v1",
                    Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload),
                    ContentType = "application/json",
                    Accept = "application/json"
                });
                if (imResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jo = DeserializeFromStream(imResponse.Body) as JObject;
                    var function = GetFunction(jo);
                    prompt += $"\n{GetOutputText(jo)}";
                    functionName = function.FunctionName;
                    switch (functionName)
                    {
                        case GetCapitalAmazonFunction.Name:
                            {
                                var parameters = function.Parameters.ToObject<GetCapitalRequest>();
                                capital = GetCapitalAmazonFunction.GetCapital(parameters.Location);
                                prompt += $"\n\n\nUser: {capital}\n\n\noutput:";
                                break;
                            }
                        case GetWeatherAmazonFunction.Name:
                            {
                                var parameters = function.Parameters.ToObject<WeatherRequest>();
                                weather = GetWeatherAmazonFunction.GetWeather(parameters.Location, parameters.Unit);
                                break;
                            }
                    }
                }
                else
                {
                    Console.WriteLine("InvokeModelAsync failed with status code " + imResponse.HttpStatusCode);
                    break;
                }
            } while (functionName != GetWeatherAmazonFunction.Name);

            return $"The current weather in {capital} is {weather?.Temperature} {weather?.Unit}";
        }

        private object DeserializeFromStream(Stream stream)
        {
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                return serializer.Deserialize(jsonTextReader);
            }
        }

        private FunctionRequest GetFunction(JObject obj)
        {
            var rs = obj.GetValue("results")[0] as JObject;
            var output = rs.GetValue("outputText").ToString();
            var str = output.Trim().Replace("\u0022", "\"").Replace("\r\n", "");
            var jo = JsonConvert.DeserializeObject<FunctionRequest>(str);
            return jo;
        }

        private string GetOutputText(JObject obj)
        {
            var rs = obj.GetValue("results")[0] as JObject;
            var output = rs.GetValue("outputText").ToString();
            return output.Trim().Replace("\u0022", "\"").Replace("\r\n", "");
        }
    }
}
