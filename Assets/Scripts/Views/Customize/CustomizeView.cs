using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
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
			public Action OnClickDecisionEventHandler { set; get; }

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
			public Action OnClickDecisionEventHandler { set; get; }

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
		/// 素体ボタン
		/// </summary>
		public GameObject BodyButton;

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
		/// 素体ボタンの表示変更
		/// </summary>
		/// <param name="bodyName">素体名</param>
		public void SetBodyButtonText( string bodyName ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Body Name is {bodyName}." );
			this.BodyButton.transform.GetChild( 0 ).GetComponent<Text>().text = $"Base：{bodyName}";
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
				view.SetBodyName( bodies[ i ].Name );
				view.OnClickDecisionEventHandler = bodies[ i ].DecisionEventHandler;
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
		/// 装備可能箇所GameObject
		/// </summary>
		public GameObject EquipablePlaceGameObject;

		/// <summary>
		/// EquipablePlaceNodeのPrefab
		/// </summary>
		public GameObject equipablePlaceNodePrefab;

		/// <summary>
		/// 装備可能箇所一覧スクロールのContent
		/// </summary>
		public GameObject equipablePlaceScrollViewContent;

		/// <summary>
		/// 装備可能箇所一覧Scroll View選択時イベントハンドラ
		/// </summary>
		public Action OnClickEquipablePlaceScrollViewEventHandler { set; get; }

		/// <summary>
		/// 装備可能箇所一覧Scroll View選択時イベント
		/// </summary>
		public void OnClickEquipmentScrollViewEvent() {
			Logger.Debug( "Start" );
			this.OnClickEquipablePlaceScrollViewEventHandler?.Invoke();
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備可能箇所一覧の設定
		/// </summary>
		/// <param name="equipablePlaces">装備可能箇所一覧</param>
		public void SetEquipablePlaces( List<EquipablePlace> equipablePlaces ) {
			Logger.Debug( "Start" );

			// 既に子要素として含まれる一覧項目を削除
			foreach( Transform child in this.equipablePlaceScrollViewContent.transform ) {
				GameObject.Destroy( child.gameObject );
			}

			List<GameObject> equipablePlaceGameObjects = new List<GameObject>();

			// 一覧から子要素生成
			equipablePlaces.ForEach( ( equipablePlace ) => {
				GameObject node = GameObject.Instantiate( this.equipablePlaceNodePrefab );
				node.transform.SetParent( this.equipablePlaceScrollViewContent.transform , false );
				equipablePlaceGameObjects.Add( node );
				EquipablePlaceNodeView view = node.GetComponent<EquipablePlaceNodeView>();
				view.OnClickDecisionButtonEventHandler = equipablePlace.OnClickDecisionEventHandler;
				view.SetText( equipablePlace.Name , null );
			} );

			foreach( int i in Enumerable.Range( 0 , equipablePlaceGameObjects.Count ) ) {
				Button button = equipablePlaceGameObjects[ i ].GetComponent<Button>();
				Navigation nav = new Navigation {
					mode = Navigation.Mode.Explicit ,
					selectOnDown = equipablePlaceGameObjects[ ( i + 1 ) % equipablePlaceGameObjects.Count ].GetComponent<Button>() ,
					selectOnUp = equipablePlaceGameObjects[ ( i - 1 + equipablePlaceGameObjects.Count ) % equipablePlaceGameObjects.Count ].GetComponent<Button>()
				};
				button.navigation = nav;
			}
			
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 強制的に装備可能箇所一覧内の項目を選択状態にする
		/// </summary>
		public void SetSelectedEquipablePlaceGameObject() {
			Logger.Debug( "Start" );
			GameObject gameObject = this.equipablePlaceScrollViewContent.transform.GetChild( 0 ).gameObject;
			this.eventSystem.SetSelectedGameObject( gameObject );
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
		/// 最初の装備項目
		/// </summary>
		private GameObject firstEquipmentNode = null;

		/// <summary>
		/// 装備一覧の設定
		/// </summary>
		/// <param name="equipments">装備一覧</param>
		public void SetEquipments( List<Equipment> equipments ) {
			Logger.Debug( "Start" );

			// 既に子要素として含まれる一覧項目を削除
			foreach( Transform child in this.equipmentScrollViewContent.transform ) {
				GameObject.Destroy( child.gameObject );
			}

			List<GameObject> equipmentGameObjects = new List<GameObject>();

			// 一覧から子要素生成
			equipments.ForEach( ( equipment ) => {
				GameObject node = GameObject.Instantiate( this.equipmentNodePrefab );
				node.transform.SetParent( this.equipmentScrollViewContent.transform , false );
				equipmentGameObjects.Add( node );
				EquipmentNodeView view = node.GetComponent<EquipmentNodeView>();
				view.OnClickDecisionEventHandler = equipment.OnClickDecisionEventHandler;
				view.SetEquipmentName( equipment.Name );
			} );

			this.firstEquipmentNode = equipmentGameObjects.Count != 0 ? equipmentGameObjects[ 0 ] : null;

			foreach( int i in Enumerable.Range( 0 , equipmentGameObjects.Count ) ) {
				Button button = equipmentGameObjects[ i ].GetComponent<Button>();
				Navigation nav = new Navigation {
					mode = Navigation.Mode.Explicit ,
					selectOnDown = equipmentGameObjects[ ( i + 1 ) % equipmentGameObjects.Count ].GetComponent<Button>() ,
					selectOnUp = equipmentGameObjects[ ( i - 1 + equipmentGameObjects.Count ) % equipmentGameObjects.Count ].GetComponent<Button>()
				};
				button.navigation = nav;
			}

			Logger.Debug( "End" );
		}
		
		/// <summary>
		/// 強制的に装備一覧内の項目を選択状態にする
		/// </summary>
		public void SetSelectedEquipmentGameObject() {
			Logger.Debug( "Start" );
			if( this.firstEquipmentNode != null ) {
				this.eventSystem.SetSelectedGameObject( this.firstEquipmentNode );
			}
			else {
				Logger.Debug( "First Equipment Node is Null." );
			}
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
		/// 強制的にボタンを選択状態にする
		/// </summary>
		/// <param name="gameObject">GameObject</param>
		public void SetSelectedGameObject( GameObject gameObject ) {
			Logger.Debug( "Start" );
			this.eventSystem.SetSelectedGameObject( gameObject );
			Logger.Debug( "End" );
		}

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