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
		public void IsValidContentUrl_ShortUrl() {
			
			TestIsValidContentUrl("http://piapro.jp/t/abcd", true);

		}

		[TestMethod]
		public void IsValidContentUrl_LongUrl() {
			
			TestIsValidContentUrl("http://piapro.jp/content/abcd1234efg", true);

		}

		[TestMethod]
		public void IsValidContentUrl_WithoutHttp() {
			
			TestIsValidContentUrl("piapro.jp/content/abcd1234efg", true);

		}

		[TestMethod]
		public void IsValidContentUrl_NotValid() {
			
			TestIsValidContentUrl("http://piapro.jp/", false);

		}

		[TestMethod]
		public void IsValidContentUrl_NotPiaproUrl() {
			
			TestIsValidContentUrl("http://google.com/", false);

		}

	}
}
