using System;
using System.Collections.Generic;

namespace Repositories.Models {

	/// <summary>
	/// チャプター一覧
	/// </summary>
	[Serializable]
	public class Chapters {

		/// <summary>
		/// チャプター一覧
		/// </summary>
		public List<Chapter> rows;

	}

	/// <summary>
	/// チャプター
	/// </summary>
	[Serializable]
	public class Chapter {

		/// <summary>
		/// チャプターID
		/// </summary>
		public int id;

		/// <summary>
		/// カテゴリ
		/// </summary>
		public int category;

		/// <summary>
		/// チャプター名
		/// </summary>
		public string name;

		/// <summary>
		/// 詳細テキスト
		/// </summary>
		public string text;

		/// <summary>
		/// 一覧順
		/// </summary>
		public int numberOrder;

		/// <summary>
		/// イベント日時
		/// </summary>
		public DateTime eventDateTime;

	}

}
