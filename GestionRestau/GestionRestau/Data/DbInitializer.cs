using BrasilBurger.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BrasilBurger.Web.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            if (await context.Clients.AnyAsync())
            {
                return;
            }

            var clients = new[]
            {
                new Client
                {
                    Nom = "Ndiaye",
                    Prenom = "Fatou",
                    Telephone = "770000001",
                    Email = "fatou.ndiaye@gmail.com",
                    Password = "fatou123"
                },
                new Client
                {
                    Nom = "Diop",
                    Prenom = "Moussa",
                    Telephone = "770000002",
                    Email = "moussa.diop@gmail.com",
                    Password = "moussa123"
                },
                new Client
                {
                    Nom = "Sarr",
                    Prenom = "Awa",
                    Telephone = "770000003",
                    Email = "awa.sarr@gmail.com",
                    Password = "awa123"
                },
                new Client
                {
                    Nom = "Fall",
                    Prenom = "Cheikh",
                    Telephone = "770000004",
                    Email = "cheikh.fall@gmail.com",
                    Password = "cheikh123"
                },
                new Client
                {
                    Nom = "Ba",
                    Prenom = "Khady",
                    Telephone = "770000005",
                    Email = "khady.ba@gmail.com",
                    Password = "khady123"
                }
            };

            context.Clients.AddRange(clients);
            await context.SaveChangesAsync();
        }
    }
}
