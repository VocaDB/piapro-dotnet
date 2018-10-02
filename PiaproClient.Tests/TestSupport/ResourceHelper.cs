using System.IO;
using System.Text;
using HtmlAgilityPack;

namespace PiaproClient.Tests.TestSupport {
	
	/// <summary>
	/// Helper methods for managing resources
	/// </summary>
	public static class ResourceHelper {

		public static Stream GetFileStream(string fileName) {

			var asm = typeof(ResourceHelper).Assembly;
			return asm.GetManifestResourceStream(asm.GetName().Name + ".TestData." + fileName);

		}

		public static HtmlDocument ReadHtmlDocument(string fileName, Encoding encoding = null) {

			using (var stream = GetFileStream(fileName)) {

				var doc = new HtmlDocument();
				doc.Load(stream, encoding ?? Encoding.Default);
				return doc;

			}

		}

		public const string TestDocumentName = "piapro.htm";

	}

}
