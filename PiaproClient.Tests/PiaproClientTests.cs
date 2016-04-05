using System;
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

		private HtmlDocument SongDocument => ResourceHelper.ReadHtmlDocument("piapro.htm", Encoding.UTF8);

		private HtmlDocument SongDocumentWithWww => ResourceHelper.ReadHtmlDocument("piapro2.html", Encoding.UTF8);

		private PostQueryResult ParseDocument() {
			return new PiaproClient().ParseDocument(SongDocument, "http://");
		}

		private PostQueryResult ParseDocumentWithWww() {
			return new PiaproClient().ParseDocument(SongDocumentWithWww, "http://");
		}

		[TestMethod]
		public void Id() {
			Assert.AreEqual("61zc7sceslg04gcx", ParseDocument().Id, "Id");
		}

		[TestMethod]
		public void Id_RelatedUrlHasWww() {
			Assert.AreEqual("c2s4xvsyt8xanbl7", ParseDocumentWithWww().Id, "Id");
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

		[TestMethod]
		public void PostType_Audio() {
			Assert.AreEqual(PostType.Audio, ParseDocument().PostType, "PostType");
		}

		[TestMethod]
		public void Date() {
			Assert.AreEqual(new DateTime(2010, 8, 21, 19, 9, 0), ParseDocument().Date, "Date");
		}

	}

}
