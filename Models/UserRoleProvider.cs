using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ShoppingSite.Models
{
    public class UserRoleProvider : RoleProvider
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

        public override string[] GetRolesForUser(string username)
        {
            using (UserDBContext db = new UserDBContext())
            {
                var userRoles = (from user in db.Users where user.UserName == username select user.UserType).ToArray();
                return userRoles;
            }
        }

        //public override string[] GetRolesForUser(string username)
        //{
        //    using (UserDBContext db = new UserDBContext())
        //    {
        //        var objUser = db.Users.FirstOrDefault(x => x.UserName == username);
        //        if (objUser == null)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            string[] ret = db.Users.Select(x => x.User_type).ToArray();
        //            return ret;
        //        }
        //    }
        //}

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