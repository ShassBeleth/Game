using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Views.Customize {

	/// <summary>
	/// 装備カスタマイズ、パラメータカスタマイズのView
	/// </summary>
	public class CustomizeView : MonoBehaviour {

		/// <summary>
		/// Event System
		/// </summary>
		public EventSystem eventSystem;

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

		#region 素体関係
		/// <summary>
		/// BodyNodeのPrefab
		/// </summary>
		public GameObject bodyNodePrefab;

		/// <summary>
		/// 素体一覧スクロールのContent
		/// </summary>
		public GameObject bodyScrollViewContent;
		
		#region 素体ボタン
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
		#endregion

		/// <summary>
		/// 素体一覧の設定
		/// </summary>
		/// <param name="bodies">素体一覧</param>
		public void SetBodies( List<Body> bodies ) {
			Logger.Debug( "Start" );

			List<GameObject> bodyGameObjects = new List<GameObject>();

			// リスト内のGameObject生成
			foreach( int i in Enumerable.Range( 0 , bodies.Count ) ) {
				GameObject node = GameObject.Instantiate( this.bodyNodePrefab );
				node.transform.SetParent( this.bodyScrollViewContent.transform , false );
				bodyGameObjects.Add( node );
				BodyNodeView view = node.GetComponent<BodyNodeView>();
				view.SetText( bodies[ i ].Name );
				view.SetOnClickDecisionButtonEventHandler( bodies[ i ].DecisionEventHandler );
			}

			foreach( int i in Enumerable.Range( 0 , bodyGameObjects.Count ) ) {
				Button button = bodyGameObjects[i].GetComponent<Button>();
				Navigation nav = new Navigation {
					mode = Navigation.Mode.Explicit ,
					selectOnDown = bodyGameObjects[ ( i + 1 ) % bodyGameObjects.Count ].GetComponent<Button>() ,
					selectOnUp = bodyGameObjects[ ( i - 1 + bodyGameObjects.Count ) % bodyGameObjects.Count ].GetComponent<Button>()
				};
				button.navigation = nav;
			}
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 素体を選択状態にする
		/// </summary>
		/// <param name="index">選択状態となる素体のIndex nullなら先頭を選択</param>
		public void SetSelectedBody( int? index ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"(Index is {(index.HasValue ? index.Value.ToString() : "Null")}." );
			GameObject selectedGameObject = null;
			foreach( int i in Enumerable.Range( 0 , this.bodyScrollViewContent.transform.childCount ) ) {
				selectedGameObject
					= index.HasValue
						? this.bodyScrollViewContent.transform.GetChild( index.Value ).gameObject
						: this.bodyScrollViewContent.transform.GetChild( 0 ).gameObject;
			}
			if( selectedGameObject != null ) {
				this.eventSystem.SetSelectedGameObject( selectedGameObject );
			}
			else {
				Logger.Warning( "Selected Game Obejct is Null." );
			}
			Logger.Debug( "End" );
		}
		
		#endregion

		#region 装備可能箇所関係
		/// <summary>
		/// EquipablePlaceNodeのPrefab
		/// </summary>
		public GameObject equipablePlaceNodePrefab;

		/// <summary>
		/// 装備可能箇所一覧スクロールのContent
		/// </summary>
		public GameObject equipablePlaceScrollViewContent;

		/// <summary>
		/// 装備可能箇所一覧の設定
		/// </summary>
		/// <param name="equipablePlaces">装備可能箇所一覧</param>
		public void SetEquipablePlaces( List<EquipablePlace> equipablePlaces ) {
			Logger.Debug( "Start" );
			equipablePlaces.ForEach( ( equipablePlace ) => {
				GameObject node = GameObject.Instantiate( this.equipablePlaceNodePrefab );
				node.transform.SetParent( this.equipablePlaceScrollViewContent.transform , false );
				EquipablePlaceNodeView view = node.GetComponent<EquipablePlaceNodeView>();
				view.SetOnClickDecisionButtonEventHandler( equipablePlace.DecisionEventHandler );
			} );
			Logger.Debug( "End" );
		}

		#endregion

		#region 装備関係
		/// <summary>
		/// EquipmentNodeのPrefab
		/// </summary>
		public GameObject equipmentNodePrefab;

		/// <summary>
		/// 装備一覧スクロールのContent
		/// </summary>
		public GameObject equipmentScrollViewContent;

		/// <summary>
		/// 装備一覧の設定
		/// </summary>
		/// <param name="equipments">装備一覧</param>
		public void SetEqupments( List<Equipment> equipments ) {
			Logger.Debug( "Start" );
			equipments.ForEach( ( equipment ) => {
				GameObject node = GameObject.Instantiate( this.equipmentNodePrefab );
				node.transform.SetParent( this.equipmentScrollViewContent.transform , false );
				EquipmentNodeView view = node.GetComponent<EquipmentNodeView>();
				view.SetOnClickDecisionButtonEventHandler( equipment.DecisionEventHandler );
			} );
			Logger.Debug( "End" );
		}
		#endregion

		#region 表示切替

		/// <summary>
		/// パラメータカスタマイズGameObject
		/// </summary>
		public GameObject customParameterGameObject;

		/// <summary>
		/// 装備カスタマイズGameObject
		/// </summary>
		public GameObject customEquipmentGameObject;

		/// <summary>
		/// 装備メニューGameObject
		/// </summary>
		public GameObject equipmentMenuGameObject;

		/// <summary>
		/// 装備素体一覧GameObject
		/// </summary>
		public GameObject bodiesGameObject;

		/// <summary>
		/// 装備一覧GameObject
		/// </summary>
		public GameObject equipmentsGameObject;

		#region 画面切り替え用Showメソッド群

		/// <summary>
		/// 装備カスタマイズ表示
		/// </summary>
		public void ShowCustomEquipment() {
			Logger.Debug( "Start" );

			this.customEquipmentGameObject.SetActive( true );
			this.customParameterGameObject.SetActive( false );

			this.equipmentMenuGameObject.SetActive( true );
			this.bodiesGameObject.SetActive( false );
			this.equipmentsGameObject.SetActive( false );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// パラメータカスタマイズ表示
		/// </summary>
		public void ShowCustomParameter() {
			Logger.Debug( "Start" );

			this.customParameterGameObject.SetActive( true );
			this.customEquipmentGameObject.SetActive( false );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備素体一覧表示
		/// </summary>
		public void ShowEquipmentBodies() {
			Logger.Debug( "Start" );

			this.customEquipmentGameObject.SetActive( true );
			this.customParameterGameObject.SetActive( false );

			this.bodiesGameObject.SetActive( true );
			this.equipmentMenuGameObject.SetActive( false );
			this.equipmentsGameObject.SetActive( false );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備メニュー表示
		/// </summary>
		public void ShowEquipmentMenu() {
			Logger.Debug( "Start" );

			this.customEquipmentGameObject.SetActive( true );
			this.customParameterGameObject.SetActive( false );

			this.equipmentMenuGameObject.SetActive( true );
			this.bodiesGameObject.SetActive( false );
			this.equipmentsGameObject.SetActive( false );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備一覧表示
		/// </summary>
		public void ShowEquipments() {
			Logger.Debug( "Start" );

			this.customEquipmentGameObject.SetActive( true );
			this.customParameterGameObject.SetActive( false );

			this.equipmentsGameObject.SetActive( true );
			this.equipmentMenuGameObject.SetActive( false );
			this.bodiesGameObject.SetActive( false );

			Logger.Debug( "End" );
		}

		#endregion
		
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
		
		/// <summary>
		/// パラメータチップ一覧の設定
		/// </summary>
		/// <param name="parameterChips">パラメータチップ一覧</param>
		public void SetParameterChips( List<ParameterChip> parameterChips ) {
			Logger.Debug( "Start" );
			parameterChips.ForEach( ( parameterChip ) => {
				// TODO ここ何もしてない
			} );
			Logger.Debug( "End" );
		}
		
	}

}