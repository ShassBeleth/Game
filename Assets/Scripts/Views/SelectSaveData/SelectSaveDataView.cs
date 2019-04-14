using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
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
			/// セーブデータ選択時イベントSubject
			/// </summary>
			public readonly Subject<Unit> OnClickedSaveDataSubject = new Subject<Unit>();

			/// <summary>
			/// セーブデータ選択時イベント購読
			/// </summary>
			public IObservable<Unit> OnClickedSaveData => this.OnClickedSaveDataSubject;

			/// <summary>
			/// 続きからボタン押下時イベントSubject
			/// </summary>
			public readonly Subject<Unit> OnClickedContinueButtonSubject = new Subject<Unit>();

			/// <summary>
			/// 続きからボタン押下時イベント購読
			/// </summary>
			public IObservable<Unit> OnClickedContinueButton => this.OnClickedContinueButtonSubject;

			/// <summary>
			/// チャプターセレクトボタン押下時イベントSubject
			/// </summary>
			public readonly Subject<Unit> OnClickedChapterSelectButtonSubject = new Subject<Unit>();

			/// <summary>
			/// チャプターセレクトボタン押下時イベント購読
			/// </summary>
			public IObservable<Unit> OnClickedChapterSelectButton => this.OnClickedChapterSelectButtonSubject;

			/// <summary>
			/// コピーボタン押下時イベントSubject
			/// </summary>
			public readonly Subject<Unit> OnClickedCopyButtonSubject = new Subject<Unit>();

			/// <summary>
			/// コピーボタン押下時イベント購読
			/// </summary>
			public IObservable<Unit> OnClickedCopyButton => this.OnClickedCopyButtonSubject;

			/// <summary>
			/// 削除ボタン押下時イベントSubject
			/// </summary>
			public readonly Subject<Unit> OnClickedDeleteButtonSubject = new Subject<Unit>();

			/// <summary>
			/// 削除ボタン押下時イベント購読
			/// </summary>
			public IObservable<Unit> OnClickedDeleteButton => this.OnClickedDeleteButtonSubject;

		}
		
		/// <summary>
		/// Event System
		/// </summary>
		public EventSystem eventSystem;

		#region セーブデータについて

		/// <summary>
		/// セーブデータ群
		/// </summary>
		public GameObject[] Saves;
		
		/// <summary>
		/// セーブデータの設定
		/// </summary>
		/// <param name="saveDataList">セーブデータのリスト</param>
		public void SetSaveDataList( List<SaveData> saveDataList ) {
			this.LogDebug( "Start" );
			
			foreach( int i in Enumerable.Range( 0 , this.Saves.Length ) ) {
				int index = i;

				this.LogDebug( $"Save Data List[ {index} ] {( !saveDataList[ index ].ExistsAlreadyData ? "Don't" : "" )} Exists Save Data" );
				this.LogDebug( $"Id is {( saveDataList[ index ].Id.ToString() )}" );
				this.LogDebug( $"User Name is {saveDataList[ index ].userName ?? "Null"}" );
				this.LogDebug( $"Latest Update Date Time is {saveDataList[ index ].latestUpdateDateTime.ToString( "yyyy/MM/dd hh:mm:ss" )}" );
				
				GameObject save = this.Saves[ index ];
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
				save.GetComponent<Button>().onClick.AddListener( () => saveDataList[ i ].OnClickedSaveDataSubject.OnNext( Unit.Default ) );

			}

			this.LogDebug( "End" );
		}

		/// <summary>
		/// 強制的にセーブデータの選択肢を設定する
		/// </summary>
		/// <param name="selectable">選択肢</param>
		public void SetSelectedSaveData( GameObject selectable ) {
			this.LogDebug( "Start" );
			this.eventSystem.SetSelectedGameObject( selectable );
			this.LogDebug( "End" );
		}

		#endregion
				
		#region パネルについて

		/// <summary>
		/// パネル
		/// </summary>
		public GameObject Panel;

		/// <summary>
		/// パネル内のコンテント
		/// </summary>
		public GameObject ContentInPanel;

		/// <summary>
		/// パネルの表示
		/// </summary>
		/// <param name="saveIndex">表示するセーブデータのIndex nullの場合非表示</param>
		/// <param name="saveData">セーブデータ</param>
		public void ShowPanel( int? saveIndex , SaveData save ) {
			this.LogDebug( "Start" );
			if( saveIndex.HasValue ) {
				this.Panel.SetActive( true );

				// パネル内のButton取得
				Button continueButton = this.ContentInPanel.transform.Find( "ContinueButton" ).GetComponent<Button>();
				Button chapterSelectButton = this.ContentInPanel.transform.Find( "ChapterSelectButton" ).GetComponent<Button>();
				Button copyButton = this.ContentInPanel.transform.Find( "CopyButton" ).GetComponent<Button>();
				Button deleteButton = this.ContentInPanel.transform.Find( "DeleteButton" ).GetComponent<Button>();

				// ボタン押下時イベント設定
				continueButton.onClick.AddListener( () => save.OnClickedContinueButtonSubject.OnNext( Unit.Default ) );
				chapterSelectButton.onClick.AddListener( () => save.OnClickedChapterSelectButtonSubject.OnNext( Unit.Default ) );
				copyButton.onClick.AddListener( () => save.OnClickedCopyButtonSubject.OnNext( Unit.Default ) );
				deleteButton.onClick.AddListener( () => save.OnClickedDeleteButtonSubject.OnNext( Unit.Default ) );
			}
			else {
				this.Panel.SetActive( false );
			}
			this.LogDebug( "End" );
		}

		/// <summary>
		/// パネル内のボタンを強制的に選択させる
		/// </summary>
		/// <param name="index">セーブデータのindex</param>
		public void SetSelectedButtonInPanel( int index ) {
			this.LogDebug( "Start" );
			this.eventSystem.SetSelectedGameObject( this.ContentInPanel.transform.Find( "ContinueButton" ).gameObject );
			this.LogDebug( "End" );
		}

		#endregion

	}

}