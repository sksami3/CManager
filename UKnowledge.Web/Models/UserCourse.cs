using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using UKnowledge.Web.Models.AuthenticationModels;

namespace UKnowledge.Web.Models
{
    public class UserCourse
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("Course")]
        public double CourseId { get; set; }
        public Course Course { get; set; }
    }
}
