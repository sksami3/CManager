using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CManager.Core.Entity.Base;

namespace CManager.Core.Entity
{
    public class Course : BaseModel
    {
        public Course()
        {
            UserCourses = new List<UserCourse>();
        }
        public string Title { get; set; }
        public string  Description { get; set; }
        public string TutorName { get; set; }
        public virtual ICollection<Attachments> Lectures { get; set; }
        //the users with student type roles
        public virtual ICollection<UserCourse> UserCourses { get; set; }
    }
}
