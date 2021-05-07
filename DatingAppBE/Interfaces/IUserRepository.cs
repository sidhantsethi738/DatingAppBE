﻿using DatingAppBE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppBE.Interfaces
{
   public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllSync();

        Task<IEnumerable<AppUser>> GetUsersAsync();

        Task<AppUser> GetUserByIdAsync(int id);

        Task<AppUser> GetUserByUserNameAsync (string username);

    }
}