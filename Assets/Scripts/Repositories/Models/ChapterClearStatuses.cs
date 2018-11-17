using System;
using System.Collections.Generic;

namespace Repositories.Models {

	/// <summary>
	/// チャプターのクリア状況一覧
	/// </summary>
	[Serializable]
	public class ChapterClearStatuses {

		/// <summary>
		/// チャプターのクリア状況一覧
		/// </summary>
		public List<ChapterClearStatus> rows;

	}

	/// <summary>
	/// チャプターのクリア状況
	/// </summary>
	[Serializable]
	public class ChapterClearStatus {

		/// <summary>
		/// セーブID
		/// </summary>
		public int saveId;

		/// <summary>
		/// チャプターID
		/// </summary>
		public int chapterId;

	}

}
