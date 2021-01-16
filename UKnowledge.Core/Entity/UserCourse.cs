using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using UKnowledge.Core.Entity.AuthenticationModels;
using UKnowledge.Core.Entity.Base;

namespace UKnowledge.Core.Entity
{
    public class UserCourse : BaseModel
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
