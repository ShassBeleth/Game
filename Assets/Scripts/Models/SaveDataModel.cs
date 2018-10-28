using System;
using System.Collections.Generic;
using Models.Chapter;

namespace Models {

	/// <summary>
	/// セーブデータのモデル
	/// </summary>
	public class SaveDataModel {

		public bool exsitsAlreadyData;

		/// <summary>
		/// ID
		/// </summary>
		public int id;

		/// <summary>
		/// ユーザ名
		/// </summary>
		public string userName;

		/// <summary>
		/// 最終更新日
		/// </summary>
		public DateTime latestUpdateDateTime;

		/// <summary>
		/// クリア済みチャプター一覧
		/// </summary>
		public List<ChapterModel> clearedChapters;

	}
}
