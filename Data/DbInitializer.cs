using Microsoft.AspNetCore.Identity;

public static class DbInitializer{
    public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager){
        if (!await roleManager.RoleExistsAsync("Admin")){
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }
        if (!await roleManager.RoleExistsAsync("User")){
            await roleManager.CreateAsync(new IdentityRole("User"));
        }
    }
}