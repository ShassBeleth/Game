using System;
using System.Collections.Generic;

namespace Repositories.Models {

	/// <summary>
	/// 指定箇所への装備による効果一覧
	/// </summary>
	[Serializable]
	public class DesignatedPlaceToEquipmentByEffects {

		/// <summary>
		/// 指定箇所への装備による効果一覧
		/// </summary>
		public List<DesignatedPlaceToEquipmentByEffect> rows;

	}

	/// <summary>
	/// 指定箇所への装備による効果
	/// </summary>
	[Serializable]
	public class DesignatedPlaceToEquipmentByEffect {
	}

}
