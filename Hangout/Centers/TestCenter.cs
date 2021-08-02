using Hangout.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.Centers
{
    public class TestCenter
    {

        public HangoutContext HangoutContext { get; } //連接資料庫內容

        public TestCenter(HangoutContext hangoutContext)
        {
            HangoutContext = hangoutContext;
        }



        public IEnumerable<Event> GetAllEvent()
        {
            return HangoutContext.Events.Where(data => data.EventId < 1000);
        }

        public IEnumerable<Member> GetAllMember()
        {
            return HangoutContext.Members.Where(data => data.MemberId < 1000);
        }


        public IEnumerable<Message> GetAllMessage()
        {
            return HangoutContext.Messages.Where(data => data.MemberId < 1000);
        }

        public IEnumerable<Participant> GetAllParticipants()
        {
            return HangoutContext.Participants.Where(data => data.ParticipantId < 1000);
        }

        public IEnumerable<Comment> GetAllComment()
        {
            return HangoutContext.Comments.Where(data => data.CommentId < 1000);
        }

        public IEnumerable<Favorite> GetAllFavorite()
        {
            return HangoutContext.Favorites.Where(data => data.FavoritesId < 1000);
        }
        public IEnumerable<Follow> GetAllFollow()
        {
            return HangoutContext.Follows.Where(data => data.FollowId < 1000);
        }

        public IEnumerable<Relationship> GetAllRelationships()
        {
            return HangoutContext.Relationships.Where(data => data.RelationshipId < 1000);
        }

        public IEnumerable<Invite> GetInvites()
        {
            return HangoutContext.Invites.Where(data => data.InviteId < 1000);
        }


    }
}
