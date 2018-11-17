using System;
using System.Collections.Generic;

namespace Repositories.Models {

	/// <summary>
	/// 素体：装備可能箇所一覧
	/// </summary>
	[Serializable]
	public class BodiesEquipablePlaces {

		/// <summary>
		/// 素体：装備可能箇所一覧
		/// </summary>
		public List<BodyEquipablePlace> rows;

	}

	/// <summary>
	/// 素体：装備可能箇所一覧
	/// </summary>
	[Serializable]
	public class BodyEquipablePlace {

		/// <summary>
		/// 素体ID
		/// </summary>
		public int bodyId;

		/// <summary>
		/// 装備可能箇所ID
		/// </summary>
		public int equipablePlaceId;

		/// <summary>
		/// 表示順
		/// </summary>
		public int displayOrder;

	}

}
