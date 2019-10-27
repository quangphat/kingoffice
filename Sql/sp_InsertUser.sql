USE [Test13]
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertUser]    Script Date: 27/10/2019 9:15:12 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_InsertUser]
(
@id int out,
@userName varchar(50),
@password varchar(50),
@fullName nvarchar(100),
@phone varchar(50),
@email varchar(50),
@provinceId int,
@districtId int,
@createdtime datetime,
@createdby int,
@workDate datetime
)
as
begin
insert into NHAN_VIEN (Ten_Dang_Nhap,Mat_Khau,Ho_Ten,Dien_Thoai,Email,WorkDate,ProvinceId,DistrictId,Trang_Thai,Xoa,CreatedTime,CreatedBy)
values (@userName,@password,@fullName,@phone,@email,@workDate,@provinceId,@districtId,1,0,@createdtime,@createdby);
SET @id=@@IDENTITY
end

