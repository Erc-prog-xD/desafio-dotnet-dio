using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiGateway.Migrations
{
    /// <inheritdoc />
    public partial class nova7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoHistoricStatus_Pedido_PedidosIdPedido",
                table: "PedidoHistoricStatus");

            migrationBuilder.DropIndex(
                name: "IX_PedidoHistoricStatus_PedidosIdPedido",
                table: "PedidoHistoricStatus");

            migrationBuilder.RenameColumn(
                name: "PedidosIdPedido",
                table: "PedidoHistoricStatus",
                newName: "PedidosIdPedidos");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "PedidoHistoricStatus",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PedidosIdPedidos",
                table: "PedidoHistoricStatus",
                newName: "PedidosIdPedido");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PedidoHistoricStatus",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoHistoricStatus_PedidosIdPedido",
                table: "PedidoHistoricStatus",
                column: "PedidosIdPedido");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoHistoricStatus_Pedido_PedidosIdPedido",
                table: "PedidoHistoricStatus",
                column: "PedidosIdPedido",
                principalTable: "Pedido",
                principalColumn: "IdPedido",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
