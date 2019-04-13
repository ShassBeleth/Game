using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// 指定箇所への装備による効果リポジトリ
	/// </summary>
	public class DesignatedPlaceToEquipmentByEffectRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static DesignatedPlaceToEquipmentByEffectRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static DesignatedPlaceToEquipmentByEffectRepository GetInstance() {
			if( Instance == null ) {
				Instance = new DesignatedPlaceToEquipmentByEffectRepository();
			}
			return Instance;
		}

		#endregion

		/// <summary>
		/// 素体一覧
		/// </summary>
		public List<DesignatedPlaceToEquipmentByEffect> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "designated_place_to_equipment_by_effects.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private DesignatedPlaceToEquipmentByEffectRepository() {
			this.LogDebug( "Start" );

			DesignatedPlaceToEquipmentByEffects designatedPlaceToEquipmentByEffects = this.Load<DesignatedPlaceToEquipmentByEffects>( this.FilePath );
			this.Rows = designatedPlaceToEquipmentByEffects.rows;

			this.LogDebug( "End" );

		}

	}
}
