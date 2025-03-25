# Translation API Client

This library provides a client for interacting with the Translation API. It offers a simple and efficient interface to handle API endpoints, allowing you to easily send requests and receive responses for further processing.

## Features

- **Translation**: Translate text to a specified language.
- **Health Check**: Check the health status of the Translation API.
- **Dependency Injection**: Easily register the library's services in the .NET dependency injection container.

## Interface

The library provides an interface for interacting with the Translation API through the `ITranslationAdapter` interface. The interface includes the following methods:

- `Task<string> TranslateAsync(string textToTranslate, string sourceLanguage, string targetLanguage)`: Translates the provided text from the source language to the target language.
- `Task<string> HealthCheckAsync()`: Checks the health of the API.

## Dependency Injection

You can register the library's services in the .NET dependency injection container using the `AddTranslationProcessor` extension method. This method registers all necessary services and configurations.

### Example

```csharp
public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddTranslationProcessor(configuration);
}
```

## Available Distributions

The library is available on GitHub Packages. You can install it using the following commands:

```shell
dotnet nuget add source --username YOUR_GITHUB_USERNAME --password YOUR_GITHUB_TOKEN --store-password-in-clear-text --name github "https://nuget.pkg.github.com/ggwozdz90/index.json"

dotnet add package TranslationApiClient
```

## API Project

The API project is available on [GitHub](https://github.com/ggwozdz90/translation-api) and as a Docker image on [Docker Hub](https://hub.docker.com/r/ggwozdz/translation-api).

## Usage

### Translation

```csharp
var adapter = serviceProvider.GetRequiredService<ITranslationAdapter>();
var result = await adapter.TranslateAsync("Text to translate", "en_US", "pl_PL");
Console.WriteLine(result);
```

### Health Check

```csharp
var adapter = serviceProvider.GetRequiredService<ITranslationAdapter>();
var result = await adapter.HealthCheckAsync();
Console.WriteLine(result);
```

## Configuration

The library uses the .NET configuration system. You can configure the base address of the Translation API, as well as the default source and target languages, and the timeout settings for HTTP clients in your `appsettings.json` file:

```json
{
  "Translation": {
    "BaseAddress": "http://localhost:8000",
    "SourceLanguage": "en_US",
    "TargetLanguage": "pl_PL",
    "TranslateRouteTimeout": 300,
    "HealthCheckRouteTimeout": 10
  }
}
```

## License

This project is licensed under the MIT License - see the [LICENSE](../LICENCE) file for details.

## Table of Contents

- [Translation API Client](#translation-api-client)
  - [Features](#features)
  - [Interface](#interface)
  - [Dependency Injection](#dependency-injection)
    - [Example](#example)
  - [Available Distributions](#available-distributions)
  - [API Project](#api-project)
  - [Usage](#usage)
    - [Translation](#translation)
    - [Health Check](#health-check)
  - [Configuration](#configuration)
  - [License](#license)
  - [Table of Contents](#table-of-contents)
