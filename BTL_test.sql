-- Tạo database
create database QUAN_LY_DIEM_SINH_VIEN1;
go

use QUAN_LY_DIEM_SINH_VIEN1;
go

-- Bảng Khoa
create table Khoa(
	MaKhoa varchar(5) primary key,
	TenKhoa varchar(100),
	TruongKhoa varchar(7) null -- sẽ thêm ràng buộc sau
);
go

-- Bảng GiangVien
create table GiangVien (
	MaGV varchar(7) primary key,
	HoTen varchar(100),
	NgSinh date check( datediff(year, NgSinh, getdate()) >= 22 ),
	GT varchar(3) check( GT in( 'Nam', 'Nu' ) ),
	MaKhoa varchar(5) references Khoa(MaKhoa),
	SDT varchar(11) check( SDT not like '%[^0-9]%' and len(SDT) in (10, 11) ),
	check( left(MaGV, len(MaGV) - 2) = MaKhoa and right(MaGV, 2) like '[0-9][0-9]' )
);
go

-- Bảng ChuNhiem
create table ChuNhiem(
	MaCN varchar(5) primary key,
	NgSinh date check( datediff(year, NgSinh, getdate()) >= 22 ),
	SDT varchar(11) check( SDT not like '%[^0-9]%' and len(SDT) in (10, 11) ),
	HoTen varchar(100),
	GT varchar(3) check( GT in( 'Nam', 'Nu' ) ),
	check( len(MaCN) = 4 and left(MaCN, 2) = 'CN' and right(MaCN, 2) like '[0-9][0-9]' )
);
go

-- Bảng Lop
create table Lop(
	MaLop varchar(6) primary key,
	MaCN varchar(5) references ChuNhiem(MaCN),
	MaKhoa varchar(5) references Khoa(MaKhoa)
);
go

-- Bảng SinhVien
create table SinhVien(
	MaSV varchar(8) primary key,
	HoTen varchar(100),
	NgSinh date check( datediff(year, NgSinh, getdate()) >= 18 ),
	GT varchar(3) check( GT in( 'Nam', 'Nu' ) ),
	MaLop varchar(6) references Lop(MaLop),
	TrangThai varchar(20) check( TrangThai in( 'Dang hoc', 'Da ra truong', 'Tam nghi' ) ),
	NamNhapHoc int not null,
	NamRaTruong int null,
	check( left(MaSV, len(MaSV) - 2) = MaLop and right(MaSV, 2) like '[0-9][0-9]' ),
	check( (NamRaTruong is null) or (NamRaTruong is not null and NamNhapHoc < NamRaTruong) ),
	check( NamNhapHoc > year(NgSinh) )
);
go

-- Bảng HocPhan
create table HocPhan(
	MaHP varchar(10) primary key,
	TenHP varchar(100),
	SoTC tinyint,
	LoaiHP varchar(12) check( LoaiHP in( 'Dai cuong', 'Chuyen nganh', 'Tu chon' ) ),
	SoTiet tinyint,
	MaKhoa varchar(5) references Khoa(MaKhoa),
	check( len(MaHP) = 4 and left(MaHP, 2) = 'HP' and right(MaHP, 2) like '[0-9][0-9]' )
);
go

-- Bảng GiangDay
create table GiangDay(
	MaHP varchar(10) references HocPhan(MaHP),
	MaGV varchar(7) references GiangVien(MaGV),
	HocKy tinyint check( HocKy in(1, 2) ),
	NamHoc varchar(9) check( len(NamHoc) = 9 
		and charindex('-', NamHoc) = 5 
		and try_cast(left(NamHoc, 4) as int) is not null 
		and try_cast(right(NamHoc, 4) as int) is not null
		and try_cast(left(NamHoc, 4) as int) < try_cast(right(NamHoc, 4) as int)
	),
	primary key(MaHP, MaGV, HocKy, NamHoc)
);
go

-- Bảng KetQuaHocPhan
create table KetQuaHocPhan (
	MaSV varchar(8) references SinhVien(MaSV),
	MaHP varchar(10) references HocPhan(MaHP),
	ThuongXuyen float check( 0 <= ThuongXuyen and ThuongXuyen <= 10 ),
	ChuyenCan float check( 0 <= ChuyenCan and ChuyenCan <= 10 ),
	GHP float check( 0 <= GHP and GHP <= 10 ),
	CHP float check( 0 <= CHP and CHP <= 10 ),
	TongKet float check( 0 <= TongKet and TongKet <= 10 ),
	HocKy tinyint check( HocKy in(1, 2) ),
	NamHoc varchar(9) check( len(NamHoc) = 9 
		and charindex('-', NamHoc) = 5 
		and try_cast(left(NamHoc, 4) as int) is not null 
		and try_cast(right(NamHoc, 4) as int) is not null
		and try_cast(left(NamHoc, 4) as int) < try_cast(right(NamHoc, 4) as int)
	),
	primary key(MaSV, MaHP)
);
go

