namespace MauiWithAPI.API.Services.Files
{
    public interface IFileService
    {
        public Tuple<int, string> SaveImage(IFormFile file);
        public bool DeleteImage(string imageFileName);
    }
}
