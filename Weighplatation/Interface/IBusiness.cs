using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weighplatation.Model;
namespace Weighplatation.Interface
{
    public interface IBusiness
    {
        int CheckDuplicateBusiness(string IdOddo);

        bool InsertBusiness(BusinessModel businessModel);
    }
}
