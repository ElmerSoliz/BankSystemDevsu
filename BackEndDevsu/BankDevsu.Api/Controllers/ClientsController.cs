using BankDevsu.Api.Contracts;
using BankDevsu.Domain.Entities;
using BankDevsu.Domain.Exceptions;
using BankDevsu.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankDevsu.Api.Controllers
{
    [ApiController]
    [Route("clients")]
    public class ClientsController : ControllerBase
    {
        private readonly BankingDbContext _ctx;
        public ClientsController(BankingDbContext ctx) => _ctx = ctx;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetAll() =>
            Ok(await _ctx.Clients.AsNoTracking().ToListAsync());

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Client>> GetById(Guid id)
        {
            var c = await _ctx.Clients.FindAsync(id);
            return c is null ? NotFound() : Ok(c);
        }

        [HttpPost]
        public async Task<ActionResult<Client>> Create([FromBody] ClientCreateDto dto)
        {
            if (await _ctx.Clients.AnyAsync(x => x.Identification == dto.Identification))
                throw new ValidationException("Identification must be unique");

            var client = new Client
            {
                Name = dto.Name,
                Gender = dto.Gender,
                Age = dto.Age,
                Identification = dto.Identification,
                Address = dto.Address,
                Phone = dto.Phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsActive = true
            };
            await _ctx.Clients.AddAsync(client);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ClientUpdateDto dto)
        {
            var client = await _ctx.Clients.FindAsync(id) ?? throw new NotFoundException("Client not found");
            client.Name = dto.Name;
            client.Gender = dto.Gender;
            client.Age = dto.Age;
            client.Address = dto.Address;
            client.Phone = dto.Phone;
            client.IsActive = dto.IsActive;
            await _ctx.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = await _ctx.Clients.FindAsync(id) ?? throw new NotFoundException("Client not found");
            _ctx.Clients.Remove(client);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}
