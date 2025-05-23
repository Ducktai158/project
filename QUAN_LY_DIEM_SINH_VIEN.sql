-- Tạo database
CREATE DATABASE QUAN_LY_DIEM_SINH_VIEN;
GO

USE QUAN_LY_DIEM_SINH_VIEN;
GO

-- Bảng Khoa
CREATE TABLE Khoa(
	MaKhoa varchar(5) PRIMARY KEY,
	TenKhoa varchar(100)
);
GO


-- Bảng GiangVien
CREATE TABLE GiangVien (
	MaGV varchar(7) PRIMARY KEY,
	HoTen varchar(100),
	NgSinh date CHECK( DATEDIFF(year, NgSinh, GETDATE()) >= 22 ),
	GT varchar(3) CHECK( GT IN( 'Nam', 'Nu' ) ),
	MaKhoa varchar(5) REFERENCES Khoa(MaKhoa),
	SDT varchar(11) CHECK( SDT NOT LIKE '%[^0-9]%' AND LEN(SDT) IN (10, 11) ),
	HocVi VARCHAR(30) CHECK(HocVi IN (
		'CN',        -- Cử nhân
		'ThS',       -- Thạc sĩ
		'TS',        -- Tiến sĩ
		'TSKH'       -- Tiến sĩ khoa học
	)),
	HocHam VARCHAR(30) CHECK(HocHam IN (
		'',          -- Không có học hàm
		'GV',        -- Giảng viên
		'GVCH',      -- Giảng viên chính
		'PGS',       -- Phó giáo sư
		'GS'         -- Giáo sư
	)),
	CHECK( LEFT(MaGV, LEN(MaGV) - 2) = MaKhoa AND RIGHT(MaGV, 2) LIKE '[0-9][0-9]' )
);
GO


-- Bảng ChuNhiem
CREATE TABLE ChuNhiem(
	MaCN varchar(5) PRIMARY KEY,
	NgSinh date CHECK( DATEDIFF(year, NgSinh, GETDATE()) >= 22 ),
	SDT varchar(11) CHECK( SDT NOT LIKE '%[^0-9]%' AND LEN(SDT) IN (10, 11) ),
	HoTen varchar(100),
	GT varchar(3) CHECK( GT IN( 'Nam', 'Nu' ) ),
	CHECK( LEN(MaCN) = 4 AND LEFT(MaCN, 2) = 'CN' AND RIGHT(MaCN, 2) LIKE '[0-9][0-9]' )
);
GO


-- Bảng Lop
CREATE TABLE Lop(
	MaLop varchar(6) PRIMARY KEY,
	MaCN varchar(5) REFERENCES ChuNhiem(MaCN),
	MaKhoa varchar(5) REFERENCES Khoa(MaKhoa)
);
GO


-- Bảng SinhVien
CREATE TABLE SinhVien(
	MaSV varchar(8) PRIMARY KEY,
	HoTen varchar(100),
	NgSinh date CHECK( DATEDIFF(year, NgSinh, GETDATE()) >= 18 ),
	GT varchar(3) CHECK( GT IN( 'Nam', 'Nu' ) ),
	MaLop varchar(6) REFERENCES Lop(MaLop),
	TrangThai varchar(20) CHECK( TrangThai IN( 'Dang hoc', 'Da ra truong', 'Tam nghi' ) ),
	NamNhapHoc int NOT NULL,
	NamRaTruong int NULL,
	CHECK( LEFT(MaSV, LEN(MaSV) - 2) = MaLop AND RIGHT(MaSV, 2) LIKE '[0-9][0-9]' ),
	CHECK( (NamRaTruong IS NULL) OR (NamRaTruong IS NOT NULL AND NamNhapHoc < NamRaTruong) ),
	CHECK( NamNhapHoc > YEAR(NgSinh) ) -- A student cannot enroll before they are born
);
GO


-- Bảng HocPhan
CREATE TABLE HocPhan(
	MaHP varchar(10) PRIMARY KEY,
	TenHP varchar(100),
	SoTC tinyint,
	LoaiHP varchar(12) CHECK( LoaiHP IN( 'Dai cuong', 'Chuyen nganh', 'Tu chon' ) ),
	SoTiet tinyint,
	MaKhoa varchar(5) REFERENCES Khoa(MaKhoa),
	CHECK( LEN(MaHP) = 4 AND LEFT(MaHP, 2) = 'HP' AND RIGHT(MaHP, 2) LIKE '[0-9][0-9]' )
);
GO


-- Bảng GiangDay
CREATE TABLE GiangDay(
	MaHP varchar(10) REFERENCES HocPhan(MaHP),
	MaGV varchar(7) REFERENCES GiangVien(MaGV),
	HocKy tinyint CHECK( HocKy IN(1, 2) ),
	MaLop varchar(6) REFERENCES Lop(MaLop),
	NamHoc varchar(9) CHECK( LEN(NamHoc) = 9
		AND CHARINDEX('-', NamHoc) = 5
		AND TRY_CAST(LEFT(NamHoc, 4) AS int) IS NOT NULL
		AND TRY_CAST(RIGHT(NamHoc, 4) AS int) IS NOT NULL
		AND TRY_CAST(LEFT(NamHoc, 4) AS int) < TRY_CAST(RIGHT(NamHoc, 4) AS int)
	),
	PRIMARY KEY(MaHP, MaGV, HocKy, MaLop, NamHoc)
);
GO


-- Bảng KetQuaHocPhan
CREATE TABLE KetQuaHocPhan (
	MaSV varchar(8) REFERENCES SinhVien(MaSV),
	MaHP varchar(10) REFERENCES HocPhan(MaHP),
	ThuongXuyen float CHECK( 0 <= ThuongXuyen AND ThuongXuyen <= 10 ),
	ChuyenCan float CHECK( 0 <= ChuyenCan AND ChuyenCan <= 10 ),
	GHP float CHECK( 0 <= GHP AND GHP <= 10 ),
	CHP float CHECK( 0 <= CHP AND CHP <= 10 ),
	TongKet float CHECK( 0 <= TongKet AND TongKet <= 10 ),
	HocKy tinyint CHECK( HocKy IN(1, 2) ),
	NamHoc varchar(9) CHECK( LEN(NamHoc) = 9
		AND CHARINDEX('-', NamHoc) = 5
		AND TRY_CAST(LEFT(NamHoc, 4) AS int) IS NOT NULL
		AND TRY_CAST(RIGHT(NamHoc, 4) AS int) IS NOT NULL
		AND TRY_CAST(LEFT(NamHoc, 4) AS int) < TRY_CAST(RIGHT(NamHoc, 4) AS int)
	),
	PRIMARY KEY(MaSV, MaHP)
);
GO


