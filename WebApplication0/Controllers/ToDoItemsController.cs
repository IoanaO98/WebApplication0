using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;
using WebApplication0.Models;
using System.IO;
using System;
using System.Net;
using System.Net.Mime;

namespace WebApplication0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        private readonly ToDoContext _context;
        static readonly HttpClient client = new HttpClient();

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

        [HttpGet("stringReverse/{id},{nameUser}")]
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

        [HttpGet("DownloadFile/{fileName}")]
        public async Task<ActionResult> DownloadFile(string url)
        {
            
            using HttpResponseMessage response = await client.GetAsync(url);
            string imageBody = await response.Content.ReadAsStringAsync();
            byte[] result = await response.Content.ReadAsByteArrayAsync();
            return File( result, "image/jpg");
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

        [HttpPost("extractVocals/{id},{text}")]
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

        [HttpGet("DistinctWords/{id},{txt}")]
        public string GetUniqueWordsCount(int id, string txt)
        {
            txt = new Regex("[^a-zA-Z0-9]").Replace(txt, " ");
            string[] words = txt.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);


            var dublicateWords = 0;
            for (var i = 0; i < words.Length; i++)
            {
                var wordFound = 0;
                foreach(var word in words)
                {
                    if (words[i] == word)
                    {
                        wordFound++;
                    }

                }
     
                if (wordFound >= 2)
                {

                    dublicateWords++;
                }
            }
            var matchQuery = from word in words
                             where word.Equals(txt, StringComparison.InvariantCultureIgnoreCase)
                             select word;
            int wordCount = matchQuery.Count();
            return "In String: " + txt + " there are " + dublicateWords + " duplicates.";
            

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
