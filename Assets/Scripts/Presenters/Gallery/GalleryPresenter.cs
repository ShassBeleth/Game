using UnityEngine;
using UnityEngine.SceneManagement;
using Views.Gallery;

namespace Presenters.Gallery {

	/// <summary>
	/// ギャラリーPresenter
	/// </summary>
	public class GalleryPresenter{
		
		/// <summary>
		/// ギャラリーView
		/// </summary>
		private GalleryView GalleryView { set; get; }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public GalleryPresenter() {
			Logger.Debug( "Start" );

			// hierarchyからViewを持つGameObject取得
			GameObject galleryGameObject = GameObject.Find( "Canvas" );

			// Viewを取得
			this.GalleryView = galleryGameObject.GetComponent<GalleryView>();

			// ギャラリーViewのEventHandler設定
			this.GalleryView.OnClickBackButtonEventHandler = this.ClickedBackButtonEvent;

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 戻るボタン押下時イベント
		/// </summary>
		private void ClickedBackButtonEvent() {
			Logger.Debug( "Start" );
			SceneManager.LoadScene( "Title" );
			Logger.Debug( "End" );
		}

	}

}