-- Bảng KetQuaNamHoc
CREATE TABLE KetQuaNamHoc(
	NamHoc varchar(9) CHECK( LEN(NamHoc) = 9
		AND CHARINDEX('-', NamHoc) = 5
		AND TRY_CAST(LEFT(NamHoc, 4) AS int) IS NOT NULL
		AND TRY_CAST(RIGHT(NamHoc, 4) AS int) IS NOT NULL
		AND TRY_CAST(LEFT(NamHoc, 4) AS int) < TRY_CAST(RIGHT(NamHoc, 4) AS int)
	),
	MaSV varchar(8) REFERENCES SinhVien(MaSV),
	TBHK1 float CHECK( 0 <= TBHK1 AND TBHK1 <= 10 ),
	TBHK2 float CHECK( 0 <= TBHK2 AND TBHK2 <= 10 ),
	TBNH float CHECK( 0 <= TBNH AND TBNH <= 10 ),
	XEPLOAI varchar(11) CHECK( XEPLOAI IN( 'Trung Binh', 'Kha', 'Gioi', 'Xuat Sac' ) ),
	PRIMARY KEY(NamHoc, MaSV)
);
GO


-- Bảng KhenThuong
CREATE TABLE KhenThuong(
	MaKT varchar(10),
	MaSV varchar(8) REFERENCES SinhVien(MaSV),
	DiemCong float CHECK( 0 <= DiemCong AND DiemCong <= 10 ),
	NamHoc varchar(9) CHECK( LEN(NamHoc) = 9
		AND CHARINDEX('-', NamHoc) = 5
		AND TRY_CAST(LEFT(NamHoc, 4) AS int) IS NOT NULL
		AND TRY_CAST(RIGHT(NamHoc, 4) AS int) IS NOT NULL
		AND TRY_CAST(LEFT(NamHoc, 4) AS int) < TRY_CAST(RIGHT(NamHoc, 4) AS int)
	),
	MoTa varchar(100),
	PRIMARY KEY(MaKT, MaSV)
);
GO


-- Trigger cập nhật điểm tổng kết học phần khi thêm hoặc sửa KetQuaHocPhan
CREATE TRIGGER trg_tinhdiemtongket_KetQuaHocPhan
ON KetQuaHocPhan
AFTER INSERT, UPDATE
AS
BEGIN
	UPDATE kqhp
	SET TongKet = i.ChuyenCan * 0.1 + i.ThuongXuyen * 0.1 + i.GHP * 0.2 + i.CHP * 0.6
	FROM KetQuaHocPhan kqhp
		JOIN inserted i ON kqhp.MaHP = i.MaHP AND kqhp.MaSV = i.MaSV
	WHERE i.ChuyenCan IS NOT NULL
	   OR i.ThuongXuyen IS NOT NULL
	   OR i.GHP IS NOT NULL
	   OR i.CHP IS NOT NULL ;
END;
GO

-- Trigger cập nhật điểm trung bình năm học trong KetQuaNamHoc
CREATE TRIGGER trg_insert_update_tinhdiemnamhoc_KetQuaHocPhan
ON KetQuaHocPhan
AFTER INSERT, UPDATE
AS
BEGIN
	-- Cập nhật điểm trung bình học kỳ 1, học kỳ 2, điểm trung bình năm học và xếp loại (có cộng điểm thưởng)
	MERGE KetQuaNamHoc AS target
	USING (
		SELECT
			kq.MaSV,
			kq.NamHoc,
			SUM(CASE WHEN kq.HocKy = 1 THEN kq.TongKet * hp.SoTC ELSE 0 END) / NULLIF(SUM(CASE WHEN kq.HocKy = 1 THEN hp.SoTC ELSE 0 END), 0) AS TBHK1,
			SUM(CASE WHEN kq.HocKy = 2 THEN kq.TongKet * hp.SoTC ELSE 0 END) / NULLIF(SUM(CASE WHEN kq.HocKy = 2 THEN hp.SoTC ELSE 0 END), 0) AS TBHK2,
			SUM(kq.TongKet * hp.SoTC) / NULLIF(SUM(hp.SoTC), 0) AS TBNH_Base, -- Base TBNH before DiemCong
			ISNULL(kt.DiemCong, 0) AS DiemCong
		FROM KetQuaHocPhan kq
		JOIN HocPhan hp ON kq.MaHP = hp.MaHP
		JOIN (SELECT DISTINCT MaSV, NamHoc FROM inserted) i ON kq.MaSV = i.MaSV AND kq.NamHoc = i.NamHoc
		LEFT JOIN (
			SELECT MaSV, NamHoc, SUM(DiemCong) AS DiemCong
			FROM KhenThuong
			GROUP BY MaSV, NamHoc
		) kt ON kq.MaSV = kt.MaSV AND kq.NamHoc = kt.NamHoc
		GROUP BY kq.MaSV, kq.NamHoc, kt.DiemCong
	) AS source
	ON target.MaSV = source.MaSV AND target.NamHoc = source.NamHoc
	WHEN MATCHED THEN
		UPDATE SET
			TBHK1 = source.TBHK1,
			TBHK2 = source.TBHK2,
			TBNH = source.TBNH_Base + source.DiemCong,
			XEPLOAI = CASE
						WHEN (source.TBNH_Base + source.DiemCong) >= 9.0 THEN 'Xuat Sac'
						WHEN (source.TBNH_Base + source.DiemCong) >= 8.0 THEN 'Gioi'
						WHEN (source.TBNH_Base + source.DiemCong) >= 6.5 THEN 'Kha'
						WHEN (source.TBNH_Base + source.DiemCong) >= 5.0 THEN 'Trung Binh'
						ELSE NULL
					  END
	WHEN NOT MATCHED THEN
		INSERT (NamHoc, MaSV, TBHK1, TBHK2, TBNH, XEPLOAI)
		VALUES (
			source.NamHoc, source.MaSV,
			source.TBHK1, source.TBHK2,
			source.TBNH_Base + source.DiemCong,
			CASE
				WHEN (source.TBNH_Base + source.DiemCong) >= 9.0 THEN 'Xuat Sac'
				WHEN (source.TBNH_Base + source.DiemCong) >= 8.0 THEN 'Gioi'
				WHEN (source.TBNH_Base + source.DiemCong) >= 6.5 THEN 'Kha'
				WHEN (source.TBNH_Base + source.DiemCong) >= 5.0 THEN 'Trung Binh'
				ELSE NULL
			END
		);
