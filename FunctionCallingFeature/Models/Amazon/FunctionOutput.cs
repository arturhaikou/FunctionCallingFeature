namespace FunctionCallingFeature.Models.Amazon
{
    public class FunctionOutput
    {
        public static object GetFunctionOutput()
        {
            return new
            {
                function_name = new
                {
                    Type = "string",
                    Description = "The name of the calling function",
                },
                Parameters = new
                {
                    Type = "object",
                    Description = "The object specified in the schema for the particular function"
                }
            };
        }
    }
}
