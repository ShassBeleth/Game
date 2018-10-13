using System;
using UnityEngine;

namespace Views.Title {

	/// <summary>
	/// タイトルView
	/// </summary>
	public class TitleView : MonoBehaviour {

		/// <summary>
		/// 何かキーが押されたか判定
		/// </summary>
		private bool IsClickAnyKey { set; get; } = false;

		/// <summary>
		/// 何かキーが押された時のイベントハンドラ
		/// </summary>
		public Action OnClickAnyKeyEventHandler { set; get; }

		/// <summary>
		/// Please Push Any KeyのGameObject
		/// </summary>
		private GameObject pleasePushAnyKeyGameObject;

		/// <summary>
		/// Main MenuのGameObject
		/// </summary>
		private GameObject mainMenuGameObject;

		/// <summary>
		/// OptionのGameObject
		/// </summary>
		private GameObject optionGameObject;

		/// <summary>
		/// Unity Update
		/// </summary>
		public void Update() {
			if( Input.anyKey ) {
				if( !this.IsClickAnyKey ) {
					Logger.Debug( "Start : Click Any Key is false." );
					this.IsClickAnyKey = true;
					this.OnClickAnyKeyEventHandler?.Invoke();
					Logger.Debug( "End" );
				}
			}
		}

		/// <summary>
		/// Please Push Any Keyの表示
		/// </summary>
		public void ShowPleasePushAnyKey() {
			Logger.Debug( "Start" );

			if( this.pleasePushAnyKeyGameObject == null ) {
				Logger.Debug( "Please Push Any Key Game Object is Null" );
				this.pleasePushAnyKeyGameObject = GameObject.Find( "PleasePushAnyKey" );
			}
			if( this.mainMenuGameObject == null ) {
				Logger.Debug( "Main Menu Game Object is Null" );
				this.mainMenuGameObject = GameObject.Find( "MainMenu" );
			}
			if( this.optionGameObject == null ) {
				Logger.Debug( "Option Game Object is Null" );
				this.optionGameObject = GameObject.Find( "OptionMenu" );
			}
			
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

			if( this.pleasePushAnyKeyGameObject == null ) {
				Logger.Debug( "Please Push Any Key Game Object is Null" );
				this.pleasePushAnyKeyGameObject = GameObject.Find( "PleasePushAnyKey" );
			}
			if( this.mainMenuGameObject == null ) {
				Logger.Debug( "Main Menu Game Object is Null" );
				this.mainMenuGameObject = GameObject.Find( "MainMenu" );
			}
			if( this.optionGameObject == null ) {
				Logger.Debug( "Option Game Object is Null" );
				this.optionGameObject = GameObject.Find( "OptionMenu" );
			}

			this.pleasePushAnyKeyGameObject.SetActive( false );
			this.mainMenuGameObject.SetActive( true );
			this.optionGameObject.SetActive( false );

			Logger.Debug( "End" );
		}

		/// <summary>
		/// Optionの表示
		/// </summary>
		public void ShowOption() {
			Logger.Debug( "Start" );

			if( this.pleasePushAnyKeyGameObject == null ) {
				Logger.Debug( "Please Push Any Key Game Object is Null" );
				this.pleasePushAnyKeyGameObject = GameObject.Find( "PleasePushAnyKey" );
			}
			if( this.mainMenuGameObject == null ) {
				Logger.Debug( "Main Menu Game Object is Null" );
				this.mainMenuGameObject = GameObject.Find( "MainMenu" );
			}
			if( this.optionGameObject == null ) {
				Logger.Debug( "Option Game Object is Null" );
				this.optionGameObject = GameObject.Find( "OptionMenu" );
			}

			this.pleasePushAnyKeyGameObject.SetActive( false );
			this.mainMenuGameObject.SetActive( false );
			this.optionGameObject.SetActive( true );

			Logger.Debug( "End" );
		}

	}

}