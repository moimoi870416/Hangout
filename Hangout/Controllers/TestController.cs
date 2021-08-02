using Hangout.Centers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hangout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private readonly TestCenter TestCenter;
        private readonly MemberCenter MemberCenter;
        public TestController(TestCenter testCenter,MemberCenter memberCenter)
        {
            TestCenter = testCenter;
            MemberCenter = memberCenter;
        }





        // GET: api/<TestController>
        [HttpGet("Event")]
        public ActionResult GetEvent()
        {
            var ans = TestCenter.GetAllEvent().ToArray();

            return Ok(ans);
        }

        // GET api/<TestController>/5
        [HttpGet("Member")]
        public ActionResult GetMember()
        {
            var ans = TestCenter.GetAllMember().ToArray();

            return Ok(ans);
        }

        [HttpGet("Message")]
        public ActionResult GetMessage()
        {
            var ans = TestCenter.GetAllMessage().ToArray();

            return Ok(ans);
        }

        [HttpGet("Comment")]
        public ActionResult GetComment()
        {
            var ans = TestCenter.GetAllComment().ToArray();

            return Ok(ans);
        }

        [HttpGet("Participant")]
        public ActionResult GetParticipants()
        {
            var ans = TestCenter.GetAllParticipants().ToArray();

            return Ok(ans);
        }

        [HttpGet("Favorite")]
        public ActionResult GetFavorite()
        {
            var ans = TestCenter.GetAllFavorite().ToArray();

            return Ok(ans);
        }

        [HttpGet("Follow")]
        public ActionResult GetFollow()
        {
            var ans = TestCenter.GetAllFollow().ToArray();

            return Ok(ans);
        }

        [HttpGet("Relationship")]
        public ActionResult GetRelationship()
        {
            var ans = TestCenter.GetAllRelationships().ToArray();

            return Ok(ans);
        }

        [HttpGet("Invite")]
        public ActionResult GetInvitep()
        {
            var ans = TestCenter.GetInvites().ToArray();

            return Ok(ans);
        }


    }
}
