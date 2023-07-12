CREATE DATABASE LK_XE
USE LK_XE

-- KHACH HANG
-- SAN PHAM

CREATE TABLE LOAISP
(
  MALOAI CHAR(5) NOT NULL PRIMARY KEY,
  TenLoai NVARCHAR(50) NULL,
)
CREATE TABLE TAIKHOAN
(
	TENDN NVARCHAR(50) PRIMARY KEY, 
	PASS NVARCHAR(50),
	LOAITK NVARCHAR(30),
)
CREATE TABLE KHACHHANG
(
	MAKH CHAR(5) NOT NULL PRIMARY KEY,
	TENKH NVARCHAR(50),
	NGAYSINH DATE,
	GIOTINH NVARCHAR(20),
	SDT NVARCHAR(20),
	TENDN NVARCHAR(50),
	FOREIGN KEY(TENDN) REFERENCES TAIKHOAN(TENDN)	
)

CREATE TABLE NHACUNGCAP
(
	MANCC CHAR(5) not null PRIMARY KEY ,
	TENNCC CHAR(5)
)
ALTER TABLE SANPHAM
ALTER COLUMN MANCC char(5) null


CREATE TABLE SANPHAM
(
	MASP CHAR(5) NOT NULL PRIMARY KEY,
	TENSP NVARCHAR(50),
	HINH NVARCHAR(50),
	SOLUONG INT,
	DONGIA float,
	MALOAI CHAR(5),
	MANCC CHAR(5),
	MOTA NVARCHAR(100),
	foreign key (MALOAI) references LOAISP(MALOAI),
	FOREIGN KEY(MANCC) REFERENCES NHACUNGCAP(MANCC)

)
CREATE TABLE DanhGia
(
	MASP CHAR(5),
	MAKH CHAR(5),
	SAO NVARCHAR(5),
	NOIDUNG NVARCHAR(100),
	FOREIGN KEY(MASP) REFERENCES SANPHAM(MASP),
	FOREIGN KEY(MAKH) REFERENCES KHACHHANG(MAKH)
)


CREATE TABLE GIOHANG
(
	MAKH CHAR(5),
	MASP CHAR(5),
	SOLUONGMU INT,
	PRIMARY KEY (MAKH, MASP),
	FOREIGN KEY(MAKH) REFERENCES KHACHHANG(MAKH),
	FOREIGN KEY(MASP) REFERENCES SANPHAM(MASP)
)
CREATE TABLE DONVIVANCHUYEN
(
	MADV CHAR(5) NOT NULL PRIMARY KEY,
	TENDV NVARCHAR(50)
)

CREATE TABLE HOADON
(
	MAHD CHAR(5) NOT NULL PRIMARY KEY,
	NGAYMUA DATE,
	TINHTRANG NVARCHAR(50),
	thanhTien float,
	DIACHIGH NVARCHAR(50),
	SDT NVARCHAR(50),
	ptTT nvarchar(50),
	MAKH CHAR(5),
	MADV CHAR(5),
	FOREIGN KEY(MAKH) REFERENCES KHACHHANG(MAKH),
	FOREIGN KEY (MADV) REFERENCES DONVIVANCHUYEN(MADV)
)

CREATE TABLE CTHD
(
	MASP CHAR(5),
	MAHD CHAR(5),
	SOLUONGMUA INT,
	TONGTIEN MONEY
	PRIMARY KEY (MASP,MAHD),
	FOREIGN KEY(MAHD) REFERENCES HOADON(MAHD),	
	FOREIGN KEY(MASP) REFERENCES SANPHAM(MASP)
)



INSERT INTO LOAISP VALUES
('LP001', N'Màn hình xe hơi'),
('LP002', N'Camera 360'),
('LP003', N'Camera hành trình'),
('LP004', N'Camera lùi')

INSERT INTO SANPHAM(MASP, TENSP, HINH, SOLUONG, DONGIA, MALOAI, MANCC) VALUES
('SP001',N'DVD Honda Civic 2007-2011','DVD-Honda-Civic-01-300x300.jpg',10,10200000,'LP001', 'NC001'),
('SP002',N'DVD Kia Forte Điều Hòa Cơ','DVD-Kia-Forte-01-300x300.jpg',10,9600000,'LP001','NC001'),
('SP003',N'DVD Nissan X trail 2014-2015','DVD-Nissan-X-trail-01-300x300.jpg',10,10800000,'LP001','NC001'),
('SP004',N'DVD Toyota Altis 2014-2016','DVD-Toyota-Altis-01-300x300.jpg',10,9800000,'LP001','NC001'),

('SP005',N'Camera 360 omniview','Camera-360-omniview-300x300.jpg',10,18900000,'LP002','NC002'),
('SP006',N'Camera 360 panorama','Camera-360-panorama-01-300x300.jpg',10,16900000,'LP002','NC001'),
('SP007',N'Camera hành trình 360 độ','DDPAI-360-300x300.jpg',10,1516000,'LP002','NC002'),
('SP008',N'Camera quan sát 360 độ','Omnivue-01-300x300.jpg',10,18899000,'LP002','NC003'),

