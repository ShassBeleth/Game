using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// パラメータチップリポジトリ
	/// </summary>
	public class ParameterChipRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static ParameterChipRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static ParameterChipRepository GetInstance() {
			if( Instance == null ) {
				Instance = new ParameterChipRepository();
			}
			return Instance;
		}

		#endregion

		/// <summary>
		/// パラメータチップ一覧
		/// </summary>
		public List<ParameterChip> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "parameter_chips.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private ParameterChipRepository() {
			this.LogDebug( "Start" );

			ParameterChips parameterChips = this.Load<ParameterChips>( this.FilePath );
			this.Rows = parameterChips.rows;

			this.LogDebug( "End" );

		}

	}
}