-- Bảng KetQuaNamHoc
create table KetQuaNamHoc(
	NamHoc varchar(9) check( len(NamHoc) = 9 
		and charindex('-', NamHoc) = 5 
		and try_cast(left(NamHoc, 4) as int) is not null 
		and try_cast(right(NamHoc, 4) as int) is not null
		and try_cast(left(NamHoc, 4) as int) < try_cast(right(NamHoc, 4) as int)
	),
	MaSV varchar(8) references SinhVien(MaSV),
	TBHK1 float check( 0 <= TBHK1 and TBHK1 <= 10 ),
	TBHK2 float check( 0 <= TBHK2 and TBHK2 <= 10 ),
	TBNH float check( 0 <= TBNH and TBNH <= 10 ),
	XEPLOAI varchar(11) check( XEPLOAI in( 'Trung Binh', 'Kha', 'Gioi', 'Xuat Sac' ) ),
	primary key(NamHoc, MaSV)
);
go

-- Bảng KhenThuong
create table KhenThuong(
	MaKT varchar(10),
	MaSV varchar(8) references SinhVien(MaSV),
	DiemCong float check( 0 <= DiemCong and DiemCong <= 10 ),
	NamHoc varchar(9) check( len(NamHoc) = 9 
		and charindex('-', NamHoc) = 5 
		and try_cast(left(NamHoc, 4) as int) is not null 
		and try_cast(right(NamHoc, 4) as int) is not null
		and try_cast(left(NamHoc, 4) as int) < try_cast(right(NamHoc, 4) as int)
	),
	MoTa varchar(100),
	primary key(MaKT, MaSV)
);
go

-- Bảng TKGV
create table TKGV (
	TenDangNhap varchar(100) primary key,
	MatKhau varchar(100),
	MaGV varchar(7) references GiangVien(MaGV)
);
go

-- Bảng TKSV
create table TKSV(
	TenDangNhap varchar(100) primary key,
	MatKhau varchar(100),
	MaSV varchar(8) references SinhVien(MaSV)
);
go

-- Bảng TKCN
create table TKCN(
	TenDangNhap varchar(100) primary key,
	MatKhau varchar(100),
	MaCN varchar(5) references ChuNhiem(MaCN)
);
go

-- Ràng buộc TruongKhoa phải là GiangVien đã có
alter table Khoa add constraint FK_Khoa_TruongKhoa foreign key (TruongKhoa) references GiangVien(MaGV);
go


-- Trigger cập nhật điểm tổng kết học phần khi thêm hoặc sửa KetQuaHocPhan
create trigger trg_tinhdiemtongket_KetQuaHocPhan
on KetQuaHocPhan
after insert, update
as 
begin
	update kqhp
	set TongKet = i.ChuyenCan * 0.1 + i.ThuongXuyen * 0.1 + i.GHP * 0.2 + i.CHP * 0.6
	from KetQuaHocPhan kqhp
		join inserted i on kqhp.MaHP = i.MaHP and kqhp.MaSV = i.MaSV
	where i.ChuyenCan is not null 
	   or i.ThuongXuyen is not null
	   or i.GHP is not null
	   or i.CHP is not null ;
end;
go


-- Trigger cập nhật điểm trung bình năm học trong KetQuaNamHoc
create trigger trg_tinhdiemnamhoc_KetQuaHocPhan
on KetQuaHocPhan
after insert, update
as 
begin
	-- Cập nhật điểm trung bình học kỳ 1, học kỳ 2, điểm trung bình năm học và xếp loại
	merge KetQuaNamHoc as target
	using (
		select
			kq.MaSV,
			kq.NamHoc,
			sum(case when kq.HocKy = 1 then kq.TongKet * hp.SoTC else 0 end) / nullif(sum(case when kq.HocKy = 1 then hp.SoTC else 0 end), 0) as TBHK1,
			sum(case when kq.HocKy = 2 then kq.TongKet * hp.SoTC else 0 end) / nullif(sum(case when kq.HocKy = 2 then hp.SoTC else 0 end), 0) as TBHK2,
			sum(kq.TongKet * hp.SoTC) / nullif(sum(hp.SoTC), 0) as TBNH,
			case
				when sum(kq.TongKet * hp.SoTC) / nullif(sum(hp.SoTC), 0) >= 9.0 then 'Xuat Sac'
				when sum(kq.TongKet * hp.SoTC) / nullif(sum(hp.SoTC), 0) >= 8.0 then 'Gioi'
				when sum(kq.TongKet * hp.SoTC) / nullif(sum(hp.SoTC), 0) >= 6.5 then 'Kha'
				when sum(kq.TongKet * hp.SoTC) / nullif(sum(hp.SoTC), 0) >= 5.0 then 'Trung Binh'
				else NULL
			end as XEPLOAI
		from KetQuaHocPhan kq
		join HocPhan hp on kq.MaHP = hp.MaHP
		group by kq.MaSV, kq.NamHoc
	) as source
	on target.MaSV = source.MaSV and target.NamHoc = source.NamHoc
	when matched then
		update set 
			TBHK1 = source.TBHK1,
			TBHK2 = source.TBHK2,
			TBNH = source.TBNH,
			XEPLOAI = source.XEPLOAI
	when not matched then
		insert (NamHoc, MaSV, TBHK1, TBHK2, TBNH, XEPLOAI) 
		values (source.NamHoc, source.MaSV, source.TBHK1, source.TBHK2, source.TBNH, source.XEPLOAI);
