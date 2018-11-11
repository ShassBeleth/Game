﻿using Repositories.Models;
using System.Collections.Generic;

namespace Repositories {

	/// <summary>
	/// チャプターリポジトリ
	/// </summary>
	public class ChapterRepository : RepositoryBase {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static ChapterRepository Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static ChapterRepository GetInstance() {
			Logger.Debug( "Start" );
			if( Instance == null ) {
				Logger.Debug( "Instance is Null." );
				Instance = new ChapterRepository();
			}
			Logger.Debug( "End" );
			return Instance;
		}

		#endregion

		/// <summary>
		/// 素体一覧
		/// </summary>
		public List<Chapter> Rows {
			private set;
			get;
		}

		/// <summary>
		/// ファイルパス
		/// </summary>
		private readonly string FilePath = "chapters.json";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private ChapterRepository() {
			Logger.Debug( "Start" );

			Chapters chapters = this.Load<Chapters>( this.FilePath );
			this.Rows = chapters.rows;

			Logger.Debug( "End" );

		}

	}
}
