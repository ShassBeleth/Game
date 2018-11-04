using System.Collections.Generic;
using UniRx;

namespace Models.Charactor {

	/// <summary>
	/// 素体Model
	/// </summary>
	public class BodyModel {

		/// <summary>
		/// ID
		/// </summary>
		public ReactiveProperty<int?> Id { set; get; } = new ReactiveProperty<int?>(null);

		/// <summary>
		/// 素体名
		/// </summary>
		public string Name { set; get; }

		/// <summary>
		/// 素体名ルビ
		/// </summary>
		public string Ruby { set; get; }

		/// <summary>
		/// フレーバーテキスト
		/// </summary>
		public string Flavor { set; get; }

		/// <summary>
		/// 装備可能箇所一覧
		/// </summary>
		public List<EquipablePlaceModel> EquipablePlaces { set; get; } = new List<EquipablePlaceModel>();
		
	}

}
