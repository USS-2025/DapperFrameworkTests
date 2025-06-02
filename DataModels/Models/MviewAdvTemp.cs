using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModels.Models;

/// <summary>
/// Table for temporary data
/// </summary>
[Keyless]
[Table("MVIEW$_ADV_TEMP")]
[Index("Id", "Seq", Name = "MVIEW$_ADV_TEMP_IDX_01")]
public partial class MviewAdvTemp
{
    [Column("ID#", TypeName = "NUMBER")]
    public decimal? Id { get; set; }

    [Column("SEQ#", TypeName = "NUMBER")]
    public decimal? Seq { get; set; }

    [Column("TEXT", TypeName = "LONG")]
    public string? Text { get; set; }
}
