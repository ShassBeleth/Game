using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Customize {

	/// <summary>
	/// 装備可能箇所一覧のNodeにつくView
	/// </summary>
	public class EquipablePlaceNodeView : MonoBehaviour {
		
		/// <summary>
		/// 決定ボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickDecisionButtonEventHandler { set; get; }

		/// <summary>
		/// 決定ボタン押下時イベント
		/// </summary>
		public void OnClickDecisionButtonEvent() {
			Logger.Debug( "Start" );
			this.OnClickDecisionButtonEventHandler?.Invoke();
			Logger.Debug( "End" );
		}
		
		/// <summary>
		/// テキストの変更
		/// </summary>
		/// <param name="partName">装備可能箇所名</param>
		/// <param name="equipmentName">装備名</param>
		public void SetText( string partName , string equipmentName ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Part Name is {partName}." );
			Logger.Debug( $"Equipment Name is {equipmentName}." );
			this.transform.GetChild( 0 ).GetComponent<Text>().text = $"{partName}:{(equipmentName ?? "None")}";
			Logger.Debug( "End" );
		}

	}

}
