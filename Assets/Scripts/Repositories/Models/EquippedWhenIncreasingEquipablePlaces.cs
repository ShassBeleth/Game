using System;
using System.Collections.Generic;

namespace Repositories.Models {

	/// <summary>
	/// 装備すると増える装備可能箇所一覧
	/// </summary>
	[Serializable]
	public class EquippedWhenIncreasingEquipablePlaces {

		/// <summary>
		/// 装備すると増える装備可能箇所一覧
		/// </summary>
		public List<EquippedWhenIncreasingEquipablePlace> rows;

	}

	/// <summary>
	/// 装備すると増える装備可能箇所
	/// </summary>
	[Serializable]
	public class EquippedWhenIncreasingEquipablePlace {
	}

}
