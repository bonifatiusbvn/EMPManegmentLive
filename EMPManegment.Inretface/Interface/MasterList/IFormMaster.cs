using EMPManegment.EntityModels.ViewModels.FormMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.MasterList
{
    public interface IFormMaster
    {
        Task<IEnumerable<FormMasterModel>> GetFormGroupList();
    }
}
