using basic_article_api.ApplicationExceptions;
using basic_article_api.ArticleApplicationAuth;
using basic_article_api.DevTests;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<HttpClient>();
builder.Services.AddAuthServices();

builder.Services.AddAuthentication().AddJwtBearer(opt =>
{
    var key = builder.Configuration.GetSection("SigningCredentials:Auth:JWK").Get<JsonWebKey>();
    var keySet = new JsonWebKeySet();
    keySet.Keys.Add(key);

    var parameters = new TokenValidationParameters()
    {
        ValidIssuer = builder.Configuration["SigningCredentials:Auth:Issuer"],
        ValidAudience = builder.Configuration["SigningCredentials:Auth:Audience"],
        IssuerSigningKeys = keySet.Keys,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero
    };

    opt.TokenValidationParameters = parameters;
});
builder.Services.AddAuthorization();

builder.Services.AddExceptionHandler<LittleExceptionHandler>();
builder.Services.AddProblemDetails();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.AddTestRoutes();

    //only in dev we have unvalidated tokens
    app.AddDevAuth();
}

app.UseAuthorization();

app.AddAuthRoutes();

app.UseExceptionHandler();

app.Run();