using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// 素体の効果リポジトリ
	/// </summary>
	public class BodyEffectRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static BodyEffectRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static BodyEffectRepository GetInstance() {
			if( Instance == null ) {
				Instance = new BodyEffectRepository();
			}
			return Instance;
		}

		#endregion

		/// <summary>
		/// 素体一覧
		/// </summary>
		public List<BodyEffect> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "body_effects.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private BodyEffectRepository() {
			this.LogDebug( "Start" );

			BodyEffects bodyEffects = this.Load<BodyEffects>( this.FilePath );
			this.Rows = bodyEffects.rows;

			this.LogDebug( "End" );
		}

	}
}
