﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using UKnowledge.Web.Models.AuthenticationModels;
using UKnowledge.Web.Models.Base;

namespace UKnowledge.Web.Models
{
    public class Favorite : BaseModel
    {
        [ForeignKey("Course")]
        public double CourseId { get; set; }
        public Course Course { get; set; }
        //User with student Role
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

    }
}
