using System.ClientModel;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.AI;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddChatClient(sp =>
{
    var azure_ai_key = sp.GetRequiredService<IConfiguration>().GetValue<string>("OpenAI:ApiKey");
    var azure_ai_url = sp.GetRequiredService<IConfiguration>().GetValue<string>("OpenAI:ApiUrl");

    return new Azure.AI.OpenAI.AzureOpenAIClient
    (
        new Uri(azure_ai_url),
        new ApiKeyCredential(azure_ai_key)
    ).AsChatClient("gpt-4o");
});

var app = builder.Build();


app.MapGet("/chat/{message}", async (HttpContext context, string message) =>
{
    var chatClient = context.RequestServices.GetRequiredService<IChatClient>();

    List<ChatMessage> chatMessages = [];

    chatMessages.Add(new ChatMessage(ChatRole.System, "친절하게 한글로 답변합니다."));
    chatMessages.Add(new ChatMessage(ChatRole.User, message));

    var response = await chatClient.CompleteAsync(chatMessages);

    return Results.Ok(response);
});

app.Run();