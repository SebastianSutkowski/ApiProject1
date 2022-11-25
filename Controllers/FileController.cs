using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.StaticFiles;
namespace _ApiProject1_.Controllers
{
    [Route("file")]
    [Authorize]
    public class FileController:ControllerBase
    {
        [HttpGet]
        [ResponseCache(Duration =1200,VaryByQueryKeys = new[] { "filename" })]
        public ActionResult GetFile([FromQuery] string filename)
        {
            var rootPath=Directory.GetCurrentDirectory();
            var filePath=$"{rootPath}/PrivateFiles/{filename}";
            var fileExists=System.IO.File.Exists(filePath);
            if (!fileExists)
            {
                return NotFound();
            }

            var fileTypeProvider = new FileExtensionContentTypeProvider().TryGetContentType(filePath, out string contentType);
            var fileContents = System.IO.File.ReadAllBytes(filePath);
            return File(fileContents, contentType, filename);
        }
        [HttpPost]
        public ActionResult Upload([FromForm]IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var rootPath = Directory.GetCurrentDirectory();
                var filePath = $"{rootPath}/PrivateFiles/{file.FileName}";
                using(var stream=new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return Ok();
            }
            return BadRequest();
        }
    }
}
