using Services.Scenes;
using Services.Scenes.Parameters;
using UnityEngine;
using Views.Ranking;
using UniRx;

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
		private readonly SceneService sceneService = SceneService.GetInstance();

		#endregion

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public RankingPresenter() {
			this.LogDebug( "Start" );

			// Viewの設定
			this.InitialViewSetting();


			this.LogDebug( "End" );
		}

		#region 初期設定

		/// <summary>
		/// Viewの設定
		/// </summary>
		private void InitialViewSetting() {
			this.LogDebug( "Start" );

			// hierarchyからViewを取得
			this.rankingView = GameObject.Find( "Canvas" ).GetComponent<RankingView>();

			// ランキングViewのEventHandler設定
			this.rankingView.OnClickedBackButton.Subscribe( _ => this.ClickedBackButtonEvent() );

			this.LogDebug( "End" );
		}

		#endregion

		#region Viewイベント

		/// <summary>
		/// 戻るボタン押下時イベント
		/// </summary>
		private void ClickedBackButtonEvent() {
			this.LogDebug( "Start" );
			this.sceneService.LoadScene(
				"Title" ,
				new TitleParameter() {
					InitialTitlePart = TitleParameter.InitialTitlePartEnum.MainMenu
				}
			);
			this.LogDebug( "End" );
		}

		#endregion

	}

}