END;
GO

EXEC sp_settriggerorder
 	@triggername = N'trg_tinhdiemtongket_KetQuaHocPhan',
 	@order = 'First',
 	@stmttype = 'INSERT';

EXEC sp_settriggerorder
 	@triggername = N'trg_insert_update_tinhdiemnamhoc_KetQuaHocPhan',
 	@order = 'Last',
 	@stmttype = 'INSERT';
GO


-- cap nhat lai diem neu ket qua hoc phan bi xoa
CREATE TRIGGER trg_delete_tinhdiemnamhoc_KetQuaHocPhan
ON KetQuaHocPhan
AFTER DELETE
AS
BEGIN
	-- Cập nhật lại điểm TB và xếp loại
	MERGE KetQuaNamHoc AS target
	USING (
		SELECT
			d.MaSV,
			d.NamHoc,
			SUM(CASE WHEN kq.HocKy = 1 THEN kq.TongKet * hp.SoTC ELSE 0 END) / NULLIF(SUM(CASE WHEN kq.HocKy = 1 THEN hp.SoTC ELSE 0 END), 0) AS TBHK1,
			SUM(CASE WHEN kq.HocKy = 2 THEN kq.TongKet * hp.SoTC ELSE 0 END) / NULLIF(SUM(CASE WHEN kq.HocKy = 2 THEN hp.SoTC ELSE 0 END), 0) AS TBHK2,
			SUM(kq.TongKet * hp.SoTC) / NULLIF(SUM(hp.SoTC), 0) AS TBNH_Base,
			ISNULL(kt.DiemCong, 0) AS DiemCong
		FROM deleted d
		LEFT JOIN KetQuaHocPhan kq ON kq.MaSV = d.MaSV AND kq.NamHoc = d.NamHoc
		LEFT JOIN HocPhan hp ON kq.MaHP = hp.MaHP
		LEFT JOIN (
			SELECT MaSV, NamHoc, SUM(DiemCong) AS DiemCong
			FROM KhenThuong
			GROUP BY MaSV, NamHoc
		) kt ON d.MaSV = kt.MaSV AND d.NamHoc = kt.NamHoc
		GROUP BY d.MaSV, d.NamHoc, kt.DiemCong
	) AS source
	ON target.MaSV = source.MaSV AND target.NamHoc = source.NamHoc
	WHEN MATCHED THEN
		UPDATE SET
			TBHK1 = source.TBHK1,
			TBHK2 = source.TBHK2,
			TBNH = source.TBNH_Base + source.DiemCong,
			XEPLOAI = CASE
						WHEN (source.TBNH_Base + source.DiemCong) >= 9.0 THEN 'Xuat Sac'
						WHEN (source.TBNH_Base + source.DiemCong) >= 8.0 THEN 'Gioi'
						WHEN (source.TBNH_Base + source.DiemCong) >= 6.5 THEN 'Kha'
						WHEN (source.TBNH_Base + source.DiemCong) >= 5.0 THEN 'Trung Binh'
						ELSE NULL
					  END;

	-- Xoá những dòng không còn học phần nào trong năm học
	DELETE FROM KetQuaNamHoc
	WHERE NOT EXISTS (
		SELECT 1
		FROM KetQuaHocPhan kq
		WHERE kq.MaSV = KetQuaNamHoc.MaSV
		  AND kq.NamHoc = KetQuaNamHoc.NamHoc
	);
END;
GO


--- thay doi diem neu co them hoac cap nhat khen thuong
CREATE TRIGGER trg_insert_update_KhenThuong
ON KhenThuong
AFTER INSERT, UPDATE
AS
BEGIN
	UPDATE KQN
	SET
		TBNH = (
			SELECT
				ISNULL(SUM(kq.TongKet * hp.SoTC) / NULLIF(SUM(hp.SoTC), 0), 0) + ISNULL(SUM(kt_sub.DiemCong), 0)
			FROM KetQuaHocPhan kq
			JOIN HocPhan hp ON kq.MaHP = hp.MaHP
			LEFT JOIN KhenThuong kt_sub ON kq.MaSV = kt_sub.MaSV AND kq.NamHoc = kt_sub.NamHoc
			WHERE kq.MaSV = KQN.MaSV AND kq.NamHoc = KQN.NamHoc
		),
		XEPLOAI = (
			SELECT
				CASE
					WHEN (ISNULL(SUM(kq.TongKet * hp.SoTC) / NULLIF(SUM(hp.SoTC), 0), 0) + ISNULL(SUM(kt_sub.DiemCong), 0)) >= 9.0 THEN 'Xuat Sac'
					WHEN (ISNULL(SUM(kq.TongKet * hp.SoTC) / NULLIF(SUM(hp.SoTC), 0), 0) + ISNULL(SUM(kt_sub.DiemCong), 0)) >= 8.0 THEN 'Gioi'
					WHEN (ISNULL(SUM(kq.TongKet * hp.SoTC) / NULLIF(SUM(hp.SoTC), 0), 0) + ISNULL(SUM(kt_sub.DiemCong), 0)) >= 6.5 THEN 'Kha'
					WHEN (ISNULL(SUM(kq.TongKet * hp.SoTC) / NULLIF(SUM(hp.SoTC), 0), 0) + ISNULL(SUM(kt_sub.DiemCong), 0)) >= 5.0 THEN 'Trung Binh'
					ELSE NULL
				END
			FROM KetQuaHocPhan kq
			JOIN HocPhan hp ON kq.MaHP = hp.MaHP
			LEFT JOIN KhenThuong kt_sub ON kq.MaSV = kt_sub.MaSV AND kq.NamHoc = kt_sub.NamHoc
			WHERE kq.MaSV = KQN.MaSV AND kq.NamHoc = KQN.NamHoc
		)
	FROM KetQuaNamHoc KQN
	WHERE EXISTS (
		SELECT 1 FROM inserted i
		WHERE i.MaSV = KQN.MaSV AND i.NamHoc = KQN.NamHoc
	);
