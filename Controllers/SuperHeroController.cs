using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Data;
using CrudDapper;
using Dapper;

namespace CrudDapper.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuperHeroController : ControllerBase
{
    private readonly IConfiguration _congiguration;

    public SuperHeroController(IConfiguration configuration)
    {
        _congiguration = configuration;
    }

    [HttpGet("GetAllSuperHeroes")]
    public async Task<ActionResult<List<SuperHero>>> GetAllSuperHeroes()
    {
        using IDbConnection connection = new SqlConnection(_congiguration.GetConnectionString("DefaultConnection"));

        var heroes = await connection.QueryAsync<SuperHero>("select * from superheros");

        return Ok(heroes);
    }

     [HttpPost("CreateSuperHero")]
     
     public async Task<ActionResult<List<SuperHero>>> CreateNewSuperHero([FromBody] SuperHero superHero)
    {
        string sqlString = @" INSERT INTO SuperHeros (Name, FirstName, LastName, Place)
                                VALUES (@Name, @FirstName, @LastName, @Place)";
                                
        using IDbConnection connection = new SqlConnection(_congiguration.GetConnectionString("DefaultConnection"));
        
        var addHero = await connection.ExecuteAsync(sqlString, 
            new {Name = superHero.Name, FirstName = superHero.FirstName, LastName = superHero.LastName, Place = superHero.Place});
        return Ok(addHero);
    }

    [HttpPut("UpdateHero")]
    public async Task<ActionResult<List<SuperHero>>> UpdateSuperHero(SuperHero superhero)
    {
        string sqlString = @"UPDATE superheros 
                                SET FirstName = @FirstName, LastName = @LastName, Place = @Place
                                WHERE Name = @Name";

        using IDbConnection connection = new SqlConnection(_congiguration.GetConnectionString("DefaultConnection"));
        var updateHero = await connection.ExecuteAsync(sqlString,
            new {Name = superhero.Name, FirstName = superhero.FirstName, LastName = superhero.LastName, Place = superhero.Place});
        
        return Ok(updateHero);
    }

    [HttpDelete("DeleteSuperHero")]
    public async Task<ActionResult<List<SuperHero>>> DeleteSuperHero(string superHeroName)
    {
        string sqlstring = @"DELETE FROM superheros WHERE Name = @Name";
        using IDbConnection connection = new SqlConnection(_congiguration.GetConnectionString("DefaultConnection"));

        var deleteHero = await connection.ExecuteAsync(sqlstring,new {Name = superHeroName});
        return Ok(deleteHero);
    }

    [HttpGet("GetAHero")]
    public async Task<ActionResult<List<SuperHero>>> GetASuperHero(string heroName)
    {
        using IDbConnection connection = new SqlConnection(_congiguration.GetConnectionString("DefaultConnection"));

        var hero = await connection.QueryAsync<SuperHero>(
            "select * from superheros where name = @Name",
            new {Name = heroName});

        return Ok(hero);
    }
}