# Cledev.OpenAI _(Beta)_
Unofficial .NET SDK for OpenAI with a [Blazor Server Playground](https://github.com/lucabriguglia/Cledev.OpenAI.Playground)

[![Main](https://github.com/lucabriguglia/Cledev.OpenAI/actions/workflows/main.yml/badge.svg)](https://github.com/lucabriguglia/Cledev.OpenAI/actions/workflows/main.yml)
[![Nuget Package](https://img.shields.io/badge/nuget-1.0.0-blue.svg)](https://www.nuget.org/packages/Cledev.OpenAI)

```
Install-Package Cledev.OpenAI
```

## API

- **Models**
  - List Models
  - Retrieve Model
- **Completions**
  - Create Completion
- **Chat**
  - Create Chat Completion
- **Edits**
  - Create Edit
- **Images**
  - Create Image
  - Create Image Edit
  - Create Image Variation
- **Embeddings**
  - Create Embeddings
- **Audio**
  - Create Transcription
  - Create Translation
- **Files**
  - List Files
  - Upload File
  - Delete File
  - Retrieve File
- **Fine-tunes**
  - Create Fine-tune
  - List Fine-tunes
  - Retrieve Fine-tune
  - Cancel Fine-tune
  - List Fine-tune Events
  - Delete Fine-tune Model
- **Moderations**
  - Create Moderation

## Configuration

### Option 1

```C#
services.AddOpenAIClient();
```

This option requires an appsettings.json file

```json
{
  "OpenAI": {
    "ApiKey": "YOUR_API_KEY",
    "Organization": "OUR_ORGANIZATION"
  }
}
```

### Option 2

```C#
services.AddOpenAIClient(options =>
{
     options.ApiKey = Environment.GetEnvironmentVariable("YOUR_API_KEY");
     options.Organization = Environment.GetEnvironmentVariable("YOUR_ORGANIZATION");
 });
```

## Usage

Inject `IOpenAIClient` interface into your service

### ChatGPT Stream Example

```C#
var request = new CreateChatCompletionRequest
{
    Model = ChatModel.Gpt35Turbo.ToStringModel(),
    Stream = true,
    MaxTokens = 500,
    Messages = new List<ChatCompletionMessage>
    {
        new("system", "You are a helpful assistant."),
        new("user", "Who won the world series in 2020?"),
        new("assistant", "The Los Angeles Dodgers won the World Series in 2020."),
        new("user", "Where was it played?")
    }
};

var completions = client.CreateChatCompletionAsStream(request);

await foreach (var completion in completions)
{
    Console.Write(completion.Choices[0].Message?.Content);
}
```

### Completion Stream Example

```C#
var request = new CreateCompletionRequest
{
    Model = CompletionsModel.TextDavinciV3.ToStringModel(),
    Stream = true,
    Prompt = "Please write a 1000 word assay about differences between functional programming and object oriented programming",
    MaxTokens = 500
};

var completions = client.CreateCompletionAsStream(request);

await foreach (var completion in completions)
{
    Console.Write(completion.Choices[0].Text);
}
```

### Audio Transcription Example (Whisper)

```C#
const string fileName = "YOUR_RECORDING.m4a";
var fileBytes = await File.ReadAllBytesAsync($"Data/{fileName}");

var request = new CreateAudioTranslationRequest
{
    Model = AudioModel.Whisper1.ToStringModel(),
    File = fileBytes,
    FileName = fileName,
    ResponseFormat = AudioResponseFormat.VerboseJson.ToStringFormat()
};

var response = await client.CreateAudioTranslation(request);

Console.Write($"{JsonSerializer.Serialize(response, jsonSerializerOptions)}");
```

### Create Image Example (Dall-E)

```C#
var request = new CreateImageRequest
{
    Prompt = "Once upon a time",
    Size = ImageSize.Size512x512.ToStringSize(),
    ResponseFormat = ImageResponseFormat.B64Json.ToStringFormat(),
    N = 1
};

var response = await client.CreateImage(Request);

<img src="@response.Data[0].Url" />
```

### Other Examples

Please, have a look at the [Blazor Server Playground](https://github.com/lucabriguglia/Cledev.OpenAI.Playground) for more examples on how to use the client.
