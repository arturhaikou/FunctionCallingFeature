namespace FunctionCallingFeature.Models.Amazon
{
    public class GetCapitalAmazonFunction
    {
        public const string Name = "get_capital";

        public static object GetFunctionDefinition()
        {
            return new
            {
                Name = Name,
                Description = "Get the capital of the location",
                input_schema = new
                {
                    Type = "object",
                    Properties = new
                    {
                        Location = new
                        {
                            Type = "string",
                            Description = "The country, e.g. Canada",
                        }
                    },
                    Required = new[] { "location" },
                }
            };
        }

        static public string GetCapital(string location)
        {
            return "Tokyo, TO";
        }
    }
}
