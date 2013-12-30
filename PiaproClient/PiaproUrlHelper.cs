using System.Linq;
using System.Text.RegularExpressions;

namespace PiaproClient {

	/// <summary>
	/// Helper methods for Piapro URLs.
	/// </summary>
	public static class PiaproUrlHelper {

		public static readonly string[] PiaproUrlRegexes = {
			@"piapro.jp/t/([\w\-]+)",		// Short URLs
			@"piapro.jp/content/([\w\-]+)"	// Long URLs
		};

		/// <summary>
		/// Tests whether an URL is a valid Piapro content URL.
		/// </summary>
		/// <param name="url">URL to be tested. Can be null or empty.</param>
		/// <returns>True if the URL is valid, otherwise false.</returns>
		public static bool IsValidContentUrl(string url) {

			if (string.IsNullOrEmpty(url))
				return false;

			return PiaproUrlRegexes.Any(r => Regex.IsMatch(url, r, RegexOptions.IgnoreCase));

		}

	}

}
