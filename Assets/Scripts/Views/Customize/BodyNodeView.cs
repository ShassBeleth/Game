using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Customize {

	/// <summary>
	/// 素体一覧のNodeにつくView
	/// </summary>
	public class BodyNodeView : MonoBehaviour {
		
		/// <summary>
		/// テキスト
		/// </summary>
		public Text Text;
		
		/// <summary>
		/// 決定ボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickDecisionEventHandler { set; get; }

		/// <summary>
		/// 決定ボタン押下時イベント
		/// </summary>
		public void OnClickDecisionEvent() {
			Logger.Debug( "Start" );
			this.OnClickDecisionEventHandler?.Invoke();
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 素体名設定
		/// </summary>
		/// <param name="bodyName">素体名</param>
		public void SetBodyName( string bodyName ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Body Name is {bodyName}." );
			this.Text.text = bodyName;
			Logger.Debug( "End" );
		}
		
	}

}