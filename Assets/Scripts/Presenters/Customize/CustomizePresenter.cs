using System.Collections.Generic;
using System.Linq;
using SceneManagers;
using SceneManagers.Parameters;
using UniRx;
using UnityEngine;
using Views.Customize;
using Views.UserController;

namespace Presenters.Customize {

	/// <summary>
	/// 装備カスタマイズ、パラメータカスタマイズのPresenter
	/// </summary>
	public class CustomizePresenter {

		/// <summary>
		/// 装備カスタマイズ、パラメータカスタマイズのView
		/// </summary>
		private CustomizeView CustomizeView { set; get; }

		/// <summary>
		/// キャラクターを回転させるView
		/// </summary>
		private ShowcaseView ShowcaseView { set; get; }

		/// <summary>
		/// ユーザ入力View
		/// </summary>
		private UserControllerView UserControllerView { set; get; }

		/// <summary>
		/// 素体一覧
		/// </summary>
		/// TODO Viewの形で持つのは嫌だ
		private List<CustomizeView.Body> bodies { set; get; }

		/// <summary>
		/// 装備一覧
		/// </summary>
		/// TODO Viewの形で持つのは嫌だ
		private List<CustomizeView.Equipment> equipments { set; get; }

		/// <summary>
		/// パラメータチップ一覧
		/// </summary>
		/// TODO Viewの形で持つのは嫌だ
		public List<CustomizeView.ParameterChip> parameterChips { set;get; }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CustomizePresenter( CustomizeParameter parameter ) {
			Logger.Debug( "Start" );

			// hierarchyからViewを持つGameObject取得
			GameObject customizeGameObject = GameObject.Find( "Canvas" );

			// Viewを取得
			this.CustomizeView = customizeGameObject.GetComponent<CustomizeView>();
			this.UserControllerView = GameObject.Find( "UserController" ).GetComponent<UserControllerView>();
			this.ShowcaseView = GameObject.Find( "Showcase" ).GetComponent<ShowcaseView>();

			// CutomizeViewのEventHandler設定
			this.CustomizeView.OnClickDecisionButtonEventHandler = this.ClickedDecisionButtonEvent;
			this.CustomizeView.OnClickSwitchEquipmentButtonEventHandler = this.ClickedSwitchEquipmentButtonEvent;
			this.CustomizeView.OnClickSwitchParameterButtonEventHandler = this.ClickedSwitchParameterButtonEvent;
			this.CustomizeView.OnClickUpdateButtonEventHandler = this.ClickedUpdateButtonEvent;
			this.CustomizeView.OnClickBackButtonEventHandler = this.ClickedBackButtonEvent;
			this.CustomizeView.OnClickBodyButtonEventHandler = this.ClickedBodyButtonFromMenuEvent;
			this.CustomizeView.OnClickBodyBackButtonEventHandler = this.ClickedBodyBackButtonEvent;
			
			this.UserControllerView.TurnCharacter.Subscribe( value => { this.ChangedTurnCharacter( value ); } );
			this.UserControllerView.MenuButtons[ "Cancel" ].Subscribe( ( value ) => { this.ChangedCancelButton( value ); } );
			this.UserControllerView.MenuButtons[ "CursorLeft" ].Subscribe( ( value ) => { this.ChangedCursorLeftButton( value ); } );
			this.UserControllerView.MenuButtons[ "CursorRight" ].Subscribe( ( value ) => { this.ChangedCursorRightButton( value ); } );

			// 素体一覧取得
			// TODO 実際は型が違うから変換が必要
			this.bodies = this.GetAcquiredBodies();
			this.CustomizeView.SetBodies( this.bodies );

			// 装備一覧取得
			// TODO 実際は型が違うから変換が必要
			this.equipments = this.GetAcquiredEquipments();
			this.CustomizeView.SetEqupments( this.equipments );

			// パラメータチップ一覧取得
			// TODO 実際は型が違うから変換が必要
			this.parameterChips = this.GetAcquiredParameterChip();
			this.CustomizeView.SetParameterChips( this.parameterChips );

			// 初期表示
			this.CustomizeView.ShowCustomEquipment();

			Logger.Debug( "End" );
		}

