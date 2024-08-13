using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure;
using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels.Chat;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.Home;
using Microsoft.EntityFrameworkCore;

namespace EMPManegment.Repository.Home
{
    public class UserHomerepo : IUserHome
    {
        public UserHomerepo(BonifatiusEmployeesContext Context)
        {
            this.Context = Context;
        }

        public BonifatiusEmployeesContext Context { get; }

        public async Task<UserResponceModel> SendMessageAsync(ChatMessagesView chatMessages)
        {
            var response = new UserResponceModel();
            try
            {
                var message = new TblChatMessage
                {
                    UserId = chatMessages.UserId,
                    UserName = chatMessages.UserName,
                    MessageText = chatMessages.MessageText,
                    SentDateTime = DateTime.Now,
                    IsRead = false,
                    IsDeleted = false,
                    ConversationId = chatMessages.ConversationId
                };
                response.Message = "Message Successfully Sent.";

                Context.TblChatMessages.Add(message);
                await Context.SaveChangesAsync();
            }
            catch (Exception)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in sending message!";
            }
            return response;
        }
        public async Task MarkMessagesAsReadAsync(Guid userId, Guid? conversationId)
        {
            var messages = Context.TblChatMessages
             .Where(m => m.UserId == userId && !m.IsRead.Value);

            if (conversationId.HasValue)
            {
                messages = messages.Where(m => m.ConversationId == conversationId);
            }

            foreach (var message in messages)
            {
                message.IsRead = true;
            }

            await Context.SaveChangesAsync();
        }

        public async Task<List<TblChatMessage>> ReceiveMessagesAsync(Guid userId, Guid? conversationId)
        {
            var query = Context.TblChatMessages
            .Where(m => m.UserId == userId && !m.IsDeleted.Value);

            if (conversationId.HasValue)
            {
                query = query.Where(m => m.ConversationId == conversationId);
            }

            return await query.OrderBy(m => m.SentDateTime).ToListAsync();

        }

        public async Task<IEnumerable<ChatMessagesView>> GetMyConversation(Guid conversationId)
        {
            var messages = await (from cm in Context.TblChatMessages
                                  join user in Context.TblUsers on cm.UserId equals user.Id
                                  where cm.ConversationId == conversationId
                                  select new ChatMessagesView
                                  {
                                      MessageId = cm.MessageId,
                                      UserId = cm.UserId,
                                      UserName = user.FirstName + " " + user.LastName,
                                      MessageText = cm.MessageText,
                                      SentDateTime = cm.SentDateTime,
                                      IsRead = cm.IsRead,
                                      IsDeleted = cm.IsDeleted,
                                      ConversationId = cm.ConversationId,
                                      UserImage = user.Image
                                  }).ToListAsync();

            return messages;
        }

        public async Task<IEnumerable<ChatMessagesView>> GetMyConversationList(Guid userId)
        {
            var result = await (from cm in Context.TblChatMessages
                                join user in Context.TblUsers on cm.UserId equals user.Id
                                where Context.TblChatMessages
                                    .Where(innerCm => innerCm.UserId == userId)
                                    .Select(innerCm => innerCm.ConversationId)
                                    .Distinct()
                                    .Contains(cm.ConversationId)
                                  && cm.UserId != userId
                                group new { cm, user } by new { cm.UserId, user.FirstName, user.LastName, user.Image, cm.ConversationId } into g
                                select new ChatMessagesView
                                {
                                    UserId = g.Key.UserId,
                                    ConversationId = g.Key.ConversationId,
                                    UserImage = g.Key.Image,
                                    UserName = g.Key.FirstName + " " + g.Key.LastName,
                                    SentDateTime = g.Max(x => x.cm.SentDateTime),
                                    MessageText = g.OrderByDescending(x => x.cm.SentDateTime).FirstOrDefault().cm.MessageText
                                })
                            .OrderByDescending(x => x.SentDateTime)
                            .ToListAsync();

            return result;

        }

        public async Task<IEnumerable<ChatMessagesView>> CheckUserConversationId(NewChatMessageModel newChatMessage)
        {
            var conversationIds = await Context.TblChatMessages
                .Where(cm => cm.UserId == newChatMessage.MyUserId)
                .Select(cm => cm.ConversationId)
                .Distinct()
                .ToListAsync();

            var messages = await (from cm in Context.TblChatMessages
                                  join user in Context.TblUsers on cm.UserId equals user.Id
                                  where conversationIds.Contains(cm.ConversationId) && cm.UserId == newChatMessage.SelectedUserId
                                  select new ChatMessagesView
                                  {
                                      MessageId = cm.MessageId,
                                      UserId = cm.UserId,
                                      UserName = user.FirstName + " " + user.LastName,
                                      MessageText = cm.MessageText,
                                      SentDateTime = cm.SentDateTime,
                                      IsRead = cm.IsRead,
                                      IsDeleted = cm.IsDeleted,
                                      ConversationId = cm.ConversationId,
                                      UserImage = user.Image
                                  })
                                  .OrderByDescending(cm => cm.SentDateTime)
                                  .ToListAsync();

            if (!messages.Any())
            {
                var userInfo = await (from user in Context.TblUsers
                                      where user.Id == newChatMessage.SelectedUserId
                                      select new ChatMessagesView
                                      {
                                          UserId = user.Id,
                                          UserName = user.FirstName + " " + user.LastName,
                                          ConversationId = Guid.NewGuid(),
                                          UserImage = user.Image,
                                          UserIdentity = user.UserName
                                      })
                                      .SingleOrDefaultAsync();

                var selectedUserChatMessage = new TblChatMessage
                {
                    UserId = newChatMessage.SelectedUserId,
                    UserName = userInfo.UserIdentity,
                    MessageText = "Hello!",
                    SentDateTime = DateTime.Now,
                    IsRead = false,
                    IsDeleted = false,
                    ConversationId = userInfo.ConversationId
                };

                Context.TblChatMessages.Add(selectedUserChatMessage);
                await Context.SaveChangesAsync();

                var myChatMessage = new TblChatMessage
                {
                    UserId = newChatMessage.MyUserId,
                    UserName = newChatMessage.MyUserIdentity,
                    MessageText = "Hello!",
                    SentDateTime = DateTime.Now,
                    IsRead = false,
                    IsDeleted = false,
                    ConversationId = userInfo.ConversationId
                };

                Context.TblChatMessages.Add(myChatMessage);
                await Context.SaveChangesAsync();


                if (userInfo != null)
                {
                    return new List<ChatMessagesView> { userInfo };
                }
            }
            return messages;
        }
    }
}

