using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
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
			/// セーブデータ選択時イベントハンドラ
			/// </summary>
			public Action OnClickedSaveData { set; get; }

			/// <summary>
			/// 続きからボタン押下時イベントハンドラ
			/// </summary>
			public Action OnClickContinueButtonEventHandler { set; get; }

			/// <summary>
			/// チャプターセレクトボタン押下時イベントハンドラ
			/// </summary>
			public Action OnClickChapterSelectButtonEventHandler { set; get; }

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
		/// Event System
		/// </summary>
		public EventSystem eventSystem;

		#region セーブデータについて

		/// <summary>
		/// セーブデータ群
		/// </summary>
		public GameObject[] saves;
		
		/// <summary>
		/// セーブデータの設定
		/// </summary>
		/// <param name="saveDataList">セーブデータのリスト</param>
		public void SetSaveDataList( List<SaveData> saveDataList ) {
			Logger.Debug( "Start" );
			
			foreach( int i in Enumerable.Range( 0 , this.saves.Length ) ) {
				int index = i;

				Logger.Debug( $"Save Data List[ {index} ] {( !saveDataList[ index ].ExistsAlreadyData ? "Don't" : "" )} Exists Save Data" );
				Logger.Debug( $"Id is {( saveDataList[ index ].Id.ToString() )}" );
				Logger.Debug( $"User Name is {saveDataList[ index ].userName ?? "Null"}" );
				Logger.Debug( $"Latest Update Date Time is {saveDataList[ index ].latestUpdateDateTime.ToString( "yyyy/MM/dd hh:mm:ss" )}" );
				
				GameObject save = this.saves[ index ];
				// ユーザ名表示
				save.transform.Find( "UserNameText" ).GetComponent<Text>().text
					= saveDataList[ index ].ExistsAlreadyData
						? saveDataList[ index ].userName
						: "New Save Data";
				// 最終更新日時表示
				save.transform.Find( "LatestUpdateDateTimeText" ).GetComponent<Text>().text
					= saveDataList[ index ].ExistsAlreadyData
						? saveDataList[ index ].latestUpdateDateTime.ToString( "yyyy/MM/dd hh:mm:ss" )
						: "----/--/-- --:--:--";

				// 選択時イベント追加
				save.GetComponent<Button>().onClick.AddListener( () => saveDataList[ i ].OnClickedSaveData() );

			}

			Logger.Debug( "End" );
		}

		/// <summary>
		/// 強制的にセーブデータの選択肢を設定する
		/// </summary>
		/// <param name="selectable">選択肢</param>
		public void SetSelectedSaveData( GameObject selectable ) {
			Logger.Debug( "Start" );
			this.eventSystem.SetSelectedGameObject( selectable );
			Logger.Debug( "End" );
		}

		#endregion

		#region 戻るボタンについて

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

		#endregion
		
		#region パネルについて

		/// <summary>
		/// パネル
		/// </summary>
		public GameObject panel;

		/// <summary>
		/// パネルの表示
		/// </summary>
		/// <param name="saveIndex">表示するセーブデータのIndex nullの場合非表示</param>
		/// <param name="saveData">セーブデータ</param>
		public void ShowPanel( int? saveIndex , SaveData save ) {
			Logger.Debug( "Start" );
			if( saveIndex.HasValue ) {
				this.panel.SetActive( true );

				// パネル内のButton取得
				Button continueButton = this.panel.transform.Find( "ContinueButton" ).GetComponent<Button>();
				Button chapterSelectButton = this.panel.transform.Find( "ChapterSelectButton" ).GetComponent<Button>();
				Button copyButton = this.panel.transform.Find( "CopyButton" ).GetComponent<Button>();
				Button deleteButton = this.panel.transform.Find( "DeleteButton" ).GetComponent<Button>();

				// ボタン押下時イベント設定
				continueButton.onClick.AddListener( () => save.OnClickContinueButtonEventHandler() );
				chapterSelectButton.onClick.AddListener( () => save.OnClickChapterSelectButtonEventHandler() );
				copyButton.onClick.AddListener( () => save.OnClickCopyButtonEventHandler() );
				deleteButton.onClick.AddListener( () => save.OnClickDeleteButtonEventHandler() );
			}
			else {
				this.panel.SetActive( false );
			}
			Logger.Debug( "End" );
		}

		/// <summary>
		/// パネル内のボタンを強制的に選択させる
		/// </summary>
		/// <param name="index">セーブデータのindex</param>
		public void SetSelectedButtonInPanel( int index ) {
			Logger.Debug( "Start" );
			this.eventSystem.SetSelectedGameObject( this.panel.transform.Find( "ContinueButton" ).gameObject );
			Logger.Debug( "End" );
		}

		#endregion

	}

}