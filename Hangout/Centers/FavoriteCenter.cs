using Hangout.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.Centers
{
    public class FavoriteCenter
    {

        public HangoutContext HangoutContext { get; } //連接資料庫內容
        public FavoriteCenter(HangoutContext hangoutContext)
        {
            HangoutContext = hangoutContext;
        }

        /// <summary>
        /// 藉由id 取得活動編號
        /// </summary>
        /// <param name="FavoriteId"></param>
        /// <returns></returns>
        public Favorite GetFavorite(int FavoriteId)
        {
            return HangoutContext.Favorites.Where(data => data.FavoritesId == FavoriteId).SingleOrDefault();
        }


        /// <summary>
        /// 藉由會員編號取得所有活動
        /// </summary>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        public IEnumerable<Favorite> GetAllFavorite(int MemberId)
        {
            return HangoutContext.Favorites.Where(data => data.MemberId == MemberId);
        }

        /// <summary>
        /// 檢查收藏是否存在
        /// </summary>
        /// <param name="MemberId"></param>
        /// <param name="EvenId"></param>
        /// <returns></returns>
        public Favorite CheckFavorite(int MemberId, int EvenId)
        {

            return HangoutContext.Favorites.Where(data => data.MemberId == MemberId && data.EventNum==EvenId).SingleOrDefault();

        }

       

    }
}