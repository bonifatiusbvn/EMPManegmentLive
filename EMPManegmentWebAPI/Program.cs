

using EMPManagment.API;
using EMPManegment.Inretface.EmployeesInterface.AddEmployee;
using EMPManegment.Inretface.Interface.CSC;
using EMPManegment.Inretface.Interface.TaskDetails;
using EMPManegment.Inretface.Interface.UserAttendance;
using EMPManegment.Inretface.Interface.UserList;
using EMPManegment.Inretface.Interface.UsersLogin;
using EMPManegment.Inretface.Interface.VendorDetails;
using EMPManegment.Inretface.Services.AddEmployeeServies;
using EMPManegment.Inretface.Services.CSC;
using EMPManegment.Inretface.Services.TaskServices;
using EMPManegment.Inretface.Services.UserAttendanceServices;
using EMPManegment.Inretface.Services.UserListServices;
using EMPManegment.Inretface.Services.UserLoginServices;
using EMPManegment.Inretface.Services.VendorDetailsServices;
using EMPManegment.Repository.AddEmpRepository;
using EMPManegment.Repository.CSCRepository;
using EMPManegment.Repository.TaskRepository;
using EMPManegment.Repository.UserAttendanceRepository;
using EMPManegment.Repository.UserListRepository;
using EMPManegment.Repository.UserLoginRepository;
using EMPManegment.Repository.VendorDetailsRepository;
using EMPManegment.Services.AddEmployee;
using EMPManegment.Services.CSC;
using EMPManegment.Services.TaskDetails;
using EMPManegment.Services.UserAttendance;
using EMPManegment.Services.UserList;
using EMPManegment.Services.UserLogin;
using EMPManegment.Services.VendorDetails;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<BonifatiusEmployeesContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("EMPDbconn")));


builder.Services.AddScoped<IAddEmpDetails, AddEmpRepo>();
builder.Services.AddScoped<ICSC, CSCRepo>();
builder.Services.AddScoped<IUserLogin, UserLoginRepo> ();
builder.Services.AddScoped<IUserDetails, UserDetailsRepo>();
builder.Services.AddScoped<IUserAttendance, UserAttendanceRepo>();
builder.Services.AddScoped<IAddVendorDetails, AddVendorRepo>();
builder.Services.AddScoped<ITaskDetails, TaskRepo>();

builder.Services.AddScoped<IAddEmpDetailsServices, EmpService>();
builder.Services.AddScoped<ICSCServices, CSCService>();
builder.Services.AddScoped<IUserLoginServices, UserLoginService>();
builder.Services.AddScoped<IUserDetailsServices, UserDetailsService>();
builder.Services.AddScoped<IUserAttendanceServices, UserAttendanceServices>();
builder.Services.AddScoped<IAddVendorDetailsServices, VendorServices>();
builder.Services.AddScoped<ITaskServices, DealTaskServices>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
