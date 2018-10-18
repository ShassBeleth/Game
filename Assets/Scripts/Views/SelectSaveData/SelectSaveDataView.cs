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
		/// セーブデータ群
		/// </summary>
		public GameObject[] saves;
		
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
		/// Event System
		/// </summary>
		public EventSystem eventSystem;

		/// <summary>
		/// 強制的にセーブデータの選択肢を設定する
		/// </summary>
		/// <param name="selectable">選択肢</param>
		public void SetSelectedSaveData( GameObject selectable ) {
			Logger.Debug( "Start" );
			this.eventSystem.SetSelectedGameObject( selectable );
			Logger.Debug( "End" );
		}
		
		/// <summary>
		/// パネルの表示
		/// </summary>
		/// <param name="saveIndex">表示するセーブデータのIndex nullの場合すべて非表示</param>
		public void ShowPanel( int? saveIndex ) {
			Logger.Debug( "Start" );
			foreach( int i in Enumerable.Range( 0 , this.saves.Length ) ) {
				this.saves[ i ].transform.Find( "Panel" ).gameObject.SetActive(
					saveIndex.HasValue && saveIndex.Value == i
				);
			}
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
			foreach( int i in Enumerable.Range( 0 , savesGameObject.transform.childCount ) ) {
				int index = i;

				GameObject save = this.saves[ index ];
				Transform panelTransform = save.transform.Find( "Panel" );
				Button newButton = panelTransform.Find( "NewButton" ).gameObject.GetComponent<Button>();
				Button decisionButton = panelTransform.Find( "DecisionButton" ).gameObject.GetComponent<Button>();
				Button copyButton = panelTransform.Find( "CopyButton" ).gameObject.GetComponent<Button>();
				Button deleteButton = panelTransform.Find( "DeleteButton" ).gameObject.GetComponent<Button>();

				// 新規追加の場合
				if( !saveDataList[ index ].ExistsAlreadyData ) {
					Logger.Debug( $"Save Data [ {index} ] Don't Exists Save Data" );
					newButton.enabled = true;
					decisionButton.enabled = false;
					copyButton.enabled = false;
					deleteButton.enabled = false;
					newButton.onClick.AddListener( () => { saveDataList[ index ].OnClickNewButtonEventHandler.Invoke(); } );
				}
				// 新規追加でない場合
				else {
					Logger.Debug( $"Save Data List[ {index} ] Exists Save Data" );
					Logger.Debug( $"Id is {saveDataList[ index ].Id}" );
					Logger.Debug( $"User Name is {saveDataList[ index ].userName}" );
					Logger.Debug( $"Latest Update Date Time is {saveDataList[ index ].latestUpdateDateTime.ToString( "yyyy/MM/dd hh:mm:ss" )}" );
					newButton.enabled = false;
					decisionButton.enabled = true;
					copyButton.enabled = true;
					deleteButton.enabled = true;
					decisionButton.onClick.AddListener( () => { saveDataList[ index ].OnClickDecisionButtonEventHandler.Invoke(); } );
					copyButton.onClick.AddListener( () => { saveDataList[ index ].OnClickCopyButtonEventHandler.Invoke(); } );
					deleteButton.onClick.AddListener( () => { saveDataList[ index ].OnClickDeleteButtonEventHandler.Invoke(); } );
				}
				save.GetComponent<Button>().onClick.AddListener( () => saveDataList[ i ].OnClickedSaveData() );

			}

			Logger.Debug( "End" );
		}

		/// <summary>
		/// パネル内のボタンを強制的に選択させる
		/// </summary>
		/// <param name="index">セーブデータIndex</param>
		/// <param name="existsAlreadyData">既にセーブデータが存在するかどうか</param>
		public void SetSelectedButtonInPanel( int index , bool existsAlreadyData ) {
			Logger.Debug( "Start" );
			this.eventSystem.SetSelectedGameObject(
				this.saves[ index ].transform.Find( "Panel" ).Find(
					!existsAlreadyData ? "NewButton" : "DecisionButton"
				).gameObject
			);
			Logger.Debug( "End" );
		}
	}

}