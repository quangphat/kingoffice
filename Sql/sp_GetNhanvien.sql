USE [Test13]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetNhanvien]    Script Date: 27/10/2019 10:35:15 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select * from NHAN_VIEN

ALTER procedure [dbo].[sp_GetNhanvien]
(
@fromDate datetime,
@toDate datetime,
@dateFilterType int,
@freeText nvarchar(30),
@offset int,
@limit int
)
as
begin
if @freeText = '' begin set @freeText = null end;
Select n.ID,n.Ten_Dang_Nhap,n.Ho_Ten,
n.Email, n.Dien_Thoai,n.CreatedTime,
n.WorkDate,n.ProvinceId
 from NHAN_VIEN n left join KHU_VUC k on n.DistrictId = k.ID
where convert(varchar(10),n.WorkDate,121) >= (convert(varchar(10),@fromDate,121))
and convert(varchar(10),n.WorkDate,121) <= (convert(varchar(10),@toDate,121))
	and (@freeText is null or n.Ho_Ten like N'%' + @freeText + '%'
		or n.Ten_Dang_Nhap like N'%' + @freeText + '%'
		or n.Dien_Thoai like N'%' + @freeText + '%'
		or n.Email like N'%' + @freeText + '%')
		and k.Loai = 2
		and n.Xoa = 0
order by n.CreatedTime desc 
	offset @offset ROWS FETCH NEXT @limit ROWS ONLY
end