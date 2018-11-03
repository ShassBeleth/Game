using UniRx;

namespace Models.Customize {

	/// <summary>
	/// カスタマイズのWindowの切り替え状態を保持するModel
	/// </summary>
	public class CustomizeWindowModel {

		/// <summary>
		/// 選択状態
		/// </summary>
		public ReactiveProperty<SelectableNameEnum> SelectableName { set; get; }

		public SelectableNameEnum BeforeSelectableName { set; get; }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="selectableName">選択状態</param>
		public CustomizeWindowModel( SelectableNameEnum selectableName ) {
			Logger.Debug( "Start" );
			this.SelectableName = new ReactiveProperty<SelectableNameEnum>( selectableName );
			this.BeforeSelectableName = SelectableNameEnum.None;
			Logger.Debug( "End" );
		}

	}

}
