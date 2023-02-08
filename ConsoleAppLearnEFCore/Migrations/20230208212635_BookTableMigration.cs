using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleAppLearnEFCore.Migrations
{
    /// <inheritdoc />
    public partial class BookTableMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorBook_Authors_AuthorsId",
                table: "AuthorBook");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorBook_Books_BooksId",
                table: "AuthorBook");

            migrationBuilder.DropForeignKey(
                name: "FK_BookSection_Books_BooksId",
                table: "BookSection");

            migrationBuilder.DropForeignKey(
                name: "FK_BookSection_Sections_SectionsId",
                table: "BookSection");

            migrationBuilder.RenameColumn(
                name: "SectionsId",
                table: "BookSection",
                newName: "BookSectionsId1");

            migrationBuilder.RenameColumn(
                name: "BooksId",
                table: "BookSection",
                newName: "BookSectionsId");

            migrationBuilder.RenameIndex(
                name: "IX_BookSection_SectionsId",
                table: "BookSection",
                newName: "IX_BookSection_BookSectionsId1");

            migrationBuilder.RenameColumn(
                name: "BooksId",
                table: "AuthorBook",
                newName: "BookAuthorsId1");

            migrationBuilder.RenameColumn(
                name: "AuthorsId",
                table: "AuthorBook",
                newName: "BookAuthorsId");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorBook_BooksId",
                table: "AuthorBook",
                newName: "IX_AuthorBook_BookAuthorsId1");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sections",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Books",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Authors",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Authors",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorBook_Authors_BookAuthorsId",
                table: "AuthorBook",
                column: "BookAuthorsId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorBook_Books_BookAuthorsId1",
                table: "AuthorBook",
                column: "BookAuthorsId1",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookSection_Books_BookSectionsId",
                table: "BookSection",
                column: "BookSectionsId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookSection_Sections_BookSectionsId1",
                table: "BookSection",
                column: "BookSectionsId1",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorBook_Authors_BookAuthorsId",
                table: "AuthorBook");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorBook_Books_BookAuthorsId1",
                table: "AuthorBook");

            migrationBuilder.DropForeignKey(
                name: "FK_BookSection_Books_BookSectionsId",
                table: "BookSection");

            migrationBuilder.DropForeignKey(
                name: "FK_BookSection_Sections_BookSectionsId1",
                table: "BookSection");

            migrationBuilder.RenameColumn(
                name: "BookSectionsId1",
                table: "BookSection",
                newName: "SectionsId");

            migrationBuilder.RenameColumn(
                name: "BookSectionsId",
                table: "BookSection",
                newName: "BooksId");

            migrationBuilder.RenameIndex(
                name: "IX_BookSection_BookSectionsId1",
                table: "BookSection",
                newName: "IX_BookSection_SectionsId");

            migrationBuilder.RenameColumn(
                name: "BookAuthorsId1",
                table: "AuthorBook",
                newName: "BooksId");

            migrationBuilder.RenameColumn(
                name: "BookAuthorsId",
                table: "AuthorBook",
                newName: "AuthorsId");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorBook_BookAuthorsId1",
                table: "AuthorBook",
                newName: "IX_AuthorBook_BooksId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sections",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorBook_Authors_AuthorsId",
                table: "AuthorBook",
                column: "AuthorsId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorBook_Books_BooksId",
                table: "AuthorBook",
                column: "BooksId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookSection_Books_BooksId",
                table: "BookSection",
                column: "BooksId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookSection_Sections_SectionsId",
                table: "BookSection",
                column: "SectionsId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
