using System;
using System.Collections.Generic;

namespace Repositories.Models {

	/// <summary>
	/// 素体一覧
	/// </summary>
	[Serializable]
	public class Bodies {

		/// <summary>
		/// 素体一覧
		/// </summary>
		public List<Body> rows;
		
	}
	
	/// <summary>
	/// 素体
	/// </summary>
	[Serializable]
	public class Body {

		/// <summary>
		/// 素体ID
		/// </summary>
		public int id;

		/// <summary>
		/// 素体名
		/// </summary>
		public string name;

		/// <summary>
		/// 素体名ルビ
		/// </summary>
		public string ruby;

		/// <summary>
		/// フレーバーテキスト
		/// </summary>
		public string flavor;

	}

}
