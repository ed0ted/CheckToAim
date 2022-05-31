using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data.Entity;

namespace CheckToAim.Models
{
    public class CheckListDBContext : DbContext
    {
        public DbSet<CheckList> CheckLists { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Sort> Sorts { get; set; }
        public DbSet<Aim> Aims { get; set; }
        public DbSet<PersonalAimList> PersonalAimLists { get; set; }
        //public DbSet<Filter> AdminFilters { get; set; }
        public CheckListDBContext() : base(@"")//Add your DB connection string here
        {

        }
        static CheckListDBContext()
        {
            Database.SetInitializer(new AimCheckDatabaseInitial());
        }
    }

    public class AimCheckDatabaseInitial : DropCreateDatabaseIfModelChanges<CheckListDBContext>
    {
        protected override void Seed(CheckListDBContext context)
        {
            context.Themes.Add(new Theme { ThemeName = "Art" });
            context.Themes.Add(new Theme { ThemeName = "Adventure" });
            context.Themes.Add(new Theme { ThemeName = "Travel" });
            context.Themes.Add(new Theme { ThemeName = "Reading" });
            context.Themes.Add(new Theme { ThemeName = "Sport" });
            context.Themes.Add(new Theme { ThemeName = "Hobby" });
            context.Themes.Add(new Theme { ThemeName = "Other" });

            context.Sorts.Add(new Sort { Name = "New" });
            context.Sorts.Add(new Sort { Name = "Popular 7 days" });
            context.Sorts.Add(new Sort { Name = "Popular this month" });
            context.Sorts.Add(new Sort { Name = "Popular this year" });
            context.Sorts.Add(new Sort { Name = "Popular all time" });


            UserRole role1 = new UserRole();

            role1.RoleName = "Admin";
            UserRole role2 = new UserRole();

            role2.RoleName = "User";

            context.UserRoles.Add(role1);
            context.UserRoles.Add(role2);
            context.SaveChanges();
            User u1 = new User { Name = "Test", Email = "test@test.com", Username = "test0test", Password = "12345678", RoleId = 1 };

            context.Users.Add(u1);
            u1 = new User { Name = "Ed", Email = "ed7777ed77@gmail.com",Username = "ed0ted",Password = "12345678"};
            context.Users.Add(u1);
            context.SaveChanges();

            CheckList c1 = new CheckList( "Body development", "Your aim for a day to develop your muscles and endurance!", context.Themes.Where(t=>t.ThemeName=="Sport").FirstOrDefault(),
                new List<string> { "10000 steps", "100 push-ups", "50 pull-ups", "100 squats", "50 burpees" },context.Users.Find("test0test"));
            CheckList c2 = new CheckList( "Create a portret", "Develop your creative thinking and drawing skills!", context.Themes.Where(t => t.ThemeName == "Art").FirstOrDefault(),
               new List<string> { "Draw eyes", "Draw nose and lips", "Draw face", "Draw hair", "Draw neck" }, context.Users.Find("test0test"));
            CheckList c3 = new CheckList( "Productivity day", "Plan for a day to complete all your tasks",context.Themes.Where(t => t.ThemeName == "Other").FirstOrDefault(),
               new List<string> { "Make your bed", "Wash yourself", "Breakfast", "Do gymnastics", "Learn/Work", "Don`t forget to drink water!", "Lunch", "Sport or a stroll", "Dinner", "Read a book", "Supper","Prepare for night" }, context.Users.Find("test0test"));
            CheckList c4 = new CheckList("10 best books to read your lifetime", "By harpersbazaar.com", context.Themes.Where(t => t.ThemeName == "Reading").FirstOrDefault(),
               new List<string> { "To Kill a Mockingbird by Harper Lee", "The Catcher in the Rye by JD Salinger", "Great Expectations by Charles Dickens",
                   "Lord of the Flies by William Golding", "The Handmaid's Tale by Margaret Atwood", "Don`t forget to drink water!", "The Scarlet Letter by Nathaniel Hawthorne",
                   "Lolita by Vladimir Nabokov", "Wuthering Heights by Emily Brontë",
                   "Lady Chatterley's Lover by DH Lawrence", "The Great Gatsby by F Scott Fitzgerald"}, context.Users.Find("test0test"));
            context.CheckLists.AddRange(new List<CheckList> { c1, c2, c3,c4 });
            


           

            context.SaveChanges();
        }
    }
}