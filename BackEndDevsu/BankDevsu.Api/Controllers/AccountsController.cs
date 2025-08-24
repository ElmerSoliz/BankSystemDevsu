using BankDevsu.Api.Contracts;
using BankDevsu.Domain.Entities;
using BankDevsu.Domain.Exceptions;
using BankDevsu.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankDevsu.Api.Controllers
{
    [ApiController]
    [Route("accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly BankingDbContext _ctx;
        public AccountsController(BankingDbContext ctx) => _ctx = ctx;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAll() =>
            Ok(await _ctx.Accounts.AsNoTracking().ToListAsync());

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Account>> GetById(Guid id)
        {
            var a = await _ctx.Accounts.FindAsync(id);
            return a is null ? NotFound() : Ok(a);
        }

        [HttpPost]
        public async Task<ActionResult<Account>> Create([FromBody] AccountCreateDto dto)
        {
            if (!await _ctx.Clients.AnyAsync(c => c.Id == dto.ClientId))
                throw new NotFoundException("Client not found");
            if (await _ctx.Accounts.AnyAsync(a => a.AccountNumber == dto.AccountNumber))
                throw new ValidationException("AccountNumber must be unique");

            var acc = new Account
            {
                ClientId = dto.ClientId,
                AccountNumber = dto.AccountNumber,
                AccountType = dto.AccountType,
                InitialBalance = dto.InitialBalance,
                CurrentBalance = dto.InitialBalance,
                IsActive = dto.IsActive
            };
            await _ctx.Accounts.AddAsync(acc);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = acc.Id }, acc);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AccountUpdateDto dto)
        {
            var acc = await _ctx.Accounts.FindAsync(id) ?? throw new NotFoundException("Account not found");
            acc.IsActive = dto.IsActive;
            await _ctx.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var acc = await _ctx.Accounts.FindAsync(id) ?? throw new NotFoundException("Account not found");
            _ctx.Accounts.Remove(acc);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}
