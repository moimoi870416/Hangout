using Hangout.Centers;
using Hangout.Models.db;
using Hangout.Parameter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Type = Hangout.Models.db.Type;
using System.Collections;
using Hangout.Output;
using Hangout.DTO;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using Hangout.PutOutput;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hangout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly MemberCenter MemberCenter;
        private readonly CommentCenter CommentCenter;
        private readonly EventCenter EventCenter;
        private readonly ParticipantCenter PartitionCenter;
        private readonly FavoriteCenter FavoriteCenter;
        private readonly FollowCenter FollowCenter;
        private readonly InviteCenter InviteCenter;

        public MemberController(MemberCenter memberCenter, CommentCenter commentCenter, EventCenter eventCenter, ParticipantCenter partitionCenter
            , FavoriteCenter favoriteCenter, FollowCenter followCenter, InviteCenter inviteCenter)
        {
            MemberCenter = memberCenter;
            CommentCenter = commentCenter;
            EventCenter = eventCenter;
            PartitionCenter = partitionCenter;
            FavoriteCenter = favoriteCenter;
            FollowCenter = followCenter;
            InviteCenter = inviteCenter;
        }

        /// <summary>
        /// 取得會員完整資訊 兩張表的內容
        /// </summary>
        /// <param name="memberId">會員編號</param>
        /// <returns></returns>
        private MemberInfo GetMemberInfo(int memberId)
        {
            var queryRecord = MemberCenter.GetMember(memberId);
            var commentRecord = CommentCenter.GetMemberComment(memberId);

            var type = MemberCenter.GetType(memberId);
            var holeEvents = GetHoldEventIds(memberId);
            var joinEvents = GetJoinEventIds(memberId);
            var inviteEvent = GetInviteEventIds(memberId);
            var photos = MemberCenter.GetPhoto(memberId);

            var memberInfo = new MemberInfo(queryRecord.MemberId, queryRecord.Account, queryRecord.Password, queryRecord.Name, photos.Select(data => data.Path), GenderOutput(queryRecord.Gender), queryRecord.Birth
                , queryRecord.CityId, queryRecord.Category, queryRecord.JobTitle, queryRecord.Intro, type.Select(data => data.TypeNum), commentRecord.Select(data => data.CommentId),
                commentRecord.Select(data => data.ObjectId), commentRecord.Select(data => data.Rate), commentRecord.Select(data => data.CommentContent),
                commentRecord.Select(data => data.Time), holeEvents, joinEvents, inviteEvent);
            return memberInfo;
        }

        private bool changeStatus(byte status)
        {
            return status == 1 ? true : false;
        }


        /// <summary>
        /// 藉由會員編號找到他舉辦過得活動Id
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        private List<int> GetHoldEventIds(int memberId)
        {
            List<int> events = new List<int>();
            var holdEvent = MemberCenter.GetEventByMember(memberId);
            foreach (var data in holdEvent)
            {
                events.Add(data.EventId);
            }
            return events;
        }

        /// <summary>
        /// 藉由會員編號找到他參加過的活動id
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        private List<int> GetJoinEventIds(int memberId)
        {
            List<int> events = new List<int>();

            var joinEvent = PartitionCenter.GetEventByParticipants(memberId);
            foreach (var data in joinEvent)
            {
                events.Add(data.EventId);
            }
            return events;
        }

        /// <summary>
        /// 藉由會員編號找到他被邀請的活動id
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        private List<int> GetInviteEventIds(int memberId)
        {
            List<int> events = new List<int>();

            var inviteEvent = InviteCenter.GetEventByInviter(memberId);
            foreach (var data in inviteEvent)
            {
                events.Add(data.EventId);
            }
            return events;
        }


        /// <summary>
        /// 新增興趣
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="typeGroup"></param>
        private void InserType(int memberId, List<int> typeGroup)
        {
            if (typeGroup == null)
            {
                return;
            }
            var currentData = MemberCenter.GetType(memberId).ToList();
            foreach (var data in currentData)
            {
                MemberCenter.HangoutContext.Remove(data);
            }
            for (int i = 0; i < typeGroup.Count; i++)
            {
                var Type = new Type();
                Type.TypeNum = typeGroup[i];
                Type.MemberId = memberId;
                MemberCenter.HangoutContext.Add(Type);
                MemberCenter.HangoutContext.SaveChanges();
            }
        }

        /// <summary>
        /// 性別轉型
        /// </summary>
        /// <param name="Gender">性別</param>
        /// <returns></returns>
        private string ChangeGender(byte? Gender)
        {
            return Gender == 1 ? "F" : "M";
        }

        /// <summary>
        /// 性別轉型
        /// </summary>
        /// <param name="Gender">性別</param>
        /// <returns></returns>
        private byte GenderOutput(string Gender)
        {
            return Gender == "F" ? (byte)1 : (byte)2;
        }

        /// <summary>
        /// 產生照片檔名
        /// </summary>
        /// <param name="Account">帳號</param>
        /// <param name="Password">密碼</param>
        /// <returns></returns
        private string GetPath(string Account, int i)
        {
            //自己製造路徑 path.combine
            return $"photo/{Account}-{i.ToString()}.jpg";
        }

        /// <summary>
        ///  避免把ID傳出 
        /// </summary>
        /// <param name="addData">新增資料</param>
        /// <returns></returns>
        private int addMember(MemberInput addData)
        {
            string newPassword = GetMd5Method(addData.Password);

            var addMember = new Member(addData.Account, newPassword, addData.Name, ChangeGender(addData.Gender), addData.Birth,
                addData.CityId, addData.Category, addData.JobTitle, addData.Intro);
            CommentCenter.HangoutContext.Add(addMember);
            CommentCenter.HangoutContext.SaveChanges();

            return addMember.MemberId;
        }

        /// <summary>
        /// 儲存照片陣列
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="phtots"></param>
        /// <returns></returns>
        private async Task Savephoto(int memberId, List<IFormFile> phtots)
        {

            var account = MemberCenter.GetMember(memberId).Account;

            for (int i = 0; i < phtots.Count; i++)
            {
                string PhotoPath = GetPath(account, i);
                var photo = new Photo();
                photo.MemberId = memberId;
                photo.Path = PhotoPath;
                MemberCenter.HangoutContext.Add(photo);
                CommentCenter.HangoutContext.SaveChanges();
                //存檔案
                FileStream fileStream = new FileStream(PhotoPath, FileMode.Create);
                var file = phtots[i];
                await file.CopyToAsync(fileStream);
            }
        }

        private async Task AddCover(int memberId, IFormFile cover)
        {
            var member = MemberCenter.GetMember(memberId);
            string photoPath = GetPath(member.Account, 0);

            var addPhoto = new Photo();
            addPhoto.MemberId = memberId;
            addPhoto.Path = photoPath;

            MemberCenter.HangoutContext.Add(addPhoto);
            MemberCenter.HangoutContext.SaveChanges();


            FileStream fileStream = new FileStream(photoPath, FileMode.Create);
            var file = cover;
            await file.CopyToAsync(fileStream);


        }

        private async Task UpdateCover(int memberId, IFormFile photo)
        {
            string photoPath = "";

            if (photo == null)
            {
                return;
            }

            var member = MemberCenter.GetMember(memberId);

            photoPath = GetPath(member.Account, 0);
            if (System.IO.File.Exists(photoPath))
            {
                System.IO.File.Delete(photoPath);
            }

            

            FileStream fileStream = new FileStream(photoPath, FileMode.Create);
            var file = photo;
            await file.CopyToAsync(fileStream);

        }

        /// <summary>
        /// 更新照片
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="photo"></param>
        private async Task UpdatePhoto(int memberId, List<IFormFile> photos)
        {
            string photoPath = "";

            if (photos == null)
            {
                return;
            }

            var member = MemberCenter.GetMember(memberId);
            var length = MemberCenter.GetPhoto(memberId).ToArray().Length;
            for (int k = 1; k < length; k++)
            {

                photoPath = GetPath(member.Account, k);
                if (System.IO.File.Exists(photoPath))
                {
                    System.IO.File.Delete(photoPath);
                }
                MemberCenter.HangoutContext.Remove(MemberCenter.GetPhotoByPath(photoPath));
                MemberCenter.HangoutContext.SaveChanges();

            }

            var account = member.Account;
            for (int i = 1; i < photos.Count + 1; i++)
            {
                {
                    photoPath = GetPath(account, i);
                    var photo = new Photo();
                    photo.MemberId = memberId;
                    photo.Path = photoPath;
                    MemberCenter.HangoutContext.Add(photo);
                    CommentCenter.HangoutContext.SaveChanges();
                    //存檔案
                    FileStream fileStream = new FileStream(photoPath, FileMode.Create);
                    var file = photos[i - 1];
                    await file.CopyToAsync(fileStream);
                }
            }
        }
        /// <summary>
        /// 密碼加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string GetMd5Method(string password)
        {
            if (password == null)
            {
                return null;
            }

            //新增一個MD5加密Provider
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

            //利用Md5Hasher將輸入字串進行轉換
            byte[] myData = md5Hasher.ComputeHash(Encoding.Default.GetBytes(password));

            //新增一個StringBuilder實體
            StringBuilder stringBuilder = new StringBuilder();

            //將輸入內容轉換成加密字串
            for (int i = 0; i < myData.Length; i++)
            {
                stringBuilder.Append(myData[i].ToString("x"));
            }

            return string.Format(stringBuilder.ToString());
        }



        /// <summary>
        /// 會員是否存在
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        private bool IsMemberExist(int memberId)
        {
            return MemberCenter.GetMember(memberId) == null ? true : false;
        }

        /// <summary>
        /// 活動是否存在
        /// </summary>
        /// <param name="eventId">活動編號</param>
        /// <returns></returns>
        private bool IsEventExist(int eventId)
        {
            return EventCenter.GetEvent(eventId) == null ? true : false;
        }

        /// <summary>
        /// 寄信
        /// </summary>
        /// <param name="ReceiveMail"></param>
        /// <param name="password"></param>
        public static void SendAutomatedEmail(string ReceiveMail, string password)
        {
            string myMail = "hangout.web0709@gmail.com";
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            //傳送給誰
            msg.To.Add(ReceiveMail);
            // 發送人地址,姓名,編碼
            msg.From = new System.Net.Mail.MailAddress(myMail, "Hangout", System.Text.Encoding.UTF8);
            //郵件標題
            msg.Subject = "Hangout 會員忘記密碼";
            //標題編碼
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            //郵件內容
            msg.Body = @$"{ReceiveMail} <p>親愛的會員您好:</p>
            <p>您的新密碼為 </p>  { password }
            <p> 登入後請盡速至會員中心更改密碼 </p>
            <p> 如有打擾請多包涵。感謝您的配合~</p> ";
            //內容編碼
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            //格式是否為html
            msg.IsBodyHtml = true;
            //郵件優先順序
            msg.Priority = System.Net.Mail.MailPriority.High;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(myMail, "Cmoney0709");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            object userState = msg;
            try
            {
                client.SendAsync(msg, userState);

            }
            catch (System.Net.Mail.SmtpException ex)
            {

            }
        }

        /// <summary>
        ///  註冊會員
        /// </summary>
        /// <param name="addData">新增資料</param>
        /// <returns></returns>
        // POST api/<MemberController>
        [HttpPost("SignUp")]
        public async Task<ActionResult> PostSignIn([FromForm] MemberInput addData)
        {
            if (addData.Account == null || addData.Password == null || addData.Name == null
               || addData.Gender == 0 || addData.CityId == 0 || addData.MemberPhoto.Count == 0
              )
            {
                return BadRequest("資料不完整,新增失敗");
            }
            var id = addMember(addData);
            InserType(id, addData.Type);
            await Savephoto(id, addData.MemberPhoto);
            return Ok(new { MemberId = id });
        }



        /// <summary>
        /// 確認會員帳號是否註冊過
        /// </summary>
        /// <param name="singin"></param>
        /// <returns></returns>
        // GET: api/<MemberController>
        [HttpPost("Check")]
        public bool PostCheck([FromBody] Singin singin)
        {
            var queryRecord = MemberCenter.GetAccout(singin.Account);
            return queryRecord == null ? false : true;
        }


        /// <summary>
        ///  會員登入
        /// </summary>
        /// <param name="Account">會員帳號</param>
        /// <param name="Password">會員密碼</param>
        /// <returns></returns>
        // GET: api/<MemberController>
        [HttpPost("LogIn")]
        public ActionResult PostLogIn([FromBody] Singin singin)
        {
            var queryRecord = MemberCenter.GetMemberByAccount(singin.Account, GetMd5Method(singin.Password));
            return queryRecord != null ? Ok(new { MemberId = queryRecord.MemberId }) : NotFound("查無此會員");
        }

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <param name="accout"></param>
        /// <returns></returns>
        [HttpPut("ForgetPassword")]
        public ActionResult PutByMemberId([FromBody] Singin singin)
        {
            var queryRecord = MemberCenter.GetAccout(singin.Account);
            if (queryRecord != null)
            {
                Random random = new Random();
                int password = random.Next(10000, 99999);
                queryRecord.Password = GetMd5Method(password.ToString());
                MemberCenter.HangoutContext.SaveChanges();
                SendAutomatedEmail(singin.Account, password.ToString());
                return Ok("已成功發信");

            }
            return NotFound("查無此會員");
        }

        /// <summary>
        /// 藉由會員編號 取得會員資訊
        /// </summary>
        /// <param name="MemberId">會員編號/param>
        /// <returns></returns>
        // GET api/<MemberController>/5
        [HttpGet("{MemberId}")]
        public ActionResult GetByMemberId(int MemberId)
        {

            if (IsMemberExist(MemberId))
            {
                return NotFound("查無此會員");
            }
            var queryRecord = GetMemberInfo(MemberId);
            return Ok(queryRecord);
        }

        /// <summary>
        /// 藉由會員編號 取得舉辦的活動資訊
        /// </summary>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        [HttpGet("{MemberId}/HostEvent")]
        public ActionResult GetHostEventByMemberId(int MemberId)
        {

            //藉由會員id 取得所有活動
            var queryRecord = MemberCenter.GetEventByMember(MemberId).ToList();
            //回傳 活動陣列 新創的類別(指定欄位)
            var holdEvent = new List<EventOutput>();
            //從 queryRecord 跌代取出每個 活動 
            foreach (var data in queryRecord)
            {
                //這是fk 對到pk
                var address = EventCenter.GetAdress(data.AddressId);
                // 新類別建構式 指定要傳出
                var evnet = new EventOutput(data.EventId, data.MemberId, data.EventName, address.CityId, address.Road, data.HostTime, data.Deadline,
                    data.EventPrice, data.Cover, data.EventContent, data.CostTime, data.PersonLimit, data.TypeId, changeStatus(data.Status), data.Lat, data.Lng);

                //加入要傳出得陣列
                holdEvent.Add(evnet);

            }
            //把陣列傳出
            return queryRecord != null ? Ok(holdEvent) : NotFound("沒有舉辦活動");
        }

        /// <summary>
        /// 藉由會員編號 取得參加的活動資訊
        /// </summary>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        [HttpGet("{MemberId}/JoinEvent")]
        public ActionResult GetJoinEventByMemberId(int MemberId)
        {
            var events = GetJoinEventIds(MemberId);
            var joinEvent = new List<EventOutput>();
            for (int i = 0; i < events.Count; i++)
            {
                var current = EventCenter.GetEvent(events[i]);
                var address = EventCenter.GetAdress(current.AddressId);
                var evnet = new EventOutput(current.EventId, current.MemberId, current.EventName, address.CityId, address.Road, current.HostTime, current.Deadline,
                    current.EventPrice, current.Cover, current.EventContent, current.CostTime, current.PersonLimit, current.TypeId, changeStatus(current.Status), current.Lat, current.Lng);
                joinEvent.Add(evnet);

            }
            return joinEvent.Count != 0 ? Ok(joinEvent) : NotFound("沒有舉辦活動");
        }

        /// <summary>
        ///  藉由會員編號 取得被邀請的活動資訊
        /// </summary>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        [HttpGet("{MemberId}/InviteEvent")]
        public ActionResult GetInviteEventByMemberId(int MemberId)
        {
            var events = GetInviteEventIds(MemberId);
            var inviteEvent = new List<EventOutput>();
            for (int i = 0; i < events.Count; i++)
            {
                var current = EventCenter.GetEvent(events[i]);
                var address = EventCenter.GetAdress(current.AddressId);
                var evnet = new EventOutput(current.EventId, current.MemberId, current.EventName, address.CityId, address.Road, current.HostTime, current.Deadline,
                    current.EventPrice, current.Cover, current.EventContent, current.CostTime, current.PersonLimit, current.TypeId, changeStatus(current.Status), current.Lat, current.Lng);
                inviteEvent.Add(evnet);
            }
            return inviteEvent.Count != 0 ? Ok(inviteEvent) : NotFound("沒有被邀請的活動");
        }

        /// <summary>
        ///  更新會員資料
        /// </summary>
        /// <param name="MemberId">會員編號</param>
        /// <param name="updateData">更新資料</param>
        /// <returns></returns>
        // PUT api/<MemberController>/5
        [HttpPut("{MemberId}")]
        public ActionResult PutByMemberId(int MemberId, [FromBody] MemberOutput updateData)
        {

            var queryRecord = MemberCenter.GetMember(MemberId);
            if (queryRecord != null)
            {
                InserType(MemberId, updateData.Type);
                queryRecord.Password = GetMd5Method(updateData.Password) ?? queryRecord.Password;
                queryRecord.Name = updateData.Name ?? queryRecord.Name;
                queryRecord.CityId = (updateData.CityId.HasValue) ? (short)(updateData.CityId) : queryRecord.CityId;
                queryRecord.Birth = (updateData.Birth.HasValue) ? (DateTime)(updateData.Birth) : queryRecord.Birth;
                queryRecord.Gender = ChangeGender(updateData.Gender);
                queryRecord.Category = updateData.Category ?? queryRecord.Category;
                queryRecord.JobTitle = updateData.JobTitle ?? queryRecord.JobTitle;
                queryRecord.Intro = updateData.Intro ?? queryRecord.Intro;
                MemberCenter.HangoutContext.SaveChanges();
                return NoContent();
            }
            return NotFound("查無此會員");
        }


        [HttpPost("/Test/{MemberId}")]
        public async Task<ActionResult> AddPhoto(int MemberId, IFormFile cover)
        {
            if (IsMemberExist(MemberId))
            {
                return NotFound("查無此會員");
            }

            await AddCover(MemberId, cover);

            return NoContent();
        }

        /// <summary>
        /// 更新封面照
        /// </summary>
        /// <param name="MemberId"></param>
        /// <param name="CoverId"></param>
        /// <param name="updateData"></param>
        /// <returns></returns>
        [HttpPut("Cover/{MemberId}")]
        public async Task<ActionResult> PutCover(int MemberId, IFormFile cover)
        {
            if (IsMemberExist(MemberId))
            {
                return NotFound("查無此會員");
            }

            await UpdateCover(MemberId, cover);

            return NoContent();
        }

        /// <summary>
        /// 更新生活照
        /// </summary>
        /// <param name="MemberId"></param>
        /// <param name="updateData"></param>
        /// <returns></returns>
        [HttpPut("Photo/{MemberId}")]
        public async Task<ActionResult> PutPhoto(int MemberId, [FromForm] PhotoPut updateData)
        {
            if (IsMemberExist(MemberId))
            {
                return NotFound("查無此會員");
            }

            await UpdatePhoto(MemberId, updateData.MemberPhoto);

            return NoContent();
        }


        /// <summary>
        /// 新增類別 避免把ID 傳出 
        /// </summary>
        /// <param name="addData">新增資料</param>
        /// <returns></returns>
        private int AddComment(CommentInput addData)
        {
            var addComment = new Comment(addData.ObjectId, addData.MemberId, addData.Rate, addData.CommentContent);
            CommentCenter.HangoutContext.Add(addComment);
            CommentCenter.HangoutContext.SaveChanges();
            return addComment.CommentId;
        }


        /// <summary>
        ///  新增評論
        /// </summary>
        /// <param name="addData">新增資料</param>
        /// <returns></returns>
        // POST api/<CommentController>
        [HttpPost("Comment")]
        public ActionResult PostByComment([FromBody] CommentInput addData)
        {
            if (addData.CommentContent == null || addData.Rate < 0)
            {
                return BadRequest("資料不完整,新增失敗");
            }
            if (IsMemberExist(addData.ObjectId) || IsMemberExist(addData.MemberId))
            {
                return BadRequest("此會員不存在");
            }


            var Id = AddComment(addData);
            return Ok(new { commentId = Id });
        }

        /// <summary>
        ///  藉由會員編號取得所有的評論
        /// </summary>
        /// <param name="ObjectId"></param>
        /// <returns></returns>
        // GET: api/<CommentController>
        [HttpGet("Comment/ObjectId={ObjectId}")]
        public ActionResult GetCommentByObjectId(int ObjectId)
        {
            var queryRecord = CommentCenter.GetCommentByObjectId(ObjectId).ToArray();
            if (queryRecord.Length == 0)
            {
                return NotFound("無符合條件評論");
            }
            return Ok(queryRecord);
        }


        /// <summary>
        ///  藉由評論編號取得 評論內容
        /// </summary>
        /// <param name="CommentId">評論編號/param>
        /// <returns></returns>
        // GET api/<CommentController>/5
        [HttpGet("Comment/{CommentId}")]
        public ActionResult GetByCommentId(int CommentId)
        {
            var queryRecord = CommentCenter.GetComment(CommentId).SingleOrDefault();
            return queryRecord != null ? Ok(queryRecord) : NotFound("查無此評論");
        }

        /// <summary>
        /// 刪除評論
        /// </summary>
        /// <param name="CommentId"></param>
        /// <returns></returns>
        [HttpDelete("Comment/{CommentId}")]
        public ActionResult DeleteByMessageId(int CommentId)
        {
            var queryRecord = CommentCenter.GetComment(CommentId).SingleOrDefault();
            if (queryRecord == null)
            {
                return NotFound("查無此評論");
            }
            CommentCenter.HangoutContext.Remove(queryRecord);
            CommentCenter.HangoutContext.SaveChanges();
            return NoContent();
        }



        /// <summary>
        /// 新增類別 避免把ID 傳出 
        /// </summary>
        /// <param name="addData">新增資料</param>
        /// <returns></returns>
        private int AddFavorite(FavoriteInput addData)
        {
            var addFavorite = new Favorite(addData.MemberId, addData.EventNum);
            FavoriteCenter.HangoutContext.Add(addFavorite);
            FavoriteCenter.HangoutContext.SaveChanges();
            return addFavorite.FavoritesId;
        }


        /// <summary>
        ///  收藏活動
        /// </summary>
        /// <param name="addData">新增資料</param>
        /// <returns></returns>
        // POST api/<CommentController>
        [HttpPost("Favorite")]
        public ActionResult PostByFavorite([FromBody] FavoriteInput addData)
        {
            if (IsMemberExist(addData.MemberId))
            {
                return BadRequest("此會員不存在");
            }
            if (IsEventExist(addData.EventNum))
            {
                return BadRequest("此活動不存在");
            }
            if (FavoriteCenter.CheckFavorite(addData.MemberId, addData.EventNum) != null)
            {
                return BadRequest("活動已收藏");
            }

            var Id = AddFavorite(addData);
            return Ok(new { favoriteId = Id });
        }

        /// <summary>
        /// 獲得活動資訊
        /// </summary>
        /// <param name="FavoriteId"></param>
        /// <returns></returns>
        [HttpGet("Favorite/{FavoriteId}")]
        public ActionResult GetByFavoriteId(int FavoriteId)
        {
            var queryRecord = FavoriteCenter.GetFavorite(FavoriteId);
            var record = EventCenter.GetEvent(queryRecord.EventNum);
            var address = EventCenter.GetAdress(record.AddressId);
            var evnet = new EventOutput(record.EventId, record.MemberId, record.EventName, address.CityId, address.Road, record.HostTime, record.Deadline,
                 record.EventPrice, record.Cover, record.EventContent, record.CostTime, record.PersonLimit, record.TypeId, changeStatus(record.Status), record.Lat, record.Lng);
            return queryRecord != null ? Ok(evnet) : NotFound("查無此活動");
        }

        /// <summary>
        /// 取得會員所有收藏活動
        /// </summary>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        [HttpGet("Favorite/MemberId={MemberId}")]
        public ActionResult GetFavoriyeByMemberId(int MemberId)
        {
            var queryRecord = FavoriteCenter.GetAllFavorite(MemberId).ToArray();
            if (queryRecord.Length == 0)
            {
                return NotFound("無符合條件評論");
            }
            var events = new List<EventOutput>();

            foreach (var data in queryRecord)
            {
                var current = EventCenter.GetEvent(data.EventNum);
                var address = EventCenter.GetAdress(current.AddressId);
                var evnet = new EventOutput(current.EventId, current.MemberId, current.EventName, address.CityId, address.Road, current.HostTime, current.Deadline,
                    current.EventPrice, current.Cover, current.EventContent, current.CostTime, current.PersonLimit, current.TypeId, changeStatus(current.Status), current.Lat, current.Lng);
                events.Add(evnet);

            }

            return Ok(events);
        }

        /// <summary>
        /// 刪除收藏活動
        /// </summary>
        /// <param name="FavoriteId"></param>
        /// <returns></returns>
        [HttpDelete("Favorite/{MemberId}/{EventId}")]
        public ActionResult DeleteByMember(int MemberId, int EventId)
        {
            var queryRecord = FavoriteCenter.CheckFavorite(MemberId, EventId);
            if (queryRecord == null)
            {
                return NotFound("查無此活動");
            }
            CommentCenter.HangoutContext.Remove(queryRecord);
            CommentCenter.HangoutContext.SaveChanges();
            return NoContent();
        }



        /// <summary>
        /// 新增類別 避免把ID 傳出 
        /// </summary>
        /// <param name="addData">新增資料</param>
        /// <returns></returns>
        private int AddFollow(FollowInput addData)
        {
            var addFollow = new Follow(addData.ProtagonistId, addData.ObjectId);
            FavoriteCenter.HangoutContext.Add(addFollow);
            FavoriteCenter.HangoutContext.SaveChanges();
            return addFollow.FollowId;
        }


        /// <summary>
        ///  追蹤
        /// </summary>
        /// <param name="addData">新增資料</param>
        /// <returns></returns>
        // POST api/<CommentController>
        [HttpPost("Follow")]
        public ActionResult PostByFollow([FromBody] FollowInput addData)
        {
            if (IsMemberExist(addData.ObjectId) || IsMemberExist(addData.ProtagonistId))
            {
                return BadRequest("此會員不存在");
            }

            if (FollowCenter.CheckFollow(addData.ProtagonistId, addData.ObjectId) != null)
            {
                return BadRequest("已追蹤");
            }

            var Id = AddFollow(addData);
            return Ok(new { FollowId = Id });
        }

        /// <summary>
        /// 刪除追蹤
        /// </summary>
        /// <param name="FollowId"></param>
        /// <returns></returns>
        [HttpDelete("Follow/{MemberId}/{ObjectId}")]
        public ActionResult DeleteFollow(int MemberId, int ObjectId)
        {
            var queryRecord = FollowCenter.CheckFollow(MemberId, ObjectId);
            if (queryRecord == null)
            {
                return NotFound("查無此追蹤");
            }
            CommentCenter.HangoutContext.Remove(queryRecord);
            CommentCenter.HangoutContext.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// /查詢會員所有追蹤即被追蹤
        /// </summary>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        [HttpGet("Follow/MemberId={MemberId}")]
        public ActionResult GetFollowByMemberId(int MemberId)
        {
            var followeds = FollowCenter.GetAllFollows(MemberId).ToArray();

            var followedList = new List<int>();

            foreach (var data in followeds)
            {

                followedList.Add(data.ObjectId);
            }
            var followers = FollowCenter.GetAllFollowsByObject(MemberId).ToArray();

            var followerList = new List<int>();
            foreach (var data in followers)
            {

                followerList.Add(data.ProtagonistId);
            }
            var follows = new FollowOutput(followedList, followerList);

            return Ok(follows);
        }

    }


}
