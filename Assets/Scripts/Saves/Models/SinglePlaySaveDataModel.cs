using System;

namespace SavesTemp.Models {
	
	/// <summary>
	/// 一人プレイのセーブデータのモデル
	/// </summary>
	public class SinglePlaySaveDataModel {

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
		public ChapterSaveDataModel[] clearedChapters;

	}

}
