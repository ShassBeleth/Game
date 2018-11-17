using System;
using System.Collections.Generic;

namespace Repositories.Models {

	/// <summary>
	/// 保持している装備一覧
	/// </summary>
	[Serializable]
	public class HavingEquipments {

		/// <summary>
		/// 保持している装備一覧
		/// </summary>
		public List<HavingEquipment> rows;

	}

	/// <summary>
	/// 保持している装備
	/// </summary>
	[Serializable]
	public class HavingEquipment {

		/// <summary>
		/// セーブID
		/// </summary>
		public int saveId;

		/// <summary>
		/// 装備ID
		/// </summary>
		public int equipmentId;

	}

}
