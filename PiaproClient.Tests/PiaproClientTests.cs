using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VocaDb.PiaproClient.Tests.TestData;

namespace VocaDb.PiaproClient.Tests {

	/// <summary>
	/// Tests for <see cref="PiaproClient"/>.
	/// </summary>
	[TestClass]
	public class PiaproClientTests {

		private readonly PiaproClient client;
		private readonly FakeHttpMessageHandler messageHandler;

		public PiaproClientTests() {
			messageHandler = new FakeHttpMessageHandler();
			var httpClient = new HttpClient(messageHandler);
			client = new PiaproClient(httpClient);
		}

		[TestMethod]
		public async Task ParseByUrlAsync() {

			var result = await client.ParseByUrlAsync("https://piapro.jp/t/hfo-");

			Assert.AreEqual("61zc7sceslg04gcx", result.Id, "result.Id");

		}

	}

	public class FakeHttpMessageHandler : HttpMessageHandler {

		public Func<Stream> ResponseFactory { get; set; } = () => ResourceHelper.GetFileStream(ResourceHelper.TestDocumentName);

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
			return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(ResponseFactory()) });
		}
	}

}
