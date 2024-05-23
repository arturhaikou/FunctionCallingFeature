namespace FunctionCallingFeature.Models.EShop
{
    public class Cart
    {
        public int Id { get; set; }

        public List<Guid> ProdcutIds { get; set; }
    }
}
