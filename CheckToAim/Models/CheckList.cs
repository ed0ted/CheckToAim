using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CheckToAim.Models.CheckListDBContext;
namespace CheckToAim.Models
{
    public class CheckList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Name must be more that 3 symbols")]
        [DataType(DataType.Text)]
        [StringLength(256, MinimumLength = 3)]
        public string Name { get; set; }
        public string Description { get; set; }


        [Required(ErrorMessage = "Write at least 5 aims and each one from the new line!")]
        [DataType(DataType.MultilineText)]
        [StringLength(1024, MinimumLength = 5)]
        public string Aims { get; set; }
       
        public User Creator { get; set; }
        public DateTime CreationDate { get; set; }

        public int ID_Theme { get; set; }
        [Required(ErrorMessage = "Choose theme")]
        public Theme Theme { get; set; }

        public int Likes { get; set; }
        public int Likes_this_week { get; set; }
        public int Likes_this_month { get; set; }
        public int Likes_this_year { get; set; }

        public CheckList()
        {

        }
        //public CheckList(string n, string desc, Theme t, string ch, User cre)
        //{


        //    Name = n;
        //    Description = desc;


        //    Aims = ch.Replace('\n',(char)007);
        //    ID_Theme = t.Id;
        //    Theme = t;
        //    Creator = cre;
        //    CreationDate = DateTime.Now;

        //    Likes = new Random(DateTime.Now.Millisecond).Next(101);
        //    Likes_this_month = 0;
        //    Likes_this_week = 0;
        //    Likes_this_year = 0;
        //}
        public CheckList(string n, string desc, Theme t, List<string> ch, User cre)
        {

            
            Name = n;
            Description = desc;


            foreach (string s in ch)/////
            {
                Aims += s + ((char)007);
            }
            ID_Theme = t.Id;
            Theme = t;
            Creator = cre;
            CreationDate = DateTime.Now;

            Likes = new Random(DateTime.Now.Millisecond).Next(101);
            Likes_this_month = 0;
            Likes_this_week = 0;
            Likes_this_year = 0;
        }
        
        public void Update_Name(string n)
        {
            Name = n;
        }
        public void Update_Descr(string d)
        {
            Description = d;
        }

        public void Update_Likes1()
        {
            Likes += 1;
            Likes_this_month++;
            Likes_this_week++;
            Likes_this_year++;
        }
        public List<string> GetAimsList()
        {
            List<string> list = Aims.Split((char)007).ToList();
            list.RemoveAt(list.Count - 1);
            return list;
        }
        public void SetAimsList(List<string> ch)
        {
            Aims = null;
            foreach (string s in ch)
            {
                Aims += s + ((char)007);
            }
        }


    }




    public class PageInfo
    {
        public int PageNumber { get; set; } // номер текущей страницы
        public int PageSize { get; set; } // кол-во объектов на странице
        public int TotalItems { get; set; } // всего объектов
        public int TotalPages  // всего страниц
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }
    public class IndexViewModel
    {
        public SelectList FilterThemes { get; set; }
        public SelectList SortBy { get; set; }
        public int theme_id { get; set; }
        public int sort_id { get; set; }
        public IEnumerable<CheckList> CheckLists { get; set; }
        public PageInfo PageInfo { get; set; }
    }

    public class AdminViewModel
    {
        public IEnumerable<User> Users { get; set; }       
        public IEnumerable<CheckList> CheckLists { get; set; }
        public SelectList Filter { get; set; }
        public int filter_id { get; set; }
       
    }
}