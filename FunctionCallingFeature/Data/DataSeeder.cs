using FunctionCallingFeature.Models.EShop;

namespace FunctionCallingFeature.Data
{
    public class DataSeeder
    {
        public static List<Product> GetProductList()
        {
            var itemFaker = new Bogus.Faker<Product>()
                .UseSeed(778)
                .RuleFor(i => i.Id, f => f.Random.Guid())
                .RuleFor(i => i.Color, f => f.Commerce.Color())
                .RuleFor(i => i.Name, f => f.Commerce.ProductName())
                .RuleFor(i => i.Description, f => f.Commerce.ProductDescription())
                .RuleFor(i => i.Size, f => f.PickRandom<ProductSize>().ToString())
                .RuleFor(i => i.Price, f => decimal.Parse(f.Commerce.Price()));

            return itemFaker.Generate(120);
        }
    }
}
