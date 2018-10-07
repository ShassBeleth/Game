using System;
using System.Collections.Generic;
using UnityEngine;
using Views.SelectSaveData;

namespace Presenters.SelectSaveData {

	/// <summary>
	/// セーブデータ選択Presenter
	/// </summary>
	public class SelectSaveDataPresenter {

		/// <summary>
		/// セーブデータ選択View
		/// </summary>
		private SelectSaveDataView selectSaveDataView;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SelectSaveDataPresenter() {
			Logger.Debug( "Start" );
			
			// hierarchyからViewを取得
			this.selectSaveDataView = GameObject.Find( "Canvas" ).GetComponent<SelectSaveDataView>();

			// セーブデータ選択ViewのEventHandler設定
			this.selectSaveDataView.OnClickBackButtonEventHandler = this.OnClickBackButtonEvent;
			#region TODO 仮
			this.selectSaveDataView.OnClickTempButtonEventHandler = this.OnClickTempButtonEvent;
			#endregion

			// TODO セーブデータの取得

			// TODO Viewに必要な情報に加工
			List<SelectSaveDataView.SaveData> saveDataList = new List<SelectSaveDataView.SaveData>() {
				new SelectSaveDataView.SaveData() {
					Id = 1 ,
					userName = "a" ,
					latestUpdateDataTime = new DateTime( 2018 , 10 , 7 , 2 , 41 , 26 )
				},
				new SelectSaveDataView.SaveData() {
					Id = 2 ,
					userName = "b" ,
					latestUpdateDataTime = new DateTime( 2018 , 10 , 7 , 2 , 41 , 26 )
				},
				new SelectSaveDataView.SaveData() {
					Id = 3 ,
					userName = "c" ,
					latestUpdateDataTime = new DateTime( 2018 , 10 , 7 , 2 , 41 , 26 )
				},
				new SelectSaveDataView.SaveData() {
					Id = 4 ,
					userName = "d" ,
					latestUpdateDataTime = new DateTime( 2018 , 10 , 7 , 2 , 41 , 26 )
				}
			};
			this.selectSaveDataView.ShowSaveData( saveDataList );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 戻るボタン押下時イベント
		/// </summary>
		private void OnClickBackButtonEvent() {
			Logger.Debug( "Start" );
			UnityEngine.SceneManagement.SceneManager.LoadScene( "Title" );
			Logger.Debug( "End" );
		}

		#region TODO 仮
		private void OnClickTempButtonEvent() {
			Logger.Debug( "Start" );
			UnityEngine.SceneManagement.SceneManager.LoadScene( "ChapterSelect" );
			Logger.Debug( "End" );
		}
		#endregion

	}

}
