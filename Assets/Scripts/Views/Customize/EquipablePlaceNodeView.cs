using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Customize {

	/// <summary>
	/// 装備可能箇所一覧のNodeにつくView
	/// </summary>
	public class EquipablePlaceNodeView : MonoBehaviour {

		/// <summary>
		/// ID
		/// </summary>
		public int Id { set; get; }

		/// <summary>
		/// 装備可能箇所名
		/// </summary>
		public string PartName { set; get; }

		/// <summary>
		/// 決定ボタン押下時イベントSubject
		/// </summary>
		public Subject<Unit> OnClickedDecisionButtonSubject = new Subject<Unit>();

		/// <summary>
		/// 決定ボタン押下時イベント
		/// </summary>
		public void OnClickDecisionButtonEvent() {
			this.LogDebug( "Start" );
			this.OnClickedDecisionButtonSubject.OnNext( Unit.Default );
			this.LogDebug( "End" );
		}
		
		/// <summary>
		/// テキストの変更
		/// </summary>
		/// <param name="equipmentName">装備名</param>
		public void SetText( string equipmentName ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Equipment Name is {equipmentName}." );
			this.transform.GetChild( 0 ).GetComponent<Text>().text = $"{this.PartName}:{(equipmentName ?? "None")}";
			this.LogDebug( "End" );
		}

	}

}
