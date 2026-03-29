using Ecclesia.Domain.Entities.Users;
using Ecclesia.Domain.Entities.Roles;
using Ecclesia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Ecclesia.Domain.Common.Constants.SchemaConstants;

namespace Ecclesia.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Si ya existe un usuario admin no hace nada
        if (await context.Users.AnyAsync())
            return;

        // Crear rol admin
        var adminRole = new RoleEntity("admin", "system admin");
        await context.Roles.AddAsync(adminRole);
        await context.SaveChangesAsync();

        // Crear permisos para el rol admin
        var permissions = new List<RolePermissionEntity>
        {
            new(adminRole.Id, SchemaConstants.AccessManager.schema, "users", "create"),
            new(adminRole.Id, SchemaConstants.AccessManager.schema, "users", "read"),
            new(adminRole.Id, SchemaConstants.AccessManager.schema, "users", "update"),
            new(adminRole.Id, SchemaConstants.AccessManager.schema, "users", "delete"),
        };
        
        await context.RolePermissions.AddRangeAsync(permissions);
        await context.SaveChangesAsync();

        // Crear usuario admin
        var adminUser = new UserEntity(
            name:         "Admin",
            email:        "admin@ecclesia.com",
            userName:     "admin",
            passwordHash: BCrypt.Net.BCrypt.HashPassword("Admin123!")
        );

        await context.Users.AddAsync(adminUser);
        await context.SaveChangesAsync();

        // Asignar rol admin al usuario
        var userRole = new UserRoleEntity(adminUser.Id, adminRole.Id);
        await context.UserRoles.AddAsync(userRole);
        await context.SaveChangesAsync();
    }
}