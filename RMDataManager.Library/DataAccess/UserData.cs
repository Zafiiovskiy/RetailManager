using RMDataManager.Library.ManagingDataFiles.DataAccess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDataManager.Library.DataAccess
{
    public class UserData
    {
        public List<UserModel> GetUserById(string Id)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var p = new { Id = Id };
            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "RMData");
            foreach(UserModel user in output){
                user.Id = Id;
            }
            return output;
        }
    }
}
