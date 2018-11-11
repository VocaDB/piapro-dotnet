using System;
using System.Text;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiaproClient.Tests.TestSupport;

namespace PiaproClient.Tests {

	/// <summary>
	/// Tests for <see cref="PiaproParser"/>.
	/// </summary>
	[TestClass]
	public class PiaproParserTests {

		private readonly PiaproParser parser = new PiaproParser();

		/// <summary>
		/// Source: https://piapro.jp/content/61zc7sceslg04gcx
		/// Normal use case.
		/// </summary>
		private HtmlDocument SongDocument => ResourceHelper.ReadHtmlDocument(ResourceHelper.TestDocumentName, Encoding.UTF8);
		private HtmlDocument SongDocumentWithWww => ResourceHelper.ReadHtmlDocument("piapro2.html", Encoding.UTF8);
		private HtmlDocument SongDocumentWithAwardTitle => ResourceHelper.ReadHtmlDocument("piapro_award.html", Encoding.UTF8);
		private HtmlDocument SongDocumentWithHttps => ResourceHelper.ReadHtmlDocument("piapro_https.html", Encoding.UTF8);
		/// <summary>
		/// Source: https://piapro.jp/t/Zh5H
		/// No related_movie element. Get contentId from JavaScript.
		/// </summary>
		private HtmlDocument SongDocumentWithNoRelated => ResourceHelper.ReadHtmlDocument("piapro_norelated.html", Encoding.UTF8);

		private PostQueryResult ParseDocument() => parser.ParseDocument(SongDocument, "http://");
		private PostQueryResult ParseDocumentWithWww() => parser.ParseDocument(SongDocumentWithWww, "http://");
		private PostQueryResult ParseDocumentWithAwardTitle() => parser.ParseDocument(SongDocumentWithAwardTitle, "http://");
		private PostQueryResult ParseDocumentWithHttps() => parser.ParseDocument(SongDocumentWithHttps, "https://");
		private PostQueryResult ParseDocumentWithNoRelated() => parser.ParseDocument(SongDocumentWithNoRelated, "https://");

		[TestMethod]
		public void Id() {
			Assert.AreEqual("61zc7sceslg04gcx", ParseDocument().Id, "Id");
		}

		[TestMethod]
		public void Id_RelatedUrlHasWww() {
			Assert.AreEqual("c2s4xvsyt8xanbl7", ParseDocumentWithWww().Id, "Id");
		}

		[TestMethod]
		public void Id_NoRelated() {
			Assert.AreEqual("xmitpgv0t5dwi2ln", ParseDocumentWithNoRelated().Id, "Id");
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
			Assert.AreEqual(new DateTime(2010, 8, 21, 19, 9, 15), ParseDocument().Date, "Date");
		}

        [TestMethod]
        public void UploadTimestamp() {
            Assert.AreEqual("20100821190915", ParseDocument().UploadTimestamp, "UploadTimestamp");
        }

        [TestMethod]
		public void RemoveHonorific_SpecialCharacters() {
			var result = parser.RemoveHonorific("Rin（ぎんすけ）さん");
			Assert.AreEqual("Rin（ぎんすけ）", result, "result");
		}

		[TestMethod]
		public void Title_Award() {
			var result = ParseDocumentWithAwardTitle();
			Assert.AreEqual("七夕恋歌", result?.Title, "result");
		}

	}

}
