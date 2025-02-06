using Microsoft.Extensions.AI;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);
// https://www.nuget.org/packages/Microsoft.Extensions.AI.OpenAI/
builder.Services.AddChatClient(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new OpenAIClient("APIKEY").AsChatClient("gpt-4o-mini");
});

var app = builder.Build();
app.Run();