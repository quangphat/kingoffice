using Business.Infrastructures;
using Business.Interfaces;
using Entity.Infrastructures;
using Entity.ViewModels;
using ExcelUtility;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Classes
{
    public class MediaBusiness : BaseBusiness, IMediaBusiness
    {
        protected readonly ITailieuRepository _rpTailieu;
        public MediaBusiness(CurrentProcess process, ITailieuRepository tailieuRepository) : base(null, process)
        {
            _rpTailieu = tailieuRepository;
        }
        public async Task<string> Download(List<HoSoQuanLyModel> datas, Entity.Enums.ExportType exportType, string webRootPath)
        {
            if (datas == null || !datas.Any())
                return string.Empty;
            string destDirectory = $"{Entity.Infrastructures.Constanst.DownloadFolder}/{DateTime.Now.Year.ToString()}/{DateTime.Now.Month.ToString()}";
            bool exists = System.IO.Directory.Exists($"{webRootPath}/{destDirectory}");
            if (!exists)
                System.IO.Directory.CreateDirectory($"{webRootPath}/{destDirectory}");
            string fileName = exportType == Entity.Enums.ExportType.DanhsachHoso ? $"{Constanst.ExportDanhsachHosoBaseFileName}{DateTime.Now.ToString("ddMMyyyyHHmmssfff")}.xlsx"
                : $"{Constanst.ExportDanhsachHosoBaseFileName}{DateTime.Now.ToString("ddMMyyyyHHmmssfff")}.xlsx";
            string fullPath = destDirectory + "/" + fileName;
            using (var stream = System.IO.File.Create(System.IO.Path.Combine(webRootPath, fullPath)))
            {
                Byte[] info = System.IO.File.ReadAllBytes(System.IO.Path.Combine($"{webRootPath}/{Constanst.ReportTemplate}/Report-DSHS.xlsx"));
                stream.Write(info, 0, info.Length);
                using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Update))
                {
                    string nameSheet = "DSHS";
                    ExcelOOXML excelOOXML = new ExcelOOXML(archive);
                    int rowindex = 4;
                    excelOOXML.InsertRow(nameSheet, rowindex, datas.Count - 1, true);
                    int stt = 1;
                    foreach (var item in datas)
                    {
                        excelOOXML.SetCellData(nameSheet, "A" + rowindex, stt.ToString());
                        excelOOXML.SetCellData(nameSheet, "B" + rowindex, item.MaHoSo.ToString());
                        excelOOXML.SetCellData(nameSheet, "C" + rowindex, item.NgayTao.ToString("dd/MM/yyyy"));
                        excelOOXML.SetCellData(nameSheet, "D" + rowindex, item.DoiTac);
                        excelOOXML.SetCellData(nameSheet, "E" + rowindex, item.CMND);
                        excelOOXML.SetCellData(nameSheet, "F" + rowindex, item.TenKH);
                        excelOOXML.SetCellData(nameSheet, "G" + rowindex, item.TrangThaiHS);
                        excelOOXML.SetCellData(nameSheet, "H" + rowindex, item.KetQuaHS);
                        excelOOXML.SetCellData(nameSheet, "I" + rowindex, item.NgayCapNhat == null ? "" : item.NgayCapNhat.ToString("dd/MM/yyyy"));
                        excelOOXML.SetCellData(nameSheet, "J" + rowindex, item.MaNV);
                        excelOOXML.SetCellData(nameSheet, "K" + rowindex, item.NhanVienBanHang);
                        excelOOXML.SetCellData(nameSheet, "L" + rowindex, item.DoiNguBanHang);
                        excelOOXML.SetCellData(nameSheet, "M" + rowindex, item.CoBaoHiem == true ? "N" : "Y");
                        excelOOXML.SetCellData(nameSheet, "N" + rowindex, item.KhuVucText);
                        excelOOXML.SetCellData(nameSheet, "O" + rowindex, item.GhiChu);
                        excelOOXML.SetCellData(nameSheet, "P" + rowindex, item.MaNVLayHS);
                        rowindex++;
                        stt++;
                    }

                    archive.Dispose();
                }
                stream.Dispose();
            }
            return $"/media/download?path={destDirectory}/{fileName}";
        }
        public async Task<string> Upload(Stream stream, string key, string name, string webRootPath)
        {
            stream.Position = 0;
            string fileUrl = string.Empty;
            var file = BusinessExtension.GetFileUploadUrl(name, webRootPath);
            using (var fileStream = System.IO.File.Create(file.FullPath))
            {
                await stream.CopyToAsync(fileStream);
                fileStream.Close();
                return file.FileUrl;
            }
        }
        public async Task<MediaUploadConfig> UploadSingle(Stream stream, string key, int fileId, string name, string webRootPath)
        {
            stream.Position = 0;
            string fileUrl = string.Empty;
            var file = BusinessExtension.GetFileUploadUrl(name, webRootPath);
            using (var fileStream = System.IO.File.Create(file.FullPath))
            {
                await stream.CopyToAsync(fileStream);
                fileStream.Close();
                fileUrl = file.FileUrl;
            }
            string deleteURL = fileId <= 0 ? $"/media/delete?key={key}" : $"/hoso/tailieu/delete/0/{fileId}";
            var update = await _rpTailieu.UpdateExistingFile(fileId, file.Name, file.FileUrl);
            if (update == true)
            {
                var _type = System.IO.Path.GetExtension(name);
                var config = new MediaUploadConfig
                {
                    initialPreview = fileUrl,
                    initialPreviewConfig = new PreviewConfig[] {
                                    new  PreviewConfig{
                                        caption = file.Name,
                                        url = deleteURL,
                                        key =key,
                                        type=_type.IndexOf("pdf") > 0 ?"pdf" : null,
                                        width ="120px"
                                        }
                                },
                    append = false,
                    Id = Guid.NewGuid()
                };
                return config;
            }
            return new MediaUploadConfig();
        }
    }
}
