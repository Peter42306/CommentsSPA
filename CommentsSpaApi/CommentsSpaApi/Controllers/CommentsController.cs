using CommentsSpaApi.Common;
using CommentsSpaApi.Data;
using CommentsSpaApi.Domain.Entities;
using CommentsSpaApi.Dtos.Comments;
using CommentsSpaApi.Services.Captcha;
using CommentsSpaApi.Services.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommentsSpaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ICaptchaService _captchaService;
        private readonly IHtmlSanitizerService _htmlSanitizerService;

        public CommentsController(
            ApplicationDbContext db, 
            ICaptchaService captchaService,
            IHtmlSanitizerService htmlSanitizerService)
        {
            _db = db;
            _captchaService = captchaService;
            _htmlSanitizerService = htmlSanitizerService;
        }

        // GET: api/comments/all
        [HttpGet("all")]
        public async Task<ActionResult<List<CommentListItemDto>>> GetAll()
        {
            var items = await _db.Comments
                .AsNoTracking()
                .OrderByDescending(c => c.CreatedAtUtc)
                .Select(c => new CommentListItemDto
                {
                    Id = c.Id,
                    ParentId = c.ParentId,
                    UserName = c.UserName,
                    Email = c.Email,
                    HomePage = c.HomePage,
                    SanitizedText = c.SanitizedText,
                    CreatedAtUtc = c.CreatedAtUtc
                })
                .ToListAsync();

            return Ok(items);
        }

        // POST: api/comments
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // CAPTCHA
            var isCaptchaValid = _captchaService.Validate(dto.CaptchaId, dto.CaptchaInput);
            if (!isCaptchaValid)
            {                
                return BadRequest(new { error = "Invalid captcha" });
            }

            // HTML sanitizer
            var sanitized = _htmlSanitizerService.Sanitize(dto.RawText);            

            var comment = new Comment
            {
                ParentId = dto.ParentId,
                UserName = dto.UserName,
                Email = dto.Email,
                HomePage = dto.HomePage,
                RawText = dto.RawText,
                SanitizedText = sanitized,
                CreatedAtUtc = DateTime.UtcNow,
                UserIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = Request.Headers["User-Agent"].ToString()
            };

            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();

            return Ok(new { comment.Id });
        }

        // GET: api/comments/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CommentListItemDto>> GetById(int id)
        {
            var comment = await _db.Comments
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            var dto = new CommentListItemDto
            {
                Id = comment.Id,
                ParentId = comment.ParentId,
                UserName = comment.UserName,
                Email = comment.Email,
                HomePage = comment.HomePage,
                SanitizedText = comment.SanitizedText,
                CreatedAtUtc = comment.CreatedAtUtc
            };

            return dto;
        }

        // GET: api/comments
        [HttpGet]
        public async Task<ActionResult<PagedResult<CommentListItemDto>>> GetRootComments(
            int page = 1,
            int pageSize = 25,
            string sortBy = "createdAt",
            string sortDirection = "desc")
        {
            if (page <= 0)
            {
                page = 1;
            }

            if (pageSize <= 0 || pageSize > 100)
            {
                pageSize = 25;
            }

            var query = _db.Comments
                .AsNoTracking()
                .Where(c => c.ParentId == null);

            sortBy = sortBy.ToLower();
            sortDirection = sortDirection.ToLower();

            if (sortBy == "username")
            {
                query = sortDirection == "asc" 
                    ? query.OrderBy(c => c.UserName) 
                    : query.OrderByDescending(c => c.UserName);
            }
            else if (sortBy == "email")
            {
                query = sortDirection == "asc"
                    ? query.OrderBy(c => c.Email)
                    : query.OrderByDescending(c => c.Email);
            }
            else
            {
                query = sortDirection == "asc"
                    ? query.OrderBy(c => c.CreatedAtUtc)
                    : query.OrderByDescending(c => c.CreatedAtUtc);
            }

            var totalCount = await query.CountAsync();

            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).Select(c => new CommentListItemDto
            {
                Id = c.Id,
                ParentId = c.ParentId,
                UserName = c.UserName,
                Email = c.Email,
                HomePage = c.HomePage,
                SanitizedText = c.SanitizedText,
                CreatedAtUtc = c.CreatedAtUtc
            }).ToListAsync();

            var result = new PagedResult<CommentListItemDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return Ok(result);
        }
    }
}
