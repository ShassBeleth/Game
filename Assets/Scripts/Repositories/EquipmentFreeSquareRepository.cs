using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// 装備することで増える空きマスリポジトリ
	/// </summary>
	public class EquipmentFreeSquareRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static EquipmentFreeSquareRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static EquipmentFreeSquareRepository GetInstance() {
			if( Instance == null ) {
				Instance = new EquipmentFreeSquareRepository();
			}
			return Instance;
		}

		#endregion

		/// <summary>
		/// 素体一覧
		/// </summary>
		public List<EquipmentFreeSquare> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "equipment_free_squares.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private EquipmentFreeSquareRepository() {
			this.LogDebug( "Start" );

			EquipmentFreeSquares equipmentFreeSquares = this.Load<EquipmentFreeSquares>( this.FilePath );
			this.Rows = equipmentFreeSquares.rows;

			this.LogDebug( "End" );

		}

	}
}
