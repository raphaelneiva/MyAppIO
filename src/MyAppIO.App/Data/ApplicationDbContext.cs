using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyAppIO.App.ViewModels;

namespace MyAppIO.App.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MyAppIO.App.ViewModels.ProdutoViewModel> ProdutoViewModel { get; set; }
        public DbSet<MyAppIO.App.ViewModels.EnderecoViewModel> EnderecoViewModel { get; set; }
    }
}