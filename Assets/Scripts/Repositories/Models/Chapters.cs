using System;
using System.Collections.Generic;

namespace Repositories.Models {

	/// <summary>
	/// チャプター一覧
	/// </summary>
	[Serializable]
	public class Chapters {

		/// <summary>
		/// チャプター一覧
		/// </summary>
		public List<Chapter> rows;

	}

	/// <summary>
	/// チャプター
	/// </summary>
	[Serializable]
	public class Chapter {
	}

}
