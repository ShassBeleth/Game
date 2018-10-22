using SceneManagers;
using SceneManagers.Parameters;
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

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public GalleryPresenter() {
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

			// Viewを取得
			this.GalleryView = GameObject.Find( "Canvas" ).GetComponent<GalleryView>();

			// ギャラリーViewのEventHandler設定
			this.GalleryView.OnClickBackButtonEventHandler = this.ClickedBackButtonEvent;

			Logger.Debug( "End" );
		}

		#endregion

		#region Viewイベント

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

		#endregion

	}

}