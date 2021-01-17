using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CManager.Core.Entity.AuthenticationModels;
using CManager.Core.Entity.Base;

namespace CManager.Core.Entity
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
