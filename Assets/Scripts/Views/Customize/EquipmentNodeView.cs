using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Customize {

	/// <summary>
	/// 装備一覧のNodeにつくView
	/// </summary>
	public class EquipmentNodeView : MonoBehaviour {

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
		/// 装備名の設定
		/// </summary>
		/// <param name="equipmentName">装備名</param>
		public void SetEquipmentName( string equipmentName ) {
			this.LogDebug( "Start" );
			this.transform.GetChild( 0 ).GetComponent<Text>().text = equipmentName ?? "None Name";
			this.LogDebug( "End" );
		}

	}

}
