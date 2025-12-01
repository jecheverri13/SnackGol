using System.Net;
using FluentAssertions;
using LibraryAuthentication;
using LibraryConnection.Context;
using LibraryConnection.DbSet;
using LibraryEncrypt.Encryption;
using LibraryEntities;
using LibraryEntities.Models;
using Microsoft.AspNetCore.Mvc;
using MSSnackGol.Controllers;
using SnackGol.Tests.Utilities;
using Xunit;

namespace SnackGol.Tests.Controllers;

public class LoginManagementControllerTests
{
    public LoginManagementControllerTests()
    {
        DbTestHelper.UseFreshInMemoryDb();
    }

    [Fact]
    public void Login_Success_Returns200_AndToken()
    {
        // Arrange: create role and user in in-memory DB
        using (var db = new ApplicationDbContext())
        {
            var role = new Role { name = "Administrador", description = "Admin role" };
            db.roles.Add(role);
            db.SaveChanges();

            var user = new User
            {
                name = "Test",
                last_name = "User",
                email = "test@example.com",
                password = EncryptCSS.Encrypt("P@ssw0rd"),
                id_role = role.id
            };
            db.users.Add(user);
            db.SaveChanges();
        }

        var jwtHandler = new JwtTokenHandler();
        var controller = new LoginManagementController(jwtHandler);
        var login = new LoginRequest { UserNname = "test@example.com", Password = "P@ssw0rd" };

        // Act
        var result = controller.Login(login) as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.OK);

        var body = result.Value as Response<Auth>;
        body.Should().NotBeNull();
        body!.response.Should().NotBeNull();
        body.response!.token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Login_InvalidCredentials_Returns401()
    {
        // Arrange: ensure DB exists but no user with this email
        var jwtHandler = new JwtTokenHandler();
        var controller = new LoginManagementController(jwtHandler);
        var login = new LoginRequest { UserNname = "nouser@example.com", Password = "badpass" };

        // Act
        var result = controller.Login(login) as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
    }
}
