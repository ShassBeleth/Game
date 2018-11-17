using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Chapter;
using Repositories;
using Repositories.Models;
using Utils;

namespace Services.Saves {

	/// <summary>
	/// セーブService
	/// </summary>
	public class SaveService {

		#region シングルトン

		/// <summary>
		/// インスタンス
		/// </summary>
		private static SaveService Instance = null;

		/// <summary>
		/// インスタンス取得
		/// </summary>
		/// <returns>インスタンス</returns>
		public static SaveService GetInstance() {
			Logger.Debug( "Start" );
			if( Instance == null ) {
				Logger.Debug( "Instance is Null." );
				Instance = new SaveService();
			}
			Logger.Debug( "End" );
			return Instance;
		}

		#endregion

		#region Repository

		/// <summary>
		/// セーブRepository
		/// </summary>
		private SaveRepository saveRepository = SaveRepository.GetInstance();

		/// <summary>
		/// チャプタークリア状況一覧
		/// </summary>
		private ChapterClearStatusRepository chapterClearStatusRepository = ChapterClearStatusRepository.GetInstance();

		#endregion
		
		/// <summary>
		/// コンストラクタ
		/// </summary>
		private SaveService() {
			Logger.Debug( "Start" );
			Logger.Debug( "End" );
		}

		/// <summary>
		/// セーブデータ一覧取得
		/// </summary>
		/// <returns>セーブデータ一覧</returns>
		public List<SaveDataModel> GetSaves() {
			Logger.Debug( "Start" );
			List<SaveDataModel> saves = this.saveRepository.Rows
				.Select( saveRow => this.ConvertModelOfRepositoryToModelOfPresenter( saveRow ) )
				.OrderBy( save => save.id )
				.ToList();
			Logger.Debug( "End" );
			return saves;
		}

		/// <summary>
		/// RepositoryModelからPresenterModelへの変換
		/// </summary>
		/// <param name="save">RepositoryModel</param>
		/// <returns>PresenterModel</returns>
		public SaveDataModel ConvertModelOfRepositoryToModelOfPresenter( Save save ) {
			Logger.Debug( "Start" );
			SaveDataModel model = new SaveDataModel() {
				id = save.id ,
				userName = "" ,
				exsitsAlreadyData = save.exsitsAlreadyData ,
				latestUpdateDateTime = DateTimeUtil.ConvertStringToDateTime( save.latestUpdateDatetime ) ,
				clearedChapters = this.chapterClearStatusRepository.Rows
						.Where( row => row.saveId == save.id )
						.Select( row => new ChapterModel() { Id = row.chapterId } )
						.OrderBy( chapterClearStatus => chapterClearStatus.Id )
						.ToList()
			};
			Logger.Debug( "End" );
			return model;
		}

		/// <summary>
		/// 新規にセーブデータを作成し保存
		/// </summary>
		/// <param name="id"></param>
		public SaveDataModel CreateNewSave( int id ) {
			Logger.Debug( "Start" );
			Save save = this.saveRepository.Rows.FirstOrDefault( row => row.id == id );
			if( save.exsitsAlreadyData ) {
				Logger.Warning( "Save Exists Already." );
				return this.ConvertModelOfRepositoryToModelOfPresenter( save );
			}
			save.exsitsAlreadyData = true;
			save.latestUpdateDatetime = this.GetNowString();
			save.isReverseHorizontalCamera = false;
			save.isReverseVerticalCamera = false;
			this.saveRepository.Write();
			Logger.Debug( "End" );
			return this.ConvertModelOfRepositoryToModelOfPresenter( save );
		}
		
		/// <summary>
		/// 指定フォーマットで現在日時を返す
		/// </summary>
		/// <returns>現在日時</returns>
		public string GetNowString() {
			Logger.Debug( "Start" );
			string now = DateTimeUtil.GetNowString();
			Logger.Debug( $"Now is {now}." );
			Logger.Debug( "End" );
			return now;
		}

		/// <summary>
		/// セーブデータを削除する
		/// </summary>
		/// <param name="id">セーブデータID</param>
		public void DeleteSaveData( int id ) {
			Logger.Debug( "Start" );
			Logger.Debug( $"Id is {id}." );

			this.saveRepository.Rows.FirstOrDefault( s => s.id == id ).exsitsAlreadyData = false;
			this.saveRepository.Write();

			Logger.Debug( "End" );
		}

	}

}
