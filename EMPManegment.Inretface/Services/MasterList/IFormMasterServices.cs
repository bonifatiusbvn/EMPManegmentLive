using EMPManegment.EntityModels.ViewModels.FormMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.MasterList
{
    public interface IFormMasterServices
    {
        Task<IEnumerable<FormMasterModel>> GetFormGroupList();
    }
}
