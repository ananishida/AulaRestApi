using AulaRestApi.Data;
using AulaRestApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AulaRestApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        [HttpGet]
        [Route("pessoas")]
        public async Task<IActionResult> getAllAsync ([FromServices] Context context)
        {
            var pessoas = await context
                .Pessoas
                .AsNoTracking()
                .ToListAsync();

            if (pessoas == null)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }

        }
        [HttpGet]
        [Route("pessoas/{id}")]
        public async Task<IActionResult> getByIdAsync([FromServices] Context context,[FromRoute] int id)
        {
            var pessoas = await context
              .Pessoas.AsNoTracking()
              .FirstOrDefaultAsync(p => p.id == id);

            return pessoas == null ? NotFound() : Ok(pessoas);
        }

        [HttpPost]
        [Route("pessoas")]
        public async Task<IActionResult> PostAsync([FromServices] Context context, [FromBody] Pessoa pessoas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                await context.Pessoas.AddAsync(pessoas);
                await context.SaveChangesAsync();
                return Created($"api/pessoas/{pessoas.id}", pessoas);
            }
            catch (Exception ex)
            {
                return BadRequest();
                throw;
            }
        }

        [HttpPost]
        [Route("pessoas/{id}")]
        public async Task<IActionResult> PutAsync([FromServices] Context context, [FromBody] Pessoa pessoas, [FromRoute] int id)
        {
            if (!ModelState.IsValid)

                return BadRequest();

            var p = await context.Pessoas
            .FirstOrDefaultAsync(x => x.id == id);

            if (p == null)
                return NotFound();
            try
            {
                p.nome = pessoas.nome;
                context.Pessoas.Update(p);
                await context.SaveChangesAsync();
                return Ok(p);
            }
            catch (Exception ex)
            {
                return BadRequest();
                throw;
            }
        }

        [HttpDelete]
        [Route("pessoas/{id}")]
        public async Task<IActionResult> DeleteAsync([FromServices] Context context, [FromRoute] int id)
        {
            var p = await context.Pessoas.FirstOrDefaultAsync(x => x.id == id);
          
            try
            {
                context.Pessoas.Remove(p);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
