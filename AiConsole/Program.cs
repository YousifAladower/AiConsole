using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static async Task Main(string[] args)
    {
        //ToDo: make the URL and model configurable
        /*var builder = Host.CreateApplicationBuilder();

        builder.Services.AddChatClient(new OllamaChatClient(new Uri("http://localhost:11434"), "llama3.1:latest"));

        var app = builder.Build();

        var chatClient = app.Services.GetRequiredService<IChatClient>();

        var response = await chatClient.GetResponseAsync("What is .NET? Reply in 50 words max.");

        Console.WriteLine(response.Text);*/



        // This example shows how to maintain a chat history with the AI model to have a more conversational interaction.

        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddChatClient(new OllamaChatClient(new Uri("http://localhost:11434"), "llama3.1:latest"));

        var app = builder.Build();

        var chatClient = app.Services.GetRequiredService<IChatClient>();

        var chatHistory = new List<ChatMessage>();

        while (true)
        {
            Console.WriteLine("Enter your prompt:");
            var userPrompt = Console.ReadLine();
            chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

            Console.WriteLine("Response from AI:");
            var chatResponse = "";
            await foreach (var item in chatClient.GetStreamingResponseAsync(chatHistory))
            {
                // We're streaming the response, so we get each message as it arrives
                Console.Write(item.Text);
                chatResponse += item.Text;
            }
            chatHistory.Add(new ChatMessage(ChatRole.Assistant, chatResponse));
            Console.WriteLine();
        }
    }
}