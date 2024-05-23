namespace FunctionCallingFeature.Services.Interfaces
{
    public interface IPurchaseService
    {
        Task<bool> BuyAsync(int orderId);
    }
}
