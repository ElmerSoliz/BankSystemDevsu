using BankDevsu.Api.Contracts;
using BankDevsu.Application.Services;
using BankDevsu.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankDevsu.Api.Controllers
{
    [ApiController]
    [Route("movements")]
    public class MovementsController : ControllerBase
    {
        private readonly BankingDbContext _ctx;
        private readonly IMovementService _service;
        public MovementsController(BankingDbContext ctx, IMovementService service) { _ctx = ctx; _service = service; }

        [HttpGet("by-account/{accountId:guid}")]
        public async Task<ActionResult> ListByAccount(Guid accountId, [FromQuery] DateTime? fromUtc, [FromQuery] DateTime? toUtc)
        {
            var q = _ctx.Movements.AsNoTracking().Where(m => m.AccountId == accountId);
            if (fromUtc.HasValue) q = q.Where(m => m.DateUtc >= fromUtc.Value);
            if (toUtc.HasValue) q = q.Where(m => m.DateUtc <= toUtc.Value);
            var list = await q.OrderBy(m => m.DateUtc).ToListAsync();
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] MovementCreateDto dto)
        {
            var created = await _service.CreateAsync(dto.AccountId, dto.MovementType, dto.Amount, dto.DateUtc);
            return Created($"/movimientos/{created.Id}", created);
        }
    }
}
