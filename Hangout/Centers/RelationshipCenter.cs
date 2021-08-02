using Hangout.Models.db; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.Centers
{
    public class RelationshipCenter
    {
        public HangoutContext HangoutContext { get; } //連接資料庫內容
        public RelationshipCenter(HangoutContext hangoutContext)
        {
            HangoutContext = hangoutContext;
        }

      
        /// <summary>
        /// 查詢會員相關之好友
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public IEnumerable<Relationship> GetRelationships(int memberId)
        {
           return HangoutContext.Relationships.Where(data => data.ProtagonistId == memberId);
        }

        /// <summary>
        /// 計算數量
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="ObjectId"></param>
        /// <returns></returns>
        public IEnumerable<Relationship> CheckCount(int memberId , int ObjectId)
        {
         return   HangoutContext.Relationships.Where(data => data.ProtagonistId == memberId && data.ObjectId == ObjectId).ToArray();
        }
        /// <summary>
        /// 檢查是否存在
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="ObjectId"></param>
        /// <returns></returns>
        public Relationship Check(int memberId, int ObjectId)
        {
            return HangoutContext.Relationships.Where(data => data.ProtagonistId == memberId && data.ObjectId == ObjectId).FirstOrDefault();
        }


    }
}
