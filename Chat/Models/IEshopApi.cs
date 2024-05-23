using RestEase;

namespace Chat.Models
{
    public interface IEshopApi
    {
        [Post("api/assistant/weather")]
        Task<string> PerformWeatherAssistantActionAsync([Body] UserRequest request);

        [Post("api/assistant/eshop")]
        Task<string> PerformEshopAssistantActionAsync([Body] UserRequest request);

        [Post("api/assistant/kernel")]
        Task<string> PerformKernelAssistantActionAsync([Body] UserRequest request);

        [Post("api/assistant/amazon")]
        Task<string> PerformAmazonlAssistantActionAsync([Body] UserRequest request);

        [Get("api/catalog")]
        Task<List<Product>> GetProductsAsync();

        [Get("api/orders")]
        Task<List<Order>> GetOrdersAsync();
    }
}
