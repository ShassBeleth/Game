using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Customize {

	/// <summary>
	/// 素体一覧のNodeにつくView
	/// </summary>
	public class BodyNodeView : MonoBehaviour {

		/// <summary>
		/// 素体Id
		/// </summary>
		public int Id { set; get; }
		
		/// <summary>
		/// テキスト
		/// </summary>
		public Text Text;

		/// <summary>
		/// 決定ボタン押下時イベントSubject
		/// </summary>
		public Subject<Unit> OnClickedDecisionButtonSubject = new Subject<Unit>();

		/// <summary>
		/// 決定ボタン押下時イベント
		/// </summary>
		public void OnClickDecisionEvent() {
			this.LogDebug( "Start" );
			this.OnClickedDecisionButtonSubject.OnNext( Unit.Default );
			this.LogDebug( "End" );
		}

		/// <summary>
		/// 素体名設定
		/// </summary>
		/// <param name="bodyName">素体名</param>
		public void SetBodyName( string bodyName ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Body Name is {bodyName}." );
			this.Text.text = bodyName;
			this.LogDebug( "End" );
		}
		
	}

}