end;


-- drop trigger trg_xoahocvien_SinhVien;
INSERT INTO ChuNhiem (MaCN, NgSinh, SDT, HoTen, GT) VALUES
('CN01', '1990-04-15', '0912345678', 'Nguyen Van Chu', 'Nam'),
('CN02', '1988-06-22', '0987654321', 'Tran Thi Chu', 'Nu');

INSERT INTO Khoa (MaKhoa, TenKhoa, TruongKhoa) VALUES
('KH01', 'Khoa CNTT', NULL),
('KH02', 'Khoa Kinh Te', NULL);

INSERT INTO Lop (MaLop, MaCN, MaKhoa) VALUES
('CNTT01', 'CN01', 'KH01'),
('KTKT02', 'CN02', 'KH02');

INSERT INTO SinhVien (MaSV, HoTen, NgSinh, GT, MaLop, TrangThai, NamNhapHoc, NamRaTruong) VALUES
('CNTT0101', 'Nguyen Van A', '2000-05-10', 'Nam', 'CNTT01', 'Dang hoc', 2019, NULL),
('CNTT0102', 'Tran Thi B', '1999-08-15', 'Nu', 'CNTT01', 'Da ra truong', 2018, 2022),
('KTKT0201', 'Le Van C', '2001-01-20', 'Nam', 'KTKT02', 'Tam nghi', 2020, NULL),
('KTKT0202', 'Pham Thi D', '2000-12-01', 'Nu', 'KTKT02', 'Dang hoc', 2019, NULL);

INSERT INTO GiangVien (MaGV, HoTen, NgSinh, GT, MaKhoa, SDT) VALUES
('KH0101', 'Nguyen Van G', '1985-03-10', 'Nam', 'KH01', '0911111111'),
('KH0102', 'Le Thi H', '1982-09-15', 'Nu', 'KH01', '0922222222'),
('KH0201', 'Tran Van I', '1980-07-20', 'Nam', 'KH02', '0933333333');
INSERT INTO HocPhan (MaHP, TenHP, SoTC, LoaiHP, SoTiet, MaKhoa) VALUES
('HP01', 'Toan Cao Cap', 3, 'Dai cuong', 45, 'KH01'),
('HP02', 'Lap Trinh C', 3, 'Chuyen nganh', 45, 'KH01'),
('HP03', 'Kinh Te Vi Mo', 2, 'Dai cuong', 30, 'KH02'),
('HP04', 'Ke Toan Tai Chinh', 3, 'Chuyen nganh', 45, 'KH02');
INSERT INTO GiangDay (MaHP, MaGV, HocKy, NamHoc) VALUES
('HP01', 'KH0101', 1, '2023-2024'),
('HP02', 'KH0102', 2, '2023-2024'),
('HP03', 'KH0201', 1, '2023-2024'),
('HP04', 'KH0201', 2, '2023-2024');

INSERT INTO KetQuaHocPhan (MaSV, MaHP, ThuongXuyen, ChuyenCan, GHP, CHP, TongKet, HocKy, NamHoc) VALUES
('CNTT0101', 'HP01', 8.0, 9.0, 8.5, 9.0, 8.7, 1, '2023-2024'),
('CNTT0102', 'HP02', 7.5, 8.0, 8.0, 8.5, 8.0, 2, '2023-2024'),
('KTKT0201', 'HP03', 6.0, 7.0, 7.5, 7.0, 7.0, 1, '2023-2024'),
('KTKT0202', 'HP04', 8.5, 9.5, 9.0, 9.0, 9.0, 2, '2023-2024');

SELECT * FROM ketquanamhoc