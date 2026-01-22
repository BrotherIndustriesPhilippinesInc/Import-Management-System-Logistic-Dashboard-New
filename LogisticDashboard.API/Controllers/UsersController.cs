using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogisticDashboard.API.Data;
using LogisticDashboard.Core;
using System.Net.Http;
using Newtonsoft.Json;
using static LogisticDashboard.API.Controllers.UsersController;
using System.Text;
using PortalAPI.DTO;
using System.Text.Json;


namespace LogisticDashboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly LogisticDashboardAPIContext _context;

        private readonly HttpClient _httpClient;

        public UsersController(LogisticDashboardAPIContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public class DataTableResponse<T>
        {
            public int Draw { get; set; }
            public int RecordsTotal { get; set; }
            public int RecordsFiltered { get; set; }
            public List<T> Data { get; set; } = new();
        }

        public class PortalUser
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public string EmailAddress { get; set; }
            public string Section { get; set; }
            public string Position { get; set; }
            public string EmployeeNumber { get; set; }
            public string Adid { get; set; }
        }

        public class PortalUserWithAdmin
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public string EmailAddress { get; set; }
            public string Section { get; set; }
            public string Position { get; set; }
            public string ADID { get; set; }
            public string EmployeeNumber { get; set; }
            public bool IsAdmin { get; set; } // comes from local db
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpPost("GetDataTableUsers")]
        public async Task<IActionResult> GetDataTableUsers([FromBody] DataTableRequest request)
        {
            var portalResponse = await GetPortalUsersForDataTableAsync(77, request);

            // Merge with local IsAdmin info
            var localUsers = await _context.Users.ToListAsync();
            var mergedData = portalResponse.Data.Select(p => new PortalUserWithAdmin
            {
                Id = p.Id,
                FullName = p.FullName,
                EmailAddress = p.EmailAddress,
                Section = p.Section,
                Position = p.Position,
                ADID = p.Adid,
                EmployeeNumber = p.EmployeeNumber,
                IsAdmin = localUsers.Any(u => u.PortalId == p.Id && u.IsAdmin)
            }).ToList();

            var result = new DataTableResponse<PortalUserWithAdmin>
            {
                Draw = portalResponse.Draw,
                RecordsTotal = portalResponse.RecordsTotal,
                RecordsFiltered = portalResponse.RecordsFiltered,
                Data = mergedData
            };

            return Ok(result);
        }

        [HttpPost("GetPortalDataTableUsers")]
        private async Task<DataTableResponse<PortalUser>> GetPortalUsersForDataTableAsync(int systemID, DataTableRequest request)
        {
            // Convert request to JSON
            var jsonContent = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
            );

            // POST to Portal API
            var response = await _httpClient.PostAsync(
                $"http://apbiphbpswb01:80/PortalAPI/api/SystemApproverLists/datatable?systemID={systemID}",
                jsonContent
            );

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Portal API call failed with status {response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();

            // Deserialize into DataTableResponse<PortalUser>
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var portalData = System.Text.Json.JsonSerializer.Deserialize<DataTableResponse<PortalUser>>(json, options);

            return portalData ?? new DataTableResponse<PortalUser> { Data = new List<PortalUser>() };
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(int id, Users users)
        {
            if (id != users.Id)
            {
                return BadRequest();
            }

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Users>> PostUsers([FromQuery] string employeeNumber, [FromQuery] bool isAdmin)
        {
            bool portalCreated = await CreatePortalUserAsync(employeeNumber);
            if (!portalCreated)
                return StatusCode(500, new { message = "Failed to create user in Portal API. It may already exist." });

            // ✅ Fetch Portal User data
            var response = await _httpClient.GetAsync(
                $"http://apbiphbpswb01:80/PortalAPI/api/SystemApproverLists/SearchEmployee?employeeNumber={employeeNumber}&systemID=77"
            );

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to fetch from Portal API.");

            var json = await response.Content.ReadAsStringAsync();

            // ✅ Deserialize API response
            var portalUser = JsonConvert.DeserializeObject<PortalUser>(json);
            if (portalUser == null)
                return BadRequest(new { message = "Invalid Portal API response." });

            // ✅ Prevent duplicates (check if already added)
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.PortalId == portalUser.Id);
            if (existing != null)
                return Conflict(new { message = "User already exists in local database." });

            // ✅ Create local user entry
            var newUser = new Users
            {
                PortalId = portalUser.Id,
                IsAdmin = isAdmin
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsers), new { id = newUser.Id }, newUser);
        }

        // DELETE: api/Users/5
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteUsers([FromQuery] string employeeNumber)
        {
            if (string.IsNullOrWhiteSpace(employeeNumber))
                return BadRequest("Employee number is required.");

            // 1️⃣ Fetch Portal user
            var portalUser = await GetPortalUserAsync(employeeNumber);
            if (portalUser == null)
                return NotFound("Portal user not found.");

            // 2️⃣ Delete locally first
            var localUser = await _context.Users.FirstOrDefaultAsync(u => u.PortalId == portalUser.Id);
            if (localUser != null)
            {
                _context.Users.Remove(localUser);
                await _context.SaveChangesAsync();
            }

            // 3️⃣ Delete in Portal API
            bool portalDeleted = await DeletePortalUserAsync(employeeNumber);
            if (!portalDeleted)
                return StatusCode(500, "Failed to delete user in Portal API.");

            return Ok(new { message = "User successfully deleted locally and in Portal API." });
        }

        [HttpPost("CreatePortalUser")]
        private async Task<bool> CreatePortalUserAsync(string employeeNumber)
        {
            var systemApproverData = new
            {
                employeeNumber = employeeNumber,
                systemID = 77,
                systemName = "Logistic Dashboard",
                approverNumber = 0
            };

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(systemApproverData),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(
                "http://apbiphbpswb01:80/PortalAPI/api/SystemApproverLists",
                jsonContent
            );

            return response.IsSuccessStatusCode;
        }

        [HttpPost("DeletePortalUser")]
        private async Task<bool> DeletePortalUserAsync(string employeeNumber)
        {
            var systemApproverData = new
            {
                employeeNumber = employeeNumber,
                systemID = 77,
                systemName = "Logistic Dashboard",
                approverNumber = 0
            };

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(systemApproverData),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(
                "http://apbiphbpswb01:80/PortalAPI/api/SystemApproverLists/Delete",
                jsonContent
            );
            Console.WriteLine(response.RequestMessage);
            return response.IsSuccessStatusCode;
        }

        [HttpGet("GetPortalUser")]
        private async Task<PortalUser?> GetPortalUserAsync(string employeeNumber)
        {
            var response = await _httpClient.GetAsync(
                $"http://apbiphbpswb01:80/PortalAPI/api/SystemApproverLists/SearchEmployee?employeeNumber={employeeNumber}&systemID=77"
            );

            if (!response.IsSuccessStatusCode)
                return null; // or throw an exception if you prefer

            var json = await response.Content.ReadAsStringAsync();

            var portalUser = JsonConvert.DeserializeObject<PortalUser>(json);

            return portalUser;
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
