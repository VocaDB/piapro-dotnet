using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PiaproClient.Tests {

	/// <summary>
	/// Tests for <see cref="PiaproUrlHelper"/>.
	/// </summary>
	[TestClass]
	public class PiaproUrlHelperTests {

		private void TestIsValidContentUrl(string url, bool expected) {
			Assert.AreEqual(PiaproUrlHelper.IsValidContentUrl(url), expected);
		}

		[TestMethod]
		public void IsValidContentUrl_ShortUrl_Http() => TestIsValidContentUrl("http://piapro.jp/t/abcd", true);

		[TestMethod]
		public void IsValidContentUrl_ShortUrl_Https() => TestIsValidContentUrl("https://piapro.jp/t/abcd", true);

		[TestMethod]
		public void IsValidContentUrl_LongUrl_Http() => TestIsValidContentUrl("http://piapro.jp/content/abcd1234efg", true);

		[TestMethod]
		public void IsValidContentUrl_LongUrl_Https() => TestIsValidContentUrl("https://piapro.jp/content/abcd1234efg", true);

		[TestMethod]
		public void IsValidContentUrl_WithoutHttp() => TestIsValidContentUrl("piapro.jp/content/abcd1234efg", true);

		[TestMethod]
		public void IsValidContentUrl_NotValid() => TestIsValidContentUrl("https://piapro.jp/", false);

		[TestMethod]
		public void IsValidContentUrl_NotPiaproUrl() => TestIsValidContentUrl("https://google.com/", false);

	}
}
