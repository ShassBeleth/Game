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
			if( Instance == null ) {
				Instance = new BodyFreeSquareRepository();
			}
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
			this.LogDebug( "Start" );

			BodyFreeSquares bodyFreeSquares = this.Load<BodyFreeSquares>( this.FilePath );
			this.Rows = bodyFreeSquares.rows;

			this.LogDebug( "End" );

		}

	}
}
