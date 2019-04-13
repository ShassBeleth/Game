using UnityEngine.SceneManagement;
using Services.Scenes.Parameters;
using Presenters.Title;
using Presenters.Gallery;
using Presenters.Ranking;
using Presenters.SelectSaveData;
using Presenters.ChapterSelect;
using Presenters.Customize;

namespace Services.Scenes {

	/// <summary>
	/// シーンService
	/// </summary>
	public class SceneService {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static SceneService Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static SceneService GetInstance() {
			if( Instance == null ) {
				Instance = new SceneService();
			}
			return Instance;
		}

		#endregion

		/// <summary>
		/// 画面遷移時に渡すパラメータ
		/// </summary>
		private static object Parameter;

		/// <summary>
		/// シングルモードで遷移した場合の前シーンの名前
		/// </summary>
		public static string BeforeSingleModeSceneName {
			private set;
			get;
		}

		/// <summary>
		/// シングルモードで遷移した場合の現在のシーン名
		/// </summary>
		public static string CurrentSingleModeSceneName {
			private set;
			get;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		private SceneService() {
			this.LogDebug( "Start" );

			// シーン切り替え時イベント追加
			SceneManager.sceneLoaded += OnSceneLoaded;

			if( EqualsActiveSceneName( "Title" ) ) {
				this.LogDebug( "Active Scene Name Equals Title." );
				new TitlePresenter();
			}
			// デバッグ用
			// 初期シーンがタイトルでなかったらタイトルに切り替え
			else {
				this.LogWarning( "Active Scene Name don't Equals Title." );
				SceneManager.LoadScene( "Title" );
			}

			this.LogDebug( "End" );
		}

		/// <summary>
		/// シーン読み込み
		/// </summary>
		/// <param name="sceneName">読み込みシーン名</param>
		/// <param name="parameter">遷移先シーンに渡すパラメータ</param>
		public void LoadScene( string sceneName , object parameter ) {
			this.LogDebug( "Start" );
			Parameter = parameter;
			SceneManager.LoadScene( sceneName );
			this.LogDebug( "End" );
		}

		/// <summary>
		/// 現在のシーン名が指定のシーン名と一致するかどうか
		/// </summary>
		/// <param name="sceneName">シーン名</param>
		/// <returns>現在のシーン名が指定のシーン名と一致するかどうか</returns>
		private static bool EqualsActiveSceneName( string sceneName )
			=> SceneManager.GetActiveScene().name.Equals( sceneName );

		/// <summary>
		/// シーン切り替え時イベント
		/// </summary>
		/// <param name="scene">遷移後シーン</param>
		/// <param name="loadSceneMode">遷移モード</param>
		private static void OnSceneLoaded(
			Scene scene ,
			LoadSceneMode loadSceneMode
		) {

			// シングルモードで遷移した場合は前シーンの名前を保持しておく
			if( loadSceneMode == LoadSceneMode.Single ) {
				BeforeSingleModeSceneName = CurrentSingleModeSceneName;
				CurrentSingleModeSceneName = scene.name;
			}
			
			// シーン切り替え時にはおおもとになるPresenterのインスタンスを生成
			switch( scene.name ) {
				case "Title":
					new TitlePresenter( Parameter as TitleParameter );
					break;
				case "Gallery":
					new GalleryPresenter();
					break;
				case "Ranking":
					new RankingPresenter();
					break;
				case "SelectSaveData":
					new SelectSaveDataPresenter( Parameter as SelectSaveDataParameter );
					break;
				case "ChapterSelect":
					new ChapterSelectPresenter( Parameter as ChapterSelectParameter );
					break;
				case "Customize":
					new CustomizePresenter( Parameter as CustomizeParameter );
					break;
				default:
					break;
			}

			// staticフィールドなので、一回使ったらパラメータ内を削除
			Parameter = null;

		}

	}

}
