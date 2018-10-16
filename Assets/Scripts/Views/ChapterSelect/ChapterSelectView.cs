using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Views.ChapterSelect {

	/// <summary>
	/// チャプター選択View
	/// </summary>
	public class ChapterSelectView : MonoBehaviour {

		/// <summary>
		/// チャプター
		/// </summary>
		public class ChapterData {

			/// <summary>
			/// ID
			/// </summary>
			public int Id { set; get; }

			/// <summary>
			/// チャプター名
			/// </summary>
			public string Name { set; get; }
			
			/// <summary>
			/// 表示するかどうか
			/// </summary>
			public bool IsShown { set; get; }

			/// <summary>
			/// 決定ボタン押下時イベントハンドラ
			/// </summary>
			public Action OnClickDecisionButtonEventHandler { set; get; }

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
		/// チャプターデータ一覧の表示
		/// </summary>
		/// <param name="chapterDataList">チャプターデータ一覧</param>
		public void ShowChapterDataList( List<ChapterData> chapterDataList ) {
			Logger.Debug( "Start" );
			foreach( ChapterData chapter in chapterDataList ) {
				Logger.Debug( "Chapter-----------------------------" );
				Logger.Debug( $"Id is {chapter.Id}" );
				Logger.Debug( $"Name is {chapter.Name}" );
				Logger.Debug( $"Shown is {chapter.IsShown}" );
			}

			GameObject chaptersGameObject = GameObject.Find( "Canvas" ).transform.Find( "Chapters").gameObject;
			foreach( int i in Enumerable.Range( 0 , chaptersGameObject.transform.childCount ) ) {
				Button chapterButton = chaptersGameObject.transform.GetChild( i ).gameObject.GetComponent<Button>();
				chapterButton.onClick.AddListener( () => { chapterDataList[ i ].OnClickDecisionButtonEventHandler?.Invoke(); } );
			}

			Logger.Debug( "End" );
		}

	}

}