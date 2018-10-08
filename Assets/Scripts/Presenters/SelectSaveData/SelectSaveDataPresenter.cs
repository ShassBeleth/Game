using System;
using System.Collections.Generic;
using Save;
using UnityEngine;
using UnityEngine.UI;
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
			this.selectSaveDataView.OnClickBackButtonEventHandler = this.ClickedBackButtonEvent;

			// Viewに必要な情報に加工
			List<SelectSaveDataView.SaveData> saveDataList = new List<SelectSaveDataView.SaveData>() {
				this.ConvertSaveData( SaveDataSerializer.LoadSinglePlaySaveData( 0 ) ) ,
				this.ConvertSaveData( SaveDataSerializer.LoadSinglePlaySaveData( 1 ) ) ,
				this.ConvertSaveData( SaveDataSerializer.LoadSinglePlaySaveData( 2 ) ) ,
				this.ConvertSaveData( SaveDataSerializer.LoadSinglePlaySaveData( 3 ) )
			};
			
			// セーブデータを見て描画
			GameObject savesGameObject = this.selectSaveDataView.transform.Find( "Saves" ).gameObject;
			for( int i = 0 ; i < savesGameObject.transform.childCount ; i++ ) {
				// ラムダ式に直接カウントを使用するので、一時変数を使用する
				int index = i;

				GameObject save = savesGameObject.transform.GetChild( index ).gameObject;

				Button newButton = save.transform.Find( "NewButton" ).gameObject.GetComponent<Button>();
				Button decisionButton = save.transform.Find( "DecisionButton" ).gameObject.GetComponent<Button>();
				Button copyButton = save.transform.Find( "CopyButton" ).gameObject.GetComponent<Button>();
				Button deleteButton = save.transform.Find( "DeleteButton" ).gameObject.GetComponent<Button>();

				// nullの場合は新規追加ボタンの描画
				if( saveDataList[ index ] == null ) {
					Logger.Debug( "Save Data [" + index + "] is Null" );
					newButton.gameObject.SetActive( true );
					decisionButton.gameObject.SetActive( false );
					copyButton.gameObject.SetActive( false );
					deleteButton.gameObject.SetActive( false );
					newButton.onClick.AddListener( () => { this.ClickedNewButtonEvent( index ); } );
				}
				// nullでない場合は新規追加ボタン以外を描画
				else {
					Logger.Debug( "saveDataList[" + index + "]" );
					Logger.Debug( "Id is " + saveDataList[ index ].Id );
					Logger.Debug( "User Name is " + saveDataList[ index ].userName );
					Logger.Debug( "Latest Update Date Time is " + saveDataList[ index ].latestUpdateDataTime.ToString( "yyyy/MM/dd hh:mm:ss" ) );
					newButton.gameObject.SetActive( false );
					decisionButton.gameObject.SetActive( true );
					copyButton.gameObject.SetActive( true );
					deleteButton.gameObject.SetActive( true );
					decisionButton.onClick.AddListener( () => { this.ClickedDecisionButtonEvent( index ); } );
					copyButton.onClick.AddListener( () => { this.ClickedCopyButtonEvent( index ); } );
					deleteButton.onClick.AddListener( () => { this.ClickedDeleteButtonEvent( index ); } );
				}
			}
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 読み込んだセーブデータをViewに渡す形に変換する
		/// </summary>
		/// <param name="singlePlaySaveDataModel">読み込んだセーブデータ</param>
		/// <returns>Viewに表示するセーブデータ</returns>
		private SelectSaveDataView.SaveData ConvertSaveData( SinglePlaySaveDataModel singlePlaySaveDataModel ) {
			Logger.Debug( "Start" );

			if( singlePlaySaveDataModel == null ) {
				Logger.Debug( "Single Play Save Data Model is Null." );
				return null;
			}
			Logger.Debug( "End" );
			return new SelectSaveDataView.SaveData() {
				Id = singlePlaySaveDataModel.Id ,
				userName = singlePlaySaveDataModel.userName ,
				latestUpdateDataTime = singlePlaySaveDataModel.latestUpdateDataTime
			};
		}

		/// <summary>
		/// 戻るボタン押下時イベント
		/// </summary>
		private void ClickedBackButtonEvent() {
			Logger.Debug( "Start" );
			UnityEngine.SceneManagement.SceneManager.LoadScene( "Title" );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 新規作成ボタン押下時イベント
		/// </summary>
		/// <param name="id">セーブデータID</param>
		private void ClickedNewButtonEvent( int id ) {
			Logger.Debug( "Start" );
			Logger.Debug( "Id is " + id );

			Logger.Debug( "End" );
		}
		
		/// <summary>
		/// 決定ボタン押下時イベント
		/// </summary>
		/// <param name="id">セーブデータID</param>
		private void ClickedDecisionButtonEvent( int id ) {
			Logger.Debug( "Start" );
			Logger.Debug( "Id is " + id );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// コピーボタン押下時イベント
		/// </summary>
		/// <param name="id">セーブデータID</param>
		private void ClickedCopyButtonEvent( int id ) {
			Logger.Debug( "Start" );
			Logger.Debug( "Id is " + id );

			Logger.Warning( "未実装" );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// 削除ボタン押下時イベント
		/// </summary>
		/// <param name="id">セーブデータID</param>
		private void ClickedDeleteButtonEvent( int id ) {
			Logger.Debug( "Start" );
			Logger.Debug( "Id is " + id );

			Logger.Debug( "End" );
		}

	}

}