END;
GO


--- tinh lai diem neu nhu da xoa khen thuong
CREATE TRIGGER trg_delete_KhenThuong
ON KhenThuong
AFTER DELETE
AS
BEGIN
 	UPDATE KQN
 	SET
 		TBNH = (
 			SELECT
 				ISNULL(SUM(kq.TongKet * hp.SoTC) / NULLIF(SUM(hp.SoTC), 0), 0) + ISNULL(SUM(kt_sub.DiemCong), 0)
 			FROM KetQuaHocPhan kq
 			JOIN HocPhan hp ON kq.MaHP = hp.MaHP
 			LEFT JOIN KhenThuong kt_sub ON kq.MaSV = kt_sub.MaSV AND kq.NamHoc = kt_sub.NamHoc
 			WHERE kq.MaSV = KQN.MaSV AND kq.NamHoc = KQN.NamHoc
 		),
 		XEPLOAI = (
 			SELECT
 				CASE
 					WHEN (ISNULL(SUM(kq.TongKet * hp.SoTC) / NULLIF(SUM(hp.SoTC), 0), 0) + ISNULL(SUM(kt_sub.DiemCong), 0)) >= 9.0 THEN 'Xuat Sac'
 					WHEN (ISNULL(SUM(kq.TongKet * hp.SoTC) / NULLIF(SUM(hp.SoTC), 0), 0) + ISNULL(SUM(kt_sub.DiemCong), 0)) >= 8.0 THEN 'Gioi'
 					WHEN (ISNULL(SUM(kq.TongKet * hp.SoTC) / NULLIF(SUM(hp.SoTC), 0), 0) + ISNULL(SUM(kt_sub.DiemCong), 0)) >= 6.5 THEN 'Kha'
 					WHEN (ISNULL(SUM(kq.TongKet * hp.SoTC) / NULLIF(SUM(hp.SoTC), 0), 0) + ISNULL(SUM(kt_sub.DiemCong), 0)) >= 5.0 THEN 'Trung Binh'
 					ELSE NULL
 				END
 			FROM KetQuaHocPhan kq
 			JOIN HocPhan hp ON kq.MaHP = hp.MaHP
 			LEFT JOIN KhenThuong kt_sub ON kq.MaSV = kt_sub.MaSV AND kq.NamHoc = kt_sub.NamHoc
 			WHERE kq.MaSV = KQN.MaSV AND kq.NamHoc = KQN.NamHoc
 		)
 	FROM KetQuaNamHoc KQN
 	WHERE EXISTS (
 		SELECT 1 FROM deleted d
 		WHERE d.MaSV = KQN.MaSV AND d.NamHoc = KQN.NamHoc
 	);
END;
GO


-- trigger Kiểm tra học hàm, học vị
CREATE TRIGGER trg_check_HocVi_HocHam_GV
ON GiangVien
AFTER INSERT, UPDATE
AS
BEGIN
	IF EXISTS (
	SELECT 1
	FROM inserted
	WHERE
	(HocVi = 'CN' AND HocHam <> '') -- Cử nhân không có học hàm
	OR (HocVi = 'ThS' AND HocHam IN ('PGS', 'GS')) -- Thạc sĩ không thể là PGS/GS
	OR (HocVi IS NULL AND HocHam IS NOT NULL) -- Không có học vị thì không có học hàm
	OR (HocVi IN ('TS', 'TSKH') AND HocHam = 'GV') -- Tiến sĩ/TSKH nên có học hàm cao hơn GV
	)
	BEGIN
	RAISERROR('Kết hợp Học vị - Học hàm không hợp lệ.', 16, 1);
	ROLLBACK;
	END
END;
GO


-- Insert du lieu cho cac bang
-- Bang Khoa
INSERT INTO Khoa (MaKhoa, TenKhoa) VALUES
('NV1', 'Nghiep vu co so'),
('NV2', 'An ninh chinh tri'),
('NV3', 'An ninh noi bo'),
('NV4', 'An ninh kinh te'),
('NV5', 'Phan gian'),
('NV6', 'An ninh xa hoi'),
('NV7', 'An ninh mang va phong chong toi pham su dung cong nghe cao'),
('NV8', 'Dieu tra hinh su'),
('NN', 'Ngoai ngu'),
('QSVT', 'Quan su - vo thuat'),
('LLCT', 'Ly luan chinh tri'),
('NCKH', 'Nghien cuu khoa hoc'),
('KL', 'Khoa luat');


-- Bang ChuNhiem
INSERT INTO ChuNhiem (MaCN, NgSinh, SDT, HoTen, GT) VALUES
('CN01', '1989-12-12', '0942668228', 'Vu Thuy Dung', 'Nu'),
('CN02', '1990-11-23', '0925835883', 'Trieu Thanh Vo', 'Nam'),
('CN03', '2000-04-30', '0913952389', 'Chu Thi Hong Anh', 'Nu'),
('CN04', '1999-11-08', '0848835266', 'Nguyen Van Thanh An', 'Nam'),
('CN05', '1983-08-06', '0124583934', 'Ha Trong Phuong', 'Nam'),
('CN06', '1980-08-08', '02033813829', 'Pham Cong Duc Tai', 'Nam');


