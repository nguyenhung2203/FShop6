using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FShop6.Migrations
{
    /// <inheritdoc />
    public partial class KhoiTao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "trangChuViewModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trangChuViewModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DanhMucModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDanhMuc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangChuViewModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DanhMucModel_trangChuViewModels_TrangChuViewModelId",
                        column: x => x.TrangChuViewModelId,
                        principalTable: "trangChuViewModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DanhMucSanPhamViewModel",
                columns: table => new
                {
                    TenDanhMuc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TrangChuViewModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucSanPhamViewModel", x => x.TenDanhMuc);
                    table.ForeignKey(
                        name: "FK_DanhMucSanPhamViewModel_trangChuViewModels_TrangChuViewModelId",
                        column: x => x.TrangChuViewModelId,
                        principalTable: "trangChuViewModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TinTucModel",
                columns: table => new
                {
                    MaTinTuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TieuDe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTaNgan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HinhAnhDaiDien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangChuViewModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinTucModel", x => x.MaTinTuc);
                    table.ForeignKey(
                        name: "FK_TinTucModel_trangChuViewModels_TrangChuViewModelId",
                        column: x => x.TrangChuViewModelId,
                        principalTable: "trangChuViewModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SanPhamModel",
                columns: table => new
                {
                    MaSanPham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDanhMucSP = table.Column<int>(type: "int", nullable: false),
                    TenSanPham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HinhAnhDaiDien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DanhMucId = table.Column<int>(type: "int", nullable: false),
                    DanhMucSanPhamViewModelTenDanhMuc = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TrangChuViewModelId = table.Column<int>(type: "int", nullable: true),
                    TrangChuViewModelId1 = table.Column<int>(type: "int", nullable: true),
                    TrangChuViewModelId2 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhamModel", x => x.MaSanPham);
                    table.ForeignKey(
                        name: "FK_SanPhamModel_DanhMucModel_DanhMucId",
                        column: x => x.DanhMucId,
                        principalTable: "DanhMucModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SanPhamModel_DanhMucSanPhamViewModel_DanhMucSanPhamViewModelTenDanhMuc",
                        column: x => x.DanhMucSanPhamViewModelTenDanhMuc,
                        principalTable: "DanhMucSanPhamViewModel",
                        principalColumn: "TenDanhMuc");
                    table.ForeignKey(
                        name: "FK_SanPhamModel_trangChuViewModels_TrangChuViewModelId",
                        column: x => x.TrangChuViewModelId,
                        principalTable: "trangChuViewModels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SanPhamModel_trangChuViewModels_TrangChuViewModelId1",
                        column: x => x.TrangChuViewModelId1,
                        principalTable: "trangChuViewModels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SanPhamModel_trangChuViewModels_TrangChuViewModelId2",
                        column: x => x.TrangChuViewModelId2,
                        principalTable: "trangChuViewModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BienTheModels",
                columns: table => new
                {
                    MaBienThe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    MaSKU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoaiBienThe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GiaNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiaBan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoLuongConLai = table.Column<int>(type: "int", nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SanPhamMaSanPham = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BienTheModels", x => x.MaBienThe);
                    table.ForeignKey(
                        name: "FK_BienTheModels_SanPhamModel_SanPhamMaSanPham",
                        column: x => x.SanPhamMaSanPham,
                        principalTable: "SanPhamModel",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BienTheModels_SanPhamMaSanPham",
                table: "BienTheModels",
                column: "SanPhamMaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DanhMucModel_TrangChuViewModelId",
                table: "DanhMucModel",
                column: "TrangChuViewModelId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhMucSanPhamViewModel_TrangChuViewModelId",
                table: "DanhMucSanPhamViewModel",
                column: "TrangChuViewModelId");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamModel_DanhMucId",
                table: "SanPhamModel",
                column: "DanhMucId");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamModel_DanhMucSanPhamViewModelTenDanhMuc",
                table: "SanPhamModel",
                column: "DanhMucSanPhamViewModelTenDanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamModel_TrangChuViewModelId",
                table: "SanPhamModel",
                column: "TrangChuViewModelId");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamModel_TrangChuViewModelId1",
                table: "SanPhamModel",
                column: "TrangChuViewModelId1");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamModel_TrangChuViewModelId2",
                table: "SanPhamModel",
                column: "TrangChuViewModelId2");

            migrationBuilder.CreateIndex(
                name: "IX_TinTucModel_TrangChuViewModelId",
                table: "TinTucModel",
                column: "TrangChuViewModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BienTheModels");

            migrationBuilder.DropTable(
                name: "TinTucModel");

            migrationBuilder.DropTable(
                name: "SanPhamModel");

            migrationBuilder.DropTable(
                name: "DanhMucModel");

            migrationBuilder.DropTable(
                name: "DanhMucSanPhamViewModel");

            migrationBuilder.DropTable(
                name: "trangChuViewModels");
        }
    }
}
