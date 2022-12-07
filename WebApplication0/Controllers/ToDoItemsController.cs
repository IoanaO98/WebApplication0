using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using WebApplication0.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplication0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        private readonly ToDoContext _context;

        public object Evironment { get; private set; }

        public ToDoItemsController(ToDoContext context)
        {
            _context = context;
        }

        // GET: api/ToDoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItems>>> GetToDoItems()
        {
            return await _context.ToDoItems.ToListAsync();
        }

        [HttpGet("{id},{nameUser}")]
        public string GetToDoItems(int id, string nameUser)
        {
            char[] charArray = nameUser.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        // GET: api/ToDoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItems>> GetToDoItems(long id)
        {
            var toDoItems = await _context.ToDoItems.FindAsync(id);

            if (toDoItems == null)
            {
                return NotFound();
            }

            return toDoItems;
        }
        // PUT: api/ToDoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoItems(long id, ToDoItems toDoItems)
        {
            if (id != toDoItems.Id)
            {
                return BadRequest();
            }

            _context.Entry(toDoItems).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoItemsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ToDoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ToDoItems>> PostToDoItems(ToDoItems toDoItems)
        {
            _context.ToDoItems.Add(toDoItems);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetToDoItems), new { id = toDoItems.Id }, toDoItems);
        }

        [HttpPost("{id},{text}")]
        public string PostStringVocals(int id, string text)
        {
            var vowels = "aeiou";
            var result = "";
            foreach (char c in text)
            {
                if (vowels.Contains(c))
                {
                    result += c;
                }
            }
            return result;
        }

        [HttpGet("{id},{txt}")]
        public string GetUniqueWordsCount( int id, string txt)
        {
            txt = " ";
            string input = txt;
            var worddistinct = input.Split(null);
            string[] strArr = worddistinct.ToArray();
            string[] StrDis = strArr.Distinct().ToArray();

            int myCount = 0;
            foreach (string str in StrDis) 
            {
                myCount =( from x1 in strArr where x1.ToString() == str select x1).Count();
                txt += str + myCount;
            }
            return txt;
        }

        // DELETE: api/ToDoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItems(long id)
        {
            var toDoItems = await _context.ToDoItems.FindAsync(id);
            if (toDoItems == null)
            {
                return NotFound();
            }

            _context.ToDoItems.Remove(toDoItems);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ToDoItemsExists(long id)
        {
            return _context.ToDoItems.Any(e => e.Id == id);
        }
    }
}
