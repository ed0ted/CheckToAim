using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CheckToAim.Models
{
    public class Theme
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        
        public string ThemeName { get; set; }
        List<CheckList> CheckLists { get; set; }


    }
    public class Sort
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        List<CheckList> CheckLists { get; set; }
    }

    public class AdminFilter
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

    }
    public class Aim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool IsCompleted { get; set; }
        public string Text { get; set; }
        public Guid Id_List { get; set; }
        public PersonalAimList BelongsTo { get; set; } //

    }
    public class PersonalAimList // - список цілей (=CheckList), який буде використовуватися для розділу виконання у певного користувача
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid Id_ParentCheckList { get; set; }
        public string Name { get; set; }
        public List<Aim> Aims { get; set; }


        public User User { get; set; }

    }

}