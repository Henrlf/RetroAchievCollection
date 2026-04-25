using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RetroAchievCollection.Enum;

namespace RetroAchievCollection.Models;

[Table("games")]
[Index(nameof(CodeIntegration), IsUnique = true)]
public class GameModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id {get; set;}

    public Guid ConsoleId {get; set;}

    [ForeignKey(nameof(ConsoleId))]
    public ConsoleModel ConsoleModel {get; set;} = null!;

    public int CodeIntegration {get; set;}

    [MaxLength(255)]
    public string Name {get; set;} = "";

    [MaxLength(255)]
    public string? Publisher {get; set;}

    [MaxLength(255)]
    public string? Developer {get; set;}

    [MaxLength(255)]
    public string? Genre {get; set;}

    public DateOnly? ReleaseDate {get; set;}

    public bool IsFavorite {get; set;}

    [MaxLength(500)]
    public string PlayCommand {get; set;} = "";

    [MaxLength(500)]
    public string ImagePath {get; set;} = "";

    public List<AchievementModel> Achievements {get; set;} = new();

    [NotMapped]
    public int AchievementsCount => Achievements.Count;

    [NotMapped]
    public int AchievementsCompleted => Achievements.Count(a => a.Status == AchievementStatus.Completed);

    [NotMapped]
    public int AchievementsCompletedHardcore => Achievements.Count(a => a.Status == AchievementStatus.CompletedHardcore);
}