# iPDFGen

iPDFGen is a free, open-source .NET library designed to simplify PDF generation by providing a unified API over multiple PDF generation providers. It allows seamless switching between providers (e.g., Puppeteer, Playwright) without changing your code, addressing common challenges like licensing changes, page limits, or provider discontinuation.

## Why iPDFGen?

- **Reliable and Flexible**: Supports multiple providers with a consistent API, making it easy to switch if a provider's terms or pricing change.
- **Open Source**: Freely available under the MIT License, with no commercial restrictions.
- **Performance-Oriented**: Optimized for typical workloads, with benchmarked performance across providers.
- **Developer-Friendly**: Simple setup and integration with .NET 8 applications.

## Quick Start

1. Install the core iPDFGen NuGet package:
   ```bash
   dotnet add package iPDFGen.Core
   ```
2. Install a provider package (fastest and the most accurate is Playwright at the moment):
   ```bash
   dotnet add package iPDFGen.Playwright
   ```
3. Configure iPDFGen in your `Program.cs`:
   ```csharp
   builder.Services.AddPdfGen(options =>
   {
       options.UsePlaywright();
   });
   ```
4. Generate a PDF from HTML:
   ```csharp
   
   OneOf<PdfGenSuccessResult, PdfGenErrorResult> generatorResult = await app.Service
       .GetRequiredServices<IPdfGenerator>()
       .Generate("<h1>Hello World!</h1>")
       
        Stream resultStream = result.Match(
            success => success.Stream,
            err => throw new Exception("Ups") // You might handle it differently upon a need
        );

        await WriteToFile(resultStream);
        
        
   async Task WriteToFile(Stream pdfStream)
   {
       await using var fileStream = File.OpenWrite("my.pdf");
       await pdfStream.CopyToAsync(fileStream);
   }
   ```

## Installation

### Prerequisites
- .NET 8 SDK or later

### Steps
1. Install the core iPDFGen package:
   ```bash
   dotnet add package iPDFGen
   ```
2. Choose a provider and install its package:
   ```bash
   # For Playwright
   dotnet add package iPDFGen.Playwright
   # For Puppeteer
   dotnet add package iPDFGen.Puppeteer
   # For Remote Server
   dotnet add package iPDFGen.RemoteServer
   ```
3. Register iPDFGen in your `ServiceCollection`:
   ```csharp
   builder.Services.AddPdfGen(options =>
   {
       options
           .UsePlaywright() 
           // .UsePuppeteer() // For Puppeteer
           // .UseRemoteServer("http://127.0.0.1:8080", "YourSharedSecret123456780$") // For remote server
           .SetMaxDegreeOfParallelism(16)
           .SetDefaultTimeout(TimeSpan.FromSeconds(30));
   });
   ```
   During a first PDF File generation iPDFGen will **automagically** download needed assemblies.
4. Inject and use `IPdfGenerator` in your services (see Quick Start for an example).

## Supported Providers

| Package Name            | Underlying Provider      | Status            | Notes                                                                 |
|-------------------------|--------------------------|-------------------|----------------------------------------------------------------------|
| `iPDFGen.Puppeteer`     | Puppeteer-Sharp          | ✅ Supported       | Fast for single-page PDFs, requires Chromium.                         |
| `iPDFGen.Playwright`    | Microsoft Playwright     | ✅ Supported       | Best performance for multi-page PDFs, supports multiple browsers.     |
| `iPDFGen.RemoteServer`  | HttpClient               | 🚧 In Development | Ideal for offloading PDF generation to a remote server.               |
| More providers          | TBD                      | 🚧 Planned        | Contributions welcome!                                                |

## Benchmarks

Benchmarks were conducted using a 2-page fake CV (`resume.A4.xs.html`) on a Linux Fedora 42 system with an
AMD Ryzen 9 7950X and .NET 8.0.16. The table below compares performance for generating one (`Single`) or eighty (`Eighty`) PDFs,
and single PDFs from URLs (`SingleByUrl`).

| Provider           | Scenario              | Mean Time    | Memory Allocated | Notes                               |
|--------------------|-----------------------|--------------|------------------|-------------------------------------|
| Puppeteer          | Single PDF            | 13.72 ms     | 763.56 KB        | Moderate speed, higher memory use.  |
| Puppeteer          | Eighty PDFs           | 179.66 ms    | 61,086.03 KB     |  Scales poorly for bulk generation. |
| Puppeteer          | Single PDF by URL     |  26.94 ms    | 202.55 KB        | Slower due to URL fetching.         |
| **Playwright**     | **Single PDF**        | **6.67 ms**  | **386.87 KB**    | **Fastest for single PDFs.**        |
| **Playwright**     | **Eighty PDFs**       | **79.53 ms** | **31,912.14 KB** | **Efficient for bulk generation.**  |
| **Playwright**     | **Single PDF by URL** | **4.66 ms**  | **126.79 KB**    | **Best for URL-based generation.**  |

**Key Observations**:
- Playwright outperforms Puppeteer in all scenarios, especially for bulk generation.
- Memory usage is significantly lower with Playwright for bulk tasks.
- URL-based generation is faster with Playwright due to optimized browser handling.

## Troubleshooting

- **Browser not found**: Ensure Chromium (or another supported browser) is installed for Puppeteer/Playwright. Run `playwright install` for Playwright.
- **Timeout errors**: Increase the timeout using `SetDefaultTimeout` or check provider-specific issues.
- **Remote server errors**: Verify the server URL and shared secret for `UseRemoteServer`.

## Contributing

We welcome contributions to iPDFGen! To get started:
1. Fork the repository on [GitHub](https://github.com/marisvigulis/iPDFGen).
2. Create a feature branch (`git checkout -b yourgithubusername/your-feature`).
3. Submit a pull request with clear descriptions and tests.

[//]: # (See [CONTRIBUTING.md]&#40;link-to-contributing-file&#41; for details.)

## License

iPDFGen is licensed under the [MIT License](LICENSE). Feel free to use, modify, and distribute it as needed.

## Contact

For questions or feedback, open an issue on [GitHub](https://github.com/marisvigulis/iPDFGen) or join our [community discussions](https://github.com/marisvigulis/iPDFGen/discussions).