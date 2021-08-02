using Hangout.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.Controllers
{
    public class MessageCenter
    {
        public HangoutContext HangoutContext { get; }
        public MessageCenter(HangoutContext hangoutContext)
        {
            HangoutContext = hangoutContext;
        }

        /// <summary>
        /// 查找指定活動編號內所有的留言
        /// </summary>
        /// <param name="EventId">活動編號</param>
        /// <returns></returns>
        public IEnumerable<Message> GetMessageByEvent(int EventId)
        {
            return HangoutContext.Messages.Where(data => data.EventId == EventId);
        }
        /// <summary>
        /// 查找與指定留言編號相同之留言
        /// </summary>
        /// <param name="MessageID">留言編號</param>
        /// <returns></returns>
        public IEnumerable<Message> GetMessages(int MessageID)
        {
            return HangoutContext.Messages.Where(data => data.MessageId == MessageID);
        }

        /// <summary>
        /// 查找指定留言編號與會員編號相同之留言
        /// </summary>
        /// <param name="MessageID">留言編號</param>
        /// <param name="MemberId">會員編號</param>
        /// <returns></returns>
        public Message GetMessagesByMember(int MessageID,int MemberId)
        {
            return HangoutContext.Messages.Where(data => data.MessageId == MessageID && data.MemberId==MemberId).SingleOrDefault();
        }


    }
}
