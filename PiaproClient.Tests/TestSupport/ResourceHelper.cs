using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace PiaproClient.Tests.TestSupport {

	public static class ResourceHelper {

		public static Stream GetFileStream(string fileName) {

			var asm = typeof(ResourceHelper).Assembly;
			//var s = asm.GetManifestResourceNames();
			return asm.GetManifestResourceStream(asm.GetName().Name + ".TestData." + fileName);

		}

		public static HtmlDocument ReadHtmlDocument(string fileName, Encoding encoding = null) {

			using (var stream = GetFileStream(fileName)) {

				var doc = new HtmlDocument();
				doc.Load(stream, encoding ?? Encoding.Default);
				return doc;

			}

		}

	}

}
