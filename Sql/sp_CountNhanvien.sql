USE [Test13]
GO
/****** Object:  StoredProcedure [dbo].[sp_CountNhanvien]    Script Date: 27/10/2019 10:35:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select * from NHAN_VIEN

ALTER procedure [dbo].[sp_CountNhanvien]
(
@fromDate datetime,
@toDate datetime,
@dateFilterType int,
@freeText nvarchar(30)
)
as
begin
if @freeText = '' begin set @freeText = null end;
Select count(*) from NHAN_VIEN n
where convert(varchar(10),n.WorkDate,121) >= (convert(varchar(10),@fromDate,121))
and convert(varchar(10),n.WorkDate,121) <= (convert(varchar(10),@toDate,121))
	and (@freeText is null or n.Ho_Ten like N'%' + @freeText + '%'
		or n.Ten_Dang_Nhap like N'%' + @freeText + '%'
		or n.Dien_Thoai like N'%' + @freeText + '%'
		or n.Email like N'%' + @freeText + '%')
		and n.Xoa = 0
end