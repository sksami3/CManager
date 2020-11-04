using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UKnowledge.Web.DbContext;
using UKnowledge.Web.Models;
using UKnowledge.Web.Models.ViewModels;

namespace UKnowledge.Web.Controllers
{
    [Authorize(Roles = "Staff")]
    public class StaffController : Controller
    {
        private readonly UKnowledgeDbContext _context;
        private readonly IWebHostEnvironment _env;
        public StaffController(UKnowledgeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        // GET: StaffController
        public ActionResult Index()
        {
            List<Course> courses = _context.Courses.ToList();
            List<CourseViewModel> courseViewModels = Helper.Helper.ConvertCourseViweModelsFromCourses(courses, User.Identity.Name);
            return View(courseViewModels);
        }
        public ActionResult CoursesCreatedByCurrentUser()
        {
            List<Course> courses = _context.Courses.Where(x => x.CreatedBy == User.Identity.Name).ToList();
            List<CourseViewModel> courseViewModels = Helper.Helper.ConvertCourseViweModelsFromCourses(courses, User.Identity.Name);
            return View(courseViewModels);
        }
        public ActionResult AddCourse()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCourse(CourseViewModel courseViewModel)
        {
            if (ModelState.IsValid)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        #region Saving Course
                        Course course = new Course();
                        course.Title = courseViewModel.Title;
                        course.TutorName = courseViewModel.TutorName;
                        course.CreatedDate = DateTime.Now;
                        course.UpdatedDate = DateTime.Now;
                        course.CreatedBy = User.Identity.Name;
                        course.ModifiedBy = User.Identity.Name;
                        course.Description = courseViewModel.Description;

                        _context.Courses.Add(course);
                        _context.SaveChanges();
                        #endregion
                        if (courseViewModel.Upload != null && courseViewModel.Upload.Count() > 0)
                        {
                            #region Saving Attachments(Lectures)
                            bool isAttachmentExtensionSupported = true;
                            bool isAttachmentValidated = true;

                            #region attachment validation
                            long totalFileSize = 0;
                            foreach (var file in courseViewModel.Upload)
                            {
                                var extension = Path.GetExtension(file.FileName);
                                var allowedExtensions = new[] { ".jpg", ".png", ".PNG", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".xps", ".pdf", ".dxf", ".zip", ".rar", ".tar", ".gzip", ".txt", ".csv" };
                                totalFileSize = totalFileSize + file.Length;
                                if (!allowedExtensions.Contains(extension))
                                {
                                    isAttachmentExtensionSupported = false;
                                    ViewBag.Message = "Can't upload " + extension + " type files";
                                }
                            }
                            if (totalFileSize > 25242880)
                            {
                                isAttachmentValidated = false;
                                ViewBag.Message = "Too Large to Upload";
                            }
                            #endregion


                            if (isAttachmentExtensionSupported && isAttachmentValidated)
                            {
                                var folderName = Path.Combine("wwwroot", "Attachments");
                                var uploads = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                                bool exists = System.IO.Directory.Exists(uploads);

                                if (!exists)
                                    System.IO.Directory.CreateDirectory(uploads);

                                //var uploads = Path.Combine(_env.ContentRootPath, "wwwroot\\Attachments");
                                List<Attachments> aList = new List<Attachments>();
                                foreach (var file in courseViewModel.Upload)
                                {
                                    if (file.Length > 0)
                                    {
                                        Attachments attachments = new Attachments();
                                        string fn = file.FileName;
                                        int extentionStart = fn.IndexOf('.');
                                        string filename = fn.Insert(extentionStart, "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month + "_" + DateTime.Now.Date.ToString() + "_" + DateTime.Now.Millisecond.ToString()).Replace(" ", "_").Replace(":", "_").Replace("/", "_");
                                        var filePath = Path.Combine(uploads, filename);
                                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                                        {
                                            file.CopyTo(fileStream);
                                        }
                                        attachments.CourseId = course.Id;
                                        attachments.FileName = fn;
                                        attachments.SavedFileName = filename;
                                        attachments.CreatedBy = User.Identity.Name;
                                        attachments.ModifiedBy = User.Identity.Name;
                                        attachments.CreatedDate = DateTime.Now;
                                        attachments.UpdatedDate = DateTime.Now;

                                        _context.Add(attachments);
                                        _context.SaveChanges();
                                    }
                                }
                            }
                            #endregion
                        }
                        scope.Complete();
                        if (ViewBag.Message != null)
                        {
                            ViewBag.Message += " but course added successfully";
                        }
                        else
                            ViewBag.Message = "Course Added Successfully.";
                        return View();
                    }
                    catch (Exception e)
                    {
                        ViewBag.Message = "Error occoured while adding.";
                        scope.Dispose();
                        return View();
                    }
                }
            }
            else
            {
                ViewBag.Message = "Error occoured while adding.";
                return View();
            }

        }
        public ActionResult CourseDetails(int? courseId)
        {
            if (courseId == null)
            {
                return NotFound();
            }
            var course = _context.Courses.FirstOrDefault(m => m.Id == courseId);

            if (course == null)
                return NotFound();

            var courseViewModel = Helper.Helper.ConvertCourseViweModelFromCourse(course);
            courseViewModel.Attachments = _context.Attachments.Where(x => x.CourseId == course.Id).ToList();

            return View(courseViewModel);
        }

