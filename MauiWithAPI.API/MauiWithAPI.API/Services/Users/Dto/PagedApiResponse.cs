﻿namespace MauiWithAPI.API.Services.Users.Dto
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
