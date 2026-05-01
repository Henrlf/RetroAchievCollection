using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using RetroAchievCollection.Enum;

namespace RetroAchievCollection.Models;

[Table("achievements")]
[Index(nameof(CodeIntegration), IsUnique = true)]
[Index(nameof(GameId))]
public class AchievementModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id {get; set;}

    public int CodeIntegration {get; set;}
    
    public Guid GameId {get; set;}
    
    [ForeignKey(nameof(GameId))]
    public GameModel GameModel {get; set;} = null!;
    
    [MaxLength(255)]
    public string Name {get; set;} = "";

    [MaxLength(500)]
    public string? Description {get; set;}

    [MaxLength(500)]
    public string ImagePath {get; set;} = "";

    public AchievementStatus Status {get; set;} = AchievementStatus.NotCompleted;
}