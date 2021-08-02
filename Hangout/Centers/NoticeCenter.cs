using Hangout.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.Centers
{
    public class NoticeCenter
    {
        public HangoutContext HangoutContext { get; } //連接資料庫內容
        public NoticeCenter(HangoutContext hangoutContext)
        {
            HangoutContext = hangoutContext;
        }

        /// <summary>
        /// 取得所有通知
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Notice> GetAllNotice()
        {
            return HangoutContext.Notices.Where(data => data.NoticeId < 1000);
        }

        /// <summary>
        /// 藉由編號獲得所有內容
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        public Notice GetNotice (int noticeId)
        {
            return HangoutContext.Notices.Where(data => data.NoticeId == noticeId).SingleOrDefault();
        }

        /// <summary>
        /// 取得該會員所有通知
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public IEnumerable<Notice> GetNoticesBymember(int memberId)
        {
            return HangoutContext.Notices.Where(data => data.ObjectId == memberId);
        }


    }
}
