using UniRx;

namespace Models.Title {

	/// <summary>
	/// タイトルのWindowの切り替え状態を保持するModel
	/// </summary>
	public class TitleWindowModel {
		
		/// <summary>
		/// Window名
		/// </summary>
		public ReactiveProperty<WindowNameEnum> windowName;
		
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="windowName">Window名</param>
		public TitleWindowModel( WindowNameEnum windowName ) {
			this.LogDebug( "Start" );
			this.windowName = new ReactiveProperty<WindowNameEnum>( windowName );
			this.LogDebug( "End" );
		}

	}

}
