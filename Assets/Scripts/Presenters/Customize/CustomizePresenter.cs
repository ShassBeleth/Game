using System.Collections.Generic;
using SceneManagers.Parameters;
using UnityEngine;
using Views.Customize;

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
		/// パラメータカスタマイズGameObject
		/// </summary>
		private GameObject CustomParameterGameObject { set; get; }

		/// <summary>
		/// 装備カスタマイズGameObject
		/// </summary>
		private GameObject CustomEquipmentGameObject { set; get; }

		/// <summary>
		/// 装備メニューGameObject
		/// </summary>
		private GameObject EquipmentMenuGameObject { set; get; }

		/// <summary>
		/// 素体一覧GameObject
		/// </summary>
		private GameObject BodiesGameObject { set; get; }

		/// <summary>
		/// 素体一覧
		/// </summary>
		private List<CustomizeView.Body> bodies { set; get; }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CustomizePresenter( CustomizeParameter parameter ) {
			Logger.Debug( "Start" );

			// hierarchyからViewを持つGameObject取得
			GameObject customizeGameObject = GameObject.Find( "Canvas" );
			this.CustomEquipmentGameObject = customizeGameObject.transform.Find( "Equipment" ).gameObject;
			this.CustomParameterGameObject = customizeGameObject.transform.Find( "Parameter" ).gameObject;
			this.EquipmentMenuGameObject = this.CustomEquipmentGameObject.transform.Find( "Menu" ).gameObject;
			this.BodiesGameObject = this.CustomEquipmentGameObject.transform.Find( "Bodies" ).gameObject;

			// Viewを取得
			this.CustomizeView = customizeGameObject.GetComponent<CustomizeView>();

			// CutomizeViewのEventHandler設定
			this.CustomizeView.OnClickDecisionButtonEventHandler = this.ClickedDecisionButtonEvent;
			this.CustomizeView.OnClickSwitchEquipmentButtonEventHandler = this.ClickedSwitchEquipmentButtonEvent;
			this.CustomizeView.OnClickSwitchParameterButtonEventHandler = this.ClickedSwitchParameterButtonEvent;
			this.CustomizeView.OnClickUpdateButtonEventHandler = this.ClickedUpdateButtonEvent;
			this.CustomizeView.OnClickBackButtonEventHandler = this.ClickedBackButtonEvent;
			this.CustomizeView.OnClickBodyButtonEventHandler = this.ClickedBodyButtonFromMenuEvent;

			// TODO 素体一覧取得
			this.bodies = new List<CustomizeView.Body>() {
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
			this.CustomizeView.SetBodies( this.bodies );

			// TODO 装備一覧取得
			List<CustomizeView.Equipment> equipments = new List<CustomizeView.Equipment>() {
				new CustomizeView.Equipment(){
					Id = 0 ,
					Name = "スシコラ"
				} ,
				new CustomizeView.Equipment() {
					Id = 1 ,
					Name = "リッター"
				} ,
				new CustomizeView.Equipment() {
					Id = 2 ,
					Name = "スパッタリー"
				}
			};
			this.CustomizeView.SetEqupments( equipments );

			// TODO パラメータチップ一覧取得
			List<CustomizeView.ParameterChip> parameterChips = new List<CustomizeView.ParameterChip>() {
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
			this.CustomizeView.SetParameterChips( parameterChips );

			// 初期表示
			this.BodiesGameObject.SetActive( false );
			this.CustomParameterGameObject.SetActive( false );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 決定ボタン押下時イベント
		/// </summary>
		public void ClickedDecisionButtonEvent() {
			Logger.Debug( "Start" );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備画面に切り替えるボタン押下時イベント
		/// </summary>
		public void ClickedSwitchEquipmentButtonEvent() {
			Logger.Debug( "Start" );
			this.CustomEquipmentGameObject.SetActive( true );
			this.CustomParameterGameObject.SetActive( false );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// パラメータ画面に切り替えるボタン押下時イベント
		/// </summary>
		public void ClickedSwitchParameterButtonEvent() {
			Logger.Debug( "Start" );
			this.CustomParameterGameObject.SetActive( true );
			this.CustomEquipmentGameObject.SetActive( false );
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
			this.BodiesGameObject.SetActive( true );
			this.EquipmentMenuGameObject.SetActive( false );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 素体選択時イベント
		/// </summary>
		/// <param name="bodyId">素体ID</param>
		public void ClickedBodyNodeDecisionButtonEvent( int bodyId ) {
			Logger.Debug( "Start" );
			Logger.Debug( "Body Id is " + bodyId );

			// 素体IDからどの素体が選ばれたか調べる
			CustomizeView.Body selectedBody = null;
			foreach( CustomizeView.Body body in this.bodies ) {
				if( bodyId == body.Id ) {
					selectedBody = body;
					break;
				}
			}
			if( selectedBody == null ) {
				Logger.Warning( "Selected Body is Null" );
			}

			// 選ばれた素体に装備できる一覧を設定する
			this.CustomizeView.SetEquipablePlaces( selectedBody.EqupablePlaces );

			// 表示切替
			this.EquipmentMenuGameObject.SetActive( true );
			this.BodiesGameObject.SetActive( false );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 装備可能箇所選択時イベント
		/// </summary>
		/// <param name="equipablePlaceId">装備可能箇所ID</param>
		public void ClickedEquipablePlaceNodeDecisionButtonEvent( int equipablePlaceId ) {
			Logger.Debug( "Start" );

			Logger.Debug( "End" );
		}

	}

}
