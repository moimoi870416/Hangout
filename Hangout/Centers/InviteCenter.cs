using Hangout.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.Centers
{
    public class InviteCenter
    {
        public HangoutContext HangoutContext { get; } //連接資料庫內容
        public InviteCenter(HangoutContext hangoutContext)
        {
            HangoutContext = hangoutContext;
        }


        /// <summary>
        /// 藉由邀請編號取得所有內容
        /// </summary>
        /// <param name="inviteId"></param>
        /// <returns></returns>
        public Invite GetInvites(int inviteId)
        {
            return HangoutContext.Invites.Where(data => data.InviteId == inviteId).SingleOrDefault();
        }

        /// <summary>
        ///  輸入被邀請者取得所有活動
        /// </summary>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        public IEnumerable<Invite> GetEventByInviter(int MemberId)
        {
            return HangoutContext.Invites.Where(data => data.ObjectId == MemberId);
        }


        /// <summary>
        /// 檢查是否邀請過
        /// </summary>
        /// <param name="MemberId">邀請人</param>
        /// <param name="objectId">被邀請人</param>
        /// <param name="EventId">活動內容</param>
        /// <returns></returns>
        public Invite InviterCheck(int MemberId, int objectId,int EventId)
        {
            return HangoutContext.Invites.Where(data => data.EventId == EventId && data.ProtagonistId == MemberId && data.ObjectId==objectId).SingleOrDefault();
        }

        public Invite UpdateInviter( int objectId, int EventId)
        {
            return HangoutContext.Invites.Where(data => data.EventId == EventId  && data.ObjectId == objectId).SingleOrDefault();
        }



    }
}
