using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// 保持しているパラメータチップリポジトリ
	/// </summary>
	public class HavingParameterChipRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static HavingParameterChipRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static HavingParameterChipRepository GetInstance() {
			Logger.Debug( "Start" );
			if( Instance == null ) {
				Logger.Debug( "Instance is Null." );
				Instance = new HavingParameterChipRepository();
			}
			Logger.Debug( "End" );
			return Instance;
		}

		#endregion

		/// <summary>
		/// 保持しているパラメータチップ一覧
		/// </summary>
		public List<HavingParameterChip> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "having_parameter_chips.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private HavingParameterChipRepository() {
			Logger.Debug( "Start" );

			HavingParameterChips havingParameterChips = this.Load<HavingParameterChips>( this.FilePath );
			this.Rows = havingParameterChips.rows;

			Logger.Debug( "End" );

		}

	}
}
