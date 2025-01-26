using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Models;
public class Patient
{
    [BsonId]
    [BsonRepresentation(BsonType.Int32)]
    public int? Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Range(0, 100)]
    public int? Age { get; set; }
    public string? Gender { get; set; }
    public string? MedicalHistory{get;set;}
}