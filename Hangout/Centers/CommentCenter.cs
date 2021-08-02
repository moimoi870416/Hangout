using Hangout.Models.db;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.Centers
{
    public class CommentCenter
    {
        public HangoutContext HangoutContext { get; } //連接資料庫內容
        public CommentCenter(HangoutContext hangoutContext)
        {
            HangoutContext = hangoutContext;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        public IEnumerable<Comment> GetMemberComment(int MemberId)
        {
            return HangoutContext.Comments.Where(data => data.MemberId == MemberId);
        }


        /// <summary>
        /// 查找指定對象編號內所有的評論
        /// </summary>
        /// <param name="ObjectId">評分對象編號</param>
        /// <returns></returns>
        public IEnumerable<Comment> GetCommentByObjectId(int ObjectId)
        {
            return HangoutContext.Comments.Where(data => data.ObjectId == ObjectId);
        }
        /// <summary>
        /// 查找指定評論編號
        /// </summary>
        /// <param name="CommentId">留言編號</param>
        /// <returns></returns>
        public IEnumerable<Comment> GetComment(int CommentId)
        {
            return HangoutContext.Comments.Where(data => data.CommentId == CommentId);
        }
    }
}
