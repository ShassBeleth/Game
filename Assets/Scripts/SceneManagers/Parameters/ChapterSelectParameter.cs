using System.Collections.Generic;

namespace SceneManagers.Parameters {

	/// <summary>
	/// ChapterSelectSceneに使用するパラメータ
	/// </summary>
	public class ChapterSelectParameter {

		/// <summary>
		/// チャプター
		/// </summary>
		public class Chapter {
			
			/// <summary>
			/// チャプターID
			/// </summary>
			public int Id { set; get; }

		}

		/// <summary>
		/// セーブデータID
		/// </summary>
		public int Id { set; get; }
		
		/// <summary>
		/// 一人プレイかどうか
		/// </summary>
		public bool IsSinglePlayMode { set; get; }

		/// <summary>
		/// クリア済みチャプター一覧
		/// </summary>
		public List<Chapter> ClearedChapters { set; get; }

	}

}
