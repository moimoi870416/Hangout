using Hangout.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.Centers
{
    public class ParticipantCenter
    {
        public HangoutContext HangoutContext { get; } //連接資料庫內容
        public ParticipantCenter(HangoutContext hangoutContext)
        {
            HangoutContext = hangoutContext;
        }

     
        /// <summary>
        ///輸入參加者編號 獲得內容 
        /// </summary>
        /// <param name="participantId">參加者編號</param>
        /// <returns></returns>
        public IEnumerable<Participant> GetParticipants(int participantId)
        {
            return HangoutContext.Participants.Where(data => data.ParticipantId == participantId);
        }

        /// <summary>
        /// 輸入活動編號取得所有參加者
        /// </summary>
        /// <param name="EventId">活動編號</param>
        /// <returns></returns>
        public IEnumerable<Participant> GetParticipantsByEvent(int EventId)
        {
            return HangoutContext.Participants.Where(data => data.EventId == EventId);
        }

        /// <summary>
        /// 輸入參加者編號取得所有參加者
        /// </summary>
        /// <param name="EventId">活動編號</param>
        /// <returns></returns>
        public IEnumerable<Participant> GetEventByParticipants(int MemberId)
        {
            return HangoutContext.Participants.Where(data => data.Participanter == MemberId);
        }


        /// <summary>
        /// 檢查是否參加過
        /// </summary>
        /// <param name="MemberId"></param>
        /// <param name="EventId"></param>
        /// <returns></returns>
        public Participant ParticipantCheck(int MemberId, int EventId)
        {
          return  HangoutContext.Participants.Where(data => data.EventId == EventId && data.Participanter == MemberId).SingleOrDefault();
        }



    }
}
