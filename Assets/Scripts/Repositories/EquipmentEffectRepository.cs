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
			Logger.Debug( "Start" );
			if( Instance == null ) {
				Logger.Debug( "Instance is Null." );
				Instance = new EquipmentEffectRepository();
			}
			Logger.Debug( "End" );
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
			Logger.Debug( "Start" );

			EquipmentEffects equipmentEffects = this.Load<EquipmentEffects>( this.FilePath );
			this.Rows = equipmentEffects.rows;

			Logger.Debug( "End" );

		}

	}
}
