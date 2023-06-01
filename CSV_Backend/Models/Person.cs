using CSV_Backend.Base;
using CsvHelper.Configuration.Attributes;

namespace CSV_Backend.Models;

public class Person : IBaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    [Format("dd-MM-yyyy")]
    public DateTime DateOfBirth { get; set; }
    public bool Married { get; set; }
    public string Phone { get; set; }
    public decimal Salary { get; set; }
}
