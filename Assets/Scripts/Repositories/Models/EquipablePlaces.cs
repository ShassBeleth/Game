using System;
using System.Collections.Generic;

namespace Repositories.Models {

	/// <summary>
	/// 装備可能箇所一覧
	/// </summary>
	[Serializable]
	public class EquipablePlaces {

		/// <summary>
		/// 装備可能箇所一覧
		/// </summary>
		public List<EquipablePlace> rows;

	}

	/// <summary>
	/// 装備可能箇所
	/// </summary>
	[Serializable]
	public class EquipablePlace {

		/// <summary>
		/// ID
		/// </summary>
		public int id;

		/// <summary>
		/// 名前
		/// </summary>
		public string name;

	}

}
