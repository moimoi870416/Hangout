using Hangout.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.Centers
{
    public class MemberCenter
    {

        public HangoutContext HangoutContext { get; } //連接資料庫內容
        public MemberCenter(HangoutContext hangoutContext)
        {
            HangoutContext = hangoutContext;
        }
        
        
        /// <summary>
        /// 查找指定會員編號
        /// </summary>
        /// <param name="MemberId">會員編號</param>
        /// <returns></returns>
        public Member GetMember(int MemberId)
        {
            return HangoutContext.Members.Where(data => data.MemberId == MemberId).SingleOrDefault();
        }


        /// <summary>
        /// 查找 符合之會員帳密
        /// </summary>
        /// <param name="account">會員帳號</param>
        /// <param name="password">會員密碼</param>
        /// <returns></returns>
        public Member GetMemberByAccount(string account , string password)
        {
         return HangoutContext.Members.Where(data => data.Account.Equals(account)&& data.Password.Equals(password)).SingleOrDefault();
        }

        /// <summary>
        /// 查找指定會員編號所舉辦的活動
        /// </summary>
        /// <param name="MemberId">會員編號</param>
        /// <returns></returns>
        public IEnumerable<Event> GetEventByMember(int MemberId)
        {
            return HangoutContext.Events.Where(data => data.MemberId == MemberId);
        }

        /// <summary>
        /// 尋找與會員編號相符得參加者
        /// </summary>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        public IEnumerable<Participant> GetEventByPartiton(int MemberId)
        {
          return  HangoutContext.Participants.Where(data => data.Participanter == MemberId);
        }




        /// <summary>
        /// 藉由會員編號找到所有活動
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public IEnumerable<Models.db.Type> GetType(int memberId)
        {
            return HangoutContext.Types.Where(data => data.MemberId == memberId);
        }



        /// <summary>
        /// 檢查帳號是否已註冊過
        /// </summary>
        /// <param name="account">輸入帳號</param>
        /// <returns></returns>
        public Member GetAccout(string account)
        {
            return HangoutContext.Members.Where(data => data.Account == account).SingleOrDefault();
        }

   
        /// <summary>
        /// 取得會員所有照片
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public IEnumerable<Photo> GetPhoto(int memberId)
        {
          return  HangoutContext.Photoes.Where(data => data.MemberId == memberId);
        }

        /// <summary>
        /// 藉由路徑找到圖片
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Photo GetPhotoByPath(string path)
        {
           return HangoutContext.Photoes.Where(data => data.Path == path).SingleOrDefault();
        }

    }
}