using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace PiaproClient {

	public class PiaproClient {

		public static int? ParseLength(string lengthStr) {

			if (string.IsNullOrEmpty(lengthStr))
				return null;

			var parts = lengthStr.Split(':');

			if (parts.Length != 2)
				return null;

			int min, sec;
			if (!int.TryParse(parts[0], out min) || !int.TryParse(parts[1], out sec))
				return null;

			var totalSec = min * 60 + sec;

			return totalSec;

		}

		private static Encoding GetEncoding(string encodingStr) {

			// New piapro uses UTF-8 by default.
			if (string.IsNullOrEmpty(encodingStr))
				return Encoding.UTF8;

			try {
				return Encoding.GetEncoding(encodingStr);
			} catch (ArgumentException) {
				return Encoding.UTF8;
			}

		}

		private int? GetLength(HtmlNode dataElem) {

			if (dataElem == null)
				return null;

			var lengthMatch = Regex.Match(dataElem.InnerHtml, @"タイム／サイズ.+(\d\d:\d\d)");

			if (!lengthMatch.Success)
				return null;

			return ParseLength(lengthMatch.Groups[1].Value);

		}

		private static bool IsFullLink(string str) {

			return (str.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase)
				|| str.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase)
				|| str.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase));

		}

		/// <summary>
		/// Makes a proper URL from a possible URL without a http:// prefix.
		/// </summary>
		/// <param name="partialLink">Partial URL. Can be null.</param>
		/// <returns>Full URL including http://. Can be null if source was null.</returns>
		public static string MakeLink(string partialLink) {

			if (string.IsNullOrEmpty(partialLink))
				return partialLink;

			if (IsFullLink(partialLink))
				return partialLink;

			return string.Format("http://{0}", partialLink);

		}

		private AudioPostQueryResult ParseByHtmlStream(Stream htmlStream, Encoding encoding, string url) {

			var doc = new HtmlDocument();
			doc.Load(htmlStream, encoding);
			return ParseDocument(doc, url);

		}

		/// <summary>
		/// Parses a Piapro HTML document.
		/// </summary>
		/// <param name="doc">HTML document. Cannot be null.</param>
		/// <param name="url">URL of the post. Cannot be null or empty.</param>
		/// <returns>Query result. Cannot be null.</returns>
		public AudioPostQueryResult ParseDocument(HtmlDocument doc, string url) {

			var dataElem = doc.DocumentNode.SelectSingleNode("//div[@class = 'dtl_data']");

			if (dataElem == null || !dataElem.InnerHtml.Contains("/music/")) {
				throw new PiaproException("Content type indicates this isn't an audio file.");
			}

			var length = GetLength(dataElem);

			var idElem = doc.DocumentNode.SelectSingleNode("//input[@name = 'id']");

			if (idElem == null) {
				throw new PiaproException("Could not find id element on page.");				
			}

			var contentId = idElem.GetAttributeValue("value", string.Empty);

			var titleElem = doc.DocumentNode.SelectSingleNode("//h1[@class = 'dtl_title']");

			if (titleElem == null) {
				throw new PiaproException("Could not find title element on page.");								
			}

			var title = HtmlEntity.DeEntitize(titleElem.InnerText).Trim();

			var authorElem = doc.DocumentNode.SelectSingleNode("//div[@class = 'dtl_by_name']/a");
			var author = (authorElem != null ? authorElem.InnerText : string.Empty);

			return new AudioPostQueryResult {
				Author = author, Id = contentId, LengthSeconds = length, Title = title, Url = url
			};

		}

		/// <summary>
		/// Parses a Piapro post specified by an URL.
		/// </summary>
		/// <param name="url">URL to Piapro post. Cannot be null or empty..</param>
		/// <returns>Result of the query. Cannot be null.</returns>
		/// <exception cref="PiaproException">If the query failed.</exception>
		public AudioPostQueryResult ParseByUrl(string url) {

			if (string.IsNullOrEmpty(url))
				throw new ArgumentException("URL cannot be null or empty", "url");

			WebRequest request;
			try {
				request = WebRequest.Create(MakeLink(url));
			} catch (UriFormatException x) {
				throw new PiaproException("Invalid Piapro URL", x);
			}

			request.Timeout = 10000;
			WebResponse response;

			try {
				response = request.GetResponse();
			} catch (WebException x) {
				throw new PiaproException("Unable to get a response from the server, try again later", x);
			}

			var enc = GetEncoding(response.Headers[HttpResponseHeader.ContentEncoding]);

			try {
				using (var stream = response.GetResponseStream()) {
					return ParseByHtmlStream(stream, enc, url);
				}
			} finally {
				response.Close();
			}

		}

	}
}
