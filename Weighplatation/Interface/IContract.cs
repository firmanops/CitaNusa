using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weighplatation.Model;
namespace Weighplatation.Interface
{
    public interface IContract
    {
        bool InsertContract(ContractModel contractModel);
        bool UpdateContract(ContractModel contractModel);

        List<ContractModel> GetAllContract();

        //List<ContractModel> GetAllContract(string param);

        ContractModel GetByContractNo(string contractNo);

        int CheckDuplicateContract(string ContractMo);

        List<ContractModel> GetByFilterContractNo(string contractNo);

    }
}
