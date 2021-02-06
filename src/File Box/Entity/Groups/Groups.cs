using System;
using System.Collections.Generic;

public record Groups
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    //Relationships
    public virtual ICollection<Files> Files { get; set; }
}