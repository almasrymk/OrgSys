using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class EditSeed2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "Data.All", "Data" });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "Name", "Name2", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 40105L, false, null, "Inventory.All", null, "Inventory", null, 401L, 0, 0L },
                    { 4010405L, false, null, "Received.Preference", null, "Preference", null, 40104L, 0, 1L },
                    { 4010404L, false, null, "Received.Delete", null, "Delete", null, 40104L, 0, 1L },
                    { 4010403L, false, null, "Received.Edit", null, "Edit", null, 40104L, 0, 1L },
                    { 4010402L, false, null, "Received.Add", null, "Add", null, 40104L, 0, 1L },
                    { 4010401L, false, null, "Received.View", null, "View", null, 40104L, 0, 1L },
                    { 40104L, false, null, "Received.All", null, "Received", null, 401L, 0, 0L },
                    { 4010305L, false, null, "Transafer.Preference", null, "Preference", null, 40103L, 0, 1L },
                    { 4010304L, false, null, "Transafer.Delete", null, "Delete", null, 40103L, 0, 1L },
                    { 4010303L, false, null, "Transafer.Edit", null, "Edit", null, 40103L, 0, 1L },
                    { 4010302L, false, null, "Transafer.Add", null, "Add", null, 40103L, 0, 1L },
                    { 4010301L, false, null, "Transafer.View", null, "View", null, 40103L, 0, 1L },
                    { 40103L, false, null, "Transafer.All", null, "Transafer", null, 401L, 0, 0L },
                    { 4010205L, false, null, "Issue.Preference", null, "Preference", null, 40102L, 0, 1L },
                    { 4010204L, false, null, "Issue.Delete", null, "Delete", null, 40102L, 0, 1L },
                    { 4010203L, false, null, "Issue.Edit", null, "Edit", null, 40102L, 0, 1L },
                    { 4010202L, false, null, "Issue.Add", null, "Add", null, 40102L, 0, 1L },
                    { 4010201L, false, null, "Issue.View", null, "View", null, 40102L, 0, 1L },
                    { 40102L, false, null, "Issue.All", null, "Issue", null, 401L, 0, 0L },
                    { 4010501L, false, null, "Inventory.View", null, "View", null, 40105L, 0, 1L },
                    { 4010502L, false, null, "Inventory.Add", null, "Add", null, 40105L, 0, 1L },
                    { 4010503L, false, null, "Inventory.Edit", null, "Edit", null, 40105L, 0, 1L },
                    { 4010504L, false, null, "Inventory.Delete", null, "Delete", null, 40105L, 0, 1L },
                    { 5010304L, false, null, "Outlay.Delete", null, "Delete", null, 50103L, 0, 1L },
                    { 5010303L, false, null, "Outlay.Edit", null, "Edit", null, 50103L, 0, 1L },
                    { 5010302L, false, null, "Outlay.Add", null, "Add", null, 50103L, 0, 1L },
                    { 5010301L, false, null, "Outlay.View", null, "View", null, 50103L, 0, 1L },
                    { 50103L, false, null, "Outlay.All", null, "Outlay", null, 501L, 0, 0L },
                    { 5010205L, false, null, "Payment.Preference", null, "Preference", null, 50102L, 0, 1L },
                    { 5010204L, false, null, "Payment.Delete", null, "Delete", null, 50102L, 0, 1L },
                    { 5010203L, false, null, "Payment.Edit", null, "Edit", null, 50102L, 0, 1L },
                    { 5010202L, false, null, "Payment.Add", null, "Add", null, 50102L, 0, 1L },
                    { 4010105L, false, null, "Addition.Preference", null, "Preference", null, 40101L, 0, 1L },
                    { 5010201L, false, null, "Payment.View", null, "View", null, 50102L, 0, 1L },
                    { 5010105L, false, null, "Collection.Preference", null, "Preference", null, 50101L, 0, 1L },
                    { 5010104L, false, null, "Collection.Delete", null, "Delete", null, 50101L, 0, 1L },
                    { 5010103L, false, null, "Collection.Edit", null, "Edit", null, 50101L, 0, 1L },
                    { 5010102L, false, null, "Collection.Add", null, "Add", null, 50101L, 0, 1L },
                    { 5010101L, false, null, "Collection.View", null, "View", null, 50101L, 0, 1L },
                    { 50101L, false, null, "Collection.All", null, "Collection", null, 501L, 0, 0L },
                    { 501L, false, null, "SafeNotices", null, "Safe Notices", null, 50L, 0, 0L }
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "Name", "Name2", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 50L, false, null, "Financials.All", null, "Financials", null, 1L, 0, 0L },
                    { 4010505L, false, null, "Inventory.Preference", null, "Preference", null, 40105L, 0, 1L },
                    { 50102L, false, null, "Payment.All", null, "Payment", null, 501L, 0, 0L },
                    { 4010104L, false, null, "Addition.Delete", null, "Delete", null, 40101L, 0, 1L },
                    { 4010103L, false, null, "Addition.Edit", null, "Edit", null, 40101L, 0, 1L },
                    { 4010102L, false, null, "Addition.Add", null, "Add", null, 40101L, 0, 1L },
                    { 3010104L, false, null, "SalesInvoices.Delete", null, "Delete", null, 30101L, 0, 1L },
                    { 3010103L, false, null, "SalesInvoices.Edit", null, "Edit", null, 30101L, 0, 1L },
                    { 3010102L, false, null, "SalesInvoices.Add", null, "Add", null, 30101L, 0, 1L },
                    { 3010101L, false, null, "SalesInvoices.View", null, "View", null, 30101L, 0, 1L },
                    { 30101L, false, null, "SalesInvoices.All", null, "Invoices", null, 301L, 0, 0L },
                    { 301L, false, null, "Sales", null, "Sales", null, 30L, 0, 0L },
                    { 30L, false, null, "Invoices.All", null, "Invoices", null, 1L, 0, 0L },
                    { 2010205L, false, null, "External.Preference", null, "Preference", null, 20102L, 0, 1L },
                    { 2010204L, false, null, "External.Delete", null, "Delete", null, 20102L, 0, 1L },
                    { 3010105L, false, null, "SalesInvoices.Preference", null, "Preference", null, 30101L, 0, 1L },
                    { 2010203L, false, null, "External.Edit", null, "Edit", null, 20102L, 0, 1L },
                    { 2010201L, false, null, "External.View", null, "View", null, 20102L, 0, 1L },
                    { 20102L, false, null, "External.All", null, "External", null, 201L, 0, 0L },
                    { 2010105L, false, null, "Internal.Preference", null, "Preference", null, 20101L, 0, 1L },
                    { 2010104L, false, null, "Internal.Delete", null, "Delete", null, 20101L, 0, 1L },
                    { 2010103L, false, null, "Internal.Edit", null, "Edit", null, 20101L, 0, 1L },
                    { 2010102L, false, null, "Internal.Add", null, "Add", null, 20101L, 0, 1L },
                    { 2010101L, false, null, "Internal.View", null, "View", null, 20101L, 0, 1L },
                    { 20101L, false, null, "Internal.All", null, "Internal", null, 201L, 0, 0L },
                    { 201L, false, null, "Orders", null, "Orders", null, 20L, 0, 0L },
                    { 2010202L, false, null, "External.Add", null, "Add", null, 20102L, 0, 1L },
                    { 5010305L, false, null, "Outlay.Preference", null, "Preference", null, 50103L, 0, 1L },
                    { 30102L, false, null, "SalesReturns.All", null, "Returns", null, 301L, 0, 0L },
                    { 3010202L, false, null, "SalesReturns.Add", null, "Add", null, 30102L, 0, 1L },
                    { 4010101L, false, null, "Addition.View", null, "View", null, 40101L, 0, 1L },
                    { 40101L, false, null, "Addition.All", null, "Addition", null, 401L, 0, 0L },
                    { 401L, false, null, "TransactionNotices", null, "Transaction Notices", null, 40L, 0, 0L },
                    { 40L, false, null, "Transactions.All", null, "Transactions", null, 1L, 0, 0L },
                    { 3020205L, false, null, "PurchasesReturns.Preference", null, "Preference", null, 30202L, 0, 1L },
                    { 3020204L, false, null, "PurchasesReturns.Delete", null, "Delete", null, 30202L, 0, 1L },
                    { 3020203L, false, null, "PurchasesReturns.Edit", null, "Edit", null, 30202L, 0, 1L },
                    { 3020202L, false, null, "PurchasesReturns.Add", null, "Add", null, 30202L, 0, 1L },
                    { 3020201L, false, null, "PurchasesReturns.View", null, "View", null, 30202L, 0, 1L },
                    { 3010201L, false, null, "SalesReturns.View", null, "View", null, 30102L, 0, 1L },
                    { 30202L, false, null, "PurchasesReturns.All", null, "Returns", null, 302L, 0, 0L },
                    { 3020104L, false, null, "PurchasesInvoices.Delete", null, "Delete", null, 30201L, 0, 1L }
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "Name", "Name2", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 3020103L, false, null, "PurchasesInvoices.Edit", null, "Edit", null, 30201L, 0, 1L },
                    { 3020102L, false, null, "PurchasesInvoices.Add", null, "Add", null, 30201L, 0, 1L },
                    { 3020101L, false, null, "PurchasesInvoices.View", null, "View", null, 30201L, 0, 1L },
                    { 30201L, false, null, "PurchasesInvoices.All", null, "Invoices", null, 302L, 0, 0L },
                    { 302L, false, null, "Purchases", null, "Purchases", null, 30L, 0, 0L },
                    { 3010205L, false, null, "SalesReturns.Preference", null, "Preference", null, 30102L, 0, 1L },
                    { 3010204L, false, null, "SalesReturns.Delete", null, "Delete", null, 30102L, 0, 1L },
                    { 3010203L, false, null, "SalesReturns.Edit", null, "Edit", null, 30102L, 0, 1L },
                    { 3020105L, false, null, "PurchasesInvoices.Preference", null, "Preference", null, 30201L, 0, 1L },
                    { 20L, false, null, "Orders.All", null, "Orders", null, 1L, 0, 0L }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 30L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 40L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 50L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 201L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 301L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 302L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 401L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 501L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20101L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20102L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 30101L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 30102L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 30201L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 30202L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 40101L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 40102L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 40103L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 40104L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 40105L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 50101L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 50102L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 50103L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2010101L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2010102L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2010103L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2010104L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2010105L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2010201L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2010202L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2010203L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2010204L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2010205L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3010101L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3010102L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3010103L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3010104L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3010105L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3010201L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3010202L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3010203L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3010204L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3010205L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3020101L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3020102L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3020103L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3020104L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3020105L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3020201L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3020202L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3020203L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3020204L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3020205L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010101L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010102L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010103L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010104L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010105L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010201L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010202L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010203L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010204L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010205L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010301L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010302L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010303L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010304L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010305L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010401L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010402L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010403L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010404L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010405L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010501L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010502L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010503L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010504L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4010505L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 5010101L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 5010102L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 5010103L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 5010104L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 5010105L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 5010201L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 5010202L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 5010203L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 5010204L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 5010205L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 5010301L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 5010302L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 5010303L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 5010304L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 5010305L);

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "Setting.All", "Setting" });
        }
    }
}
