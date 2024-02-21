using EMPManagment.Web.Helper;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EMPManegment.Web.Controllers
{
    public class ExpenseMasterController : Controller
    {
        public WebAPI WebAPI { get; }
        public IWebHostEnvironment Environment { get; }
        public APIServices APIServices { get; }
        public UserSession _userSession { get; }

        public ExpenseMasterController(WebAPI webAPI, IWebHostEnvironment environment, APIServices aPIServices, UserSession userSession)
        {
            WebAPI = webAPI;
            Environment = environment;
            APIServices = aPIServices;
            _userSession = userSession;
        }
        public async Task<IActionResult> ExpenseList()
        {
            try
            {
                List<ExpenseDetailsView> Expense = new List<ExpenseDetailsView>();
                ApiResponseModel response = await APIServices.GetAsyncId(null, "ExpenseMaster/GetExpenseDetailList");
                if (response.code == 200)
                {
                    Expense = JsonConvert.DeserializeObject<List<ExpenseDetailsView>>(response.data.ToString());
                }
                return View(Expense);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
