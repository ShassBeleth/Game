using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// セーブデータリポジトリ
	/// </summary>
	public class SaveRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static SaveRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static SaveRepository GetInstance() {
			Logger.Debug( "Start" );
			if( Instance == null ) {
				Logger.Debug( "Instance is Null." );
				Instance = new SaveRepository();
			}
			Logger.Debug( "End" );
			return Instance;
		}

		#endregion

		/// <summary>
		/// セーブデータ一覧
		/// </summary>
		public List<Save> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "saves.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private SaveRepository() {
			Logger.Debug( "Start" );

			Saves saves = this.Load<Saves>( this.FilePath );
			this.Rows = saves.rows;

			Logger.Debug( "End" );

		}

		/// <summary>
		/// セーブデータの書き込み
		/// </summary>
		public void Write() {
			Logger.Debug( "Start" );
			this.Write<Saves>( this.FilePath , new Saves() {
				rows = this.Rows
			} );
			Logger.Debug( "End" );
		}

	}
}
