USE QUAN_LY_DIEM_SINH_VIEN1;  -- Chọn cơ sở dữ liệu hiện có

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
ADD CONSTRAINT CHK_UserRole CHECK (Role IN ('SinhVien', 'GiangVien', 'Admin'));

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
