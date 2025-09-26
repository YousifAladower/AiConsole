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
        /*
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
                }*/

        /*

                // This example shows how to process multiple text files in a directory, sending each file's content to the AI model for analysis and summarization.
                // Ensure the "posts" directory exists and contains sample files
                string postsDir = "posts";
                if (!Directory.Exists(postsDir))
                {
                    Directory.CreateDirectory(postsDir);
                    for (int i = 1; i <= 5; i++)
                    {
                        string filePath = Path.Combine(postsDir, $"post{i}.txt");
                        File.WriteAllText(filePath, $"This is the content of post number {i}.");
                    }
                }

                var builder = Host.CreateApplicationBuilder();

                builder.Services.AddChatClient(new OllamaChatClient(new Uri("http://localhost:11434"), "llama3.1:latest"));

                var app = builder.Build();

                var chatClient = app.Services.GetRequiredService<IChatClient>();

                var posts = Directory.GetFiles("posts").Take(5).ToArray();
                foreach (var post in posts)
                {
                    string prompt = $$"""
                 You will receive an input text and the desired output format.
                 You need to analyze the text and produce the desired output format.
                 You not allow to change code, text, or other references.

                 # Desired response

                 Only provide a RFC8259 compliant JSON response following this format without deviation.

                 {
                    "title": "Title pulled from the front matter section",
                    "summary": "Summarize the article in no more than 100 words"
                 }

                 # Article content:

                 {{File.ReadAllText(post)}}
                 """;

                    var response = await chatClient.GetResponseAsync(prompt);
                    Console.WriteLine(response.Text);
                    Console.WriteLine(Environment.NewLine);
                }*/



        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddChatClient(new OllamaChatClient(new Uri("http://localhost:11434"), "llama3.1:latest"));

        var app = builder.Build();

        var chatClient = app.Services.GetRequiredService<IChatClient>();

        var posts = Directory.GetFiles("posts").Take(5).ToArray();
        foreach (var post in posts)
        {
            string prompt = $$"""
                  You will receive an input text and the desired output format.
                  You need to analyze the text and produce the desired output format.
                  You not allow to change code, text, or other references.

                  # Desired response

                  Only provide a RFC8259 compliant JSON response following this format without deviation.

                  {
                     "title": "Title pulled from the front matter section",
                     "tags": "Array of tags based on analyzing the article content. Tags should be lowercase."
                  }

                  # Article content:

                  {{File.ReadAllText(post)}}
                  """;

            var response = await chatClient.GetResponseAsync<PostCategory>(prompt);

            Console.WriteLine(
              $"{response.Result.Title}. Tags: {string.Join(",", response.Result.Tags)}");
        }
    }
    public class PostCategory
    {
        public string Title { get; set; } = string.Empty;
        public string[] Tags { get; set; } = [];
    }
}