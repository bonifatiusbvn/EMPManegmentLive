

using EMPManagment.API;
using EMPManegment.Inretface.Interface.CompanyMaster;
using EMPManegment.Inretface.Interface.CSC;
using EMPManegment.Inretface.Interface.ExpenseMaster;
using EMPManegment.Inretface.Interface.FormPermissionMaster;
using EMPManegment.Inretface.Interface.InvoiceMaster;
using EMPManegment.Inretface.Interface.ManualInvoice;
using EMPManegment.Inretface.Interface.MasterList;
using EMPManegment.Inretface.Interface.OrderDetails;
using EMPManegment.Inretface.Interface.ProductMaster;
using EMPManegment.Inretface.Interface.ProjectDetails;
using EMPManegment.Inretface.Interface.PurchaseRequest;
using EMPManegment.Inretface.Interface.TaskDetails;
using EMPManegment.Inretface.Interface.UserAttendance;
using EMPManegment.Inretface.Interface.UserList;
using EMPManegment.Inretface.Interface.UsersLogin;
using EMPManegment.Inretface.Interface.VendorDetails;
using EMPManegment.Inretface.Services.CompanyServices;
using EMPManegment.Inretface.Services.CSC;
using EMPManegment.Inretface.Services.ExpenseMaster;
using EMPManegment.Inretface.Services.FormPermissionMasterServices;
using EMPManegment.Inretface.Services.InvoiceMaster;
using EMPManegment.Inretface.Services.ManualInvoiceServices;
using EMPManegment.Inretface.Services.MasterList;
using EMPManegment.Inretface.Services.OrderDetails;
using EMPManegment.Inretface.Services.ProductMaster;
using EMPManegment.Inretface.Services.ProjectDetailsServices;
using EMPManegment.Inretface.Services.PurchaseRequestServices;
using EMPManegment.Inretface.Services.TaskServices;
using EMPManegment.Inretface.Services.UserAttendanceServices;
using EMPManegment.Inretface.Services.UserListServices;
using EMPManegment.Inretface.Services.UserLoginServices;
using EMPManegment.Inretface.Services.VendorDetailsServices;
using EMPManegment.Repository.CompanyRepository;
using EMPManegment.Repository.CSCRepository;
using EMPManegment.Repository.ExponseMasterRepository;
using EMPManegment.Repository.FormPermissionMasterRepository;
using EMPManegment.Repository.InvoiceMasterRepository;
using EMPManegment.Repository.ManualInvoiceRepository;
using EMPManegment.Repository.MasterListRepository;
using EMPManegment.Repository.OrderRepository;
using EMPManegment.Repository.ProductMaster;
using EMPManegment.Repository.ProjectDetailsRepository;
using EMPManegment.Repository.PurchaseRequestRepository;
using EMPManegment.Repository.TaskRepository;
using EMPManegment.Repository.UserAttendanceRepository;
using EMPManegment.Repository.UserListRepository;
using EMPManegment.Repository.UserLoginRepository;
using EMPManegment.Repository.VendorDetailsRepository;
using EMPManegment.Services.Company;
using EMPManegment.Services.CSC;
using EMPManegment.Services.ExpenseMaster;
using EMPManegment.Services.FormPermissionMaster;
using EMPManegment.Services.InvoiceMaster;
using EMPManegment.Services.ManualInvoice;
using EMPManegment.Services.MasterList;
using EMPManegment.Services.OrderDetails;
using EMPManegment.Services.ProductMaster;
using EMPManegment.Services.ProjectDetails;
using EMPManegment.Services.PurchaseRequest;
using EMPManegment.Services.TaskDetails;
using EMPManegment.Services.UserAttendance;
using EMPManegment.Services.UserList;
using EMPManegment.Services.UserLogin;
using EMPManegment.Services.VendorDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<BonifatiusEmployeesContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("EMPDbconn")));


builder.Services.AddScoped<IUserLogin, UserLoginRepo>();
builder.Services.AddScoped<IMasterList, MasterListRepo>();
builder.Services.AddScoped<IUserLogin, UserLoginRepo>();
builder.Services.AddScoped<IUserDetails, UserDetailsRepo>();
builder.Services.AddScoped<IUserAttendance, UserAttendanceRepo>();
builder.Services.AddScoped<IAddVendorDetails, AddVendorRepo>();
builder.Services.AddScoped<ITaskDetails, TaskRepo>();
builder.Services.AddScoped<IProjectDetails, ProjectDetailsRepo>();
builder.Services.AddScoped<IPurchaseOrderDetails, PurchaseOrderRepo>();
builder.Services.AddScoped<IProductMaster, ProductMasterRepo>();
builder.Services.AddScoped<IPurchaseRequest, PurchaseRequestRepo>();
builder.Services.AddScoped<IInvoiceMaster, InvoiceMasterRepo>();
builder.Services.AddScoped<IExpenseMaster, ExpenseMasterRepo>();
builder.Services.AddScoped<IFormMaster, FormMasterRepo>();
builder.Services.AddScoped<IFormPermissionMaster, FormPermissionMasterRepo>();
builder.Services.AddScoped<ICompany, CompanyRepo>();
builder.Services.AddScoped<IManualInvoice, ManualInvoiceRepo>();


builder.Services.AddScoped<IUserLoginServices, UserLoginService>();
builder.Services.AddScoped<IMasterListServices, MasterListService>();
builder.Services.AddScoped<IUserLoginServices, UserLoginService>();
builder.Services.AddScoped<IUserDetailsServices, UserDetailsService>();
builder.Services.AddScoped<IUserAttendanceServices, UserAttendanceServices>();
builder.Services.AddScoped<IAddVendorDetailsServices, VendorServices>();
builder.Services.AddScoped<ITaskServices, DealTaskServices>();
builder.Services.AddScoped<IProjectDetailServices, ProjectDetailsServices>();
builder.Services.AddScoped<IPurchaseOrderDetailsServices, PurchaseOrderDetailsServices>();
builder.Services.AddScoped<IProductMasterServices, ProductMasterServices>();
builder.Services.AddScoped<IPurchaseRequestServices, PurchaseRequestServices>();
builder.Services.AddScoped<IInvoiceMasterServices, InvoiceMasterServices>();
builder.Services.AddScoped<IExpenseMasterServices, ExpenseMasterServices>();
builder.Services.AddScoped<IFormMasterServices, FormMasterService>();
builder.Services.AddScoped<IFormPermissionMasterServices, FormPermissionMasterService>();
builder.Services.AddScoped<ICompanyServices, CompanyServices>();
builder.Services.AddScoped<IManualInvoiceServices, ManualInvoiceServices>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

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
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
