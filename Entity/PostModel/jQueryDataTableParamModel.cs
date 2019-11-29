using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.PostModel
{
    public class jQueryDataTableParamModel
    {
        /// <summary>
        /// Request sequence number sent by DataTable,
        /// same value must be returned in response
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string sSearch { get; set; }

        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int iColumns { get; set; }

        /// <summary>
        /// Number of columns that are used in sorting
        /// </summary>
        public int iSortingCols { get; set; }

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string sColumns { get; set; }
    }
    public class ProductImport
    {
        public jQueryDataTableParamModel param { get; set; }
        public List<ProductViewModel> lstSanPham { get; set; }
    }
    public class ProductImportResponse
    {
        public string sEcho { get; set; }
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public List<ProductViewModel> aaData { get; set; }
    }
    public class ImportSaveModel
    {
        public DateTime? ImportDate { get; set; }
        public List<ProductViewModel> lstSanPham { get; set; }
    }
}
