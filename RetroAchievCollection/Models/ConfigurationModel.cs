using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetroAchievCollection.Models;

[Table("configuration")]
public class ConfigurationModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id {get; set;}
    
    [MaxLength(255)]
    public string UserName {get; set;} = "";
    
    [MaxLength(255)]
    public string ApiKey {get; set;} = "";
}