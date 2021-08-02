using Hangout.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.Centers
{
    public class FollowCenter
    {

        public HangoutContext HangoutContext { get; } //連接資料庫內容
        public FollowCenter(HangoutContext hangoutContext)
        {
            HangoutContext = hangoutContext;
        }

        /// <summary>
        /// 藉由追蹤編碼 取得會員內容
        /// </summary>
        /// <param name="followId"></param>
        /// <returns></returns>
        public Follow GetFollow (int followId)
        {
            return HangoutContext.Follows.Where(data => data.FollowId == followId).SingleOrDefault();
        }

        /// <summary>
        /// 查詢會員所有追蹤者
        /// </summary>
        /// <param name="ProtagonistId"></param>
        /// <returns></returns>
        public IEnumerable<Follow> GetAllFollows(int protagonistId)
      
        {
            return HangoutContext.Follows.Where(data => data.ProtagonistId == protagonistId);
        }

        /// <summary>
        /// 查詢會員所有被追蹤名單
        /// </summary>
        /// <param name="ObjectId"></param>
        /// <returns></returns>
        public IEnumerable<Follow> GetAllFollowsByObject(int ObjectId)

        {
            return HangoutContext.Follows.Where(data => data.ObjectId == ObjectId);
        }

        /// <summary>
        /// 檢查是否追蹤過
        /// </summary>
        /// <param name="protagonistId">主角</param>
        /// <param name="ObjectId">對象</param>
        /// <returns></returns>
        public Follow CheckFollow(int protagonistId , int ObjectId)
        {
            return HangoutContext.Follows.Where(data => data.ProtagonistId == protagonistId && data.ObjectId == ObjectId).SingleOrDefault();
        }

        public Follow GetFollowByMember(int memberId ,int followId)
        {
            return HangoutContext.Follows.Where(data => data.ProtagonistId == memberId && data.FollowId == followId).SingleOrDefault();
        }

    }
}
