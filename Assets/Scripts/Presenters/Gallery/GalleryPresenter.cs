using Services.Scenes;
using Services.Scenes.Parameters;
using UnityEngine;
using Views.Gallery;

namespace Presenters.Gallery {

	/// <summary>
	/// ギャラリーPresenter
	/// </summary>
	public class GalleryPresenter{

		#region Model

		#endregion

		#region View

		/// <summary>
		/// ギャラリーView
		/// </summary>
		private GalleryView GalleryView { set; get; }

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
		public GalleryPresenter() {
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

			// Viewを取得
			this.GalleryView = GameObject.Find( "Canvas" ).GetComponent<GalleryView>();

			// ギャラリーViewのEventHandler設定
			this.GalleryView.OnClickBackButtonEventHandler = this.ClickedBackButtonEvent;

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