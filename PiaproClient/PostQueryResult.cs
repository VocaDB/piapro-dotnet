using System;

namespace PiaproClient {

	/// <summary>
	/// Result of a song post parsing.
	/// </summary>
	public class PostQueryResult {

		/// <summary>
		/// Author name, for example "ハチ".
		/// Cannot be null. Can be empty if author information could not be parsed.
		/// </summary>
		public string Author { get; set; }

		/// <summary>
		/// Post publish date.
		/// </summary>
		public DateTime? Date { get; set; }

		/// <summary>
		/// Post ID in the long format, for example "61zc7sceslg04gcx".
		/// Cannot be null or empty.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Audio length in seconds.
		/// Can be null if the length could not be parsed.
		/// </summary>
		public int? LengthSeconds { get; set; }

		/// <summary>
		/// Type of post.
		/// </summary>
		public PostType PostType { get; set; }

		/// <summary>
		/// Post title, for example "マトリョシカ　オケ".
		/// Cannot be null or empty.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// The parsed URL.
		/// Cannot be null or empty.
		/// </summary>
		public string Url { get; set; }

	}

	public enum PostType {
		
		Audio,

		Illustration,

		Other

	}

}
