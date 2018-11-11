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
	}

}
