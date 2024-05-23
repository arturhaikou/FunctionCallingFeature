using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FunctionCallingFeature.Models.Amazon
{
    public class FunctionRequest
    {
        [JsonProperty("function_name")]
        public string FunctionName { get; set; }
        public JObject Parameters { get; set; }
    }
}
