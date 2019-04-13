using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// 装備すると増える装備可能箇所リポジトリ
	/// </summary>
	public class EquippedWhenIncreasingEquipablePlaceRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static EquippedWhenIncreasingEquipablePlaceRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static EquippedWhenIncreasingEquipablePlaceRepository GetInstance() {
			if( Instance == null ) {
				Instance = new EquippedWhenIncreasingEquipablePlaceRepository();
			}
			return Instance;
		}

		#endregion

		/// <summary>
		/// 素体一覧
		/// </summary>
		public List<EquippedWhenIncreasingEquipablePlace> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "equipped_when_increasing_equipable_places.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private EquippedWhenIncreasingEquipablePlaceRepository() {
			this.LogDebug( "Start" );

			EquippedWhenIncreasingEquipablePlaces equippedWhenIncreasingEquipablePlaces = this.Load<EquippedWhenIncreasingEquipablePlaces>( this.FilePath );
			this.Rows = equippedWhenIncreasingEquipablePlaces.rows;

			this.LogDebug( "End" );

		}

	}
}
