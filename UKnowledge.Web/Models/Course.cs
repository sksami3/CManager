using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using UKnowledge.Web.Models.AuthenticationModels;
using UKnowledge.Web.Models.Base;

namespace UKnowledge.Web.Models
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
