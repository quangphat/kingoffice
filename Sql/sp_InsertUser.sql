create procedure sp_InsertUser
(
@id int out,
@code varchar(50),
@userName varchar(50),
@password varchar(50),
@fullName nvarchar(100),
@phone varchar(50),
@email varchar(50),
@createtime datetime,
@createby int
)
as
begin
insert into NHAN_VIEN (Ma,Ten_Dang_Nhap,Mat_Khau,Ho_Ten,Dien_Thoai,Email,Xoa,CreatedTime,CreatedBy)
values (@code,@userName,@password,@fullName,@phone,@email,0,@createtime,@createby);
SET @id=@@IDENTITY
end