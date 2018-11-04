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
		private CustomizeWindowModel CustomizeWindowModel { set; get; } = new CustomizeWindowModel( SelectableNameEnum.None );

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
		private ReactiveProperty<BodyModel> CreatedCharacterModel = new ReactiveProperty<BodyModel>( new BodyModel() );

		/// <summary>
		/// 選択された装備可能箇所ID
		/// </summary>
		private ReactiveProperty<int?> SelectedEquipablePlace = new ReactiveProperty<int?>( null );

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
			this.CustomizeWindowModel.SelectableName.Value = SelectableNameEnum.EquipmentMenu;

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
					EquipmanetName = element.Content.EquipmentModel?.Name ,
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
			this.UserControllerView.MenuButtons[ "Detail" ].Subscribe( ( value ) => this.ChangedDetailButton( value ) );
			this.CustomizeWindowModel.SelectableName.Subscribe( ( selectableName ) => this.ChangedSelectableName( selectableName ) );
			this.CreatedCharacterModel.Subscribe( ( bodyModel ) => this.ChangedBodyModel( bodyModel ) );
			this.SelectedEquipablePlace.Subscribe( id => this.ChangedSelectedEquipablePlace( id ) );
			this.CustomizeWindowModel.IsShownDetail.Subscribe( ( isShownDetail ) => this.ChangedShownDetail( isShownDetail ) );
			Logger.Debug( "End" );
		}

		#endregion

		#region ModelのSubscribeによるイベント

		/// <summary>
		/// 選択状態変更時イベント
		/// </summary>
		/// <param name="selectableName">選択状態</param>
		private void ChangedSelectableName( SelectableNameEnum selectableName ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Selectable Name is {selectableName}." );
			Logger.Debug( $"Before Selectable Name is {this.CustomizeWindowModel.BeforeSelectableName}." );
			switch( selectableName ) {
				case SelectableNameEnum.None:
					break;

				case SelectableNameEnum.EquipmentMenu:
					// 遷移元の選択状態を調べる
					switch( this.CustomizeWindowModel.BeforeSelectableName ) {
						case SelectableNameEnum.None:
							this.CustomizeView.ShowEquipmentMenu();
							this.CustomizeView.SetSelectedGameObject( this.CustomizeView.BodyButton );
							break;
						case SelectableNameEnum.Body:
							this.CustomizeView.ShowEquipmentMenu();
							this.CustomizeView.SetSelectedGameObject( this.CustomizeView.BodyButton );
							break;
						case SelectableNameEnum.EquipablePlaceScrollView:
							this.CustomizeView.SetSelectedGameObject( this.CustomizeView.EquipablePlaceGameObject );
							break;
						case SelectableNameEnum.ParameterMenu:
							this.CustomizeView.ShowEquipmentMenu();
							this.CustomizeView.SetSelectedGameObject( this.CustomizeView.BodyButton );
							break;
					}
					break;

				case SelectableNameEnum.EquipablePlaceScrollView:

					this.SelectedEquipablePlace.Value = null;

					// 遷移元の選択状態を調べる
					switch( this.CustomizeWindowModel.BeforeSelectableName ) {
						case SelectableNameEnum.EquipmentMenu:
							this.CustomizeView.SetSelectedEquipablePlaceGameObject();
							break;
						case SelectableNameEnum.Equipments:
							this.CustomizeView.ShowEquipmentMenu();
							this.CustomizeView.SetSelectedEquipablePlaceGameObject();
							break;
						default:
							Logger.Warning( "Unexpected Window Name" );
							break;
					}
					break;

				case SelectableNameEnum.Body:
					this.CustomizeView.ShowEquipmentBodies();
					this.CustomizeView.SetSelectedBody( this.CreatedCharacterModel.Value.Id.Value );
					break;

				case SelectableNameEnum.Equipments:
					this.CustomizeView.ShowEquipments();
					this.CustomizeView.SetSelectedEquipmentGameObject();
					break;

				case SelectableNameEnum.ParameterMenu:
					this.CustomizeView.ShowCustomParameter();
					break;

				default:
					Logger.Warning( "Unexpected Window Name" );
					break;

			}

			// 遷移元状態として保持
			this.CustomizeWindowModel.BeforeSelectableName = selectableName;

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

			if( value != 1 ) {
				return;
			}

			switch( this.CustomizeWindowModel.SelectableName.Value ) {
				case SelectableNameEnum.None:
					break;
				case SelectableNameEnum.Body:
					this.CustomizeWindowModel.SelectableName.Value = SelectableNameEnum.EquipmentMenu;
					break;
				case SelectableNameEnum.EquipmentMenu:
					break;
				case SelectableNameEnum.EquipablePlaceScrollView:
					this.CustomizeWindowModel.SelectableName.Value = SelectableNameEnum.EquipmentMenu;
					break;
				case SelectableNameEnum.Equipments:
					this.CustomizeWindowModel.SelectableName.Value = SelectableNameEnum.EquipablePlaceScrollView;
					break;
				case SelectableNameEnum.ParameterMenu:
					this.CustomizeWindowModel.SelectableName.Value = SelectableNameEnum.EquipmentMenu;
					break;
			}

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 素体変更時イベント
		/// </summary>
		/// <param name="body">素体</param>
		private void ChangedBodyModel( BodyModel body ) {
			Logger.Debug( "Start" );
			string bodyName = "None";
			if( body != null && body.Id.Value.HasValue ) {
				bodyName = body.Name;
			}
			Logger.Debug( $"Body Name is {bodyName}." );

			// 選ばれた素体に装備できる一覧を設定する
			this.CustomizeView.SetEquipablePlaces( this.ConvertEquipablePlaces( this.CreatedCharacterModel.Value.EquipablePlaces ) );

			// 素体ボタンの名前変更
			this.CustomizeView.SetBodyButtonText( bodyName );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備可能箇所選択時イベント
		/// </summary>
		/// <param name="id">装備可能箇所ID</param>
		private void ChangedSelectedEquipablePlace( int? id ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Id is {(id.HasValue ? id.Value.ToString() : "Null")}" );

			if( id.HasValue ) {

				// 装備可能箇所IDから所持装備のうち、装備できるものだけリストにする
				this.CustomizeView.SetEquipments(
					this.ConvertEquipments(
						this.HaveEquipments
							.Where( ( equipment ) => equipment.EquipablePlaceIds
								.Any( equipablePlaceId => equipablePlaceId == id )
							).ToList()
					)
				);
				
			}

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 詳細ボタン押下時イベント
		/// </summary>
		/// <param name="value">値</param>
		private void ChangedDetailButton( int value ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Value is {value}." );
			
			if( value != 1 ) {
				return;
			}
			
			this.CustomizeWindowModel.IsShownDetail.Value = !this.CustomizeWindowModel.IsShownDetail.Value;

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 詳細窓の表示非表示
		/// </summary>
		/// <param name="isShownDetail"></param>
		private void ChangedShownDetail( bool isShownDetail ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Shown Detail is {isShownDetail}." );
			this.CustomizeView.SetDetailActive( isShownDetail );
			Logger.Debug( "End" );
		}
		
		#endregion

		#region Viewイベント

		/// <summary>
		/// 決定ボタン押下時イベント
		/// </summary>
		public void ClickedDecisionButtonEvent() {
			Logger.Debug( "Start" );
			this.CustomizeWindowModel.SelectableName.Value = SelectableNameEnum.ParameterMenu;
			Logger.Debug( "End" );
		}

		/// <summary>
		/// メニューより素体ボタン押下時イベント
		/// </summary>
		public void ClickedBodyButtonFromMenuEvent() {
			Logger.Debug( "Start" );
			// ウィンドウ表示切替
			this.CustomizeWindowModel.SelectableName.Value = SelectableNameEnum.Body;
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
			this.CreatedCharacterModel.Value = this.HaveBodies.FirstOrDefault( body => body.Id.HasValue && bodyId == body.Id.Value );

			// ウィンドウ表示切替
			this.CustomizeWindowModel.SelectableName.Value = SelectableNameEnum.EquipmentMenu;
			
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備可能箇所一覧ScrollView選択時イベント
		/// </summary>
		public void ClickedEquipablePlaceScrollViewEvent() {
			Logger.Debug( "Start" );

			// 一覧選択状態変更
			this.CustomizeWindowModel.SelectableName.Value = SelectableNameEnum.EquipablePlaceScrollView;

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備可能箇所選択時イベント
		/// </summary>
		/// <param name="equipablePlaceId">装備可能箇所ID</param>
		public void ClickedEquipablePlaceNodeDecisionButtonEvent( int equipablePlaceId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Equipable Place Id is {equipablePlaceId}" );

			// 選択中の装備可能箇所IDを保持
			this.SelectedEquipablePlace.Value = equipablePlaceId;

			// 表示切替
			this.CustomizeWindowModel.SelectableName.Value = SelectableNameEnum.Equipments;

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備選択時イベント
		/// </summary>
		/// <param name="equipmentId">装備ID</param>
		public void ClickedEquipmentNodeDecisionButtonEvent( int equipmentId ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Equipment Id is {equipmentId}" );

			int selectedEquipablePlaceId = this.SelectedEquipablePlace.Value.Value;
			Logger.Debug( $"Selected Equipable Place Id is {selectedEquipablePlaceId}." );

			// CreatedCharacterに装備させる
			this.CreatedCharacterModel.Value.EquipablePlaces
				.FirstOrDefault(
					equipablePlace => equipablePlace.Id.Value == selectedEquipablePlaceId
				)
				.EquipmentModel = this.HaveEquipments.FirstOrDefault( equipment => equipment.Id.Value.Value == equipmentId );

			// 一覧更新
			this.CustomizeView.UpdateEquipablePlaceText( 
				selectedEquipablePlaceId , 
				this.HaveEquipments.FirstOrDefault( equipment => equipment.Id.Value.Value == equipmentId ).Name 
			);
						
			// 表示切替
			this.CustomizeWindowModel.SelectableName.Value = SelectableNameEnum.EquipablePlaceScrollView;
			
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
					Name = "Alice" ,
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
					Name = "Agatha" ,
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
					Name = "Amanda" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(3) ,
					Name = "Beatrice" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(4) ,
					Name = "Cassie" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(5) ,
					Name = "Claire" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(6) ,
					Name = "Claris" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(7) ,
					Name = "Diana" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(8) ,
					Name = "Eve" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(9) ,
					Name = "Fiona" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(10) ,
					Name = "Gillian" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(11) ,
					Name = "Grace" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(12) ,
					Name = "Hazel" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(13) ,
					Name = "Iris" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(14) ,
					Name = "Isabel" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(15) ,
					Name = "Jane" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(16) ,
					Name = "Jennifer" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(17) ,
					Name = "Jessica" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(18) ,
					Name = "Jessie" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(19) ,
					Name = "Joanna" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(20) ,
					Name = "Julia" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(21) ,
					Name = "Julianne" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(22) ,
					Name = "Kate" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(23) ,
					Name = "Katherine" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(24) ,
					Name = "Laura" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(25) ,
					Name = "Layla" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(26) ,
					Name = "Lily" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(27) ,
					Name = "Lydia" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(28) ,
					Name = "Mary" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(29) ,
					Name = "Melinda" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(30) ,
					Name = "Mia" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(30) ,
					Name = "Nadia" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(31) ,
					Name = "Natalie" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(32) ,
					Name = "Nina" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(33) ,
					Name = "Olivia" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(34) ,
					Name = "Patricia" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(35) ,
					Name = "Paula" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(36) ,
					Name = "Phyllis" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(37) ,
					Name = "Rachel" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(38) ,
					Name = "Rebecca" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(39) ,
					Name = "Rosa" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(40) ,
					Name = "Sarah" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(41) ,
					Name = "Sharon" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(42) ,
					Name = "Sherry" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(43) ,
					Name = "Shirley" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(44) ,
					Name = "Sonia" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(45) ,
					Name = "Sophia" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(46) ,
					Name = "Sophie" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(47) ,
					Name = "Stacy" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(48) ,
					Name = "Stella" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(49) ,
					Name = "Stephanie" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(50) ,
					Name = "Sylvia" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(51) ,
					Name = "Tina" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(52) ,
					Name = "Tracy" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(53) ,
					Name = "Viola" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(54) ,
					Name = "Vivian" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(55) ,
					Name = "Wendy" ,
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
				} ,
				new BodyModel(){
					Id = new ReactiveProperty<int?>(56) ,
					Name = "Wilhelmina" ,
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
					Name = "鉛筆" ,
					EquipablePlaceIds = { 0 , 1 , 3 }
				} ,
				new EquipmentModel() {
					Id = new ReactiveProperty<int?>(1) ,
					Name = "シャーペン" ,
					EquipablePlaceIds = { 0 , 1 , 2 }
				} ,
				new EquipmentModel() {
					Id = new ReactiveProperty<int?>(2) ,
					Name = "消しゴム" ,
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
