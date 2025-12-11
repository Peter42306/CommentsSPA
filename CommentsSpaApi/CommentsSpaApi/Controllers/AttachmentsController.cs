using CommentsSpaApi.Domain.Entities;
using CommentsSpaApi.Dtos.Attachments;
using CommentsSpaApi.Services.Attachments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommentsSpaApi.Controllers
{
    [Route("api/comments/{commentId:int}/attachments")]
    [ApiController]
    public class AttachmentsController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;

        public AttachmentsController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        
        // POST: /api/comments/{commentId}/attachments
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(
            int commentId, 
            [FromForm] AttachmentUploadRequest request)
        {
            try
            {
                var file = request.File;
                var attachment = await _attachmentService.UploadAsync(commentId, file);
                
                var dto = ToDto(attachment);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }           
        }

        // GET: /api/comments/{commentId}/attachments
        [HttpGet]
        public async Task<ActionResult<List<AttachmentDto>>> GetForComment(int commentId)
        {
            try
            {
                var attachments = await _attachmentService.GetForCommentAsync(commentId);                

                var dtos = attachments.Select(a => new AttachmentDto
                {
                    Id = a.Id,
                    OriginalFileName = a.OriginalFileName,
                    Url = "/" + a.RelativePath.Replace("\\", "/"),
                    Type = a.Type,
                    SizeBytes = a.SizeBytes,
                    Width = a.Width,
                    Height = a.Height,
                }).ToList();

                return Ok(dtos);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }


        private AttachmentDto ToDto(Attachment attachment)
        {            
            var url = "/" + attachment.RelativePath.Replace("\\","/");

            var data = new AttachmentDto
            {
                Id = attachment.Id,
                OriginalFileName = attachment.OriginalFileName,
                Url = url,
                Type = attachment.Type,
                SizeBytes = attachment.SizeBytes,
                Width = attachment.Width,
                Height = attachment.Height,
            };

            return data;
        }
    }
}
