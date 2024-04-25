using EMPManegment.EntityModels.ViewModels.FormMaster;
using EMPManegment.Inretface.Interface.MasterList;
using EMPManegment.Inretface.Services.MasterList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.MasterList
{
    public class FormMasterService:IFormMasterServices
    {
        public FormMasterService(IFormMaster form)
        {
            Form = form;
        }

        public IFormMaster Form { get; }
        public async Task<IEnumerable<FormMasterModel>> GetFormGroupList()
        {
            return await Form.GetFormGroupList();
        }
    }
}
