using System;
using System.Collections.Generic;
using System.Linq;
using Models.Chapter;
using Repositories;

namespace Services.Chapters {

	/// <summary>
	/// チャプターService
	/// </summary>
	public class ChapterService {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static ChapterService Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static ChapterService GetInstance() {
			if( Instance == null ) {
				Instance = new ChapterService();
			}
			return Instance;
		}

		#endregion

		#region Repository

		private readonly ChapterRepository chapterRepository = ChapterRepository.GetInstance();

		#endregion

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private ChapterService() {
			this.LogDebug( "Start" );

			this.LogDebug( "End" );
		}

		/// <summary>
		/// チャプター一覧を取得する
		/// </summary>
		/// <param name="clearedChapter"></param>
		/// <returns></returns>
		public List<ChapterModel> GetChapters( List<int> clearedChapters ) {
			this.LogDebug( "Start" );

			// クリア済みカテゴリ一覧
			List<int> clearedCategories = this.chapterRepository.Rows
				.Where( row => clearedChapters.Contains( row.id ) )
				.Select( row => row.category )
				.Distinct()
				.ToList();

			List<ChapterModel> models = this.chapterRepository.Rows
				// 同じカテゴリーのものだけに絞り込む
				.Where( row => clearedCategories.Contains( row.category ) )
				.Select( row => new ChapterModel() {
					Id = row.id ,
					Name = row.name ,
					IsCleared = clearedChapters.Contains( row.id ) ,
					NumberOrder = row.numberOrder ,
					Detail = row.text
				} )
				.OrderByDescending( chapter => chapter.NumberOrder )
				.ToList();
			this.LogDebug( "End" );
			return models;
		}

	}

}
