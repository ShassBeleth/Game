﻿using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// 素体：装備可能箇所リポジトリ
	/// </summary>
	public class BodyEquipablePlaceRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static BodyEquipablePlaceRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static BodyEquipablePlaceRepository GetInstance() {
			if( Instance == null ) {
				Instance = new BodyEquipablePlaceRepository();
			}
			return Instance;
		}

		#endregion

		/// <summary>
		/// 素体一覧
		/// </summary>
		public List<BodyEquipablePlace> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "bodies_equipable_places.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private BodyEquipablePlaceRepository() {
			this.LogDebug( "Start" );

			BodiesEquipablePlaces bodiesEquipablePlaces = this.Load<BodiesEquipablePlaces>( this.FilePath );
			this.Rows = bodiesEquipablePlaces.rows;

			this.LogDebug( "End" );
		}

	}
}
