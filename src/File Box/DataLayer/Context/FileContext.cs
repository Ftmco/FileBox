using Microsoft.EntityFrameworkCore;

/// <summary>
/// Data Base Context 
/// </summary>
public class FileContext : DbContext
{

    /// <summary>
    /// Constractor
    /// </summary>
    /// <param name="options">Db Context Option</param>
    public FileContext(DbContextOptions<FileContext> options):base(options)
    {

    }

  
}