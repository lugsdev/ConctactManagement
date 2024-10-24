using Microsoft.OpenApi.Models;

namespace ContactManagement.Api.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Contact Management API",
                    Description = "API para gerenciamento de contatos.",
                    Contact = new OpenApiContact
                    {
                        Name = "Grupo 2",
                        Email = "seu-email@exemplo.com",
                    },
                });
            });

            return services;
        }

        public static void UseSwaggerConfig(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                // Rota para o Swagger JSON
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contact Management API v1");
            });
        }
    }
}