('SP009',N'Camera hành trình Vietmap K9 Pro','Vietmap-K9-Pro-300x300.jpg',10,369000,'LP003','NC001'),
('SP010',N'Camera hành trình Vietmap K12','Vietmap-K12-300x300.jpg',10,3290000,'LP003','NC003'),
('SP011',N'Camera hành trình Xiaomi Yi Car','Xiaomi-Yi-Car-2-300x300.jpg',10,850000,'LP003','NC002'),
('SP012',N'Camera hành trình Xiaomi Yi Car','Xiaomi-Yi-Car-300x300.jpg',10,1050000,'LP003','NC001'),

('SP013',N'Hệ thống camera lùi EskyEC170','Esky-EC170-20-300x300.jpg',10,3300000,'LP004','NC001'),
('SP014',N'Camera lùi Garmin BC','Garmin-BC-20-300x300.jpg',10,5600000,'LP004','NC002'),
('SP015',N'Camera lùi RVS-091406','RVS-091406-01-300x300.jpg',10,4200000,'LP004','NC003'),
('SP016',N'Camera lùi RVS-091407','RVS-091407-01-300x300.jpg',10,667000,'LP004','NC003'),

INSERT INTO DONVIVANCHUYEN VALUES 

	('DV001', N'Viettel Box'),
	('DV002', N'VietNam Post'),
	('DV003', N'J$T Express')

insert into NHACUNGCAP values
('NC001',N'BuCaTi'),
('NC002',N'BMW'),
('NC003',N'Xiaomi')


create trigger tinhTongTienCTHD
on CTHD for insert
as 
begin
	declare @maHD char(5) = (select MAHD from inserted)
	declare @maSP char(5) = (select MASP from inserted)
	declare @giatien float = (select DONGIA from SANPHAM where MASP = @maSP)
	update CTHD
	set TONGTIEN = (SOLUONGMUA * @giatien)
	where MAHD = @maHD and MASP= @maSP
end





-- ham tu tang ma
CREATE FUNCTION FUNC_NEXTID(@lastID varchar(6), @prefix varchar(3), @size int)
	returns varchar(6)
as
	begin 
		if(@lastID = '')
			set @lastID = @prefix + REPLICATE(0, @size -LEN(@prefix))
		declare @num_nextId int, @nextID varchar(6)
		set @lastID = LTRIM(RTRIM(@lastID))
		set @num_nextId = REPLACE(@lastID,@prefix,'')+1
		set @size = @size - len(@prefix)
		set @nextID = @prefix + REPLICATE(0, @size -LEN(@prefix))
		set @nextID = @prefix + RIGHT(REPLICATE(0, @size) + CONVERT(varchar(MAX), @num_nextId),@size)
		return @nextID
	end

insert into KHACHHANG(MAKH, TENKH) values
('', N'hehe')
--Tu dong tang ma KHACH HANG
CREATE TRIGGER NextMaKH on KHACHHANG
for Insert
as
	begin
		declare @lastId varchar(6)
		set @lastId = (Select TOP 1 MAKH from KhachHang order by MAKH desc)
		UPDATE KhachHang set MAKH = dbo.FUNC_NEXTID(@lastId,'KH',5) where MAKH = ''
	end


CREATE TRIGGER NextHd on HOADON
for Insert
as
	begin
		declare @lastId varchar(6)
		set @lastId = (Select TOP 1 MAHD from HOADON order by MAHD desc)
		UPDATE HOADON set MAHD = dbo.FUNC_NEXTID(@lastId,'HD',5) where MAHD = ''
	end

CREATE TRIGGER NextSp on SANPHAM
for Insert
as
	begin
		declare @lastId varchar(6)
		set @lastId = (Select TOP 1 MASP from SANPHAM order by MASP desc)
		UPDATE SANPHAM set MASP = dbo.FUNC_NEXTID(@lastId,'SP',5) where MASP = ''
	end


CREATE TRIGGER NextNCC on NHACUNGCAP
for Insert
as
	begin
		declare @lastId varchar(6)
		set @lastId = (Select TOP 1 MANCC from NHACUNGCAP order by MANCC desc)
		UPDATE NHACUNGCAP set MANCC = dbo.FUNC_NEXTID(@lastId,'CC',5) where MANCC = ''
	end


CREATE TRIGGER NextLSP on LOAISP
for Insert
as
	begin
		declare @lastId varchar(6)
		set @lastId = (Select TOP 1 MALOAI from LOAISP order by MALOAI desc)
		UPDATE LOAISP set MALOAI = dbo.FUNC_NEXTID(@lastId,'CC',5) where MALOAI = ''
	end


CREATE TRIGGER NextDV on DONVIVANCHUYEN
for Insert
as
	begin
		declare @lastId varchar(6)
		set @lastId = (Select TOP 1 MADV from DONVIVANCHUYEN order by MADV desc)
		UPDATE DONVIVANCHUYEN set MADV = dbo.FUNC_NEXTID(@lastId,'DV',5) where MADV = ''
	end