﻿using Entity.Enums;
using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
