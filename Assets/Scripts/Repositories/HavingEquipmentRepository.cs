﻿using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// 保持している装備リポジトリ
	/// </summary>
	public class HavingEquipmentRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static HavingEquipmentRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static HavingEquipmentRepository GetInstance() {
			Logger.Debug( "Start" );
			if( Instance == null ) {
				Logger.Debug( "Instance is Null." );
				Instance = new HavingEquipmentRepository();
			}
			Logger.Debug( "End" );
			return Instance;
		}

		#endregion

		/// <summary>
		/// 保持している装備一覧
		/// </summary>
		public List<HavingEquipment> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "having_equipments.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private HavingEquipmentRepository() {
			Logger.Debug( "Start" );

			HavingEquipments havingEquipments = this.Load<HavingEquipments>( this.FilePath );
			this.Rows = havingEquipments.rows;

			Logger.Debug( "End" );

		}

	}
}
