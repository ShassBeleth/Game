using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Customize {

	/// <summary>
	/// 装備一覧のNodeにつくView
	/// </summary>
	public class EquipmentNodeView : MonoBehaviour {

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
		/// 装備名の設定
		/// </summary>
		/// <param name="equipmentName">装備名</param>
		public void SetEquipmentName( string equipmentName ) {
			Logger.Debug( "Start" );
			this.transform.GetChild( 0 ).GetComponent<Text>().text = equipmentName ?? "None Name";
			Logger.Debug( "End" );
		}

	}

}
