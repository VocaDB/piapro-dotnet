piapro-dotnet
=============

Piapro client for .NET, able to download metadata about content posts.

### Features
* Piapro URL testing (whether a specific URL is a possible Piapro content post)
* Download post information
  * Content type (currently only audio/illustration)
  * Title
  * Author Piapro ID and nickname
  * Upload date
  * Length (for songs)

More features will be added as needed.

### Example

#### Async

```csharp
var result = await new PiaproClient().ParseByUrlAsync("https://piapro.jp/t/0u5Z");
```

#### Synchronous

```csharp
var result = new PiaproClient().ParseByUrl("https://piapro.jp/t/0u5Z");
```