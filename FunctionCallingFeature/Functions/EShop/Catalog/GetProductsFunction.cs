using Azure.AI.OpenAI;

namespace FunctionCallingFeature.Functions.EShop.Catalog
{
    public class GetProductsFunction
    {
        public const string FunctionName = "get_products";

        public static ChatCompletionsFunctionToolDefinition GetFunctionDefinition()
        {
            return new ChatCompletionsFunctionToolDefinition
            {
                Name = FunctionName,
                Description = "Get catalog products"
            };
        }
    }
}
