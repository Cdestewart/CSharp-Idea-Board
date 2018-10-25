using Microsoft.EntityFrameworkCore;
 
namespace BeltExam.Models
{
    public class beltexamContext : DbContext
    {
        
        public beltexamContext(DbContextOptions<beltexamContext> options) : base(options) { }
            public DbSet<User> users {get;set;}   
            public DbSet<Liker> likers {get;set;}            
            public DbSet<Idea> ideas {get;set;}            

        
    }

}
