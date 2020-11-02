using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using UKnowledge.Web.Models.Base;

namespace UKnowledge.Web.Models
{
    public class Attachments : BaseModel
    {
        public string FileName { get; set; }
        [ForeignKey("Course")]
        public double CourseId { get; set; }
        public Course Course { get; set; }
    }
}
