using System;
using System.Collections.Generic;

namespace Repositories.Models {

	/// <summary>
	/// 装備すると装備できなくなる装備可能箇所一覧
	/// </summary>
	[Serializable]
	public class EquippedWhenUnequippingEquipablePlaces {

		/// <summary>
		/// 装備すると装備できなくなる装備可能箇所一覧
		/// </summary>
		public List<EquippedWhenUnequippingEquipablePlace> rows;

	}

	/// <summary>
	/// 装備すると装備できなくなる装備可能箇所
	/// </summary>
	[Serializable]
	public class EquippedWhenUnequippingEquipablePlace {

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
