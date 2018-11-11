using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// チャプターのクリア状況リポジトリ
	/// </summary>
	public class ChapterClearStatusRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static ChapterClearStatusRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static ChapterClearStatusRepository GetInstance() {
			Logger.Debug( "Start" );
			if( Instance == null ) {
				Logger.Debug( "Instance is Null." );
				Instance = new ChapterClearStatusRepository();
			}
			Logger.Debug( "End" );
			return Instance;
		}

		#endregion

		/// <summary>
		/// 素体一覧
		/// </summary>
		public List<ChapterClearStatus> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "chapter_clear_statuses.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private ChapterClearStatusRepository() {
			Logger.Debug( "Start" );

			ChapterClearStatuses chapterClearStatuses = this.Load<ChapterClearStatuses>( this.FilePath );
			this.Rows = chapterClearStatuses.rows;

			Logger.Debug( "End" );

		}

	}
}
