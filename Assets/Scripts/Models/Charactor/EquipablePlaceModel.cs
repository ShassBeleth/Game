using UniRx;

namespace Models.Charactor {

	/// <summary>
	/// 装備可能箇所Model
	/// </summary>
	public class EquipablePlaceModel {

		/// <summary>
		/// ID
		/// </summary>
		public ReactiveProperty<int> Id { set; get; }

		/// <summary>
		/// 装備可能箇所名
		/// </summary>
		public string Name { set; get; }

		/// <summary>
		/// 装備Model
		/// </summary>
		public EquipmentModel EquipmentModel { set; get; } = new EquipmentModel();

	}

}
