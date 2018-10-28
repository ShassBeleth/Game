namespace Models.Chapter {

	/// <summary>
	/// チャプターモデル
	/// </summary>
	public class ChapterModel {

		/// <summary>
		/// ID
		/// </summary>
		public int Id { set; get; }

		/// <summary>
		/// チャプター名
		/// </summary>
		public string Name { set; get; }

		/// <summary>
		/// 詳細
		/// </summary>
		public string Detail { set; get; }

		/// <summary>
		/// 時系列順
		/// </summary>
		public int TimelineOrder { set; get; }

		/// <summary>
		/// チャプター順
		/// </summary>
		public int NumberOrder { set; get; }

		/// <summary>
		/// クリア済みかどうか
		/// </summary>
		public bool IsCleared { set; get; }

		/// <summary>
		/// ノードの座標
		/// </summary>
		public int NodeCoodinate { set; get; }

		/// <summary>
		/// タイムライン上の座標
		/// </summary>
		public int CoodinateOnLine { set; get; }

	}

}
