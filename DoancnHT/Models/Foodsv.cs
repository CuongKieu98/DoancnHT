using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DoancnHT.Models
{
    public class Foodsv
    {
    }
    public class Cuahang
    {
        [Key]
        public int MaCH { get; set; }
        public string TenCH { get; set; }
        public string DiachiCH { get; set; }
        public string DienthoaiCH { get; set; }
        public string MotaCH { get; set; }
        public string DanhgiaCH { get; set; }
        public int MaDonHang { get; set; }
    }
    public class Khuyenmai
    {
        [Key]
        public int MaKM { get; set; }
        public string TenKM { get; set; }
        public string MotaKM { get; set; }
        public string TgBatDau { get; set; }
        public string TgKetThuc { get; set; }
        
        public int MaCH { get; set; }
    }
    public class Danhmucdoan
    {
        [Key]
        public int MaDM { get; set; }
        public string TenDM { get; set; }
        public string Hinhanhdm { get; set; }
        public int MaCH { get; set; }
    }
    public class Doan
    {
        [Key]
        public int MaDA { get; set; }
        public string TenDA { get; set; }
        public string Dongia { get; set; }
        public string AnhDA { get; set; }
        public string MoTa { get; set; }
        public string NgayCapNhat { get; set; }
        public string SoLuongTon { get; set; }
        public string TrangThaiDA { get; set; }
        public string DanhGiaDoAn { get; set; }
        public int MaDM { get; set; }

    }
    public class ViTienCuaHang
    {
        [Key]
        public int MaViCH { get; set; }
        public string SoDu { get; set; }
        public string NgayGiaoDich { get; set; }
        public string SoTienGiaoDich { get; set; }
        public int MaCH { get; set; }
    }
    public class dbHutechfoodContext : DbContext
    {
        public DbSet<Khuyenmai> Khuyenmais { get; set; }
        public DbSet<Cuahang> Cuahangs { get; set; }
        public DbSet<Danhmucdoan> Danhmucdoans { get; set; }
        public DbSet<Doan> Doans { get; set; }
        public DbSet<ViTienCuaHang> ViTienCuaHangs { get; set; }
    }
}