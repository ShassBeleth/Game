using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// 保持している素体リポジトリ
	/// </summary>
	public class HavingBodyRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static HavingBodyRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static HavingBodyRepository GetInstance() {
			Logger.Debug( "Start" );
			if( Instance == null ) {
				Logger.Debug( "Instance is Null." );
				Instance = new HavingBodyRepository();
			}
			Logger.Debug( "End" );
			return Instance;
		}

		#endregion

		/// <summary>
		/// 保持している素体一覧
		/// </summary>
		public List<HavingBody> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "having_bodies.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private HavingBodyRepository() {
			Logger.Debug( "Start" );

			HavingBodies havingBodies = this.Load<HavingBodies>( this.FilePath );
			this.Rows = havingBodies.rows;

			Logger.Debug( "End" );

		}

	}
}
