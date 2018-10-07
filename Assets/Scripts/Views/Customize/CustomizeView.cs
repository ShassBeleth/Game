using System;
using System.Collections.Generic;
using UnityEngine;

namespace Views.Customize {

	/// <summary>
	/// 装備カスタマイズ、パラメータカスタマイズのView
	/// </summary>
	public class CustomizeView : MonoBehaviour {

		/// <summary>
		/// 装備可能箇所
		/// </summary>
		public class EquipablePlace {

			/// <summary>
			/// ID
			/// </summary>
			public int Id { set; get; }

			/// <summary>
			/// 装備可能箇所名
			/// </summary>
			public string Name { set; get; }

			/// <summary>
			/// 決定ボタン押下時イベントハンドラ
			/// </summary>
			public Action DecisionEventHandler { set; get; }

		}

		/// <summary>
		/// 素体
		/// </summary>
		public class Body {

			/// <summary>
			/// ID
			/// </summary>
			public int Id { set; get; }

			/// <summary>
			/// 名前
			/// </summary>
			public string Name { set; get; }

			/// <summary>
			/// 装備可能箇所一覧
			/// </summary>
			public List<EquipablePlace> EqupablePlaces { set; get; }

			/// <summary>
			/// 決定ボタン押下時イベントハンドラ
			/// </summary>
			public Action DecisionEventHandler { set; get; }

		}

		/// <summary>
		/// BodyNodeのPrefab
		/// </summary>
		public GameObject bodyNodePrefab;

		/// <summary>
		/// 素体一覧スクロールのContent
		/// </summary>
		public GameObject bodyScrollViewContent;


		/// <summary>
		/// EquipablePlaceNodeのPrefab
		/// </summary>
		public GameObject equipablePlaceNodePrefab;

		/// <summary>
		/// 装備可能箇所一覧スクロールのContent
		/// </summary>
		public GameObject equipablePlaceScrollViewContent;

		/// <summary>
		/// 装備
		/// </summary>
		public class Equipment {

			/// <summary>
			/// ID
			/// </summary>
			public int Id { set; get; }

			/// <summary>
			/// 名前
			/// </summary>
			public string Name { set; get; }

		}

		/// <summary>
		/// パラメータチップ
		/// </summary>
		public class ParameterChip {

			/// <summary>
			/// ID
			/// </summary>
			public int Id { set; get; }

			/// <summary>
			/// 名前
			/// </summary>
			public string Name { set; get; }

		}

		#region 画面切り替えボタン
		/// <summary>
		/// パラメータ画面に切り替えるボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickSwitchParameterButtonEventHandler { set; get; }

		/// <summary>
		/// パラメータ画面に切り替えるボタン押下時イベント
		/// </summary>
		public void OnClickSwitchParameterButtonEvent() {
			Logger.Debug( "Start" );
			this.OnClickSwitchParameterButtonEventHandler?.Invoke();
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備画面に切り替えるボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickSwitchEquipmentButtonEventHandler { set; get; }

		/// <summary>
		/// 装備画面に切り替えるボタン押下時イベント
		/// </summary>
		public void OnClickSwitchEquipmentButtonEvent() {
			Logger.Debug( "Start" );
			this.OnClickSwitchEquipmentButtonEventHandler?.Invoke();
			Logger.Debug( "End" );
		}
		#endregion

		#region 戻るボタン
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
		#endregion
		
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
		/// 更新ボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickUpdateButtonEventHandler { set; get; }

		/// <summary>
		/// 更新ボタン押下時イベント
		/// </summary>
		public void OnClickUpdateButtonEvent() {
			Logger.Debug( "Start" );
			this.OnClickUpdateButtonEventHandler?.Invoke();
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 素体一覧の設定
		/// </summary>
		/// <param name="bodies">素体一覧</param>
		public void SetBodies( List<Body> bodies ) {
			Logger.Debug( "Start" );
			foreach( Body body in bodies ) {
				GameObject node = GameObject.Instantiate( this.bodyNodePrefab );
				node.transform.SetParent( this.bodyScrollViewContent.transform , false );
				BodyNodeView view = node.GetComponent<BodyNodeView>();
				view.SetOnClickDecisionButtonEventHandler( body.DecisionEventHandler );
			}
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備可能箇所一覧の設定
		/// </summary>
		/// <param name="equipablePlaces">装備可能箇所一覧</param>
		public void SetEquipablePlaces( List<EquipablePlace> equipablePlaces ) {
			Logger.Debug( "Start" );
			foreach( EquipablePlace equipablePlace in equipablePlaces ) {
				GameObject node = GameObject.Instantiate( this.equipablePlaceNodePrefab );
				node.transform.SetParent( this.equipablePlaceScrollViewContent.transform , false );
				EquipablePlaceNodeView view = node.GetComponent<EquipablePlaceNodeView>();
				view.SetOnClickDecisionButtonEventHandler( equipablePlace.DecisionEventHandler );
			}
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備一覧の設定
		/// </summary>
		/// <param name="equipments">装備一覧</param>
		public void SetEqupments( List<Equipment> equipments ) {
			Logger.Debug( "Start" );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// パラメータチップ一覧の設定
		/// </summary>
		/// <param name="parameterChips">パラメータチップ一覧</param>
		public void SetParameterChips( List<ParameterChip> parameterChips ) {
			Logger.Debug( "Start" );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 素体ボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickBodyButtonEventHandler { set; get; }

		/// <summary>
		/// 素体ボタン押下時イベント
		/// </summary>
		public void OnClickBodyEvent() {
			Logger.Debug( "Start" );
			this.OnClickBodyButtonEventHandler?.Invoke();
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 素体ボタン
		/// </summary>
		public Action OnClickBodyBackButtonEventHandler { set; get; }

	}

}