-- Bang GiangVien
INSERT INTO GiangVien (MaGV, HoTen, NgSinh, GT, MaKhoa, SDT, HocVi, HocHam) VALUES
-- Adjusted HocHam for NV101, removed an invalid combo
('NV101', 'Do Khoa Binh Nam', '1997-02-05', 'Nu', 'NV1', '0403191742', 'CN', ''), -- Cử nhân, không học hàm
('NV102', 'Phan Linh Yen Nam', '1995-07-05', 'Nu', 'NV1', '0108813672', 'ThS', 'GV'), -- Thạc sĩ, Giảng viên
('NV201', 'Le Linh Linh', '1996-06-26', 'Nam', 'NV2', '0204160452', 'ThS', 'GV'),
('NV301', 'Hoang Quynh Giang Yen', '1990-11-10', 'Nu', 'NV3', '0747145922', 'ThS', 'GV'),
('NV302', 'Nguyen Yen Anh', '1991-03-11', 'Nam', 'NV3', '0160473623', 'ThS', 'GVCH'),
('NV401', 'Bui Giang Linh', '1997-01-26', 'Nam', 'NV4', '0263265294', 'ThS', 'GV'),
('NV501', 'Vu Giang Minh', '1989-04-11', 'Nu', 'NV5', '0478683414', 'ThS', 'GVCH'),
('NV502', 'Vu Nam Binh Quynh', '1991-05-09', 'Nu', 'NV5', '0570402607', 'TS', 'PGS'), -- TS can be PGS
('NV601', 'Hoang Quynh Trang Yen', '1989-11-04', 'Nu', 'NV6', '0604883405', 'ThS', 'GVCH'),
('NV701', 'Le Phong Phong', '1995-02-22', 'Nu', 'NV7', '0563250597', 'TS', 'GVCH'), -- TS can be GVCH
('NV702', 'Do Linh Binh', '1993-06-17', 'Nam', 'NV7', '0490136557', 'TS', 'PGS'),
('NV703', 'Vu Giang Hieu', '1995-09-28', 'Nam', 'NV7', '0992910638', 'TS', 'GS'),
('NV704', 'Tran Giang Thao', '1991-02-22', 'Nu', 'NV7', '0148449622', 'ThS', 'GV'),
('NV705', 'Nguyen Dinh Nghia', '1987-09-09', 'Nam', 'NV7', '0912353568', 'TSKH', 'GS'),
('NV801', 'Phan Nam Trang', '1993-09-14', 'Nu', 'NV8', '0612547823', 'ThS', 'GV'),
('QSVT01', 'Pham Nam Giang', '1989-09-03', 'Nam', 'QSVT', '0690189204', 'ThS', 'GV'),
('LLCT01', 'Pham Linh Phong Trang', '1995-11-11', 'Nu', 'LLCT', '0991918924', 'ThS', 'GV'),
('NCKH01', 'Bui Minh Khoa Khoa', '1993-12-03', 'Nam', 'NCKH', '0134345000', 'ThS', 'GV'),
('KL01', 'Do Binh Minh', '1992-07-07', 'Nu', 'KL', '0700808796', 'ThS', 'GV'),
('NN01', 'Nguyen Kha Ngoc Tung', '1991-08-12', 'Nam', 'NN', '0813934535', 'ThS', 'GV');


-- Bang Lop
INSERT INTO Lop (MaLop, MaCN, MaKhoa) VALUES
('B9D53', 'CN01', 'NV7'),  -- CN01 for NV7
('B10D53', 'CN02', 'NV7'), -- CN02 for NV7
('B9D54', 'CN03', 'NV2'),  -- CN03 for NV2
('B10D54', 'CN04', 'NV3'), -- CN04 for NV3
('B8D55', 'CN03', 'NV1'),  -- CN03 also for NV1 (same CN can manage multiple classes)
('B9D55', 'CN05', 'NV7'),  -- CN05 for NV7
('B10D55', 'CN06', 'NV4'); -- CN06 for NV4


-- Bang SinhVien
INSERT INTO SinhVien (MaSV, HoTen, NgSinh, GT, MaLop, TrangThai, NamNhapHoc, NamRaTruong) VALUES
('B10D5301', 'Do Minh Cuong', '2006-07-29', 'Nam', 'B10D53', 'Dang hoc', 2024, NULL), -- Changed to 'Dang hoc' for consistency with 2024 enrollment
('B10D5302', 'Do Van Tuyet', '2004-04-07', 'Nu', 'B10D53', 'Dang hoc', 2022, NULL),
('B10D5303', 'Vo Hoang Binh', '2005-02-08', 'Nam', 'B10D53', 'Dang hoc', 2024, NULL),
('B10D5304', 'Le Quang Dung', '2003-10-05', 'Nu', 'B10D53', 'Dang hoc', 2022, NULL),
('B10D5305', 'Dang Hai Hang', '2006-04-16', 'Nu', 'B10D53', 'Dang hoc', 2025, NULL),
('B10D5401', 'Bui Hoang Mai', '2007-01-25', 'Nu', 'B10D54', 'Dang hoc', 2025, NULL), -- Changed to 'Dang hoc'
('B10D5402', 'Le Thanh Linh', '2003-11-11', 'Nu', 'B10D54', 'Da ra truong', 2022, 2024),
('B10D5403', 'Bui Van Huy', '2006-07-24', 'Nu', 'B10D54', 'Dang hoc', 2025, NULL),
('B10D5404', 'Dang Thi Huy', '2005-06-30', 'Nu', 'B10D54', 'Dang hoc', 2024, NULL),
('B10D5405', 'Bui Thanh Tuyet', '2006-02-12', 'Nam', 'B10D54', 'Dang hoc', 2024, NULL),
('B10D5501', 'Bui Van Binh', '2004-10-14', 'Nu', 'B10D55', 'Dang hoc', 2024, NULL),
('B10D5502', 'Pham Thanh Dung', '2004-06-25', 'Nam', 'B10D55', 'Dang hoc', 2024, NULL),
('B10D5503', 'Ho Minh Cuong', '2004-04-21', 'Nu', 'B10D55', 'Dang hoc', 2023, NULL),
('B10D5504', 'Dang Minh Lan', '2003-09-12', 'Nam', 'B10D55', 'Dang hoc', 2024, NULL),
('B10D5505', 'Bui Minh Huy', '2003-08-15', 'Nam', 'B10D55', 'Dang hoc', 2024, NULL),
('B8D5501', 'Pham Thi Anh', '2004-02-18', 'Nu', 'B8D55', 'Dang hoc', 2022, NULL),
('B8D5502', 'Tran Van Nam', '2004-09-15', 'Nu', 'B8D55', 'Dang hoc', 2024, NULL),
('B8D5503', 'Dang Hai Dung', '2006-05-11', 'Nu', 'B8D55', 'Dang hoc', 2024, NULL),
('B8D5504', 'Dang Hoang Dung', '2006-03-24', 'Nam', 'B8D55', 'Dang hoc', 2025, NULL),
('B8D5505', 'Ho Hai Tuyet', '2005-05-11', 'Nam', 'B8D55', 'Dang hoc', 2023, NULL),
('B9D5301', 'Le Hoang Mai', '2004-11-17', 'Nu', 'B9D53', 'Dang hoc', 2022, NULL),
('B9D5302', 'Pham Hoang Huy', '2003-09-14', 'Nam', 'B9D53', 'Da ra truong', 2022, 2024),
('B9D5303', 'Do Thi Hang', '2005-03-04', 'Nu', 'B9D53', 'Dang hoc', 2025, NULL),
('B9D5304', 'Le Thi Huy', '2004-07-16', 'Nu', 'B9D53', 'Dang hoc', 2023, NULL),
('B9D5305', 'Vo Hai Hang', '2003-05-26', 'Nu', 'B9D53', 'Dang hoc', 2024, NULL),
('B9D5401', 'Do Quang Huy', '2007-01-02', 'Nu', 'B9D54', 'Dang hoc', 2025, NULL),
('B9D5402', 'Tran Van Tuyet', '2005-03-19', 'Nu', 'B9D54', 'Dang hoc', 2023, NULL),
('B9D5403', 'Bui Quang Huy', '2004-01-17', 'Nam', 'B9D54', 'Dang hoc', 2022, NULL),
('B9D5501', 'Ho Van Linh', '2006-02-03', 'Nu', 'B9D55', 'Dang hoc', 2025, NULL),
('B9D5502', 'Do Thi Hang', '2004-09-21', 'Nam', 'B9D55', 'Da ra truong', 2022, 2025),
('B9D5503', 'Vo Hoang Huy', '2005-06-13', 'Nu', 'B9D55', 'Dang hoc', 2024, NULL); -- Changed to 'Dang hoc'


