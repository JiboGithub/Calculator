//using Calculator.Domain.Constants;
//using Microsoft.EntityFrameworkCore.Migrations;

//namespace Calculator.Api
//{
//    public partial class SchemaCreate : Migration
//    {
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            // Creating Users Table
//            migrationBuilder.CreateTable(
//                name: "Users",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Username = table.Column<string>(maxLength: 10, nullable: false)
//                },
//                constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); });

//            // Creating CalculationHistory Table
//            migrationBuilder.CreateTable(
//                name: "CalculationHistory",
//                columns: table => new
//                {
//                    Id = table.Column<int>(nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    UserId = table.Column<int>(nullable: false),
//                    OperationType = table.Column<string>(nullable: false),
//                    FirstValue = table.Column<double>(nullable: false),
//                    SecondValue = table.Column<double>(nullable: false),
//                    Result = table.Column<double>(nullable: false),
//                    CalculationDate = table.Column<DateTime>(nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_CalculationHistory", x => x.Id);
//                    table.ForeignKey("FK_CalculationHistory_Users", x => x.UserId, "Users", "Id");
//                });

//            // Creating Stored Procedures
//            migrationBuilder.Sql(QueryConstants.CreateUserQuery);
//            migrationBuilder.Sql(QueryConstants.GetUserByIdQuery);
//            migrationBuilder.Sql(QueryConstants.SaveCalculationQuery);
//            migrationBuilder.Sql(QueryConstants.GetCalculationHistoryQuery);
//        }

//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            // Dropping Stored Procedures
//            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS CreateUser");
//            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetUserById");
//            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS SaveCalculationQuery");
//            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetCalculationHistoryByUserId");

//            // Dropping Tables
//            migrationBuilder.DropTable(name: "CalculationHistory");
//            migrationBuilder.DropTable(name: "Users");
//        }
//    }

//}
