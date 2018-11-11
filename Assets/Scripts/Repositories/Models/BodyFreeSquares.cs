using System;
using System.Collections.Generic;

namespace Repositories.Models {

	/// <summary>
	/// 素体の空きマス一覧
	/// </summary>
	[Serializable]
	public class BodyFreeSquares {

		/// <summary>
		/// 素体の空きマス一覧
		/// </summary>
		public List<BodyFreeSquare> rows;

	}

	/// <summary>
	/// 素体の空きマス
	/// </summary>
	[Serializable]
	public class BodyFreeSquare {
	}

}
