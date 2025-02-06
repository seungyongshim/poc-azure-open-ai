using System.ClientModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

// https://www.nuget.org/packages/Microsoft.Extensions.AI.OpenAI/
builder.Services.AddChatClient(sp =>
{
    var apikey = sp.GetRequiredService<IConfiguration>().GetValue<string>("OpenAI:ApiKey");
    var url = sp.GetRequiredService<IConfiguration>().GetValue<string>("OpenAI:ApiUrl");

    return new OpenAIClient(new ApiKeyCredential(apikey)
    , new OpenAIClientOptions()
    {
        Endpoint = new(url)
    }) .AsChatClient("gpt-4o-mini");
});

var app = builder.Build();
app.Run();