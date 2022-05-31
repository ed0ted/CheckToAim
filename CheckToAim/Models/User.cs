using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CheckToAim.Models
{
    public class User
    {
        [Key]
        [Required(ErrorMessage = "Username must be more than 3 symbols but less that 16")]
        [StringLength(16, MinimumLength = 3)]
        public string Username { get; set; }


        [Required(ErrorMessage = "Name must be more that 2 symbols")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        
        [Required(ErrorMessage = "Enter your real email adress")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password must be at least 8 symbols")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        
        [DefaultValue (2)]
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        
        public UserRole Role { get; set; }

        public List<Guid> savedCheckLists { get; set; }
        public List<PersonalAimList> completingCheckLists { get; set; }

        public User()
        {
            
            RoleId = 2;
            
        }
    }
}