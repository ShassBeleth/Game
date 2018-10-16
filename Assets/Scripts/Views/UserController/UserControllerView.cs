using System;
using System.Collections.Generic;
using UnityEngine;

namespace Views.UserController {

	/// <summary>
	/// ユーザの入力を受け取るView
	/// </summary>
	public class UserControllerView : MonoBehaviour {

		#region メニュー時のボタン配置

		/// <summary>
		/// メニューボタンのキー名
		/// </summary>
		public string[] MenuButtonKeyNames { get; } = new string[] {
			
			// 決定
			"Submit" ,

			// キャンセル
			"Cancel" ,

			// 詳細を開く/閉じる
			"Detail" ,

			// ズームイン/ズームアウト
			"Zoom" ,

			// パラメータチップ左回転
			"TurnParameterChipLeft" ,

			// パラメータチップ右回転
			"TurnParameterChipRight"

		};

		/// <summary>
		/// メニューボタン群
		/// </summary>
		public Dictionary<string , int> MenuButtons { private set; get; } = new Dictionary<string , int>();

		/// <summary>
		/// カーソル上下
		/// </summary>
		public int CursorVertical { private set; get; } = 0;
		/// <summary>
		/// カーソル左右
		/// </summary>
		public int CursorHorizontal { private set; get; } = 0;

		/// <summary>
		/// キャラクター左回転
		/// </summary>
		public int TurnCharacter { private set; get; } = 0;

		#endregion

		#region バトル時のボタン配置

		/// <summary>
		/// バトル時のボタンのキー名
		/// </summary>
		public string[] ButtleButtonKeyNames { get; } = new string[] {
			
			// アクション
			"Action" ,

			// 攻撃1
			"Attack1" ,

			// 攻撃2
			"Attack2" ,

			// 特殊1
			"Special1" ,

			// 特殊2
			"Special2" ,

			// ジャンプ
			"Jump" ,

			// カメラリセット/ターゲット切り替え
			"CameraReset" ,

			// ガード
			"Guard" ,

			// オプション
			"Option"
			
		};

		/// <summary>
		/// バトルボタン群
		/// </summary>
		public Dictionary<string , int> ButtleButtons { private set; get; } = new Dictionary<string , int>();

		/// <summary>
		/// 水平移動
		/// </summary>
		public int HorizontalMove { private set; get; } = 0;
		/// <summary>
		/// 垂直移動
		/// </summary>
		public int VerticalMove { private set; get; } = 0;

		/// <summary>
		/// カメラ水平回転
		/// </summary>
		public int HorizontalTurnCamera { private set; get; } = 0;

		/// <summary>
		/// カメラ垂直回転
		/// </summary>
		public int VerticalTurnCamera { private set; get; } = 0;

		/// <summary>
		/// ブースト
		/// </summary>
		public int Boost { private set; get; } = 0;

		#endregion
		
		void Update() {

			#region メニュー時の操作

			// ボタンが押されていたら1増加、押されていないときは0
			foreach( string menuButtonKeyName in this.MenuButtonKeyNames ) {
				if( Input.GetButton( menuButtonKeyName ) ) {
					this.MenuButtons[ menuButtonKeyName ]++;
					Logger.Debug( $"{menuButtonKeyName} is {this.MenuButtons[ menuButtonKeyName ]}" );
				}
				else {
					this.MenuButtons[ menuButtonKeyName ] = 0;
				}
			}
			
			// カーソル左
			if( Input.GetAxis( "Horizontal" ) < 0 ) {
				this.MenuButtons[ "CursorLeft" ]++;
				Logger.Debug( $"CursorLeft is {this.MenuButtons[ "CursorLeft" ]}" );
			}
			else {
				this.MenuButtons[ "CursorLeft" ] = 0;
			}
			// カーソル右
			if( 0 < Input.GetAxis( "Horizontal" ) ) {
				this.MenuButtons[ "CursorRight" ]++;
				Logger.Debug( $"CursorRight is {this.MenuButtons[ "CursorRight" ]}" );
			}
			else {
				this.MenuButtons[ "CursorRight" ] = 0;
			}
			// カーソル下
			if( Input.GetAxis( "Vertical" ) < 0 ) {
				this.MenuButtons[ "CursorDown" ]++;
				Logger.Debug( $"CursorDown is {this.MenuButtons[ "CursorDown" ]}" );
			}
			else {
				this.MenuButtons[ "CursorDown" ] = 0;
			}
			// カーソル上
			if( 0 < Input.GetAxis( "Vertical" ) ) {
				this.MenuButtons[ "CursorUp" ]++;
				Logger.Debug( $"CursorUp is {this.MenuButtons[ "CursorUp" ]}" );
			}
			else {
				this.MenuButtons[ "CursorUp" ] = 0;
			}
			
			// カーソル上下
			this.CursorVertical = (int)( Input.GetAxis( "CursorVertical" ) * 1000 );
			if( this.CursorVertical != 0 ) {
				Logger.Debug( $"CursorVertical is {this.CursorVertical}" );
			}
			
			// カーソル左右
			this.CursorHorizontal = (int)( Input.GetAxis( "CursorHorizontal" ) * 1000 );
			if( this.CursorHorizontal != 0 ) {
				Logger.Debug( $"CursorHorizontal is {this.CursorHorizontal}" );
			}

			// キャラクター回転
			this.TurnCharacter = (int)( Input.GetAxis( "TurnCharacter" ) * 1000 );
			if( this.TurnCharacter != 0 ) {
				Logger.Debug( $"TurnCharacter is {this.TurnCharacter}" );
			}
			
			#endregion

			#region バトル時の操作

			// ボタンが押されていたら1増加、押されていないときは0
			foreach( string buttleButtonKeyName in this.ButtleButtonKeyNames ) {
				if( Input.GetButton( buttleButtonKeyName ) ) {
					this.ButtleButtons[ buttleButtonKeyName ]++;
					Logger.Debug( $"{buttleButtonKeyName} is {this.ButtleButtons[ buttleButtonKeyName ]}" );
				}
				else {
					this.ButtleButtons[ buttleButtonKeyName ] = 0;
				}
			}

			// 水平移動
			this.HorizontalMove = (int)( Input.GetAxis( "HorizontalMove" ) * 1000 );
			if( this.HorizontalMove != 0 ) {
				Logger.Debug( $"HorizontalMove is {this.HorizontalMove}" );
			}

			// 垂直移動
			this.VerticalMove = (int)( Input.GetAxis( "VerticalMove" ) * 1000 );
			if( this.VerticalMove != 0 ) {
				Logger.Debug( $"VerticalMove is {this.VerticalMove}" );
			}

			// カメラ水平回転
			this.HorizontalTurnCamera = (int)( Input.GetAxis( "HorizontalTurnCamera" ) * 1000 );
			if( this.HorizontalTurnCamera != 0 ) {
				Logger.Debug( $"HorizontalTurnCamera is {this.HorizontalTurnCamera}" );
			}

			// カメラ垂直回転
			this.VerticalTurnCamera = (int)( Input.GetAxis( "VerticalTurnCamera" ) * 1000 );
			if( this.VerticalTurnCamera != 0 ) {
				Logger.Debug( $"VerticalTurnCamera is {this.VerticalTurnCamera}" );
			}

			// ブースト
			this.Boost = (int)( ( ( Input.GetAxis( "Boost" ) * 1000 ) + 1000 ) / 2 );
			if( this.Boost != 0 ) {
				Logger.Debug( $"Boost is {this.Boost}" );
			}
			
			#endregion
			
		}

	}

}