using CommentsSpaApi.Data;
using CommentsSpaApi.Domain.Entities;
using CommentsSpaApi.Domain.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System.Net.NetworkInformation;
using static System.Net.Mime.MediaTypeNames;

namespace CommentsSpaApi.Services.Attachments
{
    public class AttachmentService : IAttachmentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        private const int MaxImageWidth = 320;
        private const int MaxImageHeight = 240;
        private const long MaxTextSizeBytes = 100 * 1024; // 100 kb

        public AttachmentService(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }


        public async Task<Attachment> UploadAsync(int commentId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is required.");
            }

            var exists = await _db.Comments.AnyAsync(c => c.Id == commentId);
            if (!exists)
            {
                throw new KeyNotFoundException("Comment not found.");
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var contentType = file.ContentType;

            var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsRoot = Path.Combine(webRoot, "uploads");
            Directory.CreateDirectory(uploadsRoot);

            if (IsTextFile(extension, contentType))
            {
                if (file.Length > MaxTextSizeBytes)
                {
                    throw new ArgumentException("Text file is too large (max 100 KB).");
                }

                var textFolder = Path.Combine(uploadsRoot, "text");
                Directory.CreateDirectory(textFolder);

                var storedFileName = $"{Guid.NewGuid():N}{extension}";
                var fullPath = Path.Combine(textFolder, storedFileName);

                using (var stream = System.IO.File.Create(fullPath))
                {
                    await file.CopyToAsync(stream);
                }

                var relativePath = Path.Combine("uploads", "text", storedFileName).Replace("\\", "/");

                var attachment = new Attachment
                {
                    CommentId = commentId,
                    OriginalFileName = file.FileName,
                    StoredFileName = storedFileName,
                    RelativePath = relativePath,
                    Type = AttachmentType.Text,
                    SizeBytes = file.Length,
                    MimeType = contentType
                };

                _db.Attachments.Add(attachment);
                await _db.SaveChangesAsync();

                return attachment;
            }
            else if (IsImageFile(extension, contentType))
            {
                var imageFolder = Path.Combine(uploadsRoot, "images");
                Directory.CreateDirectory(imageFolder);

                var storedFileName = $"{Guid.NewGuid():N}{extension}";
                var fullPath = Path.Combine(imageFolder, storedFileName);

                int width;
                int height;

                await using (var inputStream = file.OpenReadStream())
                using (var image = await SixLabors.ImageSharp.Image.LoadAsync(inputStream))
                {
                    var originalWidth = image.Width;
                    var originalHeight = image.Height;

                    (width, height) = GetNewSize(originalWidth, originalHeight);

                    if (width != originalWidth || height != originalHeight)
                    {
                        image.Mutate(x => x.Resize(width, height));
                    }

                    var encoder = GetEncoder(extension);

                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
                    await image.SaveAsync(fullPath, encoder);
                }

                var fileInfo = new FileInfo(fullPath);
                var relativePath = Path.Combine("uploads", "images", storedFileName).Replace("\\", "/");

                var attachment = new Attachment
                {
                    CommentId = commentId,
                    OriginalFileName = file.FileName,
                    StoredFileName = storedFileName,
                    RelativePath = relativePath,
                    Type = AttachmentType.Image,
                    SizeBytes = fileInfo.Length,
                    MimeType = contentType,
                    Width = width,
                    Height = height
                };

                _db.Attachments.Add(attachment);
                await _db.SaveChangesAsync();

                return attachment;
            }
            else
            {
                throw new ArgumentException("Unsupported file type.Only JPG, PNG, GIF images and TXT files are allowed.");
            }

        }


        public async Task<List<Attachment>> GetForCommentAsync(int commentId)
        {
            var exists = await _db.Comments.AnyAsync(c => c.Id == commentId);
            if (!exists)
            {
                throw new KeyNotFoundException("Comment not found.");
            }

            return await _db.Attachments
                .Where(a => a.CommentId == commentId)
                .OrderBy(a => a.Id)
                .ToListAsync();
        }


        public async Task<(Attachment attachment, string fullPath)?> GetFileAsync(int attachmentId)
        {
            var attachment = await _db.Attachments.FirstOrDefaultAsync(a => a.Id == attachmentId);
            if (attachment == null)
            {
                return null;
            }

            var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var fullPath = Path.Combine(webRoot, attachment.RelativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (!System.IO.File.Exists(fullPath))
            {
                return null;
            }

            return (attachment, fullPath);
        }

        


        private static bool IsTextFile(string extension, string contentType)
        {
            return extension == ".txt" || contentType == "text/plain";
        }


        private static bool IsImageFile(string extension, string contentType)
        {
            return 
                extension == ".jpg" || extension == ".jpeg" ||
                extension == ".png" || extension == ".gif" ||
                contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
        }


        private static (int width, int height) GetNewSize(int originalWidth, int originalHeight)
        {
            var ratioX = (double)MaxImageWidth / originalWidth;
            var ratioY = (double)MaxImageHeight / originalHeight;
            var ratio = Math.Min(1.0, Math.Min(ratioX, ratioY));

            var newWidth = (int)(originalWidth * ratio);
            var newHeight = (int)(originalHeight * ratio);

            return (newWidth, newHeight);
        }


        private static IImageEncoder GetEncoder(string extension)
        {
            extension = extension.ToLowerInvariant();

            if (extension == ".jpg" || extension == ".jpeg")
            {
                return new JpegEncoder { Quality = 90 };
            }

            else if (extension == ".png")
            {
                return new PngEncoder();
            }

            else if (extension == ".gif")
            {
                return new GifEncoder();
            }

            else
            {
                throw new NotSupportedException($"Unsupported image format {extension}");
            }
        }
        
    }
}
