using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// 装備可能箇所リポジトリ
	/// </summary>
	public class EquipablePlaceRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static EquipablePlaceRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static EquipablePlaceRepository GetInstance() {
			if( Instance == null ) {
				Instance = new EquipablePlaceRepository();
			}
			return Instance;
		}

		#endregion

		/// <summary>
		/// 素体一覧
		/// </summary>
		public List<EquipablePlace> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "equipable_places.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private EquipablePlaceRepository() {
			this.LogDebug( "Start" );

			EquipablePlaces equipablePlaces = this.Load<EquipablePlaces>( this.FilePath );
			this.Rows = equipablePlaces.rows;

			this.LogDebug( "End" );

		}

	}
}
