using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Hospital.Models;
 
public class Patient
{
    [BsonId]
    [BsonRepresentation(BsonType.Int32)]

    public int? Id { get; set; }

    [Required (ErrorMessage = "Name is required.")]
    public string? Name { get; set; }

    [Range (0, 100, ErrorMessage = "Age must be between 0 and 100.")]
    [Required (ErrorMessage = "Age is required.")]
    public int? Age { get; set; }

    [Required (ErrorMessage = "Gender is required.")]
     [RegularExpression("Male|Female|male|female", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
    public String? Gender { get; set; }

    [Required (ErrorMessage = "Medical history is required.")]
    public string? MedicalHistory{get;set;}
}
public class Doctor
{
    [BsonId]
    [BsonRepresentation(BsonType.Int32)]
    public int? Id { get; set; }

    
    [StringLength(100)]
    [Required (ErrorMessage = "Name is required.")]
    public string? Name { get; set; }

     [Range (24, 80, ErrorMessage = "Age must be between 0 and 100.")]
    [Required (ErrorMessage = "Age is required.")]
    public int? Age { get; set; }

    [Required (ErrorMessage = "Gender is required.")]
    [RegularExpression("Male|Female|male|female", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
    public String? Gender { get; set; }

    [Required (ErrorMessage = "Speciality is required.")]
    public string? Speciality { get; set; }
}

public class Appointment
{
    [BsonId]
    [BsonRepresentation(BsonType.Int32)]
    public int Id { get; set; }

    [Required (ErrorMessage = "Patient ID  is required.")]
    public int PatientId { get; set; }

    [Required (ErrorMessage = "Doctor ID  is required.")]
    public int DoctorId { get; set; }

    [Required (ErrorMessage = "Date  is required.")]
    public DateTime ADate { get; set; }
  
}
public class Counter
{
    [BsonId]
    public string Id { get; set; } 
    public int SequenceValue { get; set; }
}







