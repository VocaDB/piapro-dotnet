using HtmlAgilityPack;
using System;
using System.Text.RegularExpressions;

namespace PiaproClient {

	/// <summary>
	/// Parses Piapro HTML document.
	/// </summary>
	public class PiaproParser {

		private static int? ParseLength(string lengthStr) {

			if (string.IsNullOrEmpty(lengthStr))
				return null;

			var parts = lengthStr.Split(':');

			if (parts.Length != 2)
				return null;

			if (!int.TryParse(parts[0], out var min) || !int.TryParse(parts[1], out var sec))
				return null;

			var totalSec = min * 60 + sec;

			return totalSec;

		}

		private DateTime? GetDate(HtmlNode dataElem) {

			if (dataElem == null)
				return null;

			var match = Regex.Match(dataElem.InnerHtml, @"投稿日.+(\d\d\d\d/\d\d/\d\d \d\d:\d\d:\d\d)"); // "2010/08/21 19:09:15"

			if (!match.Success)
				return null;

			DateTime result;

			if (DateTime.TryParse(match.Groups[1].Value, out result))
				return result;
			else
				return null;

		}

		private int? GetLength(HtmlNode dataElem) {

			if (dataElem == null)
				return null;

			var lengthMatch = Regex.Match(dataElem.InnerHtml, @"タイム/サイズ.+(\d\d:\d\d)");

			if (!lengthMatch.Success)
				return null;

			return ParseLength(lengthMatch.Groups[1].Value);

		}

		/// <summary>
		/// Removes "さん" from the username, which piapro appends automatically.
		/// </summary>
		/// <param name="name">Username with possible honorific, for example "Rinさん".</param>
		/// <returns>Úsername without the honorific, for example "Rin".</returns>
		public string RemoveHonorific(string name) {

			if (string.IsNullOrEmpty(name))
				return name;

			var match = Regex.Match(name, @"^(.+)さん$");
			return match.Success ? match.Groups[1].Value : name;

		}

		private string GetContentId(HtmlDocument doc) {

			// Find both piapro.jp and www.piapro.jp
			// Note: HtmlAgilityPack does not support regex (XPath 2.0) :(
			var relatedMovieSpan = doc.DocumentNode.SelectSingleNode(
				"//a[starts-with(@href, \"http://piapro.jp/content/relate_movie/\")]" +
				"|//a[starts-with(@href, \"http://www.piapro.jp/content/relate_movie/\")]" +
				"|//a[starts-with(@href, \"https://piapro.jp/content/relate_movie/\")]" +
				"|//a[starts-with(@href, \"https://www.piapro.jp/content/relate_movie/\")]" +
				"|//a[starts-with(@href, \"https://piapro.jp/content_list_recommend/\")]"
			);

			var relatedMovieMatch = relatedMovieSpan != null ? Regex.Match(relatedMovieSpan.Attributes["href"].Value, @"https?://(?:www\.)?piapro\.jp/content(?:/relate_movie|_list_recommend)/\?id=([\d\w]+)") : null;
			var contentId = relatedMovieMatch != null && relatedMovieMatch.Success ? relatedMovieMatch.Groups[1].Value : null;
			return contentId;

		}

		private string GetUploadTimestamp(HtmlDocument doc) {

			var uploadTimestampElem = doc.DocumentNode.SelectSingleNode("//script[@type = 'application/javascript']");
			var uploadTimestampMatch = uploadTimestampElem != null ? Regex.Match(uploadTimestampElem.InnerText, "createDate\\s*:\\s*['\"]([0-9]{14})['\"]") : null;
			return uploadTimestampMatch != null && uploadTimestampMatch.Success ? uploadTimestampMatch.Groups[1].Value : null;

		}

		/// <summary>
		/// Parses a Piapro HTML document.
		/// </summary>
		/// <param name="doc">HTML document. Cannot be null.</param>
		/// <param name="url">URL of the post. Cannot be null or empty.</param>
		/// <returns>Query result. Cannot be null.</returns>
		/// <remarks>
		/// At least ID and title will be parsed.
		/// Author and length are optional.
		/// </remarks>
		/// <exception cref="PiaproException">If the query failed.</exception>
		public PostQueryResult ParseDocument(HtmlDocument doc, string url) {

			if (doc == null)
				throw new ArgumentNullException(nameof(doc));

			if (string.IsNullOrEmpty(url))
				throw new ArgumentException("URL cannot be null or empty", nameof(url));

			var dataElem = doc.DocumentNode.SelectSingleNode("//div[@class = 'cd_dtl_data']");
			var postType = PostType.Other;
			int? length = null;

			if (dataElem != null && dataElem.InnerHtml.Contains("/music/")) {
				postType = PostType.Audio;
				length = GetLength(dataElem);
			}
			else if (dataElem != null && dataElem.InnerHtml.Contains("/illust/")) {
				postType = PostType.Illustration;
			}

			var date = GetDate(dataElem);
			var contentId = GetContentId(doc);

			if (contentId == null) {
				throw new PiaproException("Could not find id element on page.");
			}

			var titleElem = doc.DocumentNode.SelectSingleNode("//h1[@class = 'cd_works-title' or @class = 'award-title']");

			if (titleElem == null) {
				throw new PiaproException("Could not find title element on page.");
			}

			var title = HtmlEntity.DeEntitize(titleElem.InnerText).Trim();

			var authorElem = doc.DocumentNode.SelectSingleNode("//a[@class = 'cd_user-name']");
			var author = (authorElem != null ? RemoveHonorific(authorElem.InnerText) : string.Empty);

			return new PostQueryResult {
				Author = author,
				Id = contentId,
				LengthSeconds = length,
				PostType = postType,
				Title = title,
				Url = url,
				Date = date,
				UploadTimestamp = GetUploadTimestamp(doc)
			};

		}


	}

}