		/// <summary>
		/// キャラクター回転値が変更された時のイベント
		/// </summary>
		/// <param name="value"></param>
		private void ChangedTurnCharacter( int value ) {
			Logger.Debug( "Start" );
			if( value < -100 || 100 < value ) {
				this.ShowcaseView.IsInput = true;
				this.ShowcaseView.IncreaseAngle = value * -0.01f;
			}
			else {
				this.ShowcaseView.IncreaseAngle = 0f;
			}
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 素体選択時の戻るボタン押下時イベント
		/// </summary>
		private void ClickedBodyBackButtonEvent() {
			Logger.Debug( "Start" );
			this.CustomizeView.ShowEquipmentMenu();
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 取得済み素体一覧取得
		/// </summary>
		/// <returns>取得済み素体一覧</returns>
		/// TODO サーバ等から取得する　返り値もレスポンスのデータそのまま
		private List<CustomizeView.Body> GetAcquiredBodies() {
			Logger.Debug( "Start" );
			List<CustomizeView.Body> list = new List<CustomizeView.Body>() {
				new CustomizeView.Body(){
					Id = 0 ,
					Name = "A" ,
					EqupablePlaces = new List<CustomizeView.EquipablePlace>() {
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "頭" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						}
					} ,
					DecisionEventHandler = () => ClickedBodyNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Body() {
					Id = 1 ,
					Name = "B" ,
					EqupablePlaces = new List<CustomizeView.EquipablePlace>() {
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "頭" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						}
					} ,
					DecisionEventHandler = () => ClickedBodyNodeDecisionButtonEvent(1)
				} ,
				new CustomizeView.Body() {
					Id = 2 ,
					Name = "C" ,
					EqupablePlaces = new List<CustomizeView.EquipablePlace>() {
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "頭" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						}
					} ,
					DecisionEventHandler = () => ClickedBodyNodeDecisionButtonEvent(1)
				} ,
				new CustomizeView.Body() {
					Id = 2 ,
					Name = "C" ,
					EqupablePlaces = new List<CustomizeView.EquipablePlace>() {
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "頭" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						}
					} ,
					DecisionEventHandler = () => ClickedBodyNodeDecisionButtonEvent(2)
				} ,
				new CustomizeView.Body() {
					Id = 2 ,
					Name = "C" ,
					EqupablePlaces = new List<CustomizeView.EquipablePlace>() {
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "頭" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						}
					} ,
					DecisionEventHandler = () => ClickedBodyNodeDecisionButtonEvent(2)
				} ,
				new CustomizeView.Body() {
					Id = 2 ,
					Name = "C" ,
					EqupablePlaces = new List<CustomizeView.EquipablePlace>() {
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "頭" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						}
					} ,
					DecisionEventHandler = () => ClickedBodyNodeDecisionButtonEvent(3)
				} ,
				new CustomizeView.Body() {
					Id = 2 ,
					Name = "C" ,
					EqupablePlaces = new List<CustomizeView.EquipablePlace>() {
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "頭" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						}
					} ,
					DecisionEventHandler = () => ClickedBodyNodeDecisionButtonEvent(4)
				} ,
				new CustomizeView.Body() {
					Id = 2 ,
					Name = "C" ,
					EqupablePlaces = new List<CustomizeView.EquipablePlace>() {
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "頭" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						}
					} ,
					DecisionEventHandler = () => ClickedBodyNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Body() {
					Id = 2 ,
					Name = "C" ,
					EqupablePlaces = new List<CustomizeView.EquipablePlace>() {
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "頭" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						}
					} ,
					DecisionEventHandler = () => ClickedBodyNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Body() {
					Id = 2 ,
					Name = "C" ,
					EqupablePlaces = new List<CustomizeView.EquipablePlace>() {
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "頭" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							DecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						}
					} ,
					DecisionEventHandler = () => ClickedBodyNodeDecisionButtonEvent(0)
				}
			};
			Logger.Debug( "End" );
			return list;
		}
		
		/// <summary>
		/// 取得済み装備一覧取得
		/// </summary>
		/// <returns>取得済み装備一覧</returns>
		/// TODO サーバ等から取得する　返り値もレスポンスデータのまま
		private List<CustomizeView.Equipment> GetAcquiredEquipments() {
			Logger.Debug( "Start" );
			List<CustomizeView.Equipment> list = new List<CustomizeView.Equipment>() {
				new CustomizeView.Equipment(){
					Id = 0 ,
					Name = "スシコラ",
					DecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 1 ,
					Name = "リッター",
					DecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					DecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				}
			};
			Logger.Debug( "End" );
			return list;
		}

		/// <summary>
		/// 取得済みパラメータチップ一覧取得
		/// </summary>
		/// <returns>取得済みパラメータチップ一覧</returns>
		/// TODO サーバ等から取得する　返り値もレスポンスデータのまま
		private List<CustomizeView.ParameterChip> GetAcquiredParameterChip() {
			Logger.Debug( "Start" );
			List<CustomizeView.ParameterChip> list = new List<CustomizeView.ParameterChip>() {
				new CustomizeView.ParameterChip(){
					Id = 0 ,
					Name = "メイン効率UP"
				} ,
				new CustomizeView.ParameterChip() {
					Id = 1 ,
					Name = "スペシャル増加"
				} ,
				new CustomizeView.ParameterChip() {
					Id = 2 ,
					Name = "イカニンジャ"
				}
			};
			Logger.Debug( "End" );
			return list;
		}

		/// <summary>
		/// 決定ボタン押下時イベント
		/// </summary>
		public void ClickedDecisionButtonEvent() {
			Logger.Debug( "Start" );
			SceneManager.GetInstance().LoadScene( "MainGame" , null );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備画面に切り替えるボタン押下時イベント
		/// </summary>
		public void ClickedSwitchEquipmentButtonEvent() {
			Logger.Debug( "Start" );
			this.CustomizeView.ShowCustomEquipment();
			Logger.Debug( "End" );
		}

		/// <summary>
		/// パラメータ画面に切り替えるボタン押下時イベント
		/// </summary>
		public void ClickedSwitchParameterButtonEvent() {
			Logger.Debug( "Start" );
			this.CustomizeView.ShowCustomParameter();
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 更新ボタン押下時イベント
		/// </summary>
		public void ClickedUpdateButtonEvent() {
			Logger.Debug( "Start" );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 戻るボタン押下時イベント
		/// </summary>
		public void ClickedBackButtonEvent() {
			Logger.Debug( "Start" );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// メニューより素体ボタン押下時イベント
		/// </summary>
		public void ClickedBodyButtonFromMenuEvent() {
			Logger.Debug( "Start" );
			this.CustomizeView.ShowEquipmentBodies();
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 素体選択時イベント
		/// </summary>
		/// <param name="bodyId">素体ID</param>
		public void ClickedBodyNodeDecisionButtonEvent( int bodyId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Body Id is {bodyId}" );

			// 素体IDからどの素体が選ばれたか調べる
			CustomizeView.Body selectedBody = this.bodies.FirstOrDefault( body => bodyId == body.Id );
			if( selectedBody == null ) {
				Logger.Warning( "Selected Body is Null" );
			}

			// 選ばれた素体に装備できる一覧を設定する
			this.CustomizeView.SetEquipablePlaces( selectedBody.EqupablePlaces );

			// 表示切替
			this.CustomizeView.ShowEquipmentMenu();

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備可能箇所選択時イベント
		/// </summary>
		/// <param name="equipablePlaceId">装備可能箇所ID</param>
		public void ClickedEquipablePlaceNodeDecisionButtonEvent( int equipablePlaceId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Equipable Place Id is {equipablePlaceId}" );

			// TODO 装備可能箇所IDから所持装備のうち、装備できるものだけリストにする
			this.CustomizeView.SetEqupments( this.equipments );

			// 表示切替
			this.CustomizeView.ShowEquipments();

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備選択時イベント
		/// </summary>
		/// <param name="equipmentId">装備ID</param>
		public void ClickedEquipmentNodeDecisionButtonEvent( int equipmentId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Equipment Id is {equipmentId}" );

			// 表示切替
			this.CustomizeView.ShowCustomEquipment();

			Logger.Debug( "End" );
		}

		/// <summary>
		/// キャンセルボタン押下時イベント
		/// </summary>
		/// <param name="value">値</param>
		private void ChangedCancelButton( int value ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Value is {value}." );

			Logger.Debug( "End" );
		}
		
		/// <summary>
		/// 十字左ボタン押下時イベント
		/// </summary>
		/// <param name="value">値</param>
		private void ChangedCursorLeftButton( int value ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Value is {value}." );
			this.CustomizeView.ShowCustomParameter();
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 十字右ボタン押下時イベント
		/// </summary>
		/// <param name="value">値</param>
		private void ChangedCursorRightButton( int value ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Value is {value}." );
			this.CustomizeView.ShowCustomEquipment();
			Logger.Debug( "End" );
		}

	}

}
