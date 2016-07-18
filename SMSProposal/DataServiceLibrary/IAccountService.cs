﻿using DataModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public interface IAccountService
    {
          Task<IEnumerable<AccountType>> Accounttypes();
          Task<IEnumerable<GenderType>> Gendertypes();
          Task<Subscriber> Add(Subscriber role);
    }
}
