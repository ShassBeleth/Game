using System;
using System.Collections.Generic;

namespace Repositories.Models {

	/// <summary>
	/// セーブデータ一覧
	/// </summary>
	[Serializable]
	public class Saves {

		/// <summary>
		/// セーブデータ一覧
		/// </summary>
		public List<Save> rows;

	}

	/// <summary>
	/// セーブデータ
	/// </summary>
	[Serializable]
	public class Save {
	}

}
