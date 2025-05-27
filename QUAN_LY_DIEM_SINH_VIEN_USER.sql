USE QUAN_LY_DIEM_SINH_VIEN;  -- Chọn cơ sở dữ liệu hiện có

-- Tạo bảng Users để lưu thông tin tài khoản
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1), -- Khóa chính, tự động tăng
    Username NVARCHAR(50) UNIQUE NOT NULL, -- Tên đăng nhập (duy nhất)
    Password NVARCHAR(255) NOT NULL,       -- Mật khẩu (nên được hash)
    Role NVARCHAR(20) NOT NULL,             -- Vai trò (SinhVien, GiangVien, Admin)
    RelatedID NVARCHAR(30) NULL,           -- Liên kết với maSV hoặc maGV
    CreatedAt DATETIME DEFAULT GETDATE(),  -- Thời điểm tạo tài khoản
    LastLogin DATETIME NULL,               -- Lần đăng nhập cuối cùng
    IsActive BIT DEFAULT 1                 -- Trạng thái hoạt động (1: kích hoạt, 0: bị khóa)
);

-- Thêm ràng buộc kiểm tra cho cột Role
ALTER TABLE Users
ADD CONSTRAINT CHK_UserRole CHECK (Role IN ('SinhVien', 'GiangVien', 'ChuNhiem', 'Admin'));

-- Tạo stored procedure để thêm mới user
go
CREATE PROCEDURE AddNewUser
    @Username NVARCHAR(50),
    @Password NVARCHAR(255),  -- Mật khẩu đã được hash
    @Role NVARCHAR(20),
    @RelatedID NVARCHAR(30)
AS
BEGIN
    INSERT INTO Users (Username, Password, Role, RelatedID)
    VALUES (@Username, @Password, @Role, @RelatedID);
END;

-- Tạo stored procedure để xác thực người dùng
go
CREATE PROCEDURE AuthenticateUser
    @Username NVARCHAR(50),
    @Password NVARCHAR(255)
AS
BEGIN
    SELECT UserID, Role, RelatedID
    FROM Users
    WHERE Username = @Username AND Password = @Password AND IsActive = 1;
    
    -- Cập nhật thời điểm đăng nhập cuối cùng nếu xác thực thành công
    IF @@ROWCOUNT > 0
    BEGIN
        UPDATE Users
        SET LastLogin = GETDATE()
        WHERE Username = @Username;
    END
END;

-- Thêm một tài khoản Admin mặc định
EXEC AddNewUser 'admin', 'admin', 'Admin', NULL;

---- Các ví dụ thêm tài khoản SinhVien và GiangVien (sau khi đã hash mật khẩu)
-- EXEC AddNewUser 'sinhvien4', 'sinhvien4', 'SinhVien', '18A10001';
-- EXEC AddNewUser 'giangvien1', 'hashed_gv_password', 'GiangVien', 'maGV001';
select * from Users

CREATE PROCEDURE GetDiemSinhVien
    @MaSV varchar(8)
AS
BEGIN
    SELECT 
        sv.MaSV,
        sv.HoTen AS HoTenSinhVien,
        sv.MaLop,
        hp.MaHP,
        hp.TenHP,
        hp.SoTC,
        hp.LoaiHP,
        hp.SoTiet,
        gd.NamHoc,
        gd.HocKy,
        gv.HoTen AS GiangVienPhuTrach,
        kqhp.ChuyenCan,
        kqhp.ThuongXuyen,
        kqhp.GHP,
        kqhp.CHP,
        kqhp.TongKet
    FROM KetQuaHocPhan kqhp
    JOIN SinhVien sv ON kqhp.MaSV = sv.MaSV
    JOIN HocPhan hp ON kqhp.MaHP = hp.MaHP
    LEFT JOIN GiangDay gd ON hp.MaHP = gd.MaHP AND gd.HocKy = kqhp.HocKy AND gd.NamHoc = kqhp.NamHoc
    LEFT JOIN GiangVien gv ON gd.MaGV = gv.MaGV
    WHERE kqhp.MaSV = @MaSV;
END;

Create PROCEDURE GetLopGiangVien
    @MaGV varchar(7)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT
        L.MaLop,
        GD.MaGV,
		l.MaCN,
        L.MaKhoa
    FROM Lop L
    INNER JOIN HocPhan HP ON L.MaKhoa = HP.MaKhoa
    INNER JOIN GiangDay GD ON HP.MaHP = GD.MaHP
    WHERE GD.MaGV = @MaGV
    ORDER BY L.MaLop;
END;
GO

CREATE PROCEDURE LoadSinhVienVaDiemTheoLop
    @MaLop VARCHAR(6), -- Tham số mã lớp
    @MaGV VARCHAR(7)   -- Tham số mã giảng viên
