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
		/// 装備可能箇所
		/// </summary>
		public List<int> EquipablePlaceIds = new List<int>();

	}

}
