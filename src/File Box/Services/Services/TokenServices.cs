using Microsoft.EntityFrameworkCore;
using Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Services
{
    public class TokenServices : ITokenRepository
    {

        #region :: Depenedency::

        private readonly FileContext _db;

        public TokenServices(FileContext db)
        {
            _db = db;
        }

        #endregion

        public async Task<bool> DeleteAsync(UserTokens tModel)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _db.UserTokens.Remove(tModel);
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

        public async Task<IEnumerable<UserTokens>> GetAllAsync()
        {
            return await Task.Run(async () => await _db.UserTokens.ToListAsync());
        }

        public async Task<IEnumerable<UserTokens>> GetAllAsync(Expression<Func<UserTokens, bool>> where)
        {
            return await Task.Run(async () => await _db.UserTokens.Where(where).ToListAsync());
        }

        public async Task<UserTokens> GetByIdAsync(object Id)
        {
            return await Task.Run(async () => await _db.UserTokens.FindAsync(Id));
        }

        public async Task<IEnumerable<UserTokens>> GetBySearchAsync(string q)
        {
            return await Task.Run(async () => await GetAllAsync(t => t.Key.Contains(q) || t.Value.Contains(q)));
        }

        public async Task<UserTokens> GetTokenByValueAsync(string value)
        {
            return await Task.Run(async () => await _db.UserTokens.FirstOrDefaultAsync(t => t.Value == value));
        }

        public async Task<IEnumerable<UserTokens>> GetTokensByKeyAsync(string key)
        {
            return await Task.Run(async () => await GetAllAsync(t => t.Key == key));
        }

        public async Task<bool> InsertAsync(UserTokens tModel)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    await _db.UserTokens.AddAsync(tModel);
                    return true;
                }
                catch
                {
                    return false;
                }
            });
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
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        public async Task<bool> UpdateAsync(UserTokens tModel)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _db.UserTokens.Update(tModel);
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
