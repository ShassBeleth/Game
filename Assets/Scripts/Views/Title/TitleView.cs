using UnityEngine;
using UnityEngine.EventSystems;

namespace Views.Title {

	/// <summary>
	/// タイトルView
	/// 基本的にScene内の画面の切り替えしかしない
	/// </summary>
	public class TitleView : MonoBehaviour {

		/// <summary>
		/// イベントシステム
		/// </summary>
		public EventSystem eventSystem;
		
		#region 画面切り替えGameObject群

		/// <summary>
		/// Please Push Any KeyのGameObject
		/// </summary>
		public GameObject pleasePushAnyKeyGameObject;

		/// <summary>
		/// Main MenuのGameObject
		/// </summary>
		public GameObject mainMenuGameObject;

		/// <summary>
		/// OptionのGameObject
		/// </summary>
		public GameObject optionGameObject;

		#endregion

		#region 画面遷移メソッド

		/// <summary>
		/// Please Push Any Keyの表示
		/// </summary>
		public void ShowPleasePushAnyKey() {
			Logger.Debug( "Start" );

			this.pleasePushAnyKeyGameObject.SetActive( true );
			this.mainMenuGameObject.SetActive( false );
			this.optionGameObject.SetActive( false );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// Main Menuの表示
		/// </summary>
		public void ShowMainMenu() {
			Logger.Debug( "Start" );

			this.mainMenuGameObject.SetActive( true );
			this.pleasePushAnyKeyGameObject.SetActive( false );
			this.optionGameObject.SetActive( false );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// Optionの表示
		/// </summary>
		public void ShowOption() {
			Logger.Debug( "Start" );

			this.optionGameObject.SetActive( true );
			this.pleasePushAnyKeyGameObject.SetActive( false );
			this.mainMenuGameObject.SetActive( false );

			Logger.Debug( "End" );
		}

		#endregion

	}

}