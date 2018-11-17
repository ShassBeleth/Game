using System;
using System.Collections.Generic;

namespace Repositories.Models {

	/// <summary>
	/// セーブデータ一覧
	/// </summary>
	[Serializable]
	public class Saves {

		/// <summary>
		/// セーブデータ一覧
		/// </summary>
		public List<Save> rows;

	}

	/// <summary>
	/// セーブデータ
	/// </summary>
	[Serializable]
	public class Save {

		/// <summary>
		/// ID
		/// </summary>
		public int id;
		
		/// <summary>
		/// 垂直方向のカメラ移動を反転させるかどうか
		/// </summary>
		public bool isReverseVerticalCamera;

		/// <summary>
		/// 水平方向のカメラ移動を反転させるかどうか
		/// </summary>
		public bool isReverseHorizontalCamera;

		/// <summary>
		/// 最終更新日時
		/// </summary>
		public string latestUpdateDatetime;

	}

}
