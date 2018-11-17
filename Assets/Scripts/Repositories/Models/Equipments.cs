using System;
using System.Collections.Generic;

namespace Repositories.Models {

	/// <summary>
	/// 装備一覧
	/// </summary>
	[Serializable]
	public class Equipments {

		/// <summary>
		/// 装備一覧
		/// </summary>
		public List<Equipment> rows;

	}

	/// <summary>
	/// 装備
	/// </summary>
	[Serializable]
	public class Equipment {

		/// <summary>
		/// 装備ID
		/// </summary>
		public int id;

		/// <summary>
		/// 装備名
		/// </summary>
		public string name;

		/// <summary>
		/// 装備名ルビ
		/// </summary>
		public string ruby;

		/// <summary>
		/// フレーバーテキスト
		/// </summary>
		public string flavor;

		/// <summary>
		/// 表示順
		/// </summary>
		public int displayOrder;

	}

}
