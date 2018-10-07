using System;
using UnityEngine;

namespace Views.Title {

	/// <summary>
	/// タイトルView
	/// </summary>
	public class TitleView : MonoBehaviour {

		/// <summary>
		/// 何かキーが押されたか判定
		/// </summary>
		private bool IsClickAnyKey { set; get; } = false;

		/// <summary>
		/// Unity Update
		/// </summary>
		public void Update() {
			if( Input.anyKey ) {
				if( !this.IsClickAnyKey ) {
					Logger.Debug( "Start : Click Any Key is false." );
					this.IsClickAnyKey = true;
					this.OnClickAnyKeyEventHandler?.Invoke();
					Logger.Debug( "End" );
				}
			}
		}

		/// <summary>
		/// 何かキーが押された時のイベントハンドラ
		/// </summary>
		public Action OnClickAnyKeyEventHandler { set; get; }

	}

}