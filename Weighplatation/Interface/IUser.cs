using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weighplatation.Model;
namespace Weighplatation.Interface
{
    public interface IUser
    {
        List<SYSUSERMODEL> GetUser();

        SYSUSERMODEL GetUserLogin(string UserID, string Password);

        BusinessUnitModel GetUnitByCode(string UnitCode);

        List<SYSUSERGROUPMENUMODEL> GetMenuUser(int groupid);

        SYSMENU GetMenu(int id);
        List<SYSMENU> GetMenuAll();

        bool InsertUser(SYSUSERMODEL sYSUSERMODEL);

        bool UpdatetUser(SYSUSERMODEL sYSUSERMODEL);

        bool InsertGroupMenu(List<SYSUSERGROUPMENUMODEL> sYSUSERGROUPMENUMODEL);

        DBBUSINESSPARTNER GetLogo(string UnitCode);

        string Encrypt(string encrypttext);

        string Decrypt(string decryptedText);
    }
}