        public ActionResult EditCourse(int? courseId)
        {
            if (courseId == null)
            {
                return NotFound();
            }
            var course = _context.Courses.FirstOrDefault(m => m.Id == courseId);

            if (course == null)
                return NotFound();

            var courseViewModel = Helper.Helper.ConvertCourseViweModelFromCourse(course);
            courseViewModel.Attachments = _context.Attachments.Where(x => x.CourseId == course.Id).ToList();

            return View(courseViewModel);
        }
        [HttpPost]
        public ActionResult EditCourse(CourseViewModel courseViewModel)
        {
            if (ModelState.IsValid)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        #region Saving Course
                        Course course = _context.Courses.Where(x => x.Id == courseViewModel.CourseId).FirstOrDefault();
                        course.Title = courseViewModel.Title;
                        course.TutorName = courseViewModel.TutorName;
                        course.UpdatedDate = DateTime.Now;
                        course.ModifiedBy = User.Identity.Name;
                        course.Description = courseViewModel.Description;

                        _context.Courses.Update(course);
                        _context.SaveChanges();
                        #endregion
                        if (courseViewModel.Upload != null && courseViewModel.Upload.Count() > 0)
                        {
                            #region Saving Attachments(Lectures)
                            bool isAttachmentExtensionSupported = true;
                            bool isAttachmentValidated = true;

                            #region attachment validation
                            long totalFileSize = 0;
                            foreach (var file in courseViewModel.Upload)
                            {
                                var extension = Path.GetExtension(file.FileName);
                                var allowedExtensions = new[] { ".jpg", ".png", ".PNG", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".xps", ".pdf", ".dxf", ".zip", ".rar", ".tar", ".gzip", ".txt", ".csv" };
                                totalFileSize = totalFileSize + file.Length;
                                if (!allowedExtensions.Contains(extension))
                                {
                                    isAttachmentExtensionSupported = false;
                                    ViewBag.Message = "Can't upload " + extension + " type files";
                                }
                            }
                            if (totalFileSize > 25242880)
                            {
                                isAttachmentValidated = false;
                                ViewBag.Message = "Too Large to Upload";
                            }
                            #endregion


                            if (isAttachmentExtensionSupported && isAttachmentValidated)
                            {
                                var folderName = Path.Combine("wwwroot", "Attachments");
                                var uploads = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                                bool exists = System.IO.Directory.Exists(uploads);

                                if (!exists)
                                    System.IO.Directory.CreateDirectory(uploads);

                                //var uploads = Path.Combine(_env.ContentRootPath, "wwwroot\\Attachments");
                                List<Attachments> aList = new List<Attachments>();
                                foreach (var file in courseViewModel.Upload)
                                {
                                    if (file.Length > 0)
                                    {
                                        Attachments attachments = new Attachments();
                                        string fn = file.FileName;
                                        int extentionStart = fn.IndexOf('.');
                                        string filename = fn.Insert(extentionStart, "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month + "_" + DateTime.Now.Date.ToString() + "_" + DateTime.Now.Millisecond.ToString()).Replace(" ", "_").Replace(":", "_").Replace("/", "_");
                                        var filePath = Path.Combine(uploads, filename);
                                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                                        {
                                            file.CopyTo(fileStream);
                                        }
                                        attachments.CourseId = course.Id;
                                        attachments.FileName = fn;
                                        attachments.SavedFileName = filename;
                                        attachments.CreatedBy = User.Identity.Name;
                                        attachments.ModifiedBy = User.Identity.Name;
                                        attachments.CreatedDate = DateTime.Now;
                                        attachments.UpdatedDate = DateTime.Now;

                                        _context.Add(attachments);
                                        _context.SaveChanges();
                                    }
                                }
                            }
                            #endregion
                        }
                        courseViewModel.Attachments = _context.Attachments.Where(x => x.CourseId == course.Id).ToList();
                        scope.Complete();
                        if (ViewBag.Message != null)
                        {
                            ViewBag.Message += " but course edited successfully";
                        }
                        else
                            ViewBag.Message = "Course Edited Successfully.";
                        return View(courseViewModel);
                    }
                    catch (Exception e)
                    {
                        courseViewModel.Attachments = _context.Attachments.Where(x => x.CourseId == courseViewModel.CourseId).ToList();
                        ViewBag.Message = "Error occoured while adding.";
                        scope.Dispose();
                        return View(courseViewModel);
                    }
                }
            }
            else
            {
                ViewBag.Message = "Error occoured while adding.";
                return View();
            }
        }
        public ActionResult DeleteCourse(int? courseId)
        {
            if (courseId == null)
            {
                return NotFound();
            }
            var course = _context.Courses.FirstOrDefault(m => m.Id == courseId);

            if (course == null)
                return NotFound();

            var courseViewModel = Helper.Helper.ConvertCourseViweModelFromCourse(course);
            courseViewModel.Attachments = _context.Attachments.Where(x => x.CourseId == course.Id).ToList();

            return View(courseViewModel);
        }
        public ActionResult ConfirmDeleteCourse(int? courseId)
        {
            if (courseId == null)
            {
                return NotFound();
            }
            var course = _context.Courses.FirstOrDefault(m => m.Id == courseId);

            if (course == null)
                return NotFound();

            _context.Courses.Remove(course);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult DeleteAttachedFile(int? attachmentId, int? courseId)
        {
            if (attachmentId == null)
            {
                return NotFound();
            }
            var attachment = _context.Attachments.FirstOrDefault(m => m.Id == attachmentId);

            if (attachment == null)
                return NotFound();

            _context.Attachments.Remove(attachment);
            _context.SaveChanges();

            return RedirectToAction("EditCourse", "Staff", new { @courseId = courseId });
        }

        #region Attachment download
        [HttpGet]
        public async Task<IActionResult> DownloadAttachmentFile(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot\\Attachments", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            //getting last 3 digits to check if it is pdf or not
            String result = filename.Substring(filename.Length - 3);
            if (result == "pdf")
            {
                string mimeType = "application/pdf";
                return File(memory, mimeType);
            }
            else
            {
                return File(memory, GetContentType(path), Path.GetFileName(path));
            }

        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
        #endregion
    }
}
