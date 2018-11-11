using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// パラメータチップのマスリポジトリ
	/// </summary>
	public class ParameterChipSquareRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static ParameterChipSquareRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static ParameterChipSquareRepository GetInstance() {
			Logger.Debug( "Start" );
			if( Instance == null ) {
				Logger.Debug( "Instance is Null." );
				Instance = new ParameterChipSquareRepository();
			}
			Logger.Debug( "End" );
			return Instance;
		}

		#endregion

		/// <summary>
		/// パラメータチップのマス一覧
		/// </summary>
		public List<ParameterChipSquare> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "parameter_chip_squares.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private ParameterChipSquareRepository() {
			Logger.Debug( "Start" );

			ParameterChipSquares parameterChipSquares = this.Load<ParameterChipSquares>( this.FilePath );
			this.Rows = parameterChipSquares.rows;

			Logger.Debug( "End" );

		}

	}
}
