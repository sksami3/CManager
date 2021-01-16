using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UKnowledge.Core.Entity;

namespace UKnowledge.Web.Models.ViewModels
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        [MinLength(100, ErrorMessage = "Description can't be less than 100 words")]
        public string Description { get; set; }
        [Required]
        [DisplayName("Tutor Name")]
        public string TutorName { get; set; }
        //this is for upload lectures
        [DisplayName("Upload Lectures")]
        public List<IFormFile> Upload { get; set; }
        public List<Attachments> Attachments { get; set; }
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }
        public bool IsCreatedByCurrentUser { get; set; }
        public bool IsSavedCourse { get; set; }
    }
}
