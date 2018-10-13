using SceneManagers;
using SceneManagers.Parameters;
using UnityEngine;
using Views.Ranking;

namespace Presenters.Ranking {

	/// <summary>
	/// ランキングSceneのPresenter
	/// </summary>
	public class RankingPresenter {

		/// <summary>
		/// ランキングView
		/// </summary>
		private RankingView rankingView { set; get; }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public RankingPresenter() {
			Logger.Debug( "Start" );

			// hierarchyからViewを取得
			this.rankingView = GameObject.Find( "Canvas" ).GetComponent<RankingView>();

			// ランキングViewのEventHandler設定
			this.rankingView.OnClickBackButtonEventHandler = this.ClickedBackButtonEvent;

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 戻るボタン押下時イベント
		/// </summary>
		private void ClickedBackButtonEvent() {
			Logger.Debug( "Start" );
			SceneManager.GetInstance().LoadScene(
				"Title" ,
				new TitleParameter() {
					InitialTitlePart = TitleParameter.InitialTitlePartEnum.MainMenu
				}
			);
			Logger.Debug( "End" );
		}

	}

}
