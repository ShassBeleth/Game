using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Views.SelectSaveData {

	/// <summary>
	/// セーブデータ選択View
	/// </summary>
	public class SelectSaveDataView : MonoBehaviour {

		/// <summary>
		/// 表示するセーブデータ
		/// </summary>
		public class SaveData {

			/// <summary>
			/// 既にセーブデータが存在するかどうか
			/// </summary>
			public bool ExistsAlreadyData { set; get; }

			/// <summary>
			/// ID
			/// </summary>
			public int Id { set; get; }

			/// <summary>
			/// ユーザ名
			/// </summary>
			public string userName { set; get; }

			/// <summary>
			/// 最終更新日
			/// </summary>
			public DateTime latestUpdateDateTime { set; get; }

			/// <summary>
			/// 新規作成ボタン押下時イベントハンドラ
			/// </summary>
			public Action OnClickNewButtonEventHandler { set; get; }

			/// <summary>
			/// 決定ボタン押下時イベントハンドラ
			/// </summary>
			public Action OnClickDecisionButtonEventHandler { set; get; }
			
			/// <summary>
			/// コピーボタン押下時イベントハンドラ
			/// </summary>
			public Action OnClickCopyButtonEventHandler { set; get; }

			/// <summary>
			/// 削除ボタン押下時イベントハンドラ
			/// </summary>
			public Action OnClickDeleteButtonEventHandler { set; get; }

		}
		
		/// <summary>
		/// 戻るボタン押下時イベントハンドラ
		/// </summary>
		public Action OnClickBackButtonEventHandler { set; get; }

		/// <summary>
		/// 戻るボタン押下時イベント
		/// </summary>
		public void OnClickBackButtonEvent() {
			Logger.Debug( "Start" );
			this.OnClickBackButtonEventHandler?.Invoke();
			Logger.Debug( "End" );
		}

		/// <summary>
		/// セーブデータの表示
		/// </summary>
		/// <param name="saveDataList">セーブデータのリスト</param>
		public void ShowSaveDataList( List<SaveData> saveDataList ) {
			Logger.Debug( "Start" );

			// セーブデータを見て描画
			GameObject savesGameObject = this.transform.Find( "Saves" ).gameObject;
			// TODO foreachに変更する　ChapterSelectView参考
			for( int i = 0 ; i < savesGameObject.transform.childCount ; i++ ) {
				int index = i;

				GameObject save = savesGameObject.transform.GetChild( index ).gameObject;

				Button newButton = save.transform.Find( "NewButton" ).gameObject.GetComponent<Button>();
				Button decisionButton = save.transform.Find( "DecisionButton" ).gameObject.GetComponent<Button>();
				Button copyButton = save.transform.Find( "CopyButton" ).gameObject.GetComponent<Button>();
				Button deleteButton = save.transform.Find( "DeleteButton" ).gameObject.GetComponent<Button>();
				
				// 新規追加の場合
				if( !saveDataList[ index ].ExistsAlreadyData ) {
					Logger.Debug( $"Save Data [ {index} ] Don't Exists Save Data" );
					newButton.gameObject.SetActive( true );
					decisionButton.gameObject.SetActive( false );
					copyButton.gameObject.SetActive( false );
					deleteButton.gameObject.SetActive( false );
					newButton.onClick.AddListener( () => { saveDataList[ index ].OnClickNewButtonEventHandler.Invoke(); } );
				}
				// 新規追加でない場合
				else {
					Logger.Debug( $"Save Data List[ {index} ] Exists Save Data" );
					Logger.Debug( $"Id is {saveDataList[ index ].Id}" );
					Logger.Debug( $"User Name is {saveDataList[ index ].userName}" );
					Logger.Debug( $"Latest Update Date Time is {saveDataList[ index ].latestUpdateDateTime.ToString( "yyyy/MM/dd hh:mm:ss" )}" );
					newButton.gameObject.SetActive( false );
					decisionButton.gameObject.SetActive( true );
					copyButton.gameObject.SetActive( true );
					deleteButton.gameObject.SetActive( true );
					decisionButton.onClick.AddListener( () => { saveDataList[ index ].OnClickDecisionButtonEventHandler.Invoke(); } );
					copyButton.onClick.AddListener( () => { saveDataList[ index ].OnClickCopyButtonEventHandler.Invoke(); } );
					deleteButton.onClick.AddListener( () => { saveDataList[ index ].OnClickDeleteButtonEventHandler.Invoke(); } );
				}
			}
			
			Logger.Debug( "End" );
		}

	}

}