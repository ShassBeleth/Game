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
			Logger.Debug( "Start" );
			if( Instance == null ) {
				Logger.Debug( "Instance is Null." );
				Instance = new ChapterService();
			}
			Logger.Debug( "End" );
			return Instance;
		}

		#endregion

	}

}
