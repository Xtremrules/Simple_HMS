﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_HMS.Interface
{
    public interface IUserRepository
    {
        IUser LoginUser(string username, string password);
    }
}