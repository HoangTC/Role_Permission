using Project1.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Project1.MyRoleProvider
{
    public class SiteRole : RoleProvider
    {
        
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string userId)
        {
            int id = Int32.Parse(userId);
            ManagerContext db = new ManagerContext();      
            var data = db.Users.FirstOrDefault(u => u.Id == id);
            var result = data.Roles.SelectMany(r => r.Permissions.Select(p => p.CodeName)).Concat(data.UserPermissions.Where(p => p.Deny == false).Select(p => p.Permisssion.CodeName)).Distinct();
            var lstDeny = data.UserPermissions.Where(up => up.Deny == true).Select(p => p.Permisssion.CodeName).Distinct();
            var result1 = result.Except(lstDeny);
            return result1.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}