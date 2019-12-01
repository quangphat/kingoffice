using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Entity.Infrastructures;
using Entity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace KingOffice.Controllers
{
    [Route("media")]
    public class MediaController : BaseController
    {
        protected readonly IMediaBusiness _bizMedia;
        protected readonly IHostingEnvironment _hosting;
        public MediaController(CurrentProcess process,
            IHostingEnvironment hostingEnvironment,
            IMediaBusiness mediaBusiness) : base(process)
        {
            _bizMedia = mediaBusiness;
            _hosting = hostingEnvironment;
        }
        [Authorize]
        [HttpPost("uploadtemp/{key}/{fileId}")]
        public async Task<IActionResult> UploadFile(string key, int fileId)
        {
            if (Request.Form != null && Request.Form.Files.Any())
            {
                MediaUploadConfig result = null;
                var file = Request.Form.Files.FirstOrDefault();
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    result = await _bizMedia.UploadSingle(stream, key, fileId, file.FileName, _hosting.WebRootPath);
                }
                return Json(result);
            }
            return ToResponse(string.Empty);
        }
        [Authorize]
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteFile(string key)
        {
            return Json(new { Result = string.Empty });
        }
        [Authorize]
        [HttpGet("download")]
        public async Task<IActionResult> Download(string path)
        {
            string mimeType = MimeTypes.GetMimeType(path);
            string ext = System.IO.Path.GetExtension(path);
            FileStream fs = new FileStream(System.IO.Path.Combine(_hosting.WebRootPath,path), FileMode.Open, FileAccess.Read);
            if (ext == ".pdf")
                Response.Headers.Add("content-disposition", "inline; filename=" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext);
            else
                Response.Headers.Add("content-disposition", "attachment; filename=" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext);
            return File(fs, mimeType);
        }
    }
}