AS
BEGIN
    SELECT
        sv.MaSV,
        sv.HoTen AS HoTenSV,
        sv.MaLop,
        gv.HoTen AS HoTenGV, -- Tên giảng viên
        hp.MaHP,
        hp.TenHP,
        hp.SoTC,
        kqhp.ChuyenCan,
        kqhp.ThuongXuyen,
        kqhp.GHP,
        kqhp.CHP,
        kqhp.TongKet,
        kqhp.HocKy,
        kqhp.NamHoc
    FROM SinhVien sv
    JOIN KetQuaHocPhan kqhp ON sv.MaSV = kqhp.MaSV
    JOIN HocPhan hp ON kqhp.MaHP = hp.MaHP
    JOIN GiangDay gd ON kqhp.MaHP = gd.MaHP
                     AND kqhp.HocKy = gd.HocKy
                     AND kqhp.NamHoc = gd.NamHoc
                     AND sv.MaLop = gd.MaLop -- Đảm bảo lớp học khớp với bản ghi giảng dạy
    JOIN GiangVien gv ON gd.MaGV = gv.MaGV -- Tham gia bảng GiangVien để lấy tên giảng viên
    WHERE sv.MaLop = @MaLop AND gd.MaGV = @MaGV -- Lọc theo mã lớp và mã giảng viên
    ORDER BY sv.MaSV, hp.MaHP;
END;

-- Chỉnh sửa stored procedure sp_XemDiemLopTheoHocKy để sử dụng dynamic SQL
CREATE PROCEDURE sp_XemDiemLopTheoHocKy (
    @MaLop VARCHAR(6),
    @HocKy TINYINT,
    @NamHoc VARCHAR(9)
)
AS
BEGIN
    -- Kiểm tra xem lớp có tồn tại không
    IF NOT EXISTS (SELECT 1 FROM Lop WHERE MaLop = @MaLop)
    BEGIN
        PRINT 'Lớp không tồn tại.';
        RETURN;
    END;

    -- Kiểm tra xem học kỳ có hợp lệ không
    IF @HocKy NOT IN (1, 2)
    BEGIN
        PRINT 'Học kỳ không hợp lệ (chỉ có học kỳ 1 hoặc 2).';
        RETURN;
    END;

    -- Kiểm tra xem năm học có hợp lệ không
    IF NOT EXISTS (SELECT 1 FROM KetQuaHocPhan WHERE NamHoc = @NamHoc)
    BEGIN
        PRINT 'Năm học không tồn tại.';
        RETURN;
    END;

    -- Khai báo biến để lưu trữ câu lệnh SQL động
    DECLARE @SQL NVARCHAR(MAX);
    DECLARE @Columns NVARCHAR(MAX);

    -- Lấy danh sách các học phần duy nhất cho lớp, học kỳ và năm học đã cho
    SELECT @Columns = COALESCE(@Columns + ', ', '') + QUOTENAME(hp.TenHP)
    FROM SinhVien sv
    JOIN KetQuaHocPhan kqhp ON sv.MaSV = kqhp.MaSV
    JOIN HocPhan hp ON kqhp.MaHP = hp.MaHP
    WHERE sv.MaLop = @MaLop
        AND kqhp.HocKy = @HocKy
        AND kqhp.NamHoc = @NamHoc
    GROUP BY hp.TenHP
    ORDER BY hp.TenHP;

    -- Xây dựng câu lệnh SQL động
    SET @SQL = N'
        SELECT 
            MaSV, 
            HoTen, 
            HocKy, 
            NamHoc,
            ' + @Columns + '
        FROM (
            SELECT 
                sv.MaSV,
                sv.HoTen,
                kqhp.HocKy,
                kqhp.NamHoc,
                hp.TenHP,
                kqhp.TongKet
            FROM SinhVien sv
            JOIN KetQuaHocPhan kqhp ON sv.MaSV = kqhp.MaSV
            JOIN HocPhan hp ON kqhp.MaHP = hp.MaHP
            WHERE sv.MaLop = ''' + @MaLop + '''
                AND kqhp.HocKy = ''' + CAST(@HocKy AS VARCHAR(10)) + '''
                AND kqhp.NamHoc = ''' + @NamHoc + '''
        ) AS SourceTable
        PIVOT (
            SUM(TongKet)
            FOR TenHP IN (' + @Columns + ')
        ) AS PivotTable
        ORDER BY MaSV;
    ';

    -- Thực thi câu lệnh SQL động
    EXEC sp_executesql @SQL;
END;
GO

-- Test stored procedure đã chỉnh sửa
EXEC sp_XemDiemLopTheoHocKy 'B10D55', 1, '2023-2024';

CREATE PROCEDURE sp_XemLopChuNhiem (
    @MaCN VARCHAR(7)
)
AS
BEGIN
    -- Lấy thông tin lớp do giáo viên chủ nhiệm
    SELECT
        cn.MaCN,
        cn.HoTen AS HoTenChuNhiem,
        l.MaLop,
        k.TenKhoa,
        COUNT(sv.MaSV) AS SoLuongSinhVien
    FROM ChuNhiem cn
    JOIN Lop l ON cn.MaCN = l.MaCN
    JOIN Khoa k ON l.MaKhoa = k.MaKhoa
    LEFT JOIN SinhVien sv ON l.MaLop = sv.MaLop
    WHERE cn.MaCN IN (SELECT MaCN FROM ChuNhiem WHERE MaCN = @MaCN)
    GROUP BY cn.MaCN, cn.HoTen, l.MaLop, k.TenKhoa;
END;
GO
EXEC sp_XemLopChuNhiem 'CN01';
SELECT * FROM KetQuaHocKy;
