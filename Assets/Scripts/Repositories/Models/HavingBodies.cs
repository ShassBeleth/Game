using System;
using System.Collections.Generic;

namespace Repositories.Models {

	/// <summary>
	/// 保持している素体一覧
	/// </summary>
	[Serializable]
	public class HavingBodies {

		/// <summary>
		/// 保持している素体一覧
		/// </summary>
		public List<HavingBody> rows;

	}

	/// <summary>
	/// 保持している素体
	/// </summary>
	[Serializable]
	public class HavingBody {

		/// <summary>
		/// セーブID
		/// </summary>
		public int saveId;

		/// <summary>
		/// 素体ID
		/// </summary>
		public int bodyId;

	}

}
