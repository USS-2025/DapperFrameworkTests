using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModels.Models;

/// <summary>
/// Representation for query join sub-graph not in AJG 
/// </summary>
[Table("MVIEW$_ADV_FJG")]
public partial class MviewAdvFjg
{
    [Key]
    [Column("FJGID#", TypeName = "NUMBER")]
    public decimal Fjgid { get; set; }

    [Column("AJGID#", TypeName = "NUMBER")]
    public decimal Ajgid { get; set; }

    [Column("FJGDESLEN", TypeName = "NUMBER")]
    public decimal Fjgdeslen { get; set; }

    [Column("FJGDES", TypeName = "LONG RAW")]
    public byte[] Fjgdes { get; set; } = null!;

    [Column("HASHVALUE", TypeName = "NUMBER")]
    public decimal Hashvalue { get; set; }

    [Column("FREQUENCY", TypeName = "NUMBER")]
    public decimal? Frequency { get; set; }

    [ForeignKey("Ajgid")]
    [InverseProperty("MviewAdvFjgs")]
    public virtual MviewAdvAjg Ajg { get; set; } = null!;

    [InverseProperty("Fjg")]
    public virtual ICollection<MviewAdvGc> MviewAdvGcs { get; set; } = new List<MviewAdvGc>();
}
