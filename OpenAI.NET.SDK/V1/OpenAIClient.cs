﻿using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using OpenAI.NET.SDK.Extensions;
using OpenAI.NET.SDK.V1.Contracts;

namespace OpenAI.NET.SDK.V1;

public class OpenAIClient : IOpenAIClient
{
    private const string ApiVersion = "v1";

    private readonly HttpClient _httpClient;

    public OpenAIClient(HttpClient httpClient, IOptions<OpenAISettings> options)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.openai.com/");
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {options.Value.ApiKey}");
        if (string.IsNullOrEmpty(options.Value.Organization) is false)
        {
            _httpClient.DefaultRequestHeaders.Add("OpenAI-Organization", $"{options.Value.Organization}");
        }
    }

    /// <inheritdoc />
    public async Task<ListModelsResponse?> ListModels()
    {
        return await _httpClient.GetFromJsonAsync<ListModelsResponse?>($"/{ApiVersion}/models");
    }

    /// <inheritdoc />
    public async Task<ListModelsResponse.ListModelsResponseData?> RetrieveModel(string id)
    {
        return await _httpClient.GetFromJsonAsync<ListModelsResponse.ListModelsResponseData?>($"/{ApiVersion}/models/{id}");
    }

    /// <inheritdoc />
    public async Task<CreateCompletionResponse?> CreateCompletion(CompletionsModel model, string? prompt = null, int? maxTokens = null)
    {
        return await CreateCompletion(new CreateCompletionRequest
        {
            Model = model.ToStringModel(),
            Prompt = prompt,
            MaxTokens = maxTokens ?? 16
        });
    }

    /// <inheritdoc />
    public async Task<CreateCompletionResponse?> CreateCompletion(CreateCompletionRequest request)
    {
        return await _httpClient.Post<CreateCompletionResponse>($"/{ApiVersion}/completions", request);
    }

    /// <inheritdoc />
    public async Task<CreateEditResponse?> CreateEdit(EditsModel model, string? input = null, string? instruction = null)
    {
        return await CreateEdit(new CreateEditRequest
        {
            Model = model.ToStringModel(),
            Input = input,
            Instruction = instruction
        });
    }

    /// <inheritdoc />
    public async Task<CreateEditResponse?> CreateEdit(CreateEditRequest request)
    {
        return await _httpClient.Post<CreateEditResponse>($"/{ApiVersion}/edits", request);
    }

    /// <inheritdoc />
    public async Task<CreateImageResponse?> CreateImage(string prompt, int? numberOfImagesToGenerate = null, ImageSize? size = null, ImageResponseFormat? responseFormat = null)
    {
        return await CreateImage(new CreateImageRequest
        {
            Prompt = prompt,
            N = numberOfImagesToGenerate ?? 1,
            Size = (size ?? ImageSize.Size1024x1024).ToStringSize(),
            ResponseFormat = (responseFormat ?? ImageResponseFormat.Url).ToStringFormat()
        });
    }

    /// <inheritdoc />
    public async Task<CreateImageResponse?> CreateImage(CreateImageRequest request)
    {
        return await _httpClient.Post<CreateImageResponse>($"/{ApiVersion}/images/generations", request);
    }

    /// <inheritdoc />
    public async Task<CreateImageResponse?> CreateImageEdit(CreateImageEditRequest request)
    {
        return await _httpClient.Post<CreateImageResponse>($"/{ApiVersion}/images/edits", request);
    }

    /// <inheritdoc />
    public async Task<CreateImageResponse?> CreateImageVariation(CreateImageVariationRequest request)
    {
        return await _httpClient.Post<CreateImageResponse>($"/{ApiVersion}/images/variations", request);
    }

    /// <inheritdoc />
    public async Task<CreateEmbeddingsResponse?> CreateEmbeddings(CreateEmbeddingsRequest request)
    {
        return await _httpClient.Post<CreateEmbeddingsResponse>($"/{ApiVersion}/embeddings", request);
    }

    public async Task<ListFilesResponse?> ListFiles()
    {
        return await _httpClient.GetFromJsonAsync<ListFilesResponse?>($"/{ApiVersion}/files");
    }
}