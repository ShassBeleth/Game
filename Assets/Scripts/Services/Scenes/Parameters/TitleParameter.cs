namespace Services.Scenes.Parameters {

	/// <summary>
	/// TitleSceneに使用するパラメータ
	/// </summary>
	public class TitleParameter {

		/// <summary>
		/// 初期表示するタイトル部品
		/// </summary>
		public enum InitialTitlePartEnum {
			PleasePushAnyKey,
			MainMenu
		}

		/// <summary>
		/// 初期表示するタイトル部品
		/// </summary>
		public InitialTitlePartEnum? InitialTitlePart { set; get; } = null;

	}

}
