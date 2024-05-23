using Azure.AI.OpenAI;
using System.Text.Json;

namespace FunctionCallingFeature.Functions.EShop.Purchase
{
    public class PurchaseOrderFunction
    {
        public const string FunctionName = "purchase_order";

        public static ChatCompletionsFunctionToolDefinition GetFunctionDefinition()
        {
            return new ChatCompletionsFunctionToolDefinition
            {
                Name = FunctionName,
                Description = "Purchase the order",
                Parameters = BinaryData.FromObjectAsJson(
                    new
                    {
                        Type = "object",
                        Properties = new
                        {
                            OrderId = new
                            {
                                Type = "number",
                                Description = "The id of the order"
                            }
                        },
                        Required = new[] { "orderId" }
                    },
                    new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            };
        }
    }
}
