using System.Collections.Generic;
using System.Linq;
using Models.Charactor;
using Models.Customize;
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
		private CustomizeWindowModel CustomizeWindowModel { set; get; } = new CustomizeWindowModel( WindowNameEnum.None );

		/// <summary>
		/// 持っている素体一覧
		/// </summary>
		private List<BodyModel> HaveBodies { set; get; } = new List<BodyModel>();

		/// <summary>
		/// 装備一覧
		/// </summary>
		private List<EquipmentModel> HaveEquipments { set; get; } = new List<EquipmentModel>();

		/// <summary>
		/// 作成したキャラクター
		/// </summary>
		private BodyModel CreatedCharacter = new BodyModel();

		/// <summary>
		/// パラメータチップ一覧
		/// </summary>
		/// TODO Viewの形で持つの嫌だ
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
			this.HaveBodies = this.GetHaveBodies();
			this.CustomizeView.SetBodies( this.ConvertBodies( this.HaveBodies ) );

			// 装備一覧取得
			this.HaveEquipments = this.GetHaveEquipments();
			this.CustomizeView.SetEquipments( this.ConvertEquipments( this.HaveEquipments ) );

			// パラメータチップ一覧取得
			// TODO 実際は型が違うから変換が必要
			this.parameterChips = this.GetAcquiredParameterChip();
			this.CustomizeView.SetParameterChips( this.parameterChips );

			// 初期表示
			this.CustomizeWindowModel.windowName.Value = WindowNameEnum.EquipmentMenu;

			Logger.Debug( "End" );
		}

		#region 変換

		/// <summary>
		/// 素体一覧をModelからViewに持たせる形に変換する
		/// </summary>
		/// <param name="bodies">素体Model一覧</param>
		/// <returns>素体一覧</returns>
		private List<CustomizeView.Body> ConvertBodies( List<BodyModel> bodies ) {
			Logger.Debug( "Start" );
			List<CustomizeView.Body> list = bodies
				.Select( ( content , index ) => new { Content = content , Index = index } )
				.Select( ( element ) => new CustomizeView.Body() {
					Id = element.Content.Id.Value ?? -1 ,
					Name = element.Content.Name ,
					EquipablePlaces = element.Content.EquipablePlaces
						.Select( ( equipablePlace ) => new CustomizeView.EquipablePlace() {
							Id = equipablePlace.Id.Value ,
							Name = equipablePlace.Name ,
							OnClickDecisionEventHandler = () => {
								this.ClickedEquipablePlaceNodeDecisionButtonEvent( equipablePlace.Id.Value );
							}
						} )
						.ToList() ,
					OnClickDecisionEventHandler = () => {
						this.ClickedBodyNodeDecisionButtonEvent( element.Content.Id.Value ?? -1 );
					}
				} )
				.ToList();
			Logger.Debug( "End" );
			return list;
		}

		/// <summary>
		/// 装備一覧をModelからViewに持たせる形に変換する
		/// </summary>
		/// <param name="equipments">装備Model一覧</param>
		/// <returns>装備一覧</returns>
		private List<CustomizeView.Equipment> ConvertEquipments( List<EquipmentModel> equipments ) {
			Logger.Debug( "Start" );
			List<CustomizeView.Equipment> list = equipments
				.Select( ( content , index ) => new { Content = content , Index = index } )
				.Select( ( element ) => new CustomizeView.Equipment(){
					Id = element.Content.Id.Value.Value ,
					Name = element.Content.Name ,
					OnClickDecisionEventHandler = () => {
						this.ClickedEquipmentNodeDecisionButtonEvent( element.Content.Id.Value.Value );
					}
				} )
				.ToList();
			Logger.Debug( "End" );
			return list;
		}

		/// <summary>
		/// 装備可能箇所一覧をModelからViewに持たせる形に変換する
		/// </summary>
		/// <param name="equipments">装備可能箇所Model一覧</param>
		/// <returns>装備可能一覧</returns>
		private List<CustomizeView.EquipablePlace> ConvertEquipablePlaces( List<EquipablePlaceModel> equipablePlaeces ) {
			Logger.Debug( "Start" );
			List<CustomizeView.EquipablePlace> list = equipablePlaeces
				.Select( ( content , index ) => new { Content = content , Index = index } )
				.Select( ( element ) => new CustomizeView.EquipablePlace() {
					Id = element.Content.Id.Value ,
					Name = element.Content.Name ,
					OnClickDecisionEventHandler = () => {
						this.ClickedEquipablePlaceNodeDecisionButtonEvent( element.Content.Id.Value );
					}
				} )
				.ToList();
			Logger.Debug( "End" );
			return list;
		}

		#endregion

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
		private void ChangedWindowName( WindowNameEnum windowName ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Window Name is {windowName}." );
			switch( windowName ) {
				case WindowNameEnum.EquipmentMenu:
					this.CustomizeView.ShowEquipmentMenu();
					break;
				case WindowNameEnum.Body:
					this.CustomizeView.ShowEquipmentBodies();
					this.CustomizeView.SetSelectedBody( this.CreatedCharacter.Id.Value );
					break;
				case WindowNameEnum.Equipments:
					this.CustomizeView.ShowEquipments();
					this.CustomizeView.SetSelectedEquipmentGameObject();
					break;
				case WindowNameEnum.ParameterMenu:
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
		/// <param name="value">値</param>
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
			this.CustomizeWindowModel.windowName.Value = WindowNameEnum.ParameterMenu;
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 十字右ボタン押下時イベント
		/// </summary>
		/// <param name="value">値</param>
		private void ChangedCursorRightButton( int value ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Value is {value}." );
			this.CustomizeWindowModel.windowName.Value = WindowNameEnum.EquipmentMenu;
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
			this.CustomizeWindowModel.windowName.Value = WindowNameEnum.Body;
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
			this.CreatedCharacter = this.HaveBodies.FirstOrDefault( body => bodyId == body.Id.Value );
			string bodyName = "None";
			if( this.CreatedCharacter == null ) {
				Logger.Warning( "Created Character is Null" );
			}
			bodyName = this.CreatedCharacter.Name;

			// 選ばれた素体に装備できる一覧を設定する
			this.CustomizeView.SetEquipablePlaces( this.ConvertEquipablePlaces( this.CreatedCharacter.EquipablePlaces ) );

			// 素体ボタンの名前変更
			this.CustomizeView.SetBodyButtonText( bodyName );

			// 表示切替
			this.CustomizeWindowModel.windowName.Value = WindowNameEnum.EquipmentMenu;

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

			// 装備可能箇所IDから所持装備のうち、装備できるものだけリストにする
			this.CustomizeView.SetEquipments( 
				this.ConvertEquipments( 
					this.HaveEquipments
						.Where( ( equipment ) => equipment.EquipablePlaceIds
							.Any( id => equipablePlaceId == id ) 
						).ToList()
				) 
			);

			// 表示切替
			this.CustomizeWindowModel.windowName.Value = WindowNameEnum.Equipments;

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
			this.CustomizeWindowModel.windowName.Value = WindowNameEnum.EquipmentMenu;

			Logger.Debug( "End" );
		}

		#endregion

		/// <summary>
		/// 取得済み素体一覧取得
		/// </summary>
		/// <returns>取得済み素体一覧</returns>
		/// TODO サーバ等から取得する　返り値もレスポンスのデータそのまま
		private List<BodyModel> GetHaveBodies() {
			Logger.Debug( "Start" );
			List<BodyModel> list = new List<BodyModel>() {
				new BodyModel(){
					Id = new ReactiveProperty<int?>(0) ,
					Name = "A" ,
					EquipablePlaces = new List<EquipablePlaceModel>() {
						new EquipablePlaceModel() {
							Id = new ReactiveProperty<int>(0) ,
							Name = "頭" ,
							EquipmentModel = new EquipmentModel() {
								Id = null
							}
						} ,
						new EquipablePlaceModel() {
							Id = new ReactiveProperty<int>(1) ,
							Name = "右腕" ,
							EquipmentModel = new EquipmentModel() {
								Id = null
							}
						} ,
						new EquipablePlaceModel() {
							Id = new ReactiveProperty<int>(2) ,
							Name = "右肩" ,
							EquipmentModel = new EquipmentModel() {
								Id = null
							}
						}
					}
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(1) ,
					Name = "B" ,
					EquipablePlaces = new List<EquipablePlaceModel>() {
						new EquipablePlaceModel() {
							Id = new ReactiveProperty<int>(0) ,
							Name = "頭" ,
							EquipmentModel = new EquipmentModel() {
								Id = null
							}
						} ,
						new EquipablePlaceModel() {
							Id = new ReactiveProperty<int>(3) ,
							Name = "左腕" ,
							EquipmentModel = new EquipmentModel() {
								Id = null
							}
						} ,
						new EquipablePlaceModel() {
							Id = new ReactiveProperty<int>(4) ,
							Name = "左肩" ,
							EquipmentModel = new EquipmentModel() {
								Id = null
							}
						}
					}
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(2) ,
					Name = "C" ,
					EquipablePlaces = new List<EquipablePlaceModel>() {
						new EquipablePlaceModel() {
							Id = new ReactiveProperty<int>(5) ,
							Name = "左足" ,
							EquipmentModel = new EquipmentModel() {
								Id = null
							}
						},
						new EquipablePlaceModel() {
							Id = new ReactiveProperty<int>(6) ,
							Name = "右足" ,
							EquipmentModel = new EquipmentModel() {
								Id = null
							}
						}
					}
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
		private List<EquipmentModel> GetHaveEquipments() {
			Logger.Debug( "Start" );
			List<EquipmentModel> list = new List<EquipmentModel>() {
				new EquipmentModel() {
					Id = new ReactiveProperty<int?>(0) ,
					Name = "ビームサーベル" ,
					EquipablePlaceIds = { 0 , 1 , 3 }
				} ,
				new EquipmentModel() {
					Id = new ReactiveProperty<int?>(1) ,
					Name = "バズーカ" ,
					EquipablePlaceIds = { 0 , 1 , 2 }
				} ,
				new EquipmentModel() {
					Id = new ReactiveProperty<int?>(2) ,
					Name = "ニードルガン" ,
					EquipablePlaceIds = { 3 , 4 , 5 , 6 }
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
