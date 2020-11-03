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
        public string SavedFileName { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
