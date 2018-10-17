using System;
using UnityEngine;

namespace Views.Title {

	/// <summary>
	/// 何かキーを押してくださいのView
	/// </summary>
	public class PleasePushAnyKeyView : MonoBehaviour {
		
		/// <summary>
		/// 何かキーが押されたか判定
		/// </summary>
		private bool IsClickAnyKey { set; get; } = false;
		
		/// <summary>
		/// 何かキーが押された時のイベントハンドラ
		/// </summary>
		public Action OnClickAnyKeyEventHandler { set; get; }

		/// <summary>
		/// Unity Update
		/// </summary>
		public void Update() {

			// 何かキーが押された時、初回のみ
			if( !this.IsClickAnyKey && Input.anyKey ) {
				Logger.Debug( "Start : Click Any Key is false." );
				this.IsClickAnyKey = true;
				this.OnClickAnyKeyEventHandler?.Invoke();
				Logger.Debug( "End" );
			}

		}
		
	}

}
