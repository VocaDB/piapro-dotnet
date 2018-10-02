using System.Net.Http;

namespace PiaproClient {

	public interface IHttpClientFactory {
		HttpClient HttpClient { get;}
	}

	public class HttpClientFactory : IHttpClientFactory {
		public HttpClient HttpClient => new HttpClient();
	}

}
