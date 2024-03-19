using CaseStudy.Controllers;
using CaseStudy.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace CaseStudy.Middlewares
{
    public class RedisCachingMiddleware(RequestDelegate Next, IDistributedCache Cache)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            if (IsGenerateTokenEndpoint(context))
            {
                await Next(context);
                return;
            }
            var cacheKey = $"request_{context.Request.Path}";
            switch (context.Request.Method)
            {
                case "GET": await TryGetCachedResult(context, cacheKey); break;
                default: await TryInvalidateCachedResult(context, cacheKey); break;
            }
        }

        private async Task TryGetCachedResult(HttpContext context, string cacheKey)
        {
            var result = await Cache.GetRecordAsync<byte[]>(cacheKey);
            if (result is not null)
            {
                context.Response.StatusCode = StatusCode.Success;
                var decodedResult = Encoding.UTF8.GetString(result);
                context.Response.ContentType = MimeType.ApplicationJson;
                await context.Response.WriteAsync(decodedResult);
                return;
            }

            context.Request.EnableBuffering();
            var originalBody = context.Response.Body;
            try
            {
                using var memoryStream = new MemoryStream();
                context.Response.Body = memoryStream;
                await Next(context);
                memoryStream.Position = 0;
                var responseBody = new StreamReader(memoryStream).ReadToEnd();
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(originalBody);
                var responseBytes = Encoding.UTF8.GetBytes(responseBody);
                await Cache.SetRecordAsync(cacheKey, responseBytes);
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }

        private async Task TryInvalidateCachedResult(HttpContext context, string cacheKey)
        {
            await Next(context);
            if (!context.Response.IsSuccessResponse())
            {
                return;
            }
            var basePath = cacheKey[..cacheKey.LastIndexOf('/')];
            var keys = new[] { cacheKey, $"{basePath}/{ControllerPath.All}" };
            foreach (var key in keys)
            {
                var result = await Cache.GetAsync(key);
                if (result is not null)
                {
                    await Cache.RemoveAsync(key);
                }
            }
        }

        private bool IsGenerateTokenEndpoint(HttpContext context)
        {
            var type = typeof(AuthController);
            var displayname = $"{type.Namespace}.{type.Name}.{nameof(AuthController.GetDataManagerJwt)} ({type.Namespace!.Split('.')[0]})";
            return context.Request.Method == "GET" && context.GetEndpoint()!.DisplayName == displayname;
        }
    }
}
