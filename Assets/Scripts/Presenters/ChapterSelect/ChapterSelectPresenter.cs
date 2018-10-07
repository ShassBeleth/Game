
using UnityEngine;
using Views.ChapterSelect;

namespace Presenters.ChapterSelect {

	/// <summary>
	/// チャプター選択Presenter
	/// </summary>
	public class ChapterSelectPresenter {

		/// <summary>
		/// チャプター選択View
		/// </summary>
		private ChapterSelectView chapterSelectView;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public ChapterSelectPresenter() {
			Logger.Debug( "Start" );

			// hierarchyからViewを取得
			this.chapterSelectView = GameObject.Find( "Canvas" ).GetComponent<ChapterSelectView>();

			// セーブデータ選択ViewのEventHandler設定
			this.chapterSelectView.OnClickBackButtonEventHandler = this.OnClickBackButtonEvent;
			#region TODO 仮
			this.chapterSelectView.OnClickTempButtonEventHandler = this.OnClickTempButtonEvent;
			#endregion

			Logger.Debug( "End" );

		}

		/// <summary>
		/// 戻るボタン押下時イベント
		/// </summary>
		public void OnClickBackButtonEvent() {
			Logger.Debug( "Start" );
			UnityEngine.SceneManagement.SceneManager.LoadScene( "SelectSaveData" );
			Logger.Debug( "End" );
		}

		#region TODO 仮
		public void OnClickTempButtonEvent() {
			Logger.Debug( "Start" );
			UnityEngine.SceneManagement.SceneManager.LoadScene( "SelectSaveData" );
			Logger.Debug( "End" );
		}
		#endregion

	}

}