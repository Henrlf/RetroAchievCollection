using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RetroAchievCollection.Models;

[Table("consoles")]
[Index(nameof(CodeIntegration), IsUnique = true)]
public class ConsoleModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id {get; set;}
    
    public int CodeIntegration {get; set;}

    [MaxLength(255)]
    public string Name {get; set;} = "";

    [MaxLength(255)]
    public string? Company {get; set;}

    [MaxLength(500)]
    public string ImagePath {get; set;} = "";
}