using System.Globalization;
using CSV_Backend.Database;
using CSV_Backend.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace CSV_Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    private readonly Repository<Person> _personRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PersonController(ApplicationContext dbContext, IWebHostEnvironment webHostEnvironment)
    {
        _personRepository = new Repository<Person>(dbContext);
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet("GetPersons")]
    public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
    {
        return await _personRepository.GetAll();
    }

    [HttpPost("Upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest();

        List<Person> result;

        string uploadsDirectory = Path.Combine(_webHostEnvironment.ContentRootPath, "uploads");

        if (!Directory.Exists(uploadsDirectory))
            Directory.CreateDirectory(uploadsDirectory); // Create the directory if it doesn't exist

        string filePath = Path.Combine(uploadsDirectory, file.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null,
            PrepareHeaderForMatch = args => args.Header.ToLower(),
        };
        
        
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.TypeConverterCache.AddConverter<DateTime>(new CustomDateTimeConverter());

            result = csv.GetRecords<Person>().ToList();
            result.ForEach(el => el.Id = 0);
            await _personRepository.AddRange(result);
        }

        return Ok(result);
    }


    [HttpPut("Update")]
    public async Task<IActionResult> UpdatePerson(Person person)
    {
        var result = await _personRepository.Update(person);
        return Ok(result);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        await _personRepository.Delete(id);
        return Ok();
    }
}