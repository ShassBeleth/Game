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
			/// 装備名
			/// </summary>
			public string EquipmanetName { set; get; }

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
			public List<EquipablePlace> EquipablePlaces { set; get; }

			/// <summary>
			/// 決定ボタン押下時イベントハンドラ
			/// </summary>
			public Action OnClickDecisionEventHandler { set; get; }

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


			public bool CanEquip { set; get; }

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
		
		/// <summary>
		/// 詳細
		/// </summary>
		public class BodyDetail {

			/// <summary>
			/// 名前
			/// </summary>
			public string Name { set; get; }

			/// <summary>
			/// ルビ
			/// </summary>
			public string Ruby { set; get; }

			/// <summary>
			/// フレーバーテキスト
			/// </summary>
			public string Flavor { set; get; }

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
			this.LogDebug( "Start" );
			this.OnClickBodyButtonEventHandler?.Invoke();
			this.LogDebug( "End" );
		}

		/// <summary>
		/// 素体ボタンの表示変更
		/// </summary>
		/// <param name="bodyName">素体名</param>
		public void SetBodyButtonText( string bodyName ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Body Name is {bodyName}." );
			this.BodyButton.transform.GetChild( 0 ).GetComponent<Text>().text = $"Base：{bodyName}";
			this.LogDebug( "End" );
		}

		#endregion

		/// <summary>
		/// 素体一覧の設定
		/// </summary>
		/// <param name="bodies">素体一覧</param>
		public void SetBodies( List<Body> bodies ) {
			this.LogDebug( "Start" );

			List<GameObject> bodyGameObjects = new List<GameObject>();

			// リスト内のGameObject生成
			foreach( int i in Enumerable.Range( 0 , bodies.Count ) ) {
				GameObject node = GameObject.Instantiate( this.bodyNodePrefab );
				node.transform.SetParent( this.bodyScrollViewContent.transform , false );
				bodyGameObjects.Add( node );
				BodyNodeView view = node.GetComponent<BodyNodeView>();
				view.Id = bodies[ i ].Id;
				view.SetBodyName( bodies[ i ].Name );
				view.OnClickDecisionEventHandler = bodies[ i ].OnClickDecisionEventHandler;
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
			this.LogDebug( "End" );
		}

		/// <summary>
		/// 素体を選択状態にする
		/// </summary>
		/// <param name="id">選択状態となる素体のId nullなら先頭を選択</param>
		public void SetSelectedBody( int? id ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"(Id is {(id.HasValue ? id.Value.ToString() : "Null")}." );

			GameObject selectedGameObject = null;
			// IDがあれば一覧からGameObjectを取得
			if( id.HasValue ) {
				foreach( int i in Enumerable.Range( 0 , this.bodyScrollViewContent.transform.childCount ) ) {
					BodyNodeView view = this.bodyScrollViewContent.transform.GetChild( i ).GetComponent<BodyNodeView>();
					if( view.Id == id.Value ) {
						selectedGameObject = view.gameObject;
						break;
					}
				}
				// 不正なIDだった場合は0番目を取得
				if( selectedGameObject == null ) {
					this.LogDebug( "Unexpected Id" );
					selectedGameObject = this.bodyScrollViewContent.transform.GetChild( 0 ).gameObject;
				}
			}
			// IDがなければ0番目を取得
			else {
				selectedGameObject = this.bodyScrollViewContent.transform.GetChild( 0 )?.gameObject;
			}

			if( selectedGameObject != null ) {
				this.eventSystem.SetSelectedGameObject( selectedGameObject );
			}
			this.LogDebug( "End" );
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
			this.LogDebug( "Start" );
			this.OnClickEquipablePlaceScrollViewEventHandler?.Invoke();
			this.LogDebug( "End" );
		}

		/// <summary>
		/// 装備可能箇所一覧の設定
		/// </summary>
		/// <param name="equipablePlaces">装備可能箇所一覧</param>
		public void SetEquipablePlaces( List<EquipablePlace> equipablePlaces ) {
			this.LogDebug( "Start" );

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
				view.Id = equipablePlace.Id;
				view.PartName = equipablePlace.Name;

				view.OnClickDecisionButtonEventHandler = equipablePlace.OnClickDecisionEventHandler;
				view.SetText( equipablePlace.EquipmanetName );
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
			
			this.LogDebug( "End" );
		}

		/// <summary>
		/// 装備可能箇所のテキスト更新
		/// </summary>
		/// <param name="name">装備可能箇所ID</param>
		/// <param name="EquipmentName">装備名</param>
		public void UpdateEquipablePlaceText( int id , string equipmentName ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Id is {id}." );
			this.LogDebug( $"Equipment Name is {equipmentName}." );

			foreach( Transform childTransform in this.equipablePlaceScrollViewContent.transform ) {
				EquipablePlaceNodeView view = childTransform.GetComponent<EquipablePlaceNodeView>();
				if( id == view.Id ) {
					view.SetText( equipmentName );
					break;
				}
			}

			this.LogDebug( "End" );
		}

		/// <summary>
		/// 強制的に装備可能箇所一覧内の項目を選択状態にする
		/// </summary>
		public void SetSelectedEquipablePlaceGameObject() {
			this.LogDebug( "Start" );
			if( this.equipablePlaceScrollViewContent.transform.childCount == 0 ) {
				this.LogDebug( "Equipable Place Scroll View Content Child Count is 0." );
				return;
			}
			GameObject gameObject = this.equipablePlaceScrollViewContent.transform.GetChild( 0 ).gameObject;
			this.eventSystem.SetSelectedGameObject( gameObject );
			this.LogDebug( "End" );
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
			this.LogDebug( "Start" );

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

			this.LogDebug( "End" );
		}
		
		/// <summary>
		/// 強制的に装備一覧内の項目を選択状態にする
		/// </summary>
		public void SetSelectedEquipmentGameObject() {
			this.LogDebug( "Start" );
			if( this.firstEquipmentNode != null ) {
				this.eventSystem.SetSelectedGameObject( this.firstEquipmentNode );
			}
			else {
				this.LogDebug( "First Equipment Node is Null." );
			}
			this.LogDebug( "End" );
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

		/// <summary>
		/// パラメータチップメニューGameObject
		/// </summary>
		public GameObject parameterChipMenuGameObject;

		/// <summary>
		/// 空きマスカスタマイズGameObject
		/// </summary>
		public GameObject customFreeSquaresGameObject;

		/// <summary>
		/// パラメータチップカスタマイズGameObject
		/// </summary>
		public GameObject customParameterChipsGameObject;

		#region 画面切り替え用Showメソッド群

		/// <summary>
		/// 装備素体一覧表示
		/// </summary>
		public void ShowEquipmentBodies() {
			this.LogDebug( "Start" );

			this.customEquipmentGameObject.SetActive( true );
			this.customParameterGameObject.SetActive( false );

			this.bodiesGameObject.SetActive( true );
			this.equipmentMenuGameObject.SetActive( false );
			this.equipmentsGameObject.SetActive( false );

			this.LogDebug( "End" );
		}

		/// <summary>
		/// 装備メニュー表示
		/// </summary>
		public void ShowEquipmentMenu() {
			this.LogDebug( "Start" );

			this.customEquipmentGameObject.SetActive( true );
			this.customParameterGameObject.SetActive( false );

			this.equipmentMenuGameObject.SetActive( true );
			this.bodiesGameObject.SetActive( false );
			this.equipmentsGameObject.SetActive( false );

			this.LogDebug( "End" );
		}

		/// <summary>
		/// 装備一覧表示
		/// </summary>
		public void ShowEquipments() {
			this.LogDebug( "Start" );

			this.customEquipmentGameObject.SetActive( true );
			this.customParameterGameObject.SetActive( false );

			this.equipmentsGameObject.SetActive( true );
			this.equipmentMenuGameObject.SetActive( false );
			this.bodiesGameObject.SetActive( false );

			this.LogDebug( "End" );
		}

		/// <summary>
		/// パラメータチップメニュー表示
		/// </summary>
		public void ShowParameterChipMenu() {
			this.LogDebug( "Start" );

			this.customParameterGameObject.SetActive( true );
			this.customEquipmentGameObject.SetActive( false );

			this.parameterChipMenuGameObject.SetActive( true );
			this.customParameterChipsGameObject.SetActive( false );
			this.customFreeSquaresGameObject.SetActive( false );

			this.LogDebug( "End" );
		}

		/// <summary>
		/// パラメータチップカスタマイズ表示
		/// </summary>
		public void ShowCustomParameterChips() {
			this.LogDebug( "Start" );

			this.customParameterGameObject.SetActive( true );
			this.customEquipmentGameObject.SetActive( false );

			this.customParameterChipsGameObject.SetActive( true );
			this.parameterChipMenuGameObject.SetActive( false );
			this.customFreeSquaresGameObject.SetActive( false );

			this.LogDebug( "End" );
		}

		/// <summary>
		/// 空きマスカスタマイズ表示
		/// </summary>
		public void ShowCustomFreeSquares() {
			this.LogDebug( "Start" );

			this.customParameterGameObject.SetActive( true );
			this.customEquipmentGameObject.SetActive( false );

			this.customFreeSquaresGameObject.SetActive( true );
			this.customParameterChipsGameObject.SetActive( false );
			this.parameterChipMenuGameObject.SetActive( false );

			this.LogDebug( "End" );
		}

		#endregion

		#endregion

		#region 装備決定ボタン
		/// <summary>
		/// 装備決定ボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickEquipmentDecisionButtonEventHandler { set; get; }

		/// <summary>
		/// 装備決定ボタン押下時イベント
		/// </summary>
		public void OnClickEquipmentDecisionButtonEvent() {
			this.LogDebug( "Start" );
			this.OnClickEquipmentDecisionButtonEventHandler?.Invoke();
			this.LogDebug( "End" );
		}
		#endregion

		#region 詳細

		/// <summary>
		/// 詳細画面GameObject
		/// </summary>
		public GameObject DetailGameObject;

		/// <summary>
		/// 素体名
		/// </summary>
		public Text BodyNameText;

		/// <summary>
		/// 素体名ルビ
		/// </summary>
		public Text BodyRubyText;

		/// <summary>
		/// 素体フレーバーテキスト
		/// </summary>
		public Text BodyFlavorText;

		/// <summary>
		/// 詳細画面のアクティブ設定
		/// </summary>
		/// <param name="isActive">アクティブかどうか</param>
		public void SetDetailActive( bool isActive ) {
			this.LogDebug( "Start" );
			this.LogDebug( $"Active is {isActive}." );
			this.DetailGameObject.SetActive( isActive );
			this.LogDebug( "End" );
		}

		/// <summary>
		/// 素体詳細設定
		/// </summary>
		/// <param name="detail">詳細情報</param>
		public void SetBodyDetail( BodyDetail detail ) {
			this.LogDebug( "Start" );

			if( detail == null ) {
				this.LogWarning( "Body Detail is Null." );
				return;
			}

			this.LogDebug( $"Name is {detail.Name}." );
			this.LogDebug( $"Ruby is {detail.Ruby}." );
			this.LogDebug( $"Flavor is {detail.Flavor}." );

			this.BodyNameText.text = detail.Name;
			this.BodyRubyText.text = detail.Ruby;
			this.BodyFlavorText.text = detail.Flavor;

			this.LogDebug( "End" );
		}

		#endregion

		#region パラメータチップのメニューについて

		#region 空きマスを変更するボタン

		/// <summary>
		/// 空きマスを変更するボタン
		/// </summary>
		public GameObject CustomFreeSquaresButton;

		/// <summary>
		/// 空きマスを変更するボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickCustomFreeSquaresButtonEventHandler;

		/// <summary>
		/// 空きマスを変更するボタン押下時イベント
		/// </summary>
		public void OnClickCustomFreeSquaresButtonEvent() {
			this.LogDebug( "Start" );
			this.OnClickCustomFreeSquaresButtonEventHandler?.Invoke();
			this.LogDebug( "End" );
		}

		#endregion

		#region パラメータカスタマイズボタン

		/// <summary>
		/// パラメータカスタマイズボタン
		/// </summary>
		public GameObject CustomParameterChipsButton;

		/// <summary>
		/// パラメータカスタマイズボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickCustomParameterChipsButtonEventHandler;

		/// <summary>
		/// パラメータカスタマイズボタン押下時イベント
		/// </summary>
		public void OnClickCustomParameterChipsButtonEvent() {
			this.LogDebug( "Start" );
			this.OnClickCustomParameterChipsButtonEventHandler?.Invoke();
			this.LogDebug( "End" );
		}

		#endregion

		#region パラメータカスタマイズの決定ボタン

		/// <summary>
		/// パラメータカスタマイズの決定ボタン
		/// </summary>
		public GameObject ParameterDecisionButton;

		/// <summary>
		/// パラメータカスタマイズの決定ボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickParameterDecisionButtonEventHandler;

		/// <summary>
		/// パラメータカスタマイズの決定ボタン押下時イベント
		/// </summary>
		public void OnClickParameterDecisionButtonEvent() {
			this.LogDebug( "Start" );
			this.OnClickParameterDecisionButtonEventHandler?.Invoke();
			this.LogDebug( "End" );
		}

		#endregion

		#endregion

		#region 空きマス変更画面について

		#region 決定ボタンについて

		/// <summary>
		/// 空きマス変更の決定ボタン
		/// </summary>
		public GameObject FreeSquaresDecisionButton;

		/// <summary>
		/// 空きマス変更の決定ボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickFreeSquaresDecisionButtonEventHandler { set; get; }

		/// <summary>
		/// 空きマス変更の決定ボタン押下時イベント
		/// </summary>
		public void OnClickFreeSquaresDecisionButtonEvent() {
			this.LogDebug( "Start" );
			this.OnClickFreeSquaresDecisionButtonEventHandler?.Invoke();
			this.LogDebug( "End" );
		}

		#endregion

		#endregion

		#region パラメータチップ変更画面について

		#region 決定ボタンについて

		/// <summary>
		/// パラメータチップ変更の決定ボタン
		/// </summary>
		public GameObject CustomParameterChipsDecisionButton;

		/// <summary>
		/// パラメータチップ変更の決定ボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickCustomParameterChipsDecisionButtonEventHandler { set; get; }

		/// <summary>
		/// パラメータチップ変更の決定ボタン押下時イベント
		/// </summary>
		public void OnClickCustomParameterChipsDecisionButtonEvent() {
			this.LogDebug( "Start" );
			this.OnClickCustomParameterChipsDecisionButtonEventHandler?.Invoke();
			this.LogDebug( "End" );
		}

		#endregion

		#endregion

		/// <summary>
		/// 強制的にボタンを選択状態にする
		/// </summary>
		/// <param name="gameObject">GameObject</param>
		public void SetSelectedGameObject( GameObject gameObject ) {
			this.LogDebug( "Start" );
			this.eventSystem.SetSelectedGameObject( gameObject );
			this.LogDebug( "End" );
		}

		/// <summary>
		/// パラメータチップ一覧の設定
		/// </summary>
		/// <param name="parameterChips">パラメータチップ一覧</param>
		public void SetParameterChips( List<ParameterChip> parameterChips ) {
			this.LogDebug( "Start" );
			parameterChips.ForEach( ( parameterChip ) => {
				// TODO ここ何もしてない
			} );
			this.LogDebug( "End" );
		}
		
	}

}