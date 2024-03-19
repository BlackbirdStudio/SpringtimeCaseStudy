namespace CaseStudy.Extensions
{
    public static class HttpContextExtensions
    {
        public static bool IsSuccessResponse(this HttpResponse response) =>
            response?.StatusCode == StatusCode.Success;
    }
}
