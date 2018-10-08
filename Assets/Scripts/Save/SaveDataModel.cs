using System;

namespace Save {

	/// <summary>
	/// 一人プレイのセーブデータのモデル
	/// </summary>
	public class SinglePlaySaveDataModel {

		/// <summary>
		/// ID
		/// </summary>
		public int Id { set; get; }

		/// <summary>
		/// ユーザ名
		/// </summary>
		public string userName { set; get; }

		/// <summary>
		/// 最終更新日
		/// </summary>
		public DateTime latestUpdateDataTime { set; get; }

	}

}
