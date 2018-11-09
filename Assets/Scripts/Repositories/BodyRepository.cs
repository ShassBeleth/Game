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
			Logger.Debug( "Start" );
			if( Instance == null ) {
				Logger.Debug( "Instance is Null." );
				Instance = new BodyRepository();
			}
			Logger.Debug( "End" );
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
			Logger.Debug( "Start" );

			Logger.Debug( $"Directory Path is {this.DirectoryPath}." );
			Logger.Debug( $"File Path is {this.FilePath}." );

			Bodies bodies = this.Load<Bodies>( this.FilePath );
			this.Rows = bodies.rows;

			Logger.Debug( "End" );

		}

	}

}
