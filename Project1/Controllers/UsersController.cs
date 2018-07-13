using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project1.DAL;
using Project1.Models;
using Project1.Configuration;

namespace Project1.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private ManagerContext db = new ManagerContext();
        private Validation valid = new Validation();

        [Authorize(Roles = CustomPermission.ShowUser)]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        [Authorize(Roles = CustomPermission.DetailsUser)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [Authorize(Roles = CustomPermission.AddUser)]
        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = CustomPermission.AddUser)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password,Email,SecurityPassword,ActivationCode,TimeGetCode,CountLogin,TimeCountLogin,ConfirmActivity")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        [Authorize(Roles = CustomPermission.AddRoleIntoUser)]
        public ActionResult AddRole(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.LstRole = user.Roles;
            ViewBag.Username = user.Username;
            ViewBag.UserId = user.Id;
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name");
            return View(user);
        }

        [Authorize(Roles = CustomPermission.AddRoleIntoUser)]
        [HttpPost]
        public ActionResult AddRole(int userId, int roleId)// add 1 role moi cho user
        {
            try
            {
                //int userid = Int32.Parse(RouteData.Values["id"].ToString());
                User user = db.Users.Find(userId);
                db.Users.Attach(user);

                Role role = db.Roles.Find(roleId);
                db.Roles.Attach(role);

                user.Roles.Add(role);

                db.SaveChanges();
                ViewBag.LstRole = user.Roles;
                ViewBag.Username = user.Username;
                ViewBag.UserId = user.Id;
            }
            catch (Exception e)
            {
                ViewBag.Username = e;
            }
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name");
            return View();
        }

        [Authorize(Roles = CustomPermission.AddPermissionIntoUser)]
        public ActionResult AddPermission(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.LstPermission = user.UserPermissions.SelectMany(p => p.Permisssion.Name).Distinct();
            ViewBag.Username = user.Username;
            ViewBag.UserId = user.Id;
            ViewBag.PermissionId = new SelectList(db.Permissions, "Id", "Name");
            return View(user);
        }

        [Authorize(Roles = CustomPermission.AddPermissionIntoUser)]
        [HttpPost]
        public ActionResult AddPermission(int userId, int permissionId)// add 1 permission moi cho user
        {
            try
            {
                User user = db.Users.Find(userId);
                UserPermission usper = new UserPermission { UserId = userId, PermissionId = permissionId, Deny = false };
                db.UserPermissions.Add(usper);
                db.SaveChanges();
                ViewBag.LstPermission = user.UserPermissions.Select(p => p.Permisssion).Distinct();
                ViewBag.Username = user.Username;
                ViewBag.UserId = user.Id;
            }
            catch (Exception e)
            {
                ViewBag.Username = e;
            }
            ViewBag.PermissionId = new SelectList(db.Permissions, "Id", "Name");
            return View();
        }

        [Authorize(Roles = CustomPermission.EditUser)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [Authorize(Roles = CustomPermission.EditUser)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,Email,SecurityPassword,ActivationCode,TimeGetCode,CountLogin,TimeCountLogin,ConfirmActivity")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [Authorize(Roles = CustomPermission.DeleteRoleUser)]
        [HttpGet]
        public ActionResult DeleteRoleInUser(int userId, int roleId)//Xóa role của user
        {
            User user = db.Users.Find(userId);
            db.Users.Attach(user);

            Role role = db.Roles.Find(roleId);
            db.Roles.Attach(role);

            user.Roles.Remove(role);
            db.SaveChanges();
            return RedirectToAction("AddRole", "Users", new { id = userId });
        }

        [Authorize(Roles = CustomPermission.DeletePermissionUser)]
        [HttpGet]
        public ActionResult DeletePermissionInUser(int userId, int permissionId)//Xóa permission chủa user
        {
            UserPermission usper = db.UserPermissions.FirstOrDefault(up => up.UserId == userId && up.PermissionId == permissionId);
            db.UserPermissions.Remove(usper);
            db.SaveChanges();
            return RedirectToAction("AddPermission", "Users", new { id = userId });
        }

        [Authorize(Roles = CustomPermission.DeleteUser)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [Authorize(Roles = CustomPermission.DeleteUser)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = CustomPermission.UserProfile)]
        [HttpGet]
        public ActionResult UserProfile()
        {
            if (Session["AccountId"] == null || Session["RoleId"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int id = (int)Session["AccountId"];
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBag.ListRole = db.Roles;
            var lstRole = user.Roles.Select(r => r.Name).ToArray();
            ViewBag.LstRole = string.Join(",", lstRole);

            ViewBag.ListPermission = db.Permissions;
            var lstPermission = user.Roles.SelectMany(r => r.Permissions.Select(p => p.CodeName)).Concat(user.UserPermissions.Select(p => p.Permisssion.CodeName)).Distinct().ToArray();
            ViewBag.LstPermission = string.Join(",", lstPermission);

            return View(user);
        }

        [Authorize(Roles = CustomPermission.UserProfile)]
        [HttpPost]
        public ActionResult UserProfile(string Username, string Email)
        {
            if (Session["AccountId"] == null || Session["RoleId"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int id = (int)Session["AccountId"];
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            string vali;
            if (user.Username == Username && user.Email != Email)
            {
                vali = valid.ValidEmail(Email);
                if (vali != "")
                    ModelState.AddModelError("Email", vali);
                else
                {
                    user.Email = Email;
                    ViewBag.Message = "Thay đổi thông tin thành công";
                    db.SaveChanges();
                }
            }
            else if (user.Username != Username && user.Email == Email)
            {
                vali = valid.ValidUserName(Username);
                if (vali != "")
                    ModelState.AddModelError("UserName", vali);
                else
                {
                    user.Username = Username;
                    ViewBag.Message = "Thay đổi thông tin thành công";
                    db.SaveChanges();
                }
            }
            else
            {
                vali = valid.ValidUserName(Username);
                if (vali != "")
                    ModelState.AddModelError("UserName", vali);
                vali = valid.ValidEmail(Email);
                if (vali != "")
                    ModelState.AddModelError("Email", vali);
                if (ModelState.IsValid)
                {
                    user.Username = Username;
                    user.Email = Email;
                    ViewBag.Message = "Thay đổi thông tin thành công";
                    db.SaveChanges();
                }
            }
            ViewBag.ListRole = db.Roles;
            var lstRole = user.Roles.Select(r => r.Name).ToArray();
            ViewBag.LstRole = string.Join(",", lstRole);

            ViewBag.ListPermission = db.Permissions;
            var lstPermission = user.Roles.SelectMany(r => r.Permissions.Select(p => p.CodeName)).Concat(user.UserPermissions.Select(p => p.Permisssion.CodeName)).Distinct().ToArray();
            ViewBag.LstPermission = string.Join(",", lstPermission);

            return View(user);
        }

        [Authorize(Roles = CustomPermission.ManagerProfile)]
        public ActionResult Profiles(int? id)
        {
            if (id == null || Session["RoleId"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            if ((int)Session["RoleId"] != 1 && ((int)Session["RoleId"] != 2 || Array.Exists(user.Roles.Select(r => r.Name).ToArray(), r => r == "SuperAdmin" || r == "Admins")))
            {
                return RedirectToAction("Logoff", "Accounts");
            }
            TempData["UserId"] = user.Id;

            ViewBag.ListRole = db.Roles;
            var lstRole = user.Roles.Select(r => r.Name).ToArray();
            ViewBag.LstRole = string.Join(",", lstRole);

            ViewBag.ListPermission = db.Permissions;
            //var lstPermission = user.Roles.SelectMany(r => r.Permissions.Select(p => p.CodeName))
            //                        .Concat(user.UserPermissions.Where(up => up.Deny == false).Select(p => p.Permisssion.CodeName))
            //                        .Distinct();
            //var lstDeny = user.UserPermissions.Where(up => up.Deny == true).Select(p => p.Permisssion.CodeName).Distinct();
            //var remains = lstPermission.Except(lstDeny);//.ToArray()
            var listPer = (db.Users.Where(u => u.Id == id)
                                  .SelectMany(u => u.Roles.SelectMany(r => r.Permissions))
                                  .Select(p => p.CodeName))
                         .Union(db.Users.Where(u => u.Id == id)
                                        .SelectMany(u => u.UserPermissions.Select(up => up.Permisssion))
                                        .Select(p => p.CodeName))
                         .Except(db.Users.Where(u => u.Id == id)
                                         .SelectMany(u => u.UserPermissions.Where(up => up.Deny == true)
                                                                           .Select(up => up.Permisssion))
                                         .Select(p => p.CodeName))
                         .Distinct();
            ViewBag.LstPermission = string.Join(",", listPer);
            var lstDeny = user.UserPermissions.Where(d => d.Deny == true).Select(p => p.Permisssion.CodeName).Distinct();
            ViewBag.ListDeny = string.Join(",", lstDeny);
            return View(user);
        }

        [Authorize(Roles = CustomPermission.EditUser)]
        public ActionResult _PermissionPartial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user.UserPermissions);
        }

        [Authorize(Roles = CustomPermission.ManagerProfile)]
        [HttpPost]
        public ActionResult ChangeRole(IEnumerable<Role> Roles)
        {
            try
            {
                int userId = int.Parse(TempData["UserId"].ToString());
                var listRole = db.Roles.Select(r => r.Name).ToArray();
                User user = db.Users.Find(userId);
                db.Users.Attach(user);
                foreach (var item in Roles)
                {
                    Role role = db.Roles.Find(item.Id);
                    db.Roles.Attach(role);
                    if (Array.Exists(listRole, r => r == item.Name))//item.Name != null)
                        user.Roles.Add(role);
                    else
                        user.Roles.Remove(role);
                }
                db.SaveChanges();
                return RedirectToAction("Profiles", "Users", new { id = userId });
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize(Roles = CustomPermission.ManagerProfile)]
        [HttpPost]
        public ActionResult ChangePermission(IEnumerable<UserPermission> UserPermissions)
        {
            try
            {
                int userId = int.Parse(TempData["UserId"].ToString());
                var listCode = db.Users.Where(u => u.Id == userId)
                                       .SelectMany(u => u.Roles.SelectMany(r => r.Permissions.Select(p => p.CodeName)).Distinct())
                                       .ToArray();
                foreach (var item in UserPermissions)
                {
                    var permissions = db.Permissions.Where(p => p.Id == item.PermissionId)
                                                    .Select(p => p.CodeName)
                                                    .FirstOrDefault();
                    var userper = db.UserPermissions.Where(u => u.UserId == userId && u.PermissionId == item.PermissionId)
                                                    .FirstOrDefault();
                    if (item.Deny == false)
                    {
                        if (userper == null && Array.Exists(listCode, r => r == permissions))
                        {
                            item.UserId = userId;
                            item.Deny = true;
                            db.UserPermissions.Add(item);
                        }
                        else if (userper != null)
                        {
                            if (Array.Exists(listCode, r => r == permissions))
                            {
                                userper.Deny = true;
                            }
                            else
                            {
                                db.UserPermissions.Remove(userper);
                            }
                        }
                    }
                    else
                    {
                        if (userper != null)
                        {
                            db.UserPermissions.Remove(userper);
                        }
                        else if (!Array.Exists(listCode, r => r == permissions))
                        {
                            if (userper == null)
                            {
                                item.UserId = userId;
                                item.Deny = false;
                                db.UserPermissions.Add(item);
                            }
                            else
                            {
                                userper.Deny = false;
                            }
                        }
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Profiles", "Users", new { id = userId });
            }
            catch (Exception e)
            {
                var a = e;
                return RedirectToAction("Index", "Home");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
