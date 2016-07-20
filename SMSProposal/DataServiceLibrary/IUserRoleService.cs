﻿using DataModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public interface IUserRoleService
    {
        Task<SubscriberRoles> AddUserRole(SubscriberRoles sroles);
        Task<bool> CheckExists(string user, string role);
    }
}
