piapro-dotnet
=============

[Piapro](https://piapro.jp/) client for .NET, able to download metadata about content posts.

Compatible with .NET Standard 2.0.

Since Piapro has no API, the client relies on HTML scraping.

### Features
* Piapro URL testing (whether a specific URL is a possible Piapro content post)
* Download post information
  * Content type (currently only audio/illustration)
  * Title
  * Author's Piapro ID and nickname
  * Upload date
  * Artwork
  * Length (for songs)

More features will be added as needed.

### Installing

```
PM> Install-Package VocaDb.PiaproClient
```

### Usage

#### Async

```csharp
var result = await new PiaproClient().ParseByUrlAsync("https://piapro.jp/t/0u5Z");
```

#### Synchronous

```csharp
var result = new PiaproClient().ParseByUrl("https://piapro.jp/t/0u5Z");
```
