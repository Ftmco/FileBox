using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Services
{
    public class FilesServices : IFileRepository
    {
        #region :: Depenedency::

        private readonly FileContext _db;

        private readonly IUserRepository _user;

        public FilesServices(FileContext db, UserServices users)
        {
            _user = users;
            _db = db;
        }

        #endregion

        public async Task<bool> DeleteAsync(Files tModel)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _db.Files.Remove(tModel);
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

        public async Task<IEnumerable<Files>> GetAllAsync()
        {
            return await Task.Run(async () => await _db.Files.ToListAsync());
        }

        public async Task<IEnumerable<Files>> GetAllAsync(Expression<Func<Files, bool>> where)
        {
            return await Task.Run(async () => await _db.Files.Where(where).ToListAsync());
        }

        public async Task<Files> GetByIdAsync(object Id)
        {
            return await Task.Run(async () => await _db.Files.FindAsync(Id));
        }

        public async Task<IEnumerable<Files>> GetBySearchAsync(string q)
        {
            return await Task.Run(async () => await GetAllAsync(f => f.FileName.Contains(q)));
        }

        public async Task<DownloadFileViewModel> GetFileByDownloadLinkAsync(string downloadLink)
        {
            return await Task.Run(async () =>
            {
                var file = await _db.Files.FirstOrDefaultAsync(f => f.DownloadLink == downloadLink);
                string path = Directory.GetCurrentDirectory() + @"\UsersFiles\";
                return new DownloadFileViewModel
                {
                    Base64 = Convert.ToBase64String(File.ReadAllBytes(path + file.FileName)),
                    FileName = file.FileName,
                    Extention = Path.GetExtension(path + file.FileName)
                };
            });
        }

        public async Task<IEnumerable<Files>> GetUserFilesAsync(IHeaderDictionary header)
        {
            return await Task.Run(async () =>
            {
                Users user = await _user.GetUserFromTokenAsync(header);
                return (user != null) ? GetUserFiles(user.UserId) : null;
            });
        }

        public async Task<IEnumerable<Files>> GetUserFilesAsync(IRequestCookieCollection cookies)
        {
            return await Task.Run(async () =>
            {
                Users user = await _user.GetUserFromTokenAsync(cookies);
                return (user != null) ? GetUserFiles(user.UserId) : null;
            });
        }

        public async Task<bool> InsertAsync(Files tModel)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    await _db.Files.AddAsync(tModel);
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

        public async Task<bool> UpdateAsync(Files tModel)
        {
            return await Task.Run(() =>
           {
               try
               {
                   _db.Files.Update(tModel);
                   return true;
               }
               catch
               {
                   return false;
               }
           });
        }

        public async Task<int> UploadNewFileAsync(FileviewModel fileviewModel, IHeaderDictionary headers)
        {
            return await Task.Run(async () =>
            {
                byte[] fileBytes = Convert.FromBase64String(fileviewModel.Base64);
                if (!string.IsNullOrEmpty(fileviewModel.Base64))
                {
                    fileviewModel.FileSize = fileBytes.Length;
                    var user = await _user.GetUserFromTokenAsync(headers);
                    if (user != null)
                    {
                        Files newFile = CreateFile(fileviewModel, user.UserId);
                        if (InsertFileToStorge(newFile.FileName, fileBytes))
                        {
                            return (await InsertAsync(newFile) && await SaveAsync()) ? (int)FileResult.Success : (int)FileResult.Exceptions;
                        }
                        return (int)FileResult.Exceptions;
                    }
                    return (int)FileResult.UserNotFound;
                }
                return (int)FileResult.NullRefrenceBase64;
            });
        }

        public async Task<int> UploadNewFileAsync(FileviewModel fileviewModel, IRequestCookieCollection cookies)
        {
            return await Task.Run(async () =>
            {
                byte[] fileBytes = Convert.FromBase64String(fileviewModel.Base64);
                if (!string.IsNullOrEmpty(fileviewModel.Base64))
                {
                    fileviewModel.FileSize = fileBytes.Length;
                    var user = await _user.GetUserFromTokenAsync(cookies);
                    if (user != null)
                    {
                        Files newFile = CreateFile(fileviewModel, user.UserId);
                        if (InsertFileToStorge(newFile.FileName, fileBytes))
                        {
                            return (await InsertAsync(newFile) && await SaveAsync()) ? (int)FileResult.Success : (int)FileResult.Exceptions;
                        }
                        return (int)FileResult.Exceptions;
                    }
                    return (int)FileResult.UserNotFound;
                }
                return (int)FileResult.NullRefrenceBase64;
            });
        }

        private Files CreateFile(FileviewModel file, Guid userId)
        {
            return new Files()
            {
                DownloadCount = 0,
                DownloadLink = Guid.NewGuid().ToString().Substring(0, 7).CreateSHA256(),
                FileId = Guid.NewGuid(),
                FileName = Guid.NewGuid().ToString().Substring(0, 8) + file.Extention,
                UploadDate = DateTime.Now,
                Size = file.FileSize,
                UserId = userId
            };
        }

        //CQSR 
        #region ::CQSR::

        private IEnumerable<Files> GetUserFiles(Guid userId)
        {
            return GetAllAsync(f => f.UserId == userId).Result;
        }

        private bool InsertFileToStorge(string fileName, byte[] fileBytes)
        {
            try
            {
                string path = Directory.GetCurrentDirectory() + @"\UsersFiles\";
                CheckDirectory(path);
                File.WriteAllBytes(path + fileName, fileBytes);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void CheckDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        #endregion
        //CQSR
    }
}
