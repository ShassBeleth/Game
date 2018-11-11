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
			Logger.Debug( "Start" );
			if( Instance == null ) {
				Logger.Debug( "Instance is Null." );
				Instance = new EquipmentFreeSquareRepository();
			}
			Logger.Debug( "End" );
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
			Logger.Debug( "Start" );

			EquipmentFreeSquares equipmentFreeSquares = this.Load<EquipmentFreeSquares>( this.FilePath );
			this.Rows = equipmentFreeSquares.rows;

			Logger.Debug( "End" );

		}

	}
}
