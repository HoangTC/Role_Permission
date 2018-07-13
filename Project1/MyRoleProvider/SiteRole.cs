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

            ////C1
            //var result = (db.Users.Where(u => u.Id == id)
            //                      .SelectMany(u => u.Roles.SelectMany(r => r.Permissions))
            //                      .Select(p => p.CodeName))
            //             .Union(db.Users.Where(u => u.Id == id)
            //                            .SelectMany(u => u.UserPermissions.Select(up => up.Permisssion))
            //                            .Select(p => p.CodeName))
            //             .Except(db.Users.Where(u => u.Id == id)
            //                             .SelectMany(u => u.UserPermissions.Where(up => up.Deny == true)
            //                                                               .Select(up => up.Permisssion))
            //                             .Select(p => p.CodeName))
            //             .Distinct().ToArray();

            //C2
            var result1 = db.Users.Where(u => u.Id == id)
                                  .SelectMany(u => u.Roles.SelectMany(r => r.Permissions.Select(p => p.CodeName))
                                                    .Union(u.UserPermissions.Select(p => p.Permisssion.CodeName))
                                                    .Except(u.UserPermissions.Where(up => up.Deny == true).Select(up => up.Permisssion.CodeName))
                                                    .Distinct()
                                                    ).ToArray();
            return result1;
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