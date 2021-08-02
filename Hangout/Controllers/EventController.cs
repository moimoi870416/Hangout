using Hangout.Centers;
using Hangout.DTO;
using Hangout.Models.db;
using Hangout.Output;
using Hangout.Parameter;
using Hangout.PutOutput;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hangout.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventCenter EventCenter;
        private readonly MessageCenter MessageCenter;
        private readonly ParticipantCenter PartitionCenter;
        private readonly MemberCenter MemberCenter;
        private readonly RelationshipCenter RelationshipCenter;
        private readonly InviteCenter InviteCenter;
        private readonly FollowCenter FollowCenter;
        private Address address;

        public EventController(EventCenter eventCenter, MessageCenter messageCenter, ParticipantCenter partitionCenter, MemberCenter memberCenter,
            RelationshipCenter relationshipCenter, InviteCenter inviteCenter, FollowCenter followCenter)

        {
            EventCenter = eventCenter;
            address = new Address();
            MessageCenter = messageCenter;
            PartitionCenter = partitionCenter;
            MemberCenter = memberCenter;
            RelationshipCenter = relationshipCenter;
            InviteCenter = inviteCenter;
            FollowCenter = followCenter;
        }

        /// <summary>
        /// 取得完整活動資訊陣列
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private IEnumerable<EventInfo> GetAllEventInfos(DateTime? date)
        {
            var queryRecord = EventCenter.GetEventByDate(date);
            List<EventInfo> ans = new List<EventInfo>();
            foreach (var data in queryRecord.ToArray())
            {
                ans.Add(GetEventInfo(data.EventId));
            }
            return ans;
        }


        /// <summary>
        /// 取得完整活動資訊
        /// </summary>
        /// <param name="EventID"></param>
        /// <returns></returns>
        private EventInfo GetEventInfo(int EventID)
        {
            var queryRecord = EventCenter.GetEvent(EventID);
            var addressRecord = EventCenter.GetEvent(EventID);
            var messageCord = MessageCenter.GetMessageByEvent(EventID);
            var partitionCord = PartitionCenter.GetParticipantsByEvent(EventID);
            var address = EventCenter.GetAdress(addressRecord.AddressId);
            var cover = GetCover(queryRecord.MemberId, queryRecord.HostTime);


            var eventInfo = new EventInfo(queryRecord.EventId, queryRecord.EventName, address.CityId, address.Road, queryRecord.HostTime, queryRecord.Deadline, queryRecord.EventPrice,
                cover, queryRecord.EventContent, queryRecord.CostTime, queryRecord.PersonLimit, queryRecord.TypeId, queryRecord.MemberId, queryRecord.Status,
                 messageCord.Select(data => data.MessageId), messageCord.Select(data => data.MemberId), messageCord.Select(data => data.Time), messageCord.Select(data => data.MessageContent),
                 partitionCord.Select(data => data.ParticipantId), partitionCord.Select(data => data.Participanter), partitionCord.Select(data => data.Motivation), queryRecord.Lat, queryRecord.Lng

                 );
            return eventInfo;

        }

        /// <summary>
        /// 新增地址
        /// </summary>
        /// <param name="cityId">城市編碼</param>
        /// <param name="road">路</param>
        /// <returns></returns>
        private int InsertAddress(short cityId, string road)
        {
            address.CityId = cityId;
            address.Road = road;
            EventCenter.HangoutContext.Add(address);
            EventCenter.HangoutContext.SaveChanges();
            return address.AddressId;
        }

        /// <summary>
        /// 更新地址
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="road"></param>
        private void UpdateAddress(short? cityId, string road)
        {
            address.CityId = (cityId == null) ? address.CityId : cityId.Value;
            address.Road = road ?? address.Road;
            EventCenter.HangoutContext.SaveChanges();
        }


        /// <summary>
        /// bool 改成數字
        /// </summary>
        /// <param name="status">活動狀態</param>
        /// <returns></returns>
        private byte ChangeStatus(bool status)
        {
            return status == true ? (byte)1 : (byte)2;
        }

        /// <summary>
        /// 取得活動照片路徑
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="hostTime"></param>
        /// <returns></returns>
        private string GetCover(int? memberId, DateTime hostTime)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = hostTime.Subtract(dtStart);
            long timeStamp = toNow.Ticks;
            timeStamp = long.Parse(timeStamp.ToString().Substring(0, timeStamp.ToString().Length - 4));


            //自己製造路徑 path.combine
            return $"photo/{memberId}-{timeStamp.ToString()}.jpg";

        }
        /// <summary>
        /// 新增活動 避免ID傳出
        /// </summary>
        /// <param name="addEvent"></param>
        private void Noticefans(Event addEvent)
        {
            var list = FollowCenter.GetAllFollowsByObject(addEvent.MemberId).ToArray();
            if (list.Count() == 0)
            {
                return;
            }
            foreach (var data in list)
            {
                var notice = new Notice(addEvent.MemberId, "你正在關注的夥伴舉辦了新的活動", data.ProtagonistId, addEvent.EventId);
                MessageCenter.HangoutContext.Add(notice);
                EventCenter.HangoutContext.SaveChanges();
            }
        }




        /// <summary>
        ///新增類別 避免把ID 以及圖片儲存 新增的時候把EventId傳出
        /// </summary>
        /// <param name="addData">新增資料</param>
        /// <returns></returns>
        private async Task<int> AddEvent(EventInput addData)
        {
            var addressId = InsertAddress(addData.CityId, addData.Road);

            string path = GetCover(addData.MemberId, addData.HostTime);
            //路徑存進資料庫
            var addEvent = new Event(addData.EventName, addressId, addData.HostTime, addData.Deadline,
                  addData.EventPrice, path, addData.EventContent, addData.CostTime, addData.PersonLimit, addData.TypeId,
                  addData.MemberId, addData.Lat, addData.Lng);
            EventCenter.HangoutContext.Add(addEvent);
            EventCenter.HangoutContext.SaveChanges();
            Noticefans(addEvent);

            //存檔案
            FileStream fileStream = new FileStream(path, FileMode.Create);
            var file = addData.Cover;
            await file.CopyToAsync(fileStream);
            return addEvent.EventId;
        }
        /// <summary>
        /// 更新照面內容
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="photo"></param>
        private void UpdatePhoto(int eventId, IFormFile photo)
        {
            if (photo == null)
            {
                return;
            }
            var Event = EventCenter.GetEvent(eventId);
            string PhotoPath = GetCover(Event.MemberId, Event.HostTime);


            if (System.IO.File.Exists(PhotoPath))
            {
                System.IO.File.Delete(PhotoPath);
            }

            FileStream fileStream = new FileStream(PhotoPath, FileMode.Create);
            var file = photo;
            file.CopyToAsync(fileStream);
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
        /// 會員是否存在
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        private bool IsMemberExist(int memberId)
        {
            return MemberCenter.GetMember(memberId) == null ? true : false;
        }

        /// <summary>
        ///  新增活動
        /// </summary>
        /// <param name="addData">新增資料</param>
        /// <returns></returns>
        // POST api/<EventController>
        [HttpPost()]
        public async Task<ActionResult> PostEvent([FromForm] EventInput addData)
        {
            if (addData.EventName == null || addData.CityId == 0 || addData.Road == null || addData.Cover == null ||
                addData.EventContent == null || addData.TypeId == 0 || addData.CostTime == 0 || addData.PersonLimit == 0 || addData.MemberId == 0)
            {
                return BadRequest("資料不完整,新增失敗");
            }

            if (IsMemberExist(addData.MemberId))
            {
                return BadRequest("會員不存在,新增失敗");
            }

            if (EventCenter.GetEventCheck(addData.MemberId, addData.HostTime) != null)
            {
                return BadRequest("舉辦活動時間以重複");
            }

            var id = await AddEvent(addData);

            return Ok(new { EventId = id });
        }

        /// <summary>
        ///  藉由活動編號取得到活動所有資訊
        /// </summary>
        /// <param name="EventId">活動編號</param>
        /// <returns></returns>
        //GET api/<EventController>/5
        [HttpGet("{EventId}")]
        public ActionResult GetByEventId(int EventId)
        {
            if (IsEventExist(EventId))
            {
                return NotFound("查無此活動");
            }

            var queryRecord = GetEventInfo(EventId);
            return Ok(queryRecord);
        }

        /// <summary>
        ///  取得指定日期前所有活動
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <returns></returns>
        // GET: api/<EventController>
        [HttpGet("date={date}")]
        public ActionResult GetEventByDate(DateTime? date)
        {
            var queryRecord = GetAllEventInfos(date);
            if (queryRecord.Count() == 0)
            {
                return NotFound("無符合條件之活動");
            }
            return Ok(queryRecord);
        }



        /// <summary>
        ///  房主更新活動內容
        /// </summary>
        /// <param name="EventId">活動編號</param>
        /// <param name="MemberId">會員編號</param>
        /// <param name="updateData">更新資料</param>
        /// <returns></returns>
        // PUT api/<EventController>/5
        [HttpPut("{EventId}/{MemberId}")]

        public ActionResult PutByEventId(int EventId, int MemberId, [FromForm] EventPut updateData)
        {

            var queryRecord = EventCenter.GetEventByMember(EventId, MemberId);
            if (queryRecord != null)
            {

                UpdateAddress(updateData.CityId, updateData.Road);
                queryRecord.EventName = updateData.EventName ?? queryRecord.EventName;
                queryRecord.HostTime = updateData.HostTime ?? queryRecord.HostTime;
                queryRecord.Deadline = updateData.Deadline ?? queryRecord.Deadline;
                queryRecord.EventPrice = (updateData.EventPrice.HasValue) ? (int)updateData.EventPrice : queryRecord.EventPrice;
                UpdatePhoto(EventId, updateData.Cover);
                queryRecord.EventContent = updateData.EventContent ?? queryRecord.EventContent;
                queryRecord.CostTime = updateData.CostTime ?? queryRecord.CostTime;
                queryRecord.PersonLimit = updateData.PersonLimit ?? queryRecord.PersonLimit;
                queryRecord.Status = ChangeStatus(updateData.Status.HasValue);
                queryRecord.TypeId = (updateData.TypeId.HasValue) ? (short)updateData.TypeId : queryRecord.TypeId;
                queryRecord.Lat = updateData.Lat ?? queryRecord.Lat;
                queryRecord.Lng = updateData.Lng ?? queryRecord.Lng;
                EventCenter.HangoutContext.SaveChanges();
                return NoContent(); ;
            }
            return NotFound("無此筆活動資料,更新失敗");
        }

        /// <summary>
        /// 新增類別 避免把ID 傳出 
        /// </summary>
        /// <param name="addData">新增資料</param>
        /// <returns></returns>
        private int AddMessage(MessageInput addData)
        {
            if (addData.Status != 0)
            {
                var AddMessage = new Message(addData.EventId, addData.MemberId, addData.MessageContent, addData.Status);
                MessageCenter.HangoutContext.Add(AddMessage);
                NoticeMessage(AddMessage);
                return AddMessage.MessageId;
            }
            var addMessage = new Message(addData.EventId, addData.MemberId, addData.MessageContent);
            MessageCenter.HangoutContext.Add(addMessage);
            NoticeMessage(addMessage);
            return addMessage.MessageId;
        }

        /// <summary>
        /// post 
        /// </summary>
        /// <param name="Message"></param>
        private void NoticeMessage(Message Message)
        {
            switch(Message.Status)
            {
                case 2:
                    var partition = PartitionCenter.GetParticipantsByEvent(Message.EventId);
                    foreach (var data in partition.ToArray())
                    {
                        var Notice = new Notice(Message.MemberId, "在你參與的活動中留言", data.Participanter, Message.EventId);
                        if (Message.MemberId == data.Participanter)
                        {
                            continue;
                        }
                        MessageCenter.HangoutContext.Add(Notice);
                        MessageCenter.HangoutContext.SaveChanges();
                    }
                    break;
            }
           var holder= EventCenter.GetEvent(Message.EventId).MemberId;
            var notice = new Notice(Message.MemberId, "在你參與的活動中留言", holder, Message.EventId);
            MessageCenter.HangoutContext.Add(notice);
            MessageCenter.HangoutContext.SaveChanges();
        }

        /// <summary>
        ///新增留言
        /// </summary>
        /// <param name="addData">新增內容</param>
        /// <returns></returns>
        // POST api/<MessageController>
        [HttpPost("Message")]
        public ActionResult PostByMessage([FromBody] MessageInput addData)
        {

            if (addData.MessageContent == null)
            {
                return BadRequest("資料不完整,新增失敗");
            }

            if (IsEventExist(addData.EventId))
            {
                return NotFound("活動不存在");
            }
            if (IsMemberExist(addData.MemberId))
            {
                return NotFound("會員不存在");
            }

            var Id = AddMessage(addData);
            return Ok(new { messageId = Id });
        }

        /// <summary>
        ///  藉由留言編號取得 留言內容
        /// </summary>
        /// <param name="MessageId">留言編號/param>
        /// <returns></returns>
        // GET api/<MessageController>/5
        [HttpGet("Message/{MessageId}")]
        public ActionResult GetByMessageId(int MessageId)
        {
            var queryRecord = MessageCenter.GetMessages(MessageId).SingleOrDefault();
            return queryRecord != null ? Ok(queryRecord) : NotFound("查無此留言");
        }

        /// <summary>
        ///取得指定活動編號之所有留言
        /// </summary>
        /// <param name="EventId">活動編號</param>
        /// <returns></returns>
        //GET: api/<MessageController>
        [HttpGet("Message/EventId={EventId}")]
        public ActionResult GetMessageByEventId(int EventId)
        {
            var queryRecord = MessageCenter.GetMessageByEvent(EventId).ToArray();
            if (queryRecord.Length == 0)
            {
                return NotFound("無符合條件之留言");
            }
            return Ok(queryRecord);
        }


        /// <summary>
        /// 更改留言內容
        /// </summary>
        /// <param name="messageId">留言編號</param>
        /// <param name="MemberId">會員編號</param>
        /// <param name="updateData">修改內容</param>
        /// <returns></returns>
        // PUT api/<MessageController>/5
        [HttpPut("Message/{MessageId}/{MemberId}")]
        public ActionResult PutByMessageId(int MessageId, int MemberId, [FromBody] MessageOutput updateData)
        {
            var queryRecord = MessageCenter.GetMessagesByMember(MessageId, MemberId);
            if (queryRecord != null)
            {
                queryRecord.MessageContent = updateData.MessageContent ?? queryRecord.MessageContent;
                queryRecord.Status = updateData.Status ?? queryRecord.Status;
                MessageCenter.HangoutContext.SaveChanges();
                return NoContent();
            }
            return NotFound("查無此留言");
        }
        /// <summary>
        ///  刪除留言
        /// </summary>
        /// <param name="MessageId"></param>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        // DELETE api/<MessageController>/5
        [HttpDelete("Message/{MessageId}/{MemberId}")]
        public ActionResult DeleteByMessageId(int MessageId, int MemberId)
        {
            var queryRecord = MessageCenter.GetMessagesByMember(MessageId, MemberId);
            if (queryRecord == null)
            {
                return NotFound("查無此留言");
            }
            MessageCenter.HangoutContext.Messages.Remove(queryRecord);
            MessageCenter.HangoutContext.SaveChanges();
            return NoContent(); ;
        }

        /// <summary>
        /// 再包一層 避免傳出ID
        /// </summary>
        /// <param name="addData">新增資料</param>
        /// <returns></returns>
        private int AddPartition(ParticipantInput addData)
        {
            var AddParticipant = new Participant(addData.EventId, addData.Participanter, addData.Motivation);
            MessageCenter.HangoutContext.Add(AddParticipant);
            var notice = new Notice(addData.Participanter, "報名了你舉辦的活動", EventCenter.GetEvent(addData.EventId).MemberId, addData.EventId);
            MessageCenter.HangoutContext.Add(notice);
            MessageCenter.HangoutContext.SaveChanges();
            return AddParticipant.ParticipantId;
        }


        /// <summary>
        /// 新增參加者人
        /// </summary>
        /// <param name="addData"></param>
        /// <returns></returns>
        // POST api/<PartitionController>
        [HttpPost("Participant")]
        public ActionResult Post([FromBody] ParticipantInput addData)
        {

            if (addData.Motivation == null)
            {
                return BadRequest("資料不完整,新增失敗");
            }

            if (IsEventExist(addData.EventId))
            {
                return NotFound("活動不存在");
            }
            if (IsMemberExist(addData.Participanter))
            {
                return NotFound("會員不存在");
            }
            if (PartitionCenter.ParticipantCheck(addData.Participanter, addData.EventId) != null)
            {
                return BadRequest("已報名過活動");
            }

            var Id = AddPartition(addData);
            return Ok(new { ParticipantId = Id });
        }

        /// <summary>
        /// 藉由參加者編號取得所有內容
        /// </summary>
        /// <param name="ParticipantId">參加者編號</param>
        /// <returns></returns>
        // GET api/<PartitionController>/5
        [HttpGet("Participant/{ParticipantId}")]
        public ActionResult GetPartition(int ParticipantId)
        {
            var queryRecord = PartitionCenter.GetParticipants(ParticipantId).SingleOrDefault();
            return queryRecord != null ? Ok(queryRecord) : NotFound("查無此留言");
        }

        /// <summary>
        /// 更新參加者狀態
        /// </summary>
        /// <param name="ParticipantId">參加編號</param>
        /// <param name="updateData">更新資料</param>
        /// <returns></returns>
        [HttpPut("Participant/{ParticipantId}")]
        public ActionResult PutByPartitionId(int ParticipantId, [FromBody] ParticipantOutput updateData)
        {
            var queryRecord = PartitionCenter.GetParticipants(ParticipantId).SingleOrDefault();
            if (queryRecord != null)
            {
                queryRecord.EventId = updateData.EventId;
                queryRecord.Participanter = updateData.Participanter;
                queryRecord.Status = updateData.Status;
                MessageCenter.HangoutContext.SaveChanges();
                return NoContent();
            }
            return NotFound("查無此參加者");
        }


        /// <summary>
        /// 藉由活動編號取得所有參加者
        /// </summary>
        /// <param name="EventId">活動編號</param>
        /// <returns></returns>
        // GET: api/<PartitionController>
        [HttpGet("Participant/Event ={EventId}")]
        public ActionResult GetPartitionByEvent(int EventId)
        {
            var queryRecord = PartitionCenter.GetParticipantsByEvent(EventId).ToArray();
            if (queryRecord.Length == 0)
            {
                return NotFound("無符合條件之留言");
            }
            return Ok(queryRecord);
        }
        /// <summary>
        /// 刪除參加者
        /// </summary>
        /// <param name="PartitionId"></param>
        /// <returns></returns>
        [HttpDelete("Participant/{ParticipantId}")]
        public ActionResult DeleteByPartitionId(int ParticipantId)
        {
            var queryRecord = PartitionCenter.GetParticipants(ParticipantId).SingleOrDefault();
            if (queryRecord == null)
            {
                return NotFound("查無此參加者");
            }
            MessageCenter.HangoutContext.Participants.Remove(queryRecord);
            MessageCenter.HangoutContext.SaveChanges();
            return NoContent(); ;
        }

        /// <summary>
        /// 舉辦成功之活動推薦人列表
        /// </summary>
        /// <param name="EventId"></param>
        /// <returns></returns>
        [HttpPost("Relationship/{EventId}")]
        public ActionResult PostByEvent(int EventId)
        {
            if (IsEventExist(EventId))
            {
                return NotFound("活動不存在");
            }

            var peopleList = new List<int>();

            peopleList.Add(EventCenter.GetEvent(EventId).MemberId);
            var addEvent = PartitionCenter.GetParticipantsByEvent(EventId);
            foreach (var people in addEvent)
            {
                peopleList.Add(people.Participanter);
            }

            for (int i = 0; i < peopleList.Count; i++)
            {
                for (int j = 0; j < peopleList.Count; j++)
                {
                    if (peopleList[i] != peopleList[j])
                    {
                        var current = new Relationship(peopleList[i], peopleList[j]);
                        EventCenter.HangoutContext.Add(current);
                        EventCenter.HangoutContext.SaveChanges();
                    }
                }
            }

            return Ok("新增成功");
        }
        /// <summary>
        /// 取得此會員推薦人
        /// </summary>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        [HttpGet("Relationship/MemberId ={MemberId}")]
        public ActionResult GetRelationshipByEvent(int MemberId)
        {
            var queryRecord = RelationshipCenter.GetRelationships(MemberId).ToArray();
            if (queryRecord.Length == 0)
            {
                return NotFound("無符合條件之推薦者");
            }


            Dictionary<int, int> dict = new Dictionary<int, int>();

            foreach (var data in queryRecord)
            {
                if (dict.TryGetValue(data.ObjectId, out int value))
                {
                    dict[data.ObjectId] = value + 1;
                }
                else
                {
                    dict.Add(data.ObjectId, 1);
                }

            }
            return Ok(dict.Select(item => new RelationshipOutput(item.Key, item.Value)));
        }

        /// <summary>
        /// 再包一層 避免傳出ID
        /// </summary>
        /// <param name="addData">新增資料</param>
        /// <returns></returns>
        private int AddInvite(InviteInput addData)
        {
            var AddInvite = new Invite(addData.ProtagonistId, addData.ObjectId, addData.EventId);
            MessageCenter.HangoutContext.Add(AddInvite);
            var notice = new Notice(addData.ProtagonistId, "邀請你參加他正在參與的活動", addData.ObjectId, addData.EventId);
            MessageCenter.HangoutContext.Add(notice);
            MessageCenter.HangoutContext.SaveChanges();
            return AddInvite.InviteId;
        }



        /// <summary>
        /// 邀請人參加活動
        /// </summary>
        /// <param name="addData"></param>
        /// <returns></returns>
        // POST api/<PartitionController>
        [HttpPost("Invite")]
        public ActionResult PostInvite([FromBody] InviteInput addData)
        {

            if (RelationshipCenter.Check(addData.ProtagonistId, addData.ObjectId) == null)
            {
                return BadRequest("無此邀請人");
            }

            if (IsEventExist(addData.EventId))
            {
                return NotFound("活動不存在");
            }
            if (InviteCenter.InviterCheck(addData.ProtagonistId, addData.ObjectId, addData.EventId) != null)
            {
                return NotFound("已邀請過");
            }


            var Id = AddInvite(addData);
            return Ok(new { InviteId = Id });
        }
        /// <summary>
        /// 藉由邀請編號 取得內容
        /// </summary>
        /// <param name="InviteId"></param>
        /// <returns></returns>
        [HttpGet("Invite/{InviteId}")]
        public ActionResult GetInvite(int InviteId)
        {
            var queryRecord = InviteCenter.GetInvites(InviteId);
            return queryRecord != null ? Ok(queryRecord) : NotFound("查無此邀請");
        }


        /// <summary>
        /// 刪除被邀請的活動
        /// </summary>
        /// <param name="MemberId"></param>
        /// <param name="EventId"></param>
        /// <returns></returns>
        [HttpDelete("Invite/{MemberId}/{EventId}")]
        public ActionResult DeleteInviteEventByMember(int MemberId, int EventId)
        {
            var queryRecord = InviteCenter.UpdateInviter(MemberId, EventId);
            if (queryRecord == null)
            {
                return NotFound("查無此邀請");
            }
            InviteCenter.HangoutContext.Remove(queryRecord);
            InviteCenter.HangoutContext.SaveChanges();
            return NoContent();
        }

    }
}
