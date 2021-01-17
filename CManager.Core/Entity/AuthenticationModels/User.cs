using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CManager.Core.Entity.AuthenticationModels
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsActive { get; set; }
        //this field is valid for both Student and Staff type users
        public virtual ICollection<UserCourse> UserCourses { get; set; }
        #region Optional
        //If account activation required
        //public Guid ActivationCode { get; set; }
        #endregion
        public virtual ICollection<Message> Messages { get; set; }
    }
}
