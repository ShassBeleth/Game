using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// 素体の空きマスリポジトリ
	/// </summary>
	public class BodyFreeSquareRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static BodyFreeSquareRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static BodyFreeSquareRepository GetInstance() {
			Logger.Debug( "Start" );
			if( Instance == null ) {
				Logger.Debug( "Instance is Null." );
				Instance = new BodyFreeSquareRepository();
			}
			Logger.Debug( "End" );
			return Instance;
		}

		#endregion

		/// <summary>
		/// 素体一覧
		/// </summary>
		public List<BodyFreeSquare> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "body_free_squares.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private BodyFreeSquareRepository() {
			Logger.Debug( "Start" );

			BodyFreeSquares bodyFreeSquares = this.Load<BodyFreeSquares>( this.FilePath );
			this.Rows = bodyFreeSquares.rows;

			Logger.Debug( "End" );

		}

	}
}
