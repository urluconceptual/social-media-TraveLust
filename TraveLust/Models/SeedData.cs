using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TraveLust.Data;

namespace TraveLust.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService
                <DbContextOptions<ApplicationDbContext>>()))
            {
                // Verificam daca in baza de date exista cel putin un rol
                // insemnand ca a fost rulat codul 
                // De aceea facem return pentru a nu insera rolurile inca o data
                // Acesta metoda trebuie sa se execute o singura data 
                if (context.Roles.Any())
                {
                    return;   // baza de date contine deja roluri
                }

                // CREAREA ROLURILOR IN BD
                // daca nu contine roluri, acestea se vor crea
                context.Roles.AddRange(
                    new IdentityRole { Id = "f2369e92-e6e9-48f0-a1e6-4e146596c283", Name = "Admin", NormalizedName = "Admin".ToUpper() },
                    new IdentityRole { Id = "5194406e-a6ae-4570-94a8-5af47a8f0468", Name = "Editor", NormalizedName = "Editor".ToUpper() },
                    new IdentityRole { Id = "1f89f81e-95cf-4360-9834-c0214c08d357", Name = "User", NormalizedName = "User".ToUpper() }
                );

                // o noua instanta pe care o vom utiliza pentru crearea parolelor utilizatorilor
                // parolele sunt de tip hash
                var hasher = new PasswordHasher<ApplicationUser>();

                // CREAREA USERILOR IN BD
                // Se creeaza cate un user pentru fiecare rol
                context.Users.AddRange(
                    new ApplicationUser
                    {
                        Id = "75c8f3aa-e6f2-40d7-b865-b807e8ec6bb5", // primary key
                        UserName = "admin@test.com",
                        EmailConfirmed = true,
                        NormalizedEmail = "ADMIN@TEST.COM",
                        Email = "admin@test.com",
                        NormalizedUserName = "ADMIN@TEST.COM",
                        PasswordHash = hasher.HashPassword(null, "Admin1!")
                    },
                    new ApplicationUser
                    {
                        Id = "13beaf34-24c9-45fc-a5d5-0f747a011392", // primary key
                        UserName = "editor@test.com",
                        EmailConfirmed = true,
                        NormalizedEmail = "EDITOR@TEST.COM",
                        Email = "editor@test.com",
                        NormalizedUserName = "EDITOR@TEST.COM",
                        PasswordHash = hasher.HashPassword(null, "Editor1!")
                    },
                    new ApplicationUser
                    {
                        Id = "43291cef-b112-4e5f-a4c7-367d2fbffa18", // primary key
                        UserName = "user@test.com",
                        EmailConfirmed = true,
                        NormalizedEmail = "USER@TEST.COM",
                        Email = "user@test.com",
                        NormalizedUserName = "USER@TEST.COM",
                        PasswordHash = hasher.HashPassword(null, "User1!")
                    }
                );

                // ASOCIEREA USER-ROLE
                context.UserRoles.AddRange(
                    new IdentityUserRole<string>
                    {
                        RoleId = "f2369e92-e6e9-48f0-a1e6-4e146596c283",
                        UserId = "75c8f3aa-e6f2-40d7-b865-b807e8ec6bb5"
                    },
                    new IdentityUserRole<string>
                    {
                        RoleId = "5194406e-a6ae-4570-94a8-5af47a8f0468",
                        UserId = "13beaf34-24c9-45fc-a5d5-0f747a011392"
                    },
                    new IdentityUserRole<string>
                    {
                        RoleId = "1f89f81e-95cf-4360-9834-c0214c08d357",
                        UserId = "43291cef-b112-4e5f-a4c7-367d2fbffa18"
                    }
                );

                context.SaveChanges();

            }
        }
    }
}