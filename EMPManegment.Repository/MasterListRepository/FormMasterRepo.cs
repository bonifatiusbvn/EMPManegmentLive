using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels.FormMaster;
using EMPManegment.Inretface.Interface.MasterList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Repository.MasterListRepository
{
    public class FormMasterRepo:IFormMaster
    {
        public FormMasterRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }
        public BonifatiusEmployeesContext Context { get; }
        public async Task<IEnumerable<FormMasterModel>> GetFormGroupList()
        {
            try
            {
                IEnumerable<FormMasterModel> FormList = Context.TblForms.ToList().Select(a => new FormMasterModel
                {
                    FormId = a.FormId,
                    FormGroup = a.FormGroup,
                    FormName = a.FormName,
                });
                return FormList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
