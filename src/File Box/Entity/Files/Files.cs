using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Files Entity
/// </summary>
public record Files
{

    /// <summary>
    /// File Primary Key
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// file Name
    /// </summary>
    [Required]
    public string FileName { get; set; }

    /// <summary>
    /// File Download Link
    /// </summary>
    [Required]
    public string DownloadLink { get; set; }

    /// <summary>
    /// File Size
    /// </summary>
    [Required]
    public float Size { get; set; }

    /// <summary>
    /// Download Count 
    /// </summary>
    [Required]
    public float DownloadCount { get; set; }

    /// <summary>
    /// Upload File Date
    /// </summary>
    [Required]
    public DateTime UploadDate { get; set; }

   
}