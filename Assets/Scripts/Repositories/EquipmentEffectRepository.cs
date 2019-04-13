using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// 装備の効果リポジトリ
	/// </summary>
	public class EquipmentEffectRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static EquipmentEffectRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static EquipmentEffectRepository GetInstance() {
			if( Instance == null ) {
				Instance = new EquipmentEffectRepository();
			}
			return Instance;
		}

		#endregion

		/// <summary>
		/// 素体一覧
		/// </summary>
		public List<EquipmentEffect> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "equipment_effects.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private EquipmentEffectRepository() {
			this.LogDebug( "Start" );

			EquipmentEffects equipmentEffects = this.Load<EquipmentEffects>( this.FilePath );
			this.Rows = equipmentEffects.rows;

			this.LogDebug( "End" );

		}

	}
}
