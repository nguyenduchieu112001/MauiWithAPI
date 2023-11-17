namespace MauiWithAPI.API.Services.Products.Dto
{
    public class PagedApiResponse<T>
    {
        public T Data { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
