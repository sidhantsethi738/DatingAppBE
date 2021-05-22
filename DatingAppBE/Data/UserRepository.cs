﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppBE.DTOs;
using DatingAppBE.Entities;
using DatingAppBE.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppBE.Data
{
    public class UserRepository: IUserRepository
    {
        private  readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context  , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public   async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUserNameAsync(string username)
        {
            return await _context.Users.
                  Include(p => p.Photos).
                SingleOrDefaultAsync(x => x.Username == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users.
                Include(p => p.Photos).
                ToListAsync();
        }

        public  async Task<bool> SaveAllSync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

       public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
                 .Where(x => x.Username == username).
                 ProjectTo<MemberDto>(_mapper.ConfigurationProvider).
                 SingleOrDefaultAsync();
        }

      public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _context.Users.
                 ProjectTo<MemberDto>(_mapper.ConfigurationProvider).ToListAsync();

        }
    }
}