-- Bang HocPhan
INSERT INTO HocPhan (MaHP, TenHP, SoTC, LoaiHP, SoTiet, MaKhoa) VALUES
('HP01', 'Cau truc du lieu', 4, 'Chuyen nganh', 70, 'NV7'),
('HP02', 'Giai thuat', 3, 'Chuyen nganh', 66, 'NV7'),
('HP03', 'Co so du lieu', 2, 'Chuyen nganh', 45, 'NV7'),
('HP04', 'Lap trinh Python', 1, 'Chuyen nganh', 36, 'NV7'),
('HP05', 'Lap trinh Web', 2, 'Chuyen nganh', 54, 'NV7'),
('HP06', 'He dieu hanh', 3, 'Chuyen nganh', 66, 'NV7'),
('HP07', 'Mang may tinh', 2, 'Chuyen nganh', 54, 'NV7'),
('HP08', 'Co so an toan thong tin', 2, 'Chuyen nganh', 54, 'NV7'),
('HP09', 'Bao mat HTTT', 2, 'Chuyen nganh', 45, 'NV7'),
('HP10', 'Kien truc may tinh', 3, 'Chuyen nganh', 66, 'NV7'),
('HP11', 'Tri tue nhan tao', 2, 'Chuyen nganh', 45, 'NV7'),
('HP12', 'Lap trinh huong doi tuong', 2, 'Chuyen nganh', 45, 'NV7'),
('HP13', 'Do an co so', 2, 'Chuyen nganh', 54, 'NV7'),
('HP14', 'Khai pha du lieu', 3, 'Chuyen nganh', 66, 'NV7'),
('HP15', 'Phan tich thiet ke HTTT', 3, 'Chuyen nganh', 66, 'NV7'),
('HP16', 'Nhap mon CNTT', 1, 'Chuyen nganh', 30, 'NV7'),
('HP17', 'Kien truc phan mem', 2, 'Chuyen nganh', 45, 'NV7'),
('HP18', 'Phat trien ung dung web', 2, 'Chuyen nganh', 54, 'NV7'),
('HP19', 'Thuc tap', 2, 'Chuyen nganh', 54, 'NV7'),
('HP20', 'Thuc hanh an toan thong tin', 2, 'Chuyen nganh', 54, 'NV7'),
('HP21', 'Giao duc quoc phong', 3, 'Tu chon', 60, 'QSVT'),
('HP22', 'Mac-Lenin', 4, 'Dai cuong', 72, 'LLCT'),
('HP23', 'Triet hoc', 2, 'Dai cuong', 54, 'LLCT'),
('HP24', 'Dieu tra hinh su', 2, 'Dai cuong', 45, 'NV3'),
('HP25', 'Giao duc the chat', 3, 'Dai cuong', 66, 'QSVT'),
('HP26', 'Tieng Anh chuyen nganh', 2, 'Dai cuong', 45, 'NN'),
('HP27', 'Ky nang mem', 4, 'Tu chon', 70, 'NV4'),
('HP28', 'Ky nang giao tiep', 2, 'Tu chon', 45, 'NV6'),
('HP29', 'Phap luat dai cuong', 2, 'Tu chon', 54, 'KL'),
('HP30', 'Tam ly hoc', 2, 'Tu chon', 45, 'NV2'),
('HP31', 'Ky thuat hinh su', 2, 'Dai cuong', 42, 'NV5'),
('HP32', 'Nghiep vu co ban', 4, 'Dai cuong', 68, 'NV1'),
('HP33', 'Nghien cuu khoa hoc', 2, 'Dai cuong', 42, 'NCKH');



-- Bang GiangDay - ENSURING ALL 7 CLASSES ARE USED LOGICALLY
INSERT INTO GiangDay (MaHP, MaGV, HocKy, MaLop, NamHoc) VALUES
-- 2023-2024 Học kỳ 1
('HP07', 'NV701', 1, 'B10D53', '2023-2024'), -- NV701 (NV7) teaches B10D53 (NV7)
('HP08', 'NV701', 1, 'B9D53', '2023-2024'),  -- NV701 (NV7) teaches B9D53 (NV7)
('HP11', 'NV703', 1, 'B9D55', '2023-2024'),  -- NV703 (NV7) teaches B9D55 (NV7)
('HP01', 'NV704', 1, 'B10D53', '2023-2024'), -- NV704 (NV7) teaches B10D53 (NV7)
('HP03', 'NV704', 1, 'B9D53', '2023-2024'),  -- NV704 (NV7) teaches B9D53 (NV7)
('HP04', 'NV704', 1, 'B9D55', '2023-2024'),  -- NV704 (NV7) teaches B9D55 (NV7)
('HP05', 'NV704', 1, 'B10D53', '2023-2024'), -- NV704 (NV7) teaches B10D53 (NV7)

