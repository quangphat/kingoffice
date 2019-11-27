using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DatabaseModels
{
    public class HosoModel
    {
        public int Id { get; set; }
        public string Code { get; set; }//
        
        public int HoSoCuaAi { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public DateTime? NgayNhandon { get; set; }
        public string Cmnd { get; set; }
        public int Gender { get; set; }
        public int DistrictId { get; set; }
        public string Address { get; set; }
        public int CourierId { get; set; }
        public int ProductId { get; set; }
        public bool IsBaohiem { get; set; }
        public int ThoihanVay { get; set; }
        public decimal BorrowAmount { get; set; }
        public int Status { get; set; }
        public int Result { get; set; }
        public string Comment { get; set; }
        public DateTime? BirthDay { get; set; }
        public DateTime? CmndDay { get; set; }
        public int UpdateBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public List<int> FileRequireIds { get; set; }
        public int? PartnerId { get; set; }
    }
    //public class HosoTestModel
    //{
    //    public int Id { get; set; }
    //    public string Code { get; set; }//

    //    public int HoSoCuaAi { get; set; }
    //    public string CustomerName { get; set; }
    //    public string Phone { get; set; }
    //    public string Phone2 { get; set; }
    //    public DateTime? NgayNhandon { get; set; }
    //    public string Cmnd { get; set; }
    //    public int Gender { get; set; }
    //    public int DistrictId { get; set; }
    //    public string Address { get; set; }
    //    public int CourierId { get; set; }
    //    public int ProductId { get; set; }
    //    public int IsBaohiem { get; set; }
    //    public int ThoihanVay { get; set; }
    //    public decimal BorrowAmount { get; set; }
    //    public int Status { get; set; }
    //    public int Result { get; set; }
    //    public string Comment { get; set; }
    //}
}
