using UniRx;

namespace Models {

	/// <summary>
	/// カスタマイズのWindowの切り替え状態を保持するModel
	/// </summary>
	public class CustomizeWindowModel {

		/// <summary>
		/// Window名列挙
		/// </summary>
		public enum WindowNameEnum {
			EquipmentMenu ,
			Body ,
			Equipments ,
			ParameterMenu ,
			None
		}

		/// <summary>
		/// Window名
		/// </summary>
		public ReactiveProperty<WindowNameEnum> windowName;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="windowName">Window名</param>
		public CustomizeWindowModel( WindowNameEnum windowName ) {
			Logger.Debug( "Start" );
			this.windowName = new ReactiveProperty<WindowNameEnum>( windowName );
			Logger.Debug( "End" );
		}

	}

}
