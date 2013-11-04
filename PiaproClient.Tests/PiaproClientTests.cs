using System.Text;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiaproClient.Tests.TestSupport;

namespace PiaproClient.Tests {

	/// <summary>
	/// Tests for <see cref="PiaproClient"/>.
	/// </summary>
	[TestClass]
	public class PiaproClientTests {

		private HtmlDocument SongDocument {
			get {
				return ResourceHelper.ReadHtmlDocument("piapro.htm", Encoding.UTF8);
			}
		}

		private AudioPostQueryResult ParseDocument() {
			return new PiaproClient().ParseDocument(SongDocument, "http://");
		}

		[TestMethod]
		public void Id() {
			Assert.AreEqual("61zc7sceslg04gcx", ParseDocument().Id, "Id");
		}

		[TestMethod]
		public void Title() {
			Assert.AreEqual("マトリョシカ　オケ", ParseDocument().Title, "Title");
		}

		[TestMethod]
		public void Length() {
			Assert.AreEqual(201, ParseDocument().LengthSeconds, "Length");
		}

		[TestMethod]
		public void Author() {
			Assert.AreEqual("ハチ", ParseDocument().Author, "Author");
		}

	}

}
