using System;
using System.Security.Principal;
using System.Web.Security;
using Common;
using System.Collections.Generic;
using System.EnterpriseServices;

namespace EHRMS.ViewModel
{
    public class UserIdentity : IIdentity, IPrincipal
    {
        private readonly FormsAuthenticationTicket _ticket;
        private readonly FormAuthenticationUserData _userData;

        public UserIdentity(FormsAuthenticationTicket ticket)
        {
            _ticket = ticket;
            _userData = _ticket.UserData.Deserialize<FormAuthenticationUserData>();
        }

        public string AuthenticationType
        {
            get { return "UserDairy"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }
        public string MasterPassword
        {
            get { return _userData.MasterPassword; }
        }
        public string Name
        {
            get { return _ticket.Name; }
        }

        public string UserId
        {
            get { return _userData.UserId.ToString(); }
        }

        public string EmailAddress
        {
            get { return _userData.EmailAddress; }
        }

        public Int32 StaffId
        {
            get { return _userData.StaffId; }
        }
        public string CompanyId
        {
            get { return _userData.CompanyId; }
        }

        public string BusinessCode
        {
            get { return _userData.BusinessCode; }
        }

        public bool IsSystemRole
        {
            get { return _userData.IsSystemRole; }
        }

        public string CompanyName
        {
            get { return _userData.CompanyName; }
        }

        public string LoginId
        {
            get { return _userData.LoginId; }
        }

        public bool IsInRole(string role)
        {
            return Roles.IsUserInRole(role);
        }
        public bool IsSuperAdmin
        {
            get { return _userData.IsSuperAdmin; }
        }

        public string[] RoleOfUser()
        {
            var role = Roles.GetRolesForUser(_userData.UserId.ToString());
            return role;
        }
        public int[] GetUserRoleId()
        {
            // 
            return _userData.UserRoleId;

        }

        public List<PermissionModel> UserPermissions
        {
            get { return _userData.Permissions; }

        }

        public IIdentity Identity
        {
            get { return this; }
        }
    }
}