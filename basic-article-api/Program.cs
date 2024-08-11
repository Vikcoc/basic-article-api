using basic_article_api.ArticleApplicationAuth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<HttpClient>();
builder.Services.AddAuthServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //only in dev we have unvalidated tokens
    app.AddDevAuth();
}

app.AddAuthRoutes();

app.MapGet("/hello", () =>
{
    return TypedResults.Ok("Hello World!");
});

app.Run();