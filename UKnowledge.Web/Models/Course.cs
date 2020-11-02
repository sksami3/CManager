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
        public string Title { get; set; }
        public string  Description { get; set; }
        [ForeignKey("Tutor")]
        public double TutorId { get; set; }
        public Tutor Tutor { get; set; }
        public ICollection<Attachments> Lectures { get; set; }
        //the users with student type roles
        public ICollection<UserCourse> UserCourses { get; set; }
    }
}
