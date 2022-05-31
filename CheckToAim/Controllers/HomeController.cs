using CheckToAim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
namespace CheckToAim.Controllers
{
    public class HomeController : Controller
    {
        CheckListDBContext context = new CheckListDBContext();
       
        
        
        public ActionResult Home(string search, int page = 1, int theme = 0, int sortby = 0)
        {
            int pageSize = 9;
            IEnumerable<CheckList> ch = new List<CheckList>();
            if (search == null)
                ch = context.CheckLists.Include("Theme").ToList();
            else
                ch = context.CheckLists.Include("Theme").Where(s => s.Name.Contains(search)).ToList();

            

            ViewBag.Lst = ch;
            IEnumerable<CheckList> ListsPerPages = ch.Skip((page - 1) * pageSize).Take(pageSize);

            if (theme != 0)
            {
                ListsPerPages = ListsPerPages.Where(p => p.Theme.Id == theme).ToList();
            }

            if (ListsPerPages.Count() == 0)
            {
                ViewBag.Message = "No CheckLists where found!";
            }

            if (sortby == 1)
            ListsPerPages = ListsPerPages.OrderByDescending(o => o.CreationDate).ToList();
               else if (sortby == 2)
                   ListsPerPages = ListsPerPages.OrderBy(o => o.Likes_this_week).ToList();
            else if (sortby == 3)
                    ListsPerPages = ListsPerPages.OrderBy(o => o.Likes_this_month).ToList();
            else if (sortby == 4)
                     ListsPerPages = ListsPerPages.OrderBy(o => o.Likes_this_year).ToList();
            else if (sortby == 5)
                    ListsPerPages = ListsPerPages.OrderBy(o => o.Likes).ToList();
             

            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = ch.Count() };
            

            List<Theme> for_view_themes = context.Themes.ToList();
            for_view_themes.Insert(0, new Theme { Id = 0, ThemeName = "Everything" });

            List<Sort> for_view_sorts = context.Sorts.ToList();


            IndexViewModel ivm = new IndexViewModel
            {
                PageInfo = pageInfo,
                CheckLists = ListsPerPages,
                FilterThemes = new SelectList(for_view_themes, "Id", "ThemeName"),
                theme_id = theme,
                SortBy = new SelectList(for_view_sorts, "Id", "Name"),
                sort_id = sortby

                
            };

