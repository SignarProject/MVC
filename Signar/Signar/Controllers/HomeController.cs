﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AsignarBusinessLayer.AsignarDatabaseDTOs;
using AsignarBusinessLayer.Services;
using AsignarBusinessLayer.Services.ServiceInterfaces;
using CustomAuth.Filters;
using System.Web.Security;
using Signar.Models;
using System.Net.Http;
using System.Web.Http;
using System.Net;


namespace Signar.Controllers
{

    [CustomAuthenticate]
    public class HomeController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewUser(CreateNewUserModel model)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            if (!ModelState.IsValid) return new HttpStatusCodeResult(1, "Input data is invalid");
            if (!model.Password.Equals(model.ConfPassword)) return new HttpStatusCodeResult(4, "Passwords do not match");
            UserDTO user = new UserDTO();
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Login = model.Email;
            user.Email = model.Email;
            user.Password = model.Password;
            user.IsAdmin = model.IsAdmin;
            try
            {
                using (var userService = new UserService())
                {
                    if (!userService.CreateItem(user)) return new HttpStatusCodeResult(5, "This Email already exists. Try another");

                    var notificationQueue = new NotificationQueueService();
                    notificationQueue.UserRegistration(user, new List<string>());
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(6, "Error during creation");
            }
            return new HttpStatusCodeResult(200, "OK");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserData(EditUserDataModel model)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            UserDTO user;
            using (UserService userService = new UserService())
            {
                user = userService.GetItem(model.UserID);
            }
            if (user.Name.Equals(model.Name) && user.Surname.Equals(model.Surname) && user.Email.Equals(model.Email)
                && user.IsAdmin == model.IsAdmin) return new HttpStatusCodeResult(3, "Nothing to update");
            if (ModelState.IsValid && user != null)
            {

                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Email = model.Email;
                user.IsAdmin = model.IsAdmin;
                using (var userService = new UserService())
                {
                    userService.UpdateItem(user);
                }
            }
            else
            {
                ModelState.AddModelError("", "Sorry, but there was an error");
                return new HttpStatusCodeResult(1, "Input data is invalid");
            }
            return RedirectToAction("TheProfile/" + model.UserID);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(1, "Input data is invalid");
            }
            if (model.OldPassword.Equals(model.NewPassword)) return new HttpStatusCodeResult(3, "Nothing to update");
            bool fail = false;
            using (UserService userService = new UserService())
            {
                UserDTO user = userService.GetItem(model.UserID);
                if (user == null) fail = true;
                else
                {
                    if (!userService.UpdatePassword(model.OldPassword, model.NewPassword, user.UserID)) fail = true;
                }
            }
            if (fail)
            {
                return new HttpStatusCodeResult(2, "Old password is incorrect");
            }
            return RedirectToAction("TheProfile/" + model.UserID);
        }

        public ActionResult DashBoard()
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null){Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account");}

            UserDTO user;
            using (UserService userService = new UserService())
            {
                user = userService.GetItem(Me.UserID);
            }
            HttpContext.Cache[User.Identity.Name] = user;
            return View();
        }

        public ActionResult TheProfile(int id)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null){Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account");}

            UserDTO user;
            if (id == 0)
            {
                int MyID = Me.UserID;
                using (UserService userService = new UserService())
                {
                    user = userService.GetItem(MyID);
                }
                if (user == null) return RedirectToAction("NotFound", "Error");
                return View(user);
            }
            if (!Me.IsAdmin)
            {
                return PartialView("~/Views/Error/NotFound.cshtml");
            }
            using (UserService userService = new UserService())
            {
                user = userService.GetItem(id);
            }
            if (user == null) return RedirectToAction("NotFound", "Error");
            return View(user);
        }

        [HttpGet]
        public ActionResult GetProject(int id)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null){Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account");}

            ProjectDTO project;
            using (ProjectService projectService = new ProjectService())
            {
                project = projectService.GetItem(id);
            }
            if (project == null) return RedirectToAction("NotFound", "Error");
            return View(project);
        }

        [HttpGet]
        public ActionResult GetComments(int BugID)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            BugDTO bug;
            using (BugService bugService = new BugService())
            {
                bug = bugService.GetItem(BugID);
            }
            if (bug== null) return RedirectToAction("NotFound", "Error");
            return PartialView("~/Views/Home/CommentsPartial.cshtml", bug.Comments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewProject(ProjectDTO model)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null){Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account");}

            if (model.Name != null && model.Name.Length > 0 && model.Prefix.Length > 10)
            {
                return new HttpStatusCodeResult(7, "Prefix length must be less than 10");
            }
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(1, "Input data is invalid");
            }
            bool res = false;
            using (ProjectService projectService = new ProjectService())
            {
                res = projectService.CreateItem(model);
            }
            if (!res)
            {
                return new HttpStatusCodeResult(8, "This prefix already exists in database, please try again");
            }
            return new HttpStatusCodeResult(200, "OK");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTask(BugDTO model)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            bool f = false;
            foreach (var err in ModelState)
            {
                if (err.Key != "Project" && err.Value.Errors.Count > 0) f = true;
            }
            //if (ModelState["Project"].Errors.Count == 1) 
            if (f)
            {
                return new HttpStatusCodeResult(1, "Input data is invalid");
            }
            bool res;
            using (BugService bugService = new BugService())
            {
                res = bugService.UpdateItem(model);
            }
            if (!res)
            {
                return new HttpStatusCodeResult(8, "Error during creation. Please try again");
            }
            return new HttpStatusCodeResult(200, "OK");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewTask(BugDTO model)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null){Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account");}

            bool f = false;
            foreach(var err in ModelState)
            {
                if (err.Key != "Project" && err.Value.Errors.Count > 0) f = true;
            }
            //if (ModelState["Project"].Errors.Count == 1) 
            if (f)
            {
                return new HttpStatusCodeResult(1, "Input data is invalid");
            }
            bool res = false;
            using (BugService bugService = new BugService())
            {
                res = bugService.CreateItem(model);
            }
            if (!res)
            {
                return new HttpStatusCodeResult(8, "Error during creation. Please try again");
            }
            return new HttpStatusCodeResult(200, "OK");
        }

        [HttpPost]
        public ActionResult CopyTask(BugDTO model)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            int res;
            using (BugService bugService = new BugService())
            {
                res = bugService.CopyTask(model);
            }
            if (res == 0)
            {
                return new HttpStatusCodeResult(8, "Error during creation. Please try again");
            }
            return Content(res.ToString());
        }

        public ActionResult DeleteUserFromProject(int ProjectID, int UserID)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null){Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account");}

            bool res = false;
            using (UserService userService = new UserService())
            {
                res = userService.DropProjectFromUser(UserID, ProjectID);
            }
            if (!res) return new HttpStatusCodeResult(11, "Sorry, there was an error. Please, try again");
            return new HttpStatusCodeResult(200, "OK");
        }

        public ActionResult DeleteProject(int ProjectID)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null){Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account");}

            using (var projectService = new ProjectService())
            {
                bool res = projectService.DeleteItem(ProjectID);
                if (!res) return new HttpStatusCodeResult(1, "ProjectIsAlreadyDeleted!");
                return new HttpStatusCodeResult(200, "OK");
            }
        }

        public ActionResult DeleteTask(int BugID)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null){Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account");}

            using (var bugService = new BugService())
            {
                bool res = bugService.DeleteItem(BugID);
                if (!res) return new HttpStatusCodeResult(1, "Bug is already deleted!");
                return new HttpStatusCodeResult(200, "OK");
            }
        }

        public ActionResult DeleteUser(int UserID)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null){Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account");}

            using (var userService = new UserService())
            {
                UserDTO user = userService.GetItem(UserID);
                if (user.Bugs.Count > 0) return new HttpStatusCodeResult(9, "You can not delete this user, because some tasks are assigned to him!");
                bool res = userService.DeleteItem(UserID);
                if (res) return new HttpStatusCodeResult(200, "OK");
                else
                    return new HttpStatusCodeResult(10, "There was an error during deleting");
            }
        }


        public ActionResult Projects(int id)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null){Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account");}
            if ((!Me.IsAdmin && id != Me.UserID) || id < 0)
            {
                return RedirectToAction("NotFound", "Error");
            }
            using (var projectService = new ProjectService())
            {
                ICollection<ProjectDTO> ProjectsCollection;
                if (id == 0) ProjectsCollection = projectService.GetAllItems();
                else ProjectsCollection = projectService.GetAllProjectsByUserId(id);
                if (ProjectsCollection == null) return RedirectToAction("NotFound", "Error");
                return View(ProjectsCollection);
            }
        }

        public ActionResult Project(int id)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null){Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account");}

            ProjectDTO project;
            using (var projectService = new ProjectService())
            {
                project = projectService.GetItem(id);
            }
            bool f = false;
            foreach (var project1 in Me.Projects)
            {
                if (project1.ProjectID == id) f = true;
            }
            if ((!Me.IsAdmin && !f) || id < 0 || project == null || (project.IsDeleted == true && !Me.IsAdmin))
            {
                return RedirectToAction("NotFound", "Error");
            }
            //using (UserService userService = new UserService())
            //{
            //    foreach(BugDTO bug in project.Bugs)
            //    {
            //        bug.User = bug.AssigneeID == null ? null : userService.GetItem((int)bug.AssigneeID);
            //    }
            //}

            return View(project);
        }

        [HttpGet]
        public ActionResult AddUsersToProject(int ProjectID)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            ICollection<UserDTO> users;
            using (UserService userService = new UserService())
            {
                users = userService.GetAllItems();
                using (ProjectService projectService = new ProjectService())
                {
                    ProjectDTO project = projectService.GetItem(ProjectID);
                    Dictionary<int, UserDTO> UsersOnProject = new Dictionary<int, UserDTO>();
                    if (project.Users != null)
                    {
                        foreach (UserDTO userI in project.Users)
                        {
                            if (!UsersOnProject.ContainsKey(userI.UserID))
                            {
                                UsersOnProject.Add(userI.UserID, userI);
                            }
                        }
                    }
                    ICollection<UserDTO> usersResult = new List<UserDTO>();
                    foreach (UserDTO user in users)
                    {
                        if (!UsersOnProject.ContainsKey(user.UserID)) usersResult.Add(user);
                    }
                    AddUsersToProjectModel res = new AddUsersToProjectModel();
                    res.users = usersResult;
                    res.user_checked = new List<bool>();
                    for (int i = 0; i < usersResult.Count; ++i) res.user_checked.Add(false);
                    res.ProjectID = ProjectID;
                    return PartialView("~/Views/Popup/AddUsersToProject.cshtml", res);
                }
            }
        }

        [HttpGet]
        public ActionResult GetAllProjects()
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            using (ProjectService projectService = new ProjectService())
            {
                if (Me.IsAdmin)
                {
                    return PartialView("~/Views/Popup/ProjectsDropdown.cshtml", projectService.GetAllItems());
                }
                else
                {
                    return PartialView("~/Views/Popup/ProjectsDropdown.cshtml", Me.Projects);
                }
            }
        }

        [HttpGet]
        public ActionResult GetAllProjectsEdit()
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            using (ProjectService projectService = new ProjectService())
            {
                if (Me.IsAdmin)
                {
                    return PartialView("~/Views/Popup/ProjectsDropdownEdit.cshtml", projectService.GetAllItems());
                }
                else
                {
                    return PartialView("~/Views/Popup/ProjectsDropdownEdit.cshtml", Me.Projects);
                }
            }
        }

        [HttpGet]
        public ActionResult GetTaskInfo(int BugID)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            BugDTO bug;
            using (BugService bugService = new BugService())
            {
                bug = bugService.GetItem(BugID);
            }
            return PartialView("~/Views/Popup/EditTask.cshtml", bug);
        }

        [HttpGet]
        public ActionResult GetTaskStatusInfo(int BugID)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            int bugStatus;
            using (BugService bugService = new BugService())
            {
                bugStatus = bugService.GetBugStatus(BugID);
            }
            return Content(bugStatus.ToString());
        }

        [HttpPost]
        public ActionResult SetTaskStatus(int Status, int BugID)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            bool f;
            using (BugService bugService = new BugService())
            {
                f = bugService.SetStatus(BugID, Status);
            }
            if (!f) return new HttpStatusCodeResult(1, "Error. Please try again");
            return new HttpStatusCodeResult(200, "OK");
        }

        [HttpPost]
        public ActionResult SetTaskAssignee(int UserID, int BugID)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            BugDTO bug;
            using (BugService bugService = new BugService())
            {
                bug = bugService.GetItem(BugID);
                if (UserID == bug.AssigneeID) return new HttpStatusCodeResult(1, "Bug is already assigned to you!");
                bugService.SetAssignee(BugID, UserID);
            }
            return PartialView("~/Views/Home/PartialAssignee.cshtml", Me);
        }

        [HttpGet]
        public ActionResult GetUsersOnProject(int ProjectID)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            using (ProjectService projectService = new ProjectService())
            {
                return PartialView("~/Views/Popup/UsersOnProjectsDropdown.cshtml", projectService.GetItem(ProjectID).Users);
            }
        }

        [HttpGet]
        public ActionResult GetUsersOnProjectEdit(int ProjectID)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            using (ProjectService projectService = new ProjectService())
            {
                return PartialView("~/Views/Popup/UsersOnProjectsDropdownEdit.cshtml", projectService.GetItem(ProjectID).Users);
            }
        }

        [HttpPost]
        public ActionResult AddUsersToProject(AddUsersToProjectModel model)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            using (ProjectService projectService = new ProjectService())
            {
                bool f = false;
                for (int k = 0; k < model.user_checked.Count; ++k)
                {
                    if (model.user_checked[k])
                    {
                        projectService.AddUserToProject(model.users_id[k], model.ProjectID);
                        f = true;
                    }
                }
                if (!f)
                {
                    return new HttpStatusCodeResult(1, "Nothing to change!");
                }
                return new HttpStatusCodeResult(200, "OK");
            }
        }

        [HttpPost]
        public ActionResult EditProject(EditProjectModel model)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(1, "Input data is invalid");
            }
            using (ProjectService projectService = new ProjectService())
            {
                ProjectDTO project = projectService.GetItem(model.ProjectID);
                project.Name = model.Title;
                projectService.UpdateItem(project);
                return Content(project.Name);
            }

        }

        [HttpPost]
        public ActionResult ReviveProject(int ProjectID)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            using (ProjectService projectService = new ProjectService())
            {
                if (!projectService.ReviveProject(ProjectID)) return new HttpStatusCodeResult(1, "Project is alive already");
            }
            return new HttpStatusCodeResult(200, "OK");
        }

        public ActionResult Task(int id)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            BugDTO bug;
            using (var bugService = new BugService())
            {
                bug = bugService.GetItem(id);
            }
            //bool f = false;
            //foreach (var bug1 in Me.Bugs)
            //{
            //    if (bug1.BugID == id) f = true;
            //}
            if (/*(!Me.IsAdmin && !f) || */id < 0 || bug == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            return View(bug);
        }

        public ActionResult Filters()
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            return View();
        }

        public ActionResult Users()
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            using (var userService = new UserService())
            {
                return View(userService.GetAllItems());
            }
        }

        [HttpGet]
        public ActionResult GetAssignee(int UserID)
        {
            UserDTO Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }
            using (UserService userService = new UserService())
            {
                HttpContext.Cache[User.Identity.Name] = userService.GetItem(Me.UserID);
            }
            Me = HttpContext.Cache[User.Identity.Name] as UserDTO;
            if (Me == null) { Response.Cookies["auth"].Expires = DateTime.Now; Session.Abandon(); return RedirectToAction("Login", "Account"); }

            UserDTO user;
            using (UserService userService = new UserService())
            {
                user = userService.GetItem(UserID);
            }
                return PartialView("~/Views/Home/PartialAssignee.cshtml", user);
        }

        [HttpGet]
        public ActionResult GetAvatarContent(int UserID)
        {
            string res;;
            using (UserService userService = new UserService())
            {
                UserDTO user = userService.GetItem(UserID);
                res = "<img src =\"" + user.AvatarPath + "\" id=\"avatar-popup\" />";
            }
            return Content(res);
        }

        [HttpGet]
        public ActionResult GetAvatarContentBig(int UserID)
        {
            string res; ;
            using (UserService userService = new UserService())
            {
                UserDTO user = userService.GetItem(UserID);
                res = "<img src =\"" + user.AvatarPath + "\" class=\"profile-av\" />";
            }
            return Content(res);
        }

        [HttpPost]
        public ActionResult ResetAvatar(int UserID)
        {
            using (UserService userService = new UserService())
            {
                UserDTO user = userService.GetItem(UserID);
                userService.ClearUserPhoto(user);
            }
            return new HttpStatusCodeResult(200, "OK");
        }

        [CustomAuthorize]
        public ActionResult Signout()
        {
            Response.Cookies.Add(new HttpCookie("auth", null));
            Response.Cookies["auth"].Expires = DateTime.Now;
            Session.Abandon();
            HttpContext.Cache.Remove(User.Identity.Name);
            return RedirectToAction("Login", "Account", new { area = "" });
        }
    }
}