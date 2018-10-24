using System.Collections.Generic;
using System.Linq;
using Models;
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

		#region Model

		/// <summary>
		/// WindowModel
		/// </summary>
		private CustomizeWindowModel CustomizeWindowModel { set; get; } = new CustomizeWindowModel( CustomizeWindowModel.WindowNameEnum.None );

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
		private List<CustomizeView.ParameterChip> parameterChips { set; get; }

		#endregion

		#region View

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

		#endregion

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CustomizePresenter( CustomizeParameter parameter ) {
			Logger.Debug( "Start" );

			// Viewの設定
			this.InitialViewSetting();

			// ModelのSubscribeを設定
			this.InitialModelSubscribeSetting();

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
			this.CustomizeWindowModel.windowName.Value = CustomizeWindowModel.WindowNameEnum.EquipmentMenu;

			Logger.Debug( "End" );
		}

		#region 初期設定

		/// <summary>
		/// Viewの設定
		/// </summary>
		private void InitialViewSetting() {
			Logger.Debug( "Start" );

			// hierarchyからViewを持つGameObject取得
			GameObject customizeGameObject = GameObject.Find( "Canvas" );

			// Viewを取得
			this.CustomizeView = customizeGameObject.GetComponent<CustomizeView>();
			this.UserControllerView = GameObject.Find( "UserController" ).GetComponent<UserControllerView>();
			this.ShowcaseView = GameObject.Find( "Showcase" ).GetComponent<ShowcaseView>();

			// CutomizeViewのEventHandler設定
			this.CustomizeView.OnClickDecisionButtonEventHandler = this.ClickedDecisionButtonEvent;
			this.CustomizeView.OnClickBodyButtonEventHandler = this.ClickedBodyButtonFromMenuEvent;
			this.CustomizeView.OnClickEquipablePlaceScrollViewEventHandler = this.ClickedEquipablePlaceScrollViewEvent;

			Logger.Debug( "End" );
		}

		/// <summary>
		/// ModelのSubscribeを設定
		/// </summary>
		private void InitialModelSubscribeSetting() {
			Logger.Debug( "Start" );
			this.UserControllerView.TurnCharacter.Subscribe( value => { this.ChangedTurnCharacter( value ); } );
			this.UserControllerView.MenuButtons[ "Cancel" ].Subscribe( ( value ) => { this.ChangedCancelButton( value ); } );
			this.UserControllerView.MenuButtons[ "CursorLeft" ].Subscribe( ( value ) => { this.ChangedCursorLeftButton( value ); } );
			this.UserControllerView.MenuButtons[ "CursorRight" ].Subscribe( ( value ) => { this.ChangedCursorRightButton( value ); } );
			this.CustomizeWindowModel.windowName.Subscribe( ( windowName ) => this.ChangedWindowName( windowName ) );
			Logger.Debug( "End" );
		}

		#endregion

		#region ModelのSubscribeによるイベント

		/// <summary>
		/// Window名変更時イベント
		/// </summary>
		/// <param name="windowName">Window名</param>
		private void ChangedWindowName( CustomizeWindowModel.WindowNameEnum windowName ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Window Name is {windowName}." );
			switch( windowName ) {
				case CustomizeWindowModel.WindowNameEnum.EquipmentMenu:
					this.CustomizeView.ShowEquipmentMenu();
					break;
				case CustomizeWindowModel.WindowNameEnum.Body:
					this.CustomizeView.ShowEquipmentBodies();
					this.CustomizeView.SetSelectedBody( null );
					break;
				case CustomizeWindowModel.WindowNameEnum.Equipments:
					this.CustomizeView.ShowEquipments();
					this.CustomizeView.SetSelectedEquipmentGameObject();
					break;
				case CustomizeWindowModel.WindowNameEnum.ParameterMenu:
					this.CustomizeView.ShowCustomParameter();
					break;
				default:
					Logger.Warning( "Unexpected Window Name" );
					break;
			}
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
			this.CustomizeWindowModel.windowName.Value = CustomizeWindowModel.WindowNameEnum.ParameterMenu;
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 十字右ボタン押下時イベント
		/// </summary>
		/// <param name="value">値</param>
		private void ChangedCursorRightButton( int value ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Value is {value}." );
			this.CustomizeWindowModel.windowName.Value = CustomizeWindowModel.WindowNameEnum.EquipmentMenu;
			Logger.Debug( "End" );
		}

		#endregion

		#region Viewイベント

		/// <summary>
		/// 決定ボタン押下時イベント
		/// </summary>
		public void ClickedDecisionButtonEvent() {
			Logger.Debug( "Start" );
			SceneManager.GetInstance().LoadScene( "MainGame" , null );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// メニューより素体ボタン押下時イベント
		/// </summary>
		public void ClickedBodyButtonFromMenuEvent() {
			Logger.Debug( "Start" );
			this.CustomizeWindowModel.windowName.Value = CustomizeWindowModel.WindowNameEnum.Body;
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
			string bodyName = "None";
			if( selectedBody == null ) {
				Logger.Warning( "Selected Body is Null" );
			}
			bodyName = selectedBody.Name;

			// 選ばれた素体に装備できる一覧を設定する
			this.CustomizeView.SetEquipablePlaces( selectedBody.EqupablePlaces );

			// 素体ボタンの名前変更
			this.CustomizeView.SetBodyButtonText( bodyName );

			// 表示切替
			this.CustomizeWindowModel.windowName.Value = CustomizeWindowModel.WindowNameEnum.EquipmentMenu;

			// 素体ボタンを選択状態にする
			this.CustomizeView.SetSelectedGameObject( this.CustomizeView.BodyButton );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備可能箇所一覧ScrollView選択時イベント
		/// </summary>
		public void ClickedEquipablePlaceScrollViewEvent() {
			Logger.Debug( "Start" );

			// 一覧の項目を強制的に選択状態にする
			this.CustomizeView.SetSelectedEquipablePlaceGameObject();

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
			this.CustomizeWindowModel.windowName.Value = CustomizeWindowModel.WindowNameEnum.Equipments;

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
			this.CustomizeWindowModel.windowName.Value = CustomizeWindowModel.WindowNameEnum.EquipmentMenu;

			Logger.Debug( "End" );
		}

		#endregion

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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "右肘" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "左肘" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "右肩" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "左肩" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "胸部" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "腹" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "背中" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "腰" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "右足" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "左足" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "右膝" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 0 ,
							Name = "左膝" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 1 ,
							Name = "右腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
						} ,
						new CustomizeView.EquipablePlace() {
							Id = 2 ,
							Name = "左腕" ,
							OnClickDecisionEventHandler = () => ClickedEquipablePlaceNodeDecisionButtonEvent(0)
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
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 1 ,
					Name = "リッター",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー",
					OnClickDecisionEventHandler = () => ClickedEquipmentNodeDecisionButtonEvent(0)
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

	}

}
