--SELECT name as Vietbank FROM sys.Tables order by name

--select * from NHAN_VIEN


--all admin
select * from NHAN_VIEN where ID in (select Ma_nv from NHAN_VIEN_QUYEN)


--all leader
select distinct (nv.Id),nv.Ma,nv.Ten_Dang_nhap, nv.Ho_Ten from NHAN_VIEN nv inner join NHOM n
on nv.id = n.Ma_Nguoi_QL
 where nv.id = 35


exec sp_CheckIsAdmin 35

exec sp_CheckIsTeamlead 35