﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CycleCycleCycle.Models;

namespace CycleCycleCycle.Services
{
    public interface IAccountService
    {
        Account Find(string username);
        Account Find(int id);
        Account Create(string username);
    }
}
