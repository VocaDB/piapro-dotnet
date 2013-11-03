namespace PiaproClient {

	/// <summary>
	/// Result of an audio post query.
	/// </summary>
	public class AudioPostQueryResult {

		/// <summary>
		/// Author name, for example "ハチ".
		/// </summary>
		public string Author { get; set; }

		/// <summary>
		/// Post ID in the long format, for example "61zc7sceslg04gcx".
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Audio length in seconds.
		/// </summary>
		public int? LengthSeconds { get; set; }

		/// <summary>
		/// Post title, for example "マトリョシカ　オケ".
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// The parsed URL.
		/// </summary>
		public string Url { get; set; }

	}

}
