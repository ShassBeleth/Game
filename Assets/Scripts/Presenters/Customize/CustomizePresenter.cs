using System.Collections.Generic;
using System.Linq;
using Models.Charactor;
using Models.Customize;
using Services.Characters;
using Services.Scenes;
using Services.Scenes.Parameters;
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
		
		#region Service

		/// <summary>
		/// シーンService
		/// </summary>
		private SceneService sceneService = SceneService.GetInstance();

		/// <summary>
		/// キャラクターService
		/// </summary>
		private CharacterService characterService = CharacterService.GetInstance();

		#endregion

		/// <summary>
		/// セーブデータID
		/// </summary>
		private int saveId;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CustomizePresenter( CustomizeParameter parameter ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Save Data Id is {parameter.SaveId}." );

			this.saveId = parameter.SaveId;

			// Viewの設定
			this.InitialViewSetting();

			// ModelのSubscribeを設定
			this.InitialModelSubscribeSetting();

			// 素体一覧取得
			this.CustomizeView.SetBodies( 
				this.ConvertBodies( 
					this.characterService.GetHavingBodies( this.saveId ) 
				) 
			);
			
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
			this.CustomizeView.OnClickDecisionButtonEventHandler = this.ClickedEquipmentDecisionButtonEvent;
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

			List<EquipablePlaceModel> models = this.characterService.GetEquipablePlaces(
				this.CreatedCharacterModel.Value
			);

			// 選ばれた素体に装備できる一覧を設定する
			this.CustomizeView.SetEquipablePlaces( 
				this.ConvertEquipablePlaces( models )
			);

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
						this.characterService
							.GetHavingEquipments( 
								this.saveId , 
								id.Value , 
								this.CreatedCharacterModel.Value 
							)
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

			// 詳細窓を表示する場合は選択状態を見て表示する内容を決める
			if( isShownDetail ) {
				if( this.CustomizeWindowModel.SelectableName.Value == SelectableNameEnum.EquipmentMenu ) {
					CustomizeView.BodyDetail detail = new CustomizeView.BodyDetail() {
						Name = this.CreatedCharacterModel.Value.Name ,
						Ruby = this.CreatedCharacterModel.Value.Ruby ,
						Flavor = this.CreatedCharacterModel.Value.Flavor
					};
					this.CustomizeView.SetBodyDetail( detail );
				}
			}

			this.CustomizeView.SetDetailActive( isShownDetail );
			Logger.Debug( "End" );
		}
		
		#endregion

		#region Viewイベント

		/// <summary>
		/// 装備の決定ボタン押下時イベント
		/// </summary>
		public void ClickedEquipmentDecisionButtonEvent() {
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
			this.CreatedCharacterModel.Value = this.characterService.GetHavingBodies( this.saveId ).FirstOrDefault( body => body.Id.HasValue && bodyId == body.Id.Value );
			this.CreatedCharacterModel.Value.EquipablePlaces = this.characterService.GetEquipablePlaces( this.CreatedCharacterModel.Value );

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

			EquipmentModel equipmentModel = this.characterService.GetEquipment( equipmentId );

			// CreatedCharacterに装備させる
			this.CreatedCharacterModel.Value.EquipablePlaces
				.FirstOrDefault(
					equipablePlace => equipablePlace.Id.Value == selectedEquipablePlaceId
				)
				.EquipmentModel = equipmentModel;
			
			// 選ばれた素体に装備できる一覧を設定する
			List<EquipablePlaceModel> models
				= this.characterService.GetEquipablePlaces(
					this.CreatedCharacterModel.Value
				);
			this.CustomizeView.SetEquipablePlaces(
				this.ConvertEquipablePlaces( models )
			);

			// 一覧更新
			// TODO ここ
			this.CustomizeView.UpdateEquipablePlaceText( 
				selectedEquipablePlaceId ,
				equipmentModel.Name
			);
						
			// 表示切替
			this.CustomizeWindowModel.SelectableName.Value = SelectableNameEnum.EquipablePlaceScrollView;
			
			Logger.Debug( "End" );
		}

		#endregion
		
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
