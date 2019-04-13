using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// 装備すると装備できなくなる装備可能箇所リポジトリ
	/// </summary>
	public class EquippedWhenUnequippingEquipablePlaceRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static EquippedWhenUnequippingEquipablePlaceRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static EquippedWhenUnequippingEquipablePlaceRepository GetInstance() {
			if( Instance == null ) {
				Instance = new EquippedWhenUnequippingEquipablePlaceRepository();
			}
			return Instance;
		}

		#endregion

		/// <summary>
		/// 素体一覧
		/// </summary>
		public List<EquippedWhenUnequippingEquipablePlace> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "equipped_when_unequipping_equipable_places.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private EquippedWhenUnequippingEquipablePlaceRepository() {
			this.LogDebug( "Start" );

			EquippedWhenUnequippingEquipablePlaces equippedWhenUnequippingEquipablePlaces = this.Load<EquippedWhenUnequippingEquipablePlaces>( this.FilePath );
			this.Rows = equippedWhenUnequippingEquipablePlaces.rows;

			this.LogDebug( "End" );

		}

	}
}
