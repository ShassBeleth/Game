using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {
	
	/// <summary>
	/// 素体リポジトリ
	/// </summary>
	public class BodyRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static BodyRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static BodyRepository GetInstance() {
			if( Instance == null ) {
				Instance = new BodyRepository();
			}
			return Instance;
		}

		#endregion

		/// <summary>
		/// 素体一覧
		/// </summary>
		public List<Body> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "bodies.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private BodyRepository() {
			this.LogDebug( "Start" );
			
			Bodies bodies = this.Load<Bodies>( this.FilePath );
			this.Rows = bodies.rows;

			this.LogDebug( "End" );

		}

	}

}
