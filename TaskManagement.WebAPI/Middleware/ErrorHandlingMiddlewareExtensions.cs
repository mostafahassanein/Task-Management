namespace TaskManagement.WebAPI.Middleware
{
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
