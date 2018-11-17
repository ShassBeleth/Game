using Services.Scenes;
using Services.Scenes.Parameters;
using UnityEngine;
using Views.Ranking;

namespace Presenters.Ranking {

	/// <summary>
	/// ランキングSceneのPresenter
	/// </summary>
	public class RankingPresenter {

		#region Model

		#endregion

		#region View

		/// <summary>
		/// ランキングView
		/// </summary>
		private RankingView rankingView { set; get; }

		#endregion

		#region Service

		/// <summary>
		/// シーンService
		/// </summary>
		private SceneService sceneService = SceneService.GetInstance();

		#endregion

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public RankingPresenter() {
			Logger.Debug( "Start" );

			// Viewの設定
			this.InitialViewSetting();


			Logger.Debug( "End" );
		}

		#region 初期設定

		/// <summary>
		/// Viewの設定
		/// </summary>
		private void InitialViewSetting() {
			Logger.Debug( "Start" );

			// hierarchyからViewを取得
			this.rankingView = GameObject.Find( "Canvas" ).GetComponent<RankingView>();

			// ランキングViewのEventHandler設定
			this.rankingView.OnClickBackButtonEventHandler = this.ClickedBackButtonEvent;

			Logger.Debug( "End" );
		}

		#endregion

		#region Viewイベント

		/// <summary>
		/// 戻るボタン押下時イベント
		/// </summary>
		private void ClickedBackButtonEvent() {
			Logger.Debug( "Start" );
			this.sceneService.LoadScene(
				"Title" ,
				new TitleParameter() {
					InitialTitlePart = TitleParameter.InitialTitlePartEnum.MainMenu
				}
			);
			Logger.Debug( "End" );
		}

		#endregion

	}

}