-- 2023-2024 Học kỳ 2
('HP02', 'NV703', 2, 'B9D53', '2023-2024'),  -- NV703 (NV7) teaches B9D53 (NV7)
('HP09', 'NV703', 2, 'B10D53', '2023-2024'), -- NV703 (NV7) teaches B10D53 (NV7)
('HP01', 'NV704', 2, 'B9D55', '2023-2024'),  -- NV704 (NV7) teaches B9D55 (NV7)

-- 2024-2025 Học kỳ 1
('HP22', 'LLCT01', 1, 'B10D54', '2024-2025'), -- LLCT01 (LLCT) teaches B10D54 (NV3) - General subject
('HP23', 'LLCT01', 1, 'B9D54', '2024-2025'),  -- LLCT01 (LLCT) teaches B9D54 (NV2) - General subject
('HP30', 'NV201', 1, 'B9D54', '2024-2025'),  -- NV201 (NV2) teaches B9D54 (NV2)
('HP28', 'NV601', 1, 'B9D55', '2024-2025'),  -- NV601 (NV6) teaches B9D55 (NV7) - Tu chon
('HP07', 'NV701', 1, 'B10D53', '2024-2025'), -- NV701 (NV7) teaches B10D53 (NV7)
('HP16', 'NV701', 1, 'B9D53', '2024-2025'),  -- NV701 (NV7) teaches B9D53 (NV7)
('HP06', 'NV702', 1, 'B9D55', '2024-2025'),  -- NV702 (NV7) teaches B9D55 (NV7)
('HP12', 'NV702', 1, 'B10D53', '2024-2025'), -- NV702 (NV7) teaches B10D53 (NV7)
('HP02', 'NV703', 1, 'B9D53', '2024-2025'),  -- NV703 (NV7) teaches B9D53 (NV7)
('HP19', 'NV703', 1, 'B9D55', '2024-2025'),  -- NV703 (NV7) teaches B9D55 (NV7)
('HP03', 'NV704', 1, 'B10D53', '2024-2025'), -- NV704 (NV7) teaches B10D53 (NV7)
('HP05', 'NV704', 1, 'B9D53', '2024-2025'),  -- NV704 (NV7) teaches B9D53 (NV7)
('HP10', 'NV704', 1, 'B9D55', '2024-2025'),  -- NV704 (NV7) teaches B9D55 (NV7)
('HP17', 'NV704', 1, 'B10D53', '2024-2025'), -- NV704 (NV7) teaches B10D53 (NV7)
('HP32', 'NV102', 1, 'B8D55', '2024-2025'),  -- NV102 (NV1) teaches B8D55 (NV1) - NEWLY ADDED to cover B8D55
('HP27', 'NV401', 1, 'B10D55', '2024-2025'), -- NV401 (NV4) teaches B10D55 (NV4) - NEWLY ADDED to cover B10D55

-- 2024-2025 Học kỳ 2
('HP22', 'LLCT01', 2, 'B9D54', '2024-2025'),  -- LLCT01 (LLCT) teaches B9D54 (NV2)
('HP23', 'LLCT01', 2, 'B10D54', '2024-2025'), -- LLCT01 (LLCT) teaches B10D54 (NV3)
('HP30', 'NV201', 2, 'B9D54', '2024-2025'),  -- NV201 (NV2) teaches B9D54 (NV2)
('HP24', 'NV302', 2, 'B10D54', '2024-2025'), -- NV302 (NV3) teaches B10D54 (NV3)
('HP08', 'NV701', 2, 'B9D53', '2024-2025'),  -- NV701 (NV7) teaches B9D53 (NV7)
('HP15', 'NV701', 2, 'B10D53', '2024-2025'), -- NV701 (NV7) teaches B10D53 (NV7)
('HP16', 'NV701', 2, 'B9D55', '2024-2025'),  -- NV701 (NV7) teaches B9D55 (NV7)
('HP06', 'NV702', 2, 'B10D53', '2024-2025'), -- NV702 (NV7) teaches B10D53 (NV7)
('HP02', 'NV703', 2, 'B9D53', '2024-2025'),  -- NV703 (NV7) teaches B9D53 (NV7)
('HP11', 'NV703', 2, 'B9D55', '2024-2025'),  -- NV703 (NV7) teaches B9D55 (NV7)
('HP13', 'NV703', 2, 'B10D53', '2024-2025'), -- NV703 (NV7) teaches B10D53 (NV7)
('HP19', 'NV703', 2, 'B9D53', '2024-2025'),  -- NV703 (NV7) teaches B9D53 (NV7)
('HP17', 'NV704', 2, 'B9D55', '2024-2025'),  -- NV704 (NV7) teaches B9D55 (NV7)
('HP32', 'NV102', 2, 'B8D55', '2024-2025'),  -- NV102 (NV1) teaches B8D55 (NV1) - NEWLY ADDED
('HP27', 'NV401', 2, 'B10D55', '2024-2025'); -- NV401 (NV4) teaches B10D55 (NV4) - NEWLY ADDED
GO


