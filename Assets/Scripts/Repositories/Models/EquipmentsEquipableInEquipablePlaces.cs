using System;
using System.Collections.Generic;

namespace Repositories.Models {

	/// <summary>
	/// 装備可能箇所に装備できる装備一覧
	/// </summary>
	[Serializable]
	public class EquipmentsEquipableInEquipablePlaces {

		/// <summary>
		/// 装備可能箇所に装備できる装備一覧
		/// </summary>
		public List<EquipmentEquipableInEquipablePlace> rows;

	}

	/// <summary>
	/// 装備可能箇所に装備できる装備
	/// </summary>
	[Serializable]
	public class EquipmentEquipableInEquipablePlace {

		/// <summary>
		/// 装備ID
		/// </summary>
		public int equipmentId;

		/// <summary>
		/// 装備可能箇所ID
		/// </summary>
		public int equipablePlaceId;

	}

}
