using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoAPI.Models;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly DemoContext _context;

        public DemoController(DemoContext context)
        {
            _context = context;
        }

        // GET: api/Demo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DemoItemDTO>>> GetDemoItems()
        {
          if (_context.DemoItems == null)
          {
              return NotFound();
          }
            return await _context.DemoItems
                .Select( demoItem => ItemToDTO( demoItem ) )
                .ToListAsync();
        }

        // GET: api/Demo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DemoItemDTO>> GetDemoItem(long id)
        {
            if (_context.DemoItems == null)
            {
                return NotFound();
            }

            var demoItem = await _context.DemoItems.FindAsync(id);

            if (demoItem == null)
            {
                return NotFound();
            }

            return ItemToDTO( demoItem );
        }

        // PUT: api/Demo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDemoItem(long id, DemoItemDTO demoItemDTO)
        {
            if (id != demoItemDTO.Id)
            {
                return BadRequest();
            }

            // _context.Entry(demoItemDTO).State = EntityState.Modified;

            var demoItem = await _context.DemoItems.FindAsync( id );

            if (demoItem == null)
            {
                return NotFound();
            }

            demoItem.Name = demoItemDTO.Name;
            demoItem.IsComplete = demoItemDTO.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!DemoItemExists(id))
            {
                return NotFound();
            }
            // {
            //     if (!DemoItemExists(id))
            //     {
            //         return NotFound();
            //     }
            //     else
            //     {
            //         throw;
            //     }
            // }

            return NoContent();
        }

        // POST: api/Demo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DemoItemDTO>> PostDemoItem(DemoItemDTO demoItemDTO)
        {
            //   if (_context.DemoItems == null)
            //   {
            //       return Problem("Entity set 'DemoContext.DemoItems'  is null.");
            //   }

            var demoItem = new DemoItem
            {
                Name = demoItemDTO.Name,
                IsComplete = demoItemDTO.IsComplete
            };

            _context.DemoItems.Add(demoItem);
            await _context.SaveChangesAsync();

            // nameof keyword is used to avoid hard-coding the action name in the CreatedAtAction call
            // return CreatedAtAction("GetDemoItem", new { id = demoItem.Id }, demoItem);
            // return CreatedAtAction(nameof(GetDemoItem), new { id = demoItem.Id }, demoItem);
            return CreatedAtAction( nameof(GetDemoItem), new { id = demoItem.Id }, ItemToDTO( demoItem ));
        }

        // DELETE: api/Demo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDemoItem(long id)
        {
            if (_context.DemoItems == null)
            {
                return NotFound();
            }
            var demoItem = await _context.DemoItems.FindAsync(id);
            if (demoItem == null)
            {
                return NotFound();
            }

            _context.DemoItems.Remove(demoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DemoItemExists(long id)
        {
            return (_context.DemoItems?.Any(demoItem => demoItem.Id == id)).GetValueOrDefault();
        }

        private static DemoItemDTO ItemToDTO(DemoItem demoItem) => new DemoItemDTO
        {
           Id = demoItem.Id,
           Name = demoItem.Name,
           IsComplete = demoItem.IsComplete
        };
    }
}
