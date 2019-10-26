using Entity.Enums;
using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Business.Infrastructures
{
    public static class BusinessExtension
    {
        public static string JoinTrangThai()
        {
            return ((int)TrangThaiHoSo.TuChoi).ToString() + ","
                + ((int)TrangThaiHoSo.NhapLieu).ToString() + ","
                + ((int)TrangThaiHoSo.ThamDinh).ToString() + ","
                + ((int)TrangThaiHoSo.BoSungHoSo).ToString() + ","
                + ((int)TrangThaiHoSo.Cancel).ToString() + ","
                + ((int)TrangThaiHoSo.DaDoiChieu).ToString() + ","
                + ((int)TrangThaiHoSo.PCB).ToString() + ","
                + ((int)TrangThaiHoSo.GiaiNgan).ToString();
        }
        public static bool IsValidEmail(string email, int maxLength = 255)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            if (email.Length > maxLength)
            {
                return false;
            }

            var patternEmail =
            "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";

            return Regex.IsMatch(email, patternEmail);
        }
        public static string[] GetFilesMissing(List<LoaiTaiLieuModel> loaiTailieus, List<FileUploadModel> filesUpload)
        {
            var names = new List<string>();
            if (!filesUpload.Any())
            {
                return loaiTailieus.Where(p => p.BatBuoc == 1).Select(p => p.Ten).ToArray();

            }

            var keys = filesUpload.Select(p => Convert.ToInt32(p.Key)).ToList();
            var mustHaveIds = loaiTailieus.Where(p => p.BatBuoc == 1).Select(p => p.ID).ToList();
            foreach (int id in mustHaveIds)
            {
                if (!keys.Contains(id))
                    names.Add(loaiTailieus.FirstOrDefault(p => p.ID == id).Ten);

            }
            return names.ToArray();
        }
        public static DateTime ConvertddMMyyyyToDateTime(this string str)
        {
            string[] p = str.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            DateTime date = new DateTime(Convert.ToInt32(p[2]), Convert.ToInt32(p[1]), Convert.ToInt32(p[0]));
            return date;

        }
        public static DateTime ConvertddMMyyyyHHssToDateTime(this string str)
        {
            string ddMMYYYY = str.Substring(0, 10);
            string[] aa = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string HHMM = str.Substring(ddMMYYYY.Length + 1, str.Length - ddMMYYYY.Length - 1);
            string[] p = ddMMYYYY.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            string[] h = HHMM.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            DateTime date = new DateTime(Convert.ToInt32(p[2]), Convert.ToInt32(p[1]), Convert.ToInt32(p[0]), Convert.ToInt16(h[0]), Convert.ToInt16(h[1]), 0);
            return date;

        }
        public static DateTime ConvertyyyyMMddToDateTime(this string str)
        {
            string[] p = str.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            DateTime date = new DateTime(Convert.ToInt32(p[0]), Convert.ToInt32(p[1]), Convert.ToInt32(p[2]));
            return date;

        }
        public static object ConvertddMMyyyy(this string str)
        {
            string[] p = str.Split(new string[] { "-", "/", "." }, StringSplitOptions.RemoveEmptyEntries);
            DateTime date = new DateTime(Convert.ToInt32(p[2]), Convert.ToInt32(p[1]), Convert.ToInt32(p[0]));
            return date;

        }
        public static string GenerateAutoCode(ref AutoIDModel model)
        {
            string result = string.Empty;
            if (model == null)
                model = new AutoIDModel();
            string suffixes = "00" + DateTime.Now.Month.ToString();
            suffixes = suffixes.Substring(suffixes.Length - 2, 2);
            if (model.Prefix == DateTime.Now.Year.ToString().Substring(2, 2))
            {

                if (model.Suffixes == suffixes)
                {
                    model.Value++;
                }
                else
                {
                    model.Suffixes = suffixes;
                    model.Value = 1;
                }
            }
            else
            {
                model.Prefix = DateTime.Now.Year.ToString().Substring(2, 2);
                model.Suffixes = suffixes;
                model.Value = 1;
            }
            int length = 6;
            string valueDefault = "000000" + model.Value.ToString();
            result = valueDefault.Substring(valueDefault.Length - length, length);

            return model.Prefix + model.Suffixes + result;
        }
        public static string GetSHA256Hash(string input)
        {
            if (String.IsNullOrEmpty(input))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(input);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
        public static FileModel GetFileUploadUrl(string fileInputName, string webRootPath)
        {
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + fileInputName.Trim().Replace(" ", "_");
            string root = System.IO.Path.Combine(webRootPath + "/Upload", "HoSo");
            string pathTemp = "";
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);
            pathTemp = DateTime.Now.Year.ToString();
            string pathYear = System.IO.Path.Combine(root, pathTemp);
            if (!Directory.Exists(pathYear))
                Directory.CreateDirectory(pathYear);
            pathTemp += "/" + DateTime.Now.Month.ToString();
            string pathMonth = System.IO.Path.Combine(root, pathTemp);
            if (!Directory.Exists(pathMonth))
                Directory.CreateDirectory(pathMonth);
            pathTemp += "/" + DateTime.Now.Day.ToString();
            string pathDay = System.IO.Path.Combine(root, pathTemp);
            if (!Directory.Exists(pathDay))
                Directory.CreateDirectory(pathDay);
            string path = System.IO.Path.Combine(pathDay, fileName);
            string url = "/Upload/HoSo/" + pathTemp + "/" + fileName;
            return new FileModel
            {
                FileUrl = url,
                Name = fileName,
                FullPath = path
            };

        }
    }
}
