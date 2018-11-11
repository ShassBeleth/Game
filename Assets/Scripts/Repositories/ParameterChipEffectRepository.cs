using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// パラメータチップの効果リポジトリ
	/// </summary>
	public class ParameterChipEffectRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static ParameterChipEffectRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static ParameterChipEffectRepository GetInstance() {
			Logger.Debug( "Start" );
			if( Instance == null ) {
				Logger.Debug( "Instance is Null." );
				Instance = new ParameterChipEffectRepository();
			}
			Logger.Debug( "End" );
			return Instance;
		}

		#endregion

		/// <summary>
		/// パラメータチップの効果一覧
		/// </summary>
		public List<ParameterChipEffect> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "parameter_chip_effects.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private ParameterChipEffectRepository() {
			Logger.Debug( "Start" );

			ParameterChipEffects parameterChipEffects = this.Load<ParameterChipEffects>( this.FilePath );
			this.Rows = parameterChipEffects.rows;

			Logger.Debug( "End" );

		}

	}
}
