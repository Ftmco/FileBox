using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UserServices : IUserRepository
    {
        #region ::Dependency::

        private readonly FileContext _db;

        private readonly ITokenRepository _token;

        public UserServices(FileContext db, TokenServices token)
        {
            _token = token;
            _db = db;
        }

        #endregion

        public async Task<bool> DeleteAsync(Users tModel)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _db.Users.Remove(tModel);
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        public async Task<bool> DeleteAsync(object Id)
        {
            return await Task.Run(async () => await DeleteAsync(await GetByIdAsync(Id)));
        }

        public async void Dispose()
        {
            await _db.DisposeAsync();
        }

        public async Task<IEnumerable<Users>> GetAllAsync()
        {
            return await Task.Run(async () => await _db.Users.ToListAsync());
        }

        public async Task<IEnumerable<Users>> GetAllAsync(Expression<Func<Users, bool>> where)
        {
            return await Task.Run(async () => await _db.Users.Where(where).ToListAsync());
        }

        public async Task<Users> GetByIdAsync(object Id)
        {
            return await Task.Run(async () => await _db.Users.FindAsync(Id));
        }

        public async Task<IEnumerable<Users>> GetBySearchAsync(string q)
        {
            return await Task.Run(async () => await GetAllAsync(u=> u.UserName.Contains(q) || u.Email.Contains(q)));
        }

        public Task<Users> GetUserByEmailAddress(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<Users> GetUserFromTokenAsync(IHeaderDictionary header)
        {
            return await Task.Run(async () =>
            {
                string value = header["Token"];
                UserTokens token = await _token.GetTokenByValueAsync(value);
                return (token != null) ? await GetByIdAsync(token.UserId) : null;
            });
        }

        public async Task<Users> GetUserFromTokenAsync(IRequestCookieCollection cookies)
        {
            return await Task.Run(async () =>
            {
                string value = cookies["Token"];
                UserTokens token = await _token.GetTokenByValueAsync(value);
                return (token != null) ? await GetByIdAsync(token.UserId) : null;
            });
        }

        public async Task<bool> InsertAsync(Users tModel)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    await _db.Users.AddAsync(tModel);
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        public bool IsExist(string UserName)
        {
            return _db.Users.Any(u => u.UserName == UserName.Trim().ToLower());
        }

        public async Task<bool> IsExistAsync(Guid Id)
        {
            return await Task.Run(async () => await _db.Users.AnyAsync(u => u.UserId == Id));
        }

        public async Task<bool> IsExistAsync(string EmailAddress)
        {
            return await Task.Run(async () => await _db.Users.AnyAsync(u => u.Email == EmailAddress.Trim().ToLower()));
        }

        public bool Save()
        {
            try
            {
                _db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public async Task<bool> SaveAsync()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    await _db.SaveChangesAsync();
                    return false;
                }
                catch
                {
                    return false;
                }
            });
        }

        public async Task<bool> UpdateAsync(Users tModel)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _db.Users.Update(tModel);
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }
    }
}
