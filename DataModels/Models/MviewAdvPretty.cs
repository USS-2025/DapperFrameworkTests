using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModels.Models;

/// <summary>
/// Table for sql parsing
/// </summary>
[Keyless]
[Table("MVIEW$_ADV_PRETTY")]
[Index("Queryid", Name = "MVIEW$_ADV_PRETTY_IDX_01")]
public partial class MviewAdvPretty
{
    [Column("QUERYID#", TypeName = "NUMBER")]
    public decimal? Queryid { get; set; }

    [Column("SQL_TEXT", TypeName = "LONG")]
    public string? SqlText { get; set; }
}
