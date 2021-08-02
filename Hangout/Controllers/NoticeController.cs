using Hangout.Centers;
using Hangout.Parameter;
using Hangout.PutOutput;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hangout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticeController : ControllerBase
    {
        private readonly NoticeCenter NoticeCenter;

        public NoticeController(NoticeCenter noticeCenter)
        {
            NoticeCenter = noticeCenter;
        }



        /// <summary>
        /// 取得該會員所有通知
        /// </summary>
        /// <returns></returns>
        // GET: api/<NoticeController>
        [HttpGet("{MemberId}")]
        public ActionResult GetNoticeByMember(int MemberId)
        {
            var ans = NoticeCenter.GetNoticesBymember(MemberId).ToArray();

            return Ok(ans);
        }

        //// GET api/<NoticeController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        /// <summary>
        /// 更新通知狀態
        /// </summary>
        /// <param name="NoticeId"></param>
        /// <param name="updateData"></param>
        /// <returns></returns>
        // PUT api/<NoticeController>/5
        [HttpPut("{NoticeId}")]
        public ActionResult NoticePut(int NoticeId, [FromBody] NoticeOutput updateData)
        {
            var queryRecord = NoticeCenter.GetNotice(NoticeId);
            if (queryRecord != null)
            {
                queryRecord.Status = (updateData.Status.HasValue) ? (byte)updateData.Status : queryRecord.Status;
                NoticeCenter.HangoutContext.SaveChanges();
                return NoContent();
            }
            return NotFound("查無此通知");

        }

        // DELETE api/<NoticeController>/5
        [HttpDelete("{NoticeId}")]
        public ActionResult Delete(int NoticeId)
        {
            var queryRecord = NoticeCenter.GetNotice(NoticeId);
            if (queryRecord == null)
            {
                return NotFound("查無此通知");
            }
            NoticeCenter.HangoutContext.Notices.Remove(queryRecord);
            NoticeCenter.HangoutContext.SaveChanges();
            return NoContent(); ;
        }
    }
}
