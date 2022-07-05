using FluentValidation;

namespace ToDosMinimalApi
{
    public static class ValidatorExtension
    {
        public static RouteHandlerBuilder WithValidator<T>(this RouteHandlerBuilder builder)
            where T : class
        {
            builder.Add(EndpointBuilder =>
            {
                var orginalDelegate = EndpointBuilder.RequestDelegate;
                EndpointBuilder.RequestDelegate = async HttpContent =>
                {
                   var validator = HttpContent.RequestServices.GetRequiredService<IValidator<T>>();

                    HttpContent.Request.EnableBuffering();

                   var body =  await HttpContent.Request.ReadFromJsonAsync<T>();

                   if(body == null)
                    {
                        HttpContent.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await HttpContent.Response.WriteAsync("Couldn`t map body to request model");
                        return;
                    }

                   var ValidatioResult = validator.Validate(body);
                    if(!ValidatioResult.IsValid)
                    {
                        HttpContent.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await HttpContent.Response.WriteAsJsonAsync(ValidatioResult.Errors);
                        return;
                    }
                    HttpContent.Request.Body.Position = 0;
                    await orginalDelegate(HttpContent);
                };
            });
            return builder;
        }
    }
}
