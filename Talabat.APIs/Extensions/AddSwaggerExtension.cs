namespace Talabat.APIs.Extensions
{
    public static class AddSwaggerExtension
    {
        public static WebApplication UseSwaggerMiddlewares(this WebApplication app)//the caller of you "app." its type is web application and before it this key word to know he will be the caller and this will be extension function
        {



            app.UseSwagger();
            app.UseSwaggerUI();

            return app;

        }
    }
}
