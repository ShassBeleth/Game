using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// 装備可能箇所に装備できる装備リポジトリ
	/// </summary>
	public class EquipmentEquipableInEquipablePlaceRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static EquipmentEquipableInEquipablePlaceRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static EquipmentEquipableInEquipablePlaceRepository GetInstance() {
			Logger.Debug( "Start" );
			if( Instance == null ) {
				Logger.Debug( "Instance is Null." );
				Instance = new EquipmentEquipableInEquipablePlaceRepository();
			}
			Logger.Debug( "End" );
			return Instance;
		}

		#endregion

		/// <summary>
		/// 素体一覧
		/// </summary>
		public List<EquipmentEquipableInEquipablePlace> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "equipments_equipable_in_equipable_places.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private EquipmentEquipableInEquipablePlaceRepository() {
			Logger.Debug( "Start" );

			EquipmentsEquipableInEquipablePlaces equipmentsEquipableInEquipablePlaces = this.Load<EquipmentsEquipableInEquipablePlaces>( this.FilePath );
			this.Rows = equipmentsEquipableInEquipablePlaces.rows;

			Logger.Debug( "End" );

		}

	}
}
