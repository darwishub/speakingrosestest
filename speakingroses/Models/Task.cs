using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace speakingrosestest.Models;

[Keyless]
public partial class _Task
{
    public int TaskId { get; set; }

    [StringLength(250)]
    public string? Title { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public byte Priority { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DueDate { get; set; }

    public bool Status { get; set; }
}