-- Bang KetQuaHocPhan
-- Adjusted scores to create more realistic distribution and test trigger logic.
-- Some TongKet scores are set to NULL to let the trigger calculate them.
INSERT INTO KetQuaHocPhan (MaSV, MaHP, ThuongXuyen, ChuyenCan, GHP, CHP, TongKet, HocKy, NamHoc)
VALUES
-- Original data: TongKet was NULL, so it will be calculated by the trigger
('B9D5403', 'HP04', 7.0, 7.5, 8.0, 8.5, NULL, 2, '2022-2023'), -- TB
('B10D5505', 'HP13', 8.5, 9.0, 8.8, 9.2, NULL, 1, '2023-2024'), -- Gioi
('B10D5304', 'HP21', 9.0, 8.5, 9.0, 9.5, NULL, 2, '2024-2025'), -- Xuat sac
('B9D5503', 'HP32', 6.0, 6.5, 7.0, 7.2, NULL, 2, '2024-2025'), -- Kha (adjusted for a realistic scenario for a student)
('B9D5503', 'HP26', 5.0, 5.5, 5.8, 6.0, NULL, 2, '2023-2024'), -- Trung Binh
('B9D5501', 'HP10', 8.0, 7.8, 8.5, 8.2, NULL, 2, '2024-2025'), -- Gioi
('B8D5505', 'HP25', 7.5, 7.0, 7.2, 7.8, NULL, 1, '2023-2024'), -- Kha
('B10D5505', 'HP25', 6.0, 6.2, 6.5, 6.8, NULL, 2, '2022-2023'), -- Kha
('B9D5402', 'HP18', 7.2, 7.0, 7.5, 7.8, NULL, 2, '2024-2025'), -- Kha
('B9D5304', 'HP31', 8.0, 8.2, 8.5, 8.8, NULL, 1, '2023-2024'), -- Gioi
('B9D5303', 'HP20', 7.5, 7.0, 7.3, 7.6, NULL, 1, '2024-2025'), -- Kha
('B9D5402', 'HP28', 6.5, 6.0, 6.3, 6.8, NULL, 2, '2023-2024'), -- Kha
('B10D5304', 'HP31', 7.0, 7.2, 7.5, 7.8, NULL, 2, '2022-2023'), -- Kha
('B10D5504', 'HP28', 8.0, 8.2, 8.5, 8.8, NULL, 2, '2023-2024'), -- Gioi
('B10D5401', 'HP26', 7.5, 7.0, 7.3, 7.6, NULL, 2, '2022-2023'), -- Kha
('B9D5301', 'HP33', 6.8, 6.5, 7.0, 7.2, NULL, 2, '2022-2023'), -- Kha
('B9D5301', 'HP13', 8.0, 8.5, 8.2, 8.8, NULL, 2, '2022-2023'), -- Gioi
('B10D5404', 'HP23', 7.0, 6.8, 7.2, 7.5, NULL, 1, '2024-2025'), -- Kha
('B10D5302', 'HP28', 7.8, 7.5, 8.0, 8.2, NULL, 1, '2023-2024'), -- Gioi
('B8D5505', 'HP19', 8.0, 8.5, 8.2, 8.8, NULL, 1, '2023-2024'), -- Gioi
('B10D5505', 'HP18', 7.0, 7.2, 7.5, 7.8, NULL, 1, '2023-2024'), -- Kha
('B8D5501', 'HP17', 6.5, 6.0, 6.8, 7.0, NULL, 1, '2024-2025'), -- Kha
('B10D5503', 'HP23', 7.5, 7.0, 7.3, 7.6, NULL, 1, '2023-2024'), -- Kha
('B8D5504', 'HP20', 8.2, 8.0, 8.5, 8.8, NULL, 2, '2023-2024'), -- Gioi
('B9D5403', 'HP21', 7.0, 7.5, 7.8, 8.0, NULL, 1, '2023-2024'), -- Gioi
('B9D5302', 'HP13', 8.5, 8.8, 9.0, 9.2, NULL, 1, '2024-2025'), -- Xuat Sac (adjusted for a realistic scenario for a student)
('B10D5302', 'HP31', 9.0, 9.2, 9.5, 9.8, NULL, 2, '2023-2024'), -- Xuat Sac
('B10D5402', 'HP02', 8.0, 8.5, 8.8, 9.0, NULL, 1, '2022-2023'), -- Gioi

-- Additional KetQuaHocPhan entries to better reflect assignments from GiangDay
-- Ensure students from all classes have some grades
('B8D5501', 'HP32', 7.5, 7.0, 7.2, 7.8, NULL, 1, '2024-2025'), -- B8D5501 (NV1) takes HP32 (NV1)
('B10D5502', 'HP27', 8.0, 8.2, 8.5, 8.8, NULL, 1, '2024-2025'), -- B10D5502 (NV4) takes HP27 (NV4)
('B9D5401', 'HP22', 7.0, 7.5, 7.8, 8.0, NULL, 1, '2024-2025'), -- B9D5401 (NV2) takes HP22 (LLCT)
('B10D5403', 'HP23', 6.5, 6.8, 7.0, 7.2, NULL, 1, '2024-2025'); -- B10D5403 (NV3) takes HP23 (LLCT)
GO

-- Bang KhenThuong
-- Adjusted DiemCong for more variety.
INSERT INTO KhenThuong (MaKT, MaSV, DiemCong, NamHoc, MoTa) VALUES
('KT01', 'B9D5301', 0.5, '2022-2023', 'Bang khen hoc tap'),
('KT02', 'B9D5402', 0.8, '2024-2025', 'Dat giai Olympic tin hoc'),
('KT03', 'B10D5503', 0.7, '2023-2024', 'Tham gia van nghe xuat sac'),
('KT04', 'B9D5302', 0.3, '2022-2023', 'Dat giai the duc the thao'),
('KT05', 'B9D5402', 0.9, '2023-2024', 'Dat giai Olympic toan quoc'),
('KT06', 'B9D5302', 0.4, '2023-2024', 'Dat giai the duc the thao'),
('KT07', 'B10D5404', 0.6, '2024-2025', 'Dat giai the duc the thao'),
('KT08', 'B10D5402', 0.2, '2023-2024', 'Tham gia hoat dong xa hoi'),
('KT09', 'B9D5402', 1.0, '2023-2024', 'Giai nhat Olympic'), -- Student B9D5402 has multiple awards for 2023-2024, sum of DiemCong will apply
('KT10', 'B10D5501', 0.7, '2024-2025', 'Giay khen sinh vien xuat sac'),
('KT11', 'B10D5505', 0.1, '2024-2025', 'Tham gia hoat dong the thao'),
('KT12', 'B9D5502', 0.5, '2023-2024', 'Giay khen nghien cuu khoa hoc'),
('KT13', 'B10D5501', 0.8, '2024-2025', 'Bang khen cong hien doan the'),
('KT14', 'B8D5502', 0.6, '2024-2025', 'Dat giai the duc the thao cap truong'),
('KT15', 'B9D5403', 0.2, '2024-2025', 'Dat giai Olympic tieng anh');
GO

SELECT * FROM KETQUANAMHOC