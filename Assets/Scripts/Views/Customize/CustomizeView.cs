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
		/// EquipmentNodeのPrefab
		/// </summary>
		public GameObject equipmentNodePrefab;

		/// <summary>
		/// 装備一覧スクロールのContent
		/// </summary>
		public GameObject equipmentScrollViewContent;

		#region 表示切替に使用するGameObject群

		/// <summary>
		/// パラメータカスタマイズGameObject
		/// </summary>
		private GameObject customParameterGameObject;

		/// <summary>
		/// 装備カスタマイズGameObject
		/// </summary>
		private GameObject customEquipmentGameObject;

		/// <summary>
		/// 装備メニューGameObject
		/// </summary>
		private GameObject equipmentMenuGameObject;

		/// <summary>
		/// 装備素体一覧GameObject
		/// </summary>
		private GameObject bodiesGameObject;

		/// <summary>
		/// 装備一覧GameObject
		/// </summary>
		private GameObject equipmentsGameObject;

		#endregion
		
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

			/// <summary>
			/// 決定ボタン押下時イベントハンドラ
			/// </summary>
			public Action DecisionEventHandler { set; get; }

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

		#region 決定ボタン
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
		#endregion

		#region 更新ボタン
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
		#endregion

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
			foreach( Equipment equipment in equipments ) {
				GameObject node = GameObject.Instantiate( this.equipmentNodePrefab );
				node.transform.SetParent( this.equipmentScrollViewContent.transform , false );
				EquipmentNodeView view = node.GetComponent<EquipmentNodeView>();
				view.SetOnClickDecisionButtonEventHandler( equipment.DecisionEventHandler );
			}
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

		#region 画面切り替え用Showメソッド群

		/// <summary>
		/// GameObjectのActive切り替え
		/// </summary>
		/// <param name="gameObject">GameObject</param>
		/// <param name="gameObjectName">オブジェクト名</param>
		/// <param name="active">表示／非表示</param>
		private void SetActive( ref GameObject gameObject , string gameObjectName , bool active ) {
			Logger.Debug( "Start" );

			if( gameObject == null ) {
				Logger.Debug( $"{gameObjectName} is Null" );
				gameObject = GameObject.Find( gameObjectName );
			}
			gameObject.SetActive( active );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// Equipment内のGameObjectのActive切り替え
		/// </summary>
		/// <param name="gameObject">GameObject</param>
		/// <param name="gameObjectName">オブジェクト名</param>
		/// <param name="active">表示／非表示</param>
		private void SetActiveOfEquipment( ref GameObject gameObject , string gameObjectName , bool active ) {
			Logger.Debug( "Start" );

			this.SetActive( ref this.customEquipmentGameObject , "Equipment" , true );
			
			if( gameObject == null ) {
				Logger.Debug( $"{gameObjectName} is Null" );
				gameObject = this.customEquipmentGameObject.transform.Find( gameObjectName ).gameObject;
			}
			gameObject.SetActive( active );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備カスタマイズ表示
		/// </summary>
		public void ShowCustomEquipment() {
			Logger.Debug( "Start" );

			this.SetActive( ref this.customEquipmentGameObject , "Equipment" , true );
			this.SetActive( ref this.customParameterGameObject , "Parameter" , false );

			this.SetActiveOfEquipment( ref this.equipmentMenuGameObject , "Menu" , true );
			this.SetActiveOfEquipment( ref this.bodiesGameObject , "Bodies" , false );
			this.SetActiveOfEquipment( ref this.equipmentsGameObject , "Equipments" , false );
			
			Logger.Debug( "End" );
		}

		/// <summary>
		/// パラメータカスタマイズ表示
		/// </summary>
		public void ShowCustomParameter() {
			Logger.Debug( "Start" );

			this.SetActive( ref this.customParameterGameObject , "Parameter" , true );
			this.SetActive( ref this.customEquipmentGameObject , "Equipment" , false );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備素体一覧表示
		/// </summary>
		public void ShowEquipmentBodies() {
			Logger.Debug( "Start" );

			this.SetActive( ref this.customEquipmentGameObject , "Equipment" , true );
			this.SetActive( ref this.customParameterGameObject , "Parameter" , false );
			
			this.SetActiveOfEquipment( ref this.bodiesGameObject , "Bodies" , true );
			this.SetActiveOfEquipment( ref this.equipmentMenuGameObject , "Menu" , false );
			this.SetActiveOfEquipment( ref this.equipmentsGameObject , "Equipments" , false );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備メニュー表示
		/// </summary>
		public void ShowEquipmentMenu() {
			Logger.Debug( "Start" );

			this.SetActive( ref this.customEquipmentGameObject , "Equipment" , true );
			this.SetActive( ref this.customParameterGameObject , "Parameter" , false );

			this.SetActiveOfEquipment( ref this.equipmentMenuGameObject , "Menu" , true );
			this.SetActiveOfEquipment( ref this.bodiesGameObject , "Bodies" , false );
			this.SetActiveOfEquipment( ref this.equipmentsGameObject , "Equipments" , false );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備一覧表示
		/// </summary>
		public void ShowEquipments() {
			Logger.Debug( "Start" );

			this.SetActive( ref this.customEquipmentGameObject , "Equipment" , true );
			this.SetActive( ref this.customParameterGameObject , "Parameter" , false );

			this.SetActiveOfEquipment( ref this.equipmentsGameObject , "Equipments" , true );
			this.SetActiveOfEquipment( ref this.equipmentMenuGameObject , "Menu" , false );
			this.SetActiveOfEquipment( ref this.bodiesGameObject , "Bodies" , false );

			Logger.Debug( "End" );

		}

		#endregion

	}

}