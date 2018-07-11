using Project1.DAL;
using Project1.Models;
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
            //var data = db.Users.FirstOrDefault(u => u.Id == id);
            //var result = (db.Users.FirstOrDefault(u => u.Id == id).Roles.SelectMany(r => r.Permissions.Select(p => p.CodeName)))
            //    .Union(db.Users.FirstOrDefault(u => u.Id == id).UserPermissions.Where(p => p.Deny != true).Select(p => p.Permisssion.CodeName));

            var result = (db.Users.Where(u => u.Id == id)
                                  .SelectMany(u => u.Roles.SelectMany(r => r.Permissions))
                                  .Select(p => p.CodeName))
                         .Union(db.Users.Where(u => u.Id == id)
                                        .SelectMany(u => u.UserPermissions.Select(up => up.Permisssion))
                                        .Select(p => p.CodeName))
                          .Distinct()
                          .Except(db.Users.Where(u => u.Id == id)
                                          .SelectMany(u => u.UserPermissions.Where(up => up.Deny == true).Select(up => up.Permisssion))
                                          .Select(p => p.CodeName)
                                          ).ToArray();

               // .Select(ro => ro.Roles.SelectMany(r => r.Permissions.Select(p => p.CodeName)).Union(ro.UserPermissions.Select(p => p.Permisssion.CodeName)));
            //    .Union(db.Users.FirstOrDefault(u => u.Id == id).UserPermissions.Where(p => p.Deny != true).Select(p => p.Permisssion.CodeName));

            //var lstDeny = data.UserPermissions.Where(up => up.Deny == true).Select(p => p.Permisssion.CodeName).Distinct();
            //var result1 = result.Except(lstDeny).ToArray();
            return result;
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