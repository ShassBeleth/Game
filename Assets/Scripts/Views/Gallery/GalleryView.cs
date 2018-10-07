using System;
using UnityEngine;

namespace Views.Gallery {

	/// <summary>
	/// ギャラリーView
	/// </summary>
	public class GalleryView : MonoBehaviour {

		/// <summary>
		/// 戻るボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickBackButtonEventHandler { set; get; }

		/// <summary>
		/// 戻るボタン押下時イベント
		/// </summary>
		public void OnClickBackButtonEvent() {
			Logger.Debug( "Start" );
			this.OnClickBackButtonEventHandler?.Invoke();
			Logger.Debug( "End" );
		}

	}

}