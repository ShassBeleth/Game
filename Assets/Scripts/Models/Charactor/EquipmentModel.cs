using System.Collections.Generic;
using UniRx;

namespace Models.Charactor {

	/// <summary>
	/// 装備Model
	/// </summary>
	public class EquipmentModel {

		/// <summary>
		/// ID
		/// </summary>
		public ReactiveProperty<int?> Id { set; get; } = new ReactiveProperty<int?>();

		/// <summary>
		/// 装備名
		/// </summary>
		public string Name { set; get; }

		/// <summary>
		/// 装備名ルビ
		/// </summary>
		public string Ruby { set; get; }

		/// <summary>
		/// フレーバーテキスト
		/// </summary>
		public string Flavor { set; get; }

		/// <summary>
		/// 表示順
		/// </summary>
		public int DisplayOrder { set; get; }

		/// <summary>
		/// 装備できるかどうか
		/// </summary>
		public bool CanEquip { set; get; }
		
	}

}
