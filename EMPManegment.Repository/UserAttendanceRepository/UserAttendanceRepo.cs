﻿using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.UserAttendance;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace EMPManegment.Repository.UserAttendanceRepository
{
    public class UserAttendanceRepo : IUserAttendance
    {
        public UserAttendanceRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }

        public BonifatiusEmployeesContext Context { get; }



        public async Task<IEnumerable<UserAttendanceModel>> GetUserAttendanceList()
        {
            IEnumerable<UserAttendanceModel> users = from a in Context.TblAttendances
                                                     join
                                                     b in Context.TblUsers on a.User.Id equals b.Id
                                                     select new UserAttendanceModel 
                                                     {
                                                         UserName = b.FirstName + ' ' + b.LastName,
                                                         UserId =a.UserId,
                                                         AttendanceId = a.Id,
                                                         Date = a.Date,
                                                         Intime = a.Intime,
                                                         OutTime = a.OutTime,
                                                         //TotalHours = a.TotalHours,
                                                         CreatedBy = a.CreatedBy,
                                                         TotalHours = a.OutTime - a.Intime,
                                                         CreatedOn = a.CreatedOn,
                                                        
                                                     };

            return users;
        }

        public async Task<UserAttendanceResponseModel> GetUserAttendanceInTime(UserAttendanceRequestModel userAttendance)
        {
            UserAttendanceResponseModel response = new UserAttendanceResponseModel();
            var data = Context.TblAttendances.Where(e => e.UserId == userAttendance.UserId && e.Date == DateTime.Today).FirstOrDefault();
            try
            {
                if (data != null)
                {
                    UserAttendanceModel attendanceModel = new UserAttendanceModel()
                    {
                        UserId = data.UserId,
                        Date = data.Date,
                        Intime = data.Intime,
                        TotalHours = data.OutTime - data.Intime,
                        CreatedBy = data.CreatedBy,
                        CreatedOn = data.CreatedOn,
                        OutTime = data.OutTime,
                        AttendanceId = data.Id,
                    };
                    response.Code = 200;
                    response.Data = attendanceModel;
                }
                
                return response;

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public async Task<UserAttendanceModel> UpdateUserOutTime(UserAttendanceModel userAttendance)
        {
            try
            {
                var data = Context.TblAttendances.FirstOrDefault(a => a.UserId == userAttendance.UserId);
                if (data != null)
                {
                    data.OutTime = userAttendance.OutTime;

                    Context.TblAttendances.Update(data);
                    Context.SaveChanges();
                }
                return userAttendance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<UserAttendanceModel>> GetUserAttendanceById(int attendanceId)
        {
            IEnumerable<UserAttendanceModel> attendance = from a in Context.TblAttendances
                             join
                             b in Context.TblUsers on a.User.Id equals b.Id where a.Id == attendanceId
                             select new UserAttendanceModel
                             {
                                 UserName = b.FirstName + ' ' + b.LastName,
                                 UserId = a.UserId,
                                 AttendanceId = a.Id,
                                 Date = a.Date,
                                 Intime = a.Intime,
                                 OutTime = a.OutTime,
                                 TotalHours = a.OutTime - a.Intime,
                                 CreatedOn = a.CreatedOn,
                                 CreatedBy = a.CreatedBy,
                             };
            
            return attendance;
        }
    }
}

       