            return View(ivm);

            
        }
        //only registered user will access this method
        [Authorize (Roles = "User")] //redirect to login page
        public ActionResult UserProfile(int page=1)
        {
            ViewBag.Name = context.Users.Where(a => a.Email == User.Identity.Name).FirstOrDefault().Name;
            List<CheckList> ListsPerPages = context.CheckLists.Include("Creator").Where(u => u.Creator.Email == User.Identity.Name).ToList();
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = 6, TotalItems = ListsPerPages.Count() };
            IndexViewModel ivm = new IndexViewModel
            {
                
                CheckLists = ListsPerPages,
                PageInfo = pageInfo
                


            };
            return View(ivm);
        }
        //only admin

        [Authorize (Roles = "Admin")]
        public ActionResult AdminArea(string search, int filter = 0)
        {
            List<AdminFilter> AdminFilters = new List<AdminFilter>();
            AdminFilters.Add(new AdminFilter { Id = 0, Name = "Choose..." });
            AdminFilters.Add(new AdminFilter {Id=1, Name = "Users" });
            AdminFilters.Add(new AdminFilter {Id=2, Name = "CheckToAim`s" });
            
            ViewBag.AdminFilters = AdminFilters;
            AdminViewModel avm = new AdminViewModel
            {
                Users = context.Users.ToList(),
                CheckLists = context.CheckLists.Include("Theme").Include("Creator").ToList(),
                filter_id = filter,
                Filter = new SelectList(AdminFilters, "Id", "Name")
            };
            if (filter == 1)
            {
                   avm = new AdminViewModel
                    {
                        Users = context.Users.Where(u => u.Username.Contains(search)).ToList(),
                        filter_id = filter,
                        Filter = new SelectList(AdminFilters, "Id", "Name")
                    };
                
                  


                ViewBag.Type = 1;

               

            }

            if(filter == 2)
            {
                Guid g = new Guid();
                try
                {
                    g = Guid.Parse(search);
                }
                catch
                {

                }
                    if(g != Guid.Empty)
                    avm = new AdminViewModel
                    {
                        CheckLists = context.CheckLists.Where(c => c.ID.Equals(g)).ToList(),
                        filter_id = filter,
                        Filter = new SelectList(AdminFilters, "Id", "Name")
                    };
                else
                    avm = new AdminViewModel
                {
                    CheckLists = context.CheckLists.ToList(),
                    filter_id = filter,
                    Filter = new SelectList(AdminFilters, "Id", "Name")
                };

                ViewBag.Type = 2;
            }
            


            return View(avm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

       
        //[Authorize]
        public void LikeCheckList(Guid guid, string user)
        {
            User current_user = context.Users.FirstOrDefault(a => a.Email == user); //+ перевірка чи є вже цей пост лайкнутим
            
            context.CheckLists.Where(c => c.ID == guid).FirstOrDefault().Update_Likes1();
            context.SaveChanges();

        }


        public void AddToCompleteCheckList(Guid id)
        {

            User current_user = context.Users.Include("completingCheckLists").FirstOrDefault(a => a.Email == User.Identity.Name);
            if (current_user != null)
            {


                CheckList ch = context.CheckLists.Where(c => c.ID == id).FirstOrDefault(); ;
                bool Added = false;
                foreach (var item in current_user.completingCheckLists)
                {
                    if (item.Id_ParentCheckList == id)
                        Added = true;
                }
                if (!Added)
                {

                    context.PersonalAimLists.Add(new PersonalAimList { Id_ParentCheckList = id, Name = ch.Name, User = context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault() });
                    context.SaveChanges();
                    List<Aim> aims = new List<Aim>();
                    List<string> list = ch.GetAimsList();
                    PersonalAimList l = context.PersonalAimLists.Include("User").Where(u => u.User.Email == User.Identity.Name).FirstOrDefault(a => a.Id_ParentCheckList == id);
                    foreach (var item in list)
                    {
                        aims.Add(new Aim { Text = item, IsCompleted = false, BelongsTo = l, Id_List = l.Id });
                    }
                    l.Aims = aims;
                    context.SaveChanges();



                }
            }
        }


        [HttpPost]
        public ActionResult UpdateAim(bool check, Guid aimId)
        {
           context.Aims.FirstOrDefault(a=>a.Id==aimId).IsCompleted = check;
            context.SaveChanges();
            return View();
        }

        public ActionResult CompleteCheckToAim()
        {

            AddToCompleteCheckList(context.CheckLists.FirstOrDefault().ID);
            List<PersonalAimList> list = context.PersonalAimLists.Include("Aims").Include("User").Where(p => p.User.Email == User.Identity.Name).ToList();
            
            

            return View(list);
        }

        public ActionResult CheckList(Guid id)
        {
            if(id != Guid.Empty)
            {
                CheckList checkList = context.CheckLists.Include("Creator").Include("Theme").FirstOrDefault(c => c.ID == id);
                ViewBag.CheckList = checkList;
                ViewBag.Aims_count = checkList.Aims.Count(f => f == (char)007);
                
            }
            else
            {
                ViewBag.Message = "No such CheckList!";
            }



            return View();
        }
        [HttpGet]
        public ActionResult CreateCheckList()
        {

            List<Theme> for_view_themes = context.Themes.ToList();
           

            ViewBag.Themes = new SelectList(for_view_themes, "Id", "ThemeName");
            ViewBag.Redir = false;
            return View();
        }
        [HttpPost]
        public ActionResult CreateCheckList(CheckList checkList)
        {
            List<Theme> for_view_themes = context.Themes.ToList();


            ViewBag.Themes = new SelectList(for_view_themes, "Id", "ThemeName");
            if(checkList.Name!=null)
            {
                if (checkList.Name.Length < 3)
                {
                    ModelState.AddModelError(nameof(checkList.Name), "Too short name");
                }
                else if (checkList.Aims.Count(f => f == '\n') < 4)
                {
                    ModelState.AddModelError(nameof(checkList.Aims), "Add more than 5 aims and write each from the new line!");

                }
                else if (checkList.ID_Theme == 0)
                {
                    ModelState.AddModelError(nameof(checkList.Theme.ThemeName), "Choose theme!");
                }
                else
                {
                    context.CheckLists.Add(new CheckList(checkList.Name, checkList.Description, context.Themes.Where(t => t.Id == checkList.ID_Theme).FirstOrDefault(), checkList.Aims.Split('\n').ToList(), context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault()));
                    context.SaveChanges();
                    ViewBag.Message = "CheckList created successfully! You will be redirected to your profile page!";
                    ViewBag.Redir = true;
                    Response.AddHeader("REFRESH", "5;URL=Profile");


                    RedirectToAction("Profile", "Home");
                }
            }
           
            return View();
        }
        [HttpGet]
        public ActionResult EditCheckList(Guid id)
        {
            List<Theme> for_view_themes = context.Themes.ToList();


            ViewBag.Themes = new SelectList(for_view_themes, "Id", "ThemeName");

            CheckList checkList = context.CheckLists.Where(c => c.ID == id).FirstOrDefault();
            //ViewBag.ch = checkList;

            return View(checkList);
        }
        [HttpPost]
        public ActionResult EditCheckList(CheckList checkList)
        {
            List<Theme> for_view_themes = context.Themes.ToList();
            ViewBag.Themes = new SelectList(for_view_themes, "Id", "ThemeName");

            CheckList ch = context.CheckLists.Where(c => c.ID == checkList.ID).FirstOrDefault();
            context.CheckLists.Remove(ch);
            context.SaveChanges();
            ch.Name = checkList.Name;
            ch.Description = checkList.Description;
            ch.Aims = checkList.Aims;
            ch.Theme = context.Themes.Where(t => t.Id == checkList.ID_Theme).FirstOrDefault();
            
            context.CheckLists.Add(ch);
            context.SaveChanges();

            return View();
        }

        [HttpGet]
        public ActionResult EditUser(string id)
        {
            List<UserRole> for_view_roles = context.UserRoles.ToList();


            ViewBag.Roles = new SelectList(for_view_roles, "RoleId", "RoleName");

            User u = context.Users.Where(c => c.Username == id).FirstOrDefault();
            //ViewBag.ch = checkList;

            return View(u);
        }
        [HttpPost]
        public ActionResult EditUser(User user)
        {
            List<UserRole> for_view_roles = context.UserRoles.ToList();
            ViewBag.Roles = new SelectList(for_view_roles, "RoleId", "RoleName");

            User u = context.Users.Where(c => c.Username == user.Username).FirstOrDefault();
            context.Users.Remove(u);
            context.SaveChanges();
            u.Name = u.Name;
            u.Username = user.Username;
            u.Password = user.Password;
            u.Email = user.Email;
            u.RoleId = user.RoleId;
            u.Role = context.UserRoles.Where(us=>us.RoleId == user.RoleId).FirstOrDefault();
            
            context.Users.Add(u);
            context.SaveChanges();

            return View();
        }

        [HttpGet]
        public ActionResult DeleteCheckList(Guid? id)
        {
            CheckList checkList = context.CheckLists.Include("Creator").Include("Theme").Where(c => c.ID == id).FirstOrDefault();

            return View(checkList);
        }
        [HttpPost]
        public ActionResult DeleteCheckList(Guid id)
        {
            context.CheckLists.Remove(context.CheckLists.Where(c => c.ID == id).FirstOrDefault());
            context.SaveChanges();
            ViewBag.Message = "Deleted successfully!";
            return View();
        }

        public ActionResult Delete_Completing(Guid id)
        {
            context.PersonalAimLists.Remove(context.PersonalAimLists.Where(p => p.Id_ParentCheckList == id).FirstOrDefault());

            return View();
        }


    }
}