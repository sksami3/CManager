using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NToastNotify;
using UKnowledge.Core.Entity;
using UKnowledge.Core.Entity.AuthenticationModels;
using UKnowledge.Core.Interfaces.Services;
using UKnowledge.Web.DbContext;
using UKnowledge.Web.Models;
using UKnowledge.Web.Models.ViewModels;
using UKnowledge.Web.Socket;

namespace UKnowledge.Web.Controllers
{
    [Authorize(Roles = "Staff")]
    public class StaffController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IAttachmentsService _attachmentsService;
        private readonly IWebHostEnvironment _env;
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private UserManager<User> _userManager { get; }
        public StaffController(ICourseService courseService, IWebHostEnvironment env,
            IAttachmentsService attachmentsService,
            IHubContext<NotificationHub> notificationHubContext,
            UserManager<User> userManager)
        {
            _courseService = courseService;
            _attachmentsService = attachmentsService;
            _env = env;
            _notificationHubContext = notificationHubContext;
            _userManager = userManager;
        }
        // GET: StaffController
        public async Task<ActionResult> Index()
        {
            var courses = await _courseService.GetCourses();
            List<CourseViewModel> courseViewModels = Helper.Helper.ConvertCourseViweModelsFromCourses(courses.ToList(), User.Identity.Name);
            return View(courseViewModels);
        }
        public async Task<ActionResult> CoursesCreatedByCurrentUser()
        {
            List<Course> courses = await _courseService.GetCoursesCreatedByCurrentUser(User);//.Where(x => x.CreatedBy == User.Identity.Name).ToList();
            List<CourseViewModel> courseViewModels = Helper.Helper.ConvertCourseViweModelsFromCourses(courses, User.Identity.Name);
            return View(courseViewModels);
        }
        public async Task<ActionResult> AddCourse()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddCourse(CourseViewModel courseViewModel)
        {
            if (ModelState.IsValid)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        #region Saving Course
                        Course course = Helper.Helper.ConvertCourseFromCourseViewModel(courseViewModel);
                        course.CreatedBy = User.Identity.Name;
                        await _courseService.Add(course);
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

                                        await _attachmentsService.Add(attachments);
                                    }
                                }
                            }
                            #endregion
                        }
                        scope.Complete();
                        if (ViewBag.Message != null)
                        {
                            await _notificationHubContext.Clients.User("ff1").SendAsync("add", User.Identity.Name + "Created New Course");
                            ViewBag.Message += " but course added successfully";
                        }
                        else
                        {
                            await _notificationHubContext.Clients.User("ff1").SendAsync("add", User.Identity.Name + " has Created New Course!!!");
                            ViewBag.Message = "Course Added Successfully.";
                        }

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
        public async Task<ActionResult> CourseDetails(int courseId)
        {
            var course = await _courseService.GetCourseById(courseId);

            if (course == null)
                return NotFound();

            var courseViewModel = Helper.Helper.ConvertCourseViweModelFromCourse(course);
            courseViewModel.Attachments = await _attachmentsService.GetAttachmentsByCourseId(course.Id);//Where(x => x.CourseId == course.Id).ToList();

            return View(courseViewModel);
        }

        public async Task<ActionResult> EditCourse(int courseId)
        {
            var course = await _courseService.GetCourseById(courseId);

            if (course == null)
                return NotFound();

            var courseViewModel = Helper.Helper.ConvertCourseViweModelFromCourse(course);
            courseViewModel.Attachments = await _attachmentsService.GetAttachmentsByCourseId(courseId);

            return View(courseViewModel);
        }
        [HttpPost]
        public async Task<ActionResult> EditCourse(CourseViewModel courseViewModel)
        {
            if (ModelState.IsValid)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        #region Saving Course
                        Course course = await _courseService.GetCourseById(courseViewModel.CourseId);

                        await _courseService.Update(course);
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

                                        await _attachmentsService.Add(attachments);
                                    }
                                }
                            }
                            #endregion
                        }
                        courseViewModel.Attachments = await _attachmentsService.GetAttachmentsByCourseId(course.Id);
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
                        courseViewModel.Attachments = await _attachmentsService.GetAttachmentsByCourseId(courseViewModel.CourseId);
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
        public async Task<ActionResult> DeleteCourse(int courseId)
        {
            var course = await _courseService.GetCourseById(courseId);

            if (course == null)
                return NotFound();

            var courseViewModel = Helper.Helper.ConvertCourseViweModelFromCourse(course);
            courseViewModel.Attachments = await _attachmentsService.GetAttachmentsByCourseId(course.Id);

            return View(courseViewModel);
        }
        public async Task<ActionResult> ConfirmDeleteCourse(int courseId)
        {
            await _courseService.Remove(courseId);

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> DeleteAttachedFile(int attachmentId, int courseId)
        {
            var attachment = await _attachmentsService.GetAttachmentsById(attachmentId);

            if (attachment == null)
                return NotFound();

            await _courseService.Remove(attachmentId);
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
