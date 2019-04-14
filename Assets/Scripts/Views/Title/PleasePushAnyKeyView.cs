using System;
using UniRx;
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
		/// 何かキーが押された時のイベントSubject
		/// </summary>
		private readonly Subject<Unit> OnClickedAnyKeySubject = new Subject<Unit>();

		/// <summary>
		/// 何かキーが押された時のイベント購読
		/// </summary>
		public IObservable<Unit> OnClickedAnyKey => this.OnClickedAnyKeySubject;

		/// <summary>
		/// Unity Update
		/// </summary>
		public void Update() {

			// 何かキーが押された時、初回のみ
			if( !this.IsClickAnyKey && Input.anyKey ) {
				this.IsClickAnyKey = true;
				Observable.NextFrame().Subscribe( _ => {
					this.LogDebug( "Start : Click Any Key is false." );
					this.OnClickedAnyKeySubject.OnNext( Unit.Default );
				} );
				this.LogDebug( "End" );
			}

		}
		
	}

}
