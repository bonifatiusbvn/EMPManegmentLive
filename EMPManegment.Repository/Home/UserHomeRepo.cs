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
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Inretface.Interface.Home;
using Microsoft.EntityFrameworkCore;
#nullable disable

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
        public async Task<UserResponceModel> MarkMessagesAsReadAsync(ChatMessagesView chatMessage)
        {
            var response = new UserResponceModel();
            try
            {
                var messages = Context.TblChatMessages
                    .Where(m => m.UserId == chatMessage.UserId
                                && m.ConversationId == chatMessage.ConversationId
                                && m.IsRead == false)
                    .ToList();

                if (!messages.Any())
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Message = "No unread messages found for the specified user and conversation.";
                    return response;
                }

                foreach (var message in messages)
                {
                    message.IsRead = true;
                    Context.TblChatMessages.Update(message);
                }

                await Context.SaveChangesAsync();

                response.Code = (int)HttpStatusCode.OK;
                response.Message = "Messages marked as read successfully!";
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error marking messages as read: " + ex.Message;
            }

            return response;
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
                                  where cm.ConversationId == conversationId && cm.IsDeleted == false
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
                                    .Where(innerCm => innerCm.UserId == userId && cm.IsDeleted == false)
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
            try
            {
                var conversationIds = await Context.TblChatMessages
                    .Where(cm => cm.UserId == newChatMessage.MyUserId)
                    .Select(cm => cm.ConversationId)
                    .Distinct()
                    .ToListAsync();

                var messages = await (from cm in Context.TblChatMessages
                                      join user in Context.TblUsers on cm.UserId equals user.Id
                                      where conversationIds.Contains(cm.ConversationId) && cm.UserId == newChatMessage.SelectedUserId && cm.IsDeleted == false
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ChatMessagesView>> GetUsersNewMessageList(Guid userId)
        {
            try
            {
                // Step 1: Get all unique conversation IDs 
                var conversationIds = await Context.TblChatMessages
                    .Where(cm => cm.UserId == userId)
                    .Select(cm => cm.ConversationId)
                    .Distinct()
                    .ToListAsync();

                // Step 2: Get messages based on the conversation IDs, excluding specified user and including only unread messages
                var messages = await (from cm in Context.TblChatMessages
                                      join user in Context.TblUsers on cm.UserId equals user.Id
                                      where conversationIds.Contains(cm.ConversationId)
                                            && cm.UserId != userId
                                            && cm.IsRead == false
                                            && cm.IsDeleted == false
                                      select new
                                      {
                                          cm.UserId,
                                          user.FirstName,
                                          user.LastName,
                                          user.Image,
                                          cm.ConversationId,
                                          cm.SentDateTime,
                                          cm.MessageText
                                      })
                                      .ToListAsync();

                // Step 3: Group and select results
                var result = messages
                    .GroupBy(x => new
                    {
                        x.UserId,
                        x.FirstName,
                        x.LastName,
                        x.Image,
                        x.ConversationId
                    })
                    .Select(g => new ChatMessagesView
                    {
                        UserId = g.Key.UserId,
                        ConversationId = g.Key.ConversationId,
                        UserImage = g.Key.Image,
                        UserName = $"{g.Key.FirstName} {g.Key.LastName}",
                        SentDateTime = g.Max(x => x.SentDateTime),
                        MessageText = g.FirstOrDefault(x => x.SentDateTime == g.Max(m => m.SentDateTime))?.MessageText ?? string.Empty
                    })
                    .OrderByDescending(x => x.SentDateTime)
                    .ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AllNotificationModel> GetUsersAllNotificationList(Guid userId)
        {
            var allnotifications = new AllNotificationModel();

            try
            {
                // Step 1: Get all unique conversation IDs 
                var conversationIds = await Context.TblChatMessages
                    .Where(cm => cm.UserId == userId)
                    .Select(cm => cm.ConversationId)
                    .Distinct()
                    .ToListAsync();

                // Step 2: Get messages based on the conversation IDs, excluding specified user and including only unread messages
                var messages = await (from cm in Context.TblChatMessages
                                      join user in Context.TblUsers on cm.UserId equals user.Id
                                      where conversationIds.Contains(cm.ConversationId)
                                            && cm.UserId != userId
                                            && cm.IsRead == false
                                            && cm.IsDeleted == false
                                      select new
                                      {
                                          cm.UserId,
                                          user.FirstName,
                                          user.LastName,
                                          user.Image,
                                          cm.ConversationId,
                                          cm.SentDateTime,
                                          cm.MessageText
                                      })
                                      .ToListAsync();

                // Step 3: Group and select results
                var chatMessagesQuery = messages
                    .GroupBy(x => new
                    {
                        x.UserId,
                        x.FirstName,
                        x.LastName,
                        x.Image,
                        x.ConversationId
                    })
                    .Select(g => new ChatMessagesView
                    {
                        UserId = g.Key.UserId,
                        ConversationId = g.Key.ConversationId,
                        UserImage = g.Key.Image,
                        UserName = $"{g.Key.FirstName} {g.Key.LastName}",
                        SentDateTime = g.Max(x => x.SentDateTime),
                        MessageText = g.FirstOrDefault(x => x.SentDateTime == g.Max(m => m.SentDateTime))?.MessageText ?? string.Empty
                    })
                    .OrderByDescending(x => x.SentDateTime)
                    .ToList();

                allnotifications.Messages = chatMessagesQuery;


                var tasksQuery = from a in Context.TblTaskDetails
                                 join b in Context.TblUsers on a.UserId equals b.Id
                                 join c in Context.TblTaskMasters on a.TaskType equals c.Id
                                 where a.UserId == userId
                                 orderby a.UpdatedOn
                                 select new TaskDetailsView
                                 {
                                     Id = a.Id,
                                     UserId = b.Id,
                                     TaskType = a.TaskType,
                                     TaskStatus = a.TaskStatus,
                                     TaskDate = a.TaskDate,
                                     TaskDetails = a.TaskDetails,
                                     TaskEndDate = a.TaskEndDate,
                                     TaskTitle = a.TaskTitle,
                                     UserProfile = b.Image,
                                     UserName = b.UserName,
                                     FirstName = b.FirstName,
                                     LastName = b.LastName,
                                     TaskTypeName = c.TaskType,
                                     CreatedBy = a.CreatedBy,
                                     UpdatedOn = a.UpdatedOn
                                 };

                allnotifications.Tasks = await tasksQuery.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return allnotifications;
        }

        public async Task<UserResponceModel> DeleteChatMessage(int MessageId)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var chatDetails = Context.TblChatMessages.Where(a => a.MessageId == MessageId).FirstOrDefault();

                if (chatDetails != null)
                {

                    chatDetails.IsDeleted = true;
                    Context.TblChatMessages.Update(chatDetails);
                    Context.SaveChanges();
                    response.Message = "Message is successfully deleted.";
                    response.Data = chatDetails.ConversationId;
                }
                else
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Message = "Message does not found";
                }
            }
            catch
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in deleting message.";
            }

            return response;
        }
    }
}

