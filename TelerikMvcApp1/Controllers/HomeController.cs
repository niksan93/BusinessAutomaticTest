using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using MongoDB.Bson;
using MongoDB.Driver;
using TelerikMvcApp1.Models;

namespace TelerikMvcApp1.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Информация о View листа пользователей.
        /// </summary>
        public async Task<ActionResult> Index()
        {
            var programContext = new Context();

            var users = await programContext.Users
                .Find(_ => true)
                .ToListAsync();

            var departments = await programContext.Departments
                .Find(_ => true)
                .ToListAsync();

            IList<IndexUser> indexUsers = new List<IndexUser>(); 
            // Создание модели для визуализации в листе с информацией о названиях departments.
            foreach (var user in users)
            {
                var departmentTitle = string.Empty;
                if (user.Department >= departments.Count)
                    departmentTitle = "Undefined";
                else
                    departmentTitle = programContext.Departments.Find(u => u.DepartmentId == user.Department).First().Title;
                var us = new IndexUser
                {
                    UserID = user.UserID,
                    _id = user._id,
                    UserName = user.UserName,
                    Department = departmentTitle
                };
                indexUsers.Add(us);
            }

            return View(indexUsers);
        }

        /// <summary>
        /// Открытие нового View для добавления пользователя.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> NewUser()
        {
            var programContext = new Context();
            var allDeps = await programContext.Departments
                .Find(_ => true)
                .ToListAsync();
            return View(new NewUserModel
            {
                Department = allDeps
            });
        }

        /// <summary>
        /// Добавление нового пользователя в бд Users.
        /// </summary>
        /// <param name="user">Новый пользователь</param>
        [HttpPost]
        public async Task<ActionResult> NewUser(NewUserModel user)
        {
            var programContext = new Context();

            var count = programContext.Users.Find(_ => true).Count();

            var newUser = new User()
            {
                Department = user.DepartmentId,
                UserName = user.UserName,
                UserID = (int)count + 1
            };

            await programContext.Users.InsertOneAsync(newUser);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Удаление пользователя из бд Users.
        /// </summary>
        /// <param name="user">Удаляемый пользователь</param>
        public async Task<ActionResult> DeleteUser(User user)
        {
            var programContext = new Context();

            await programContext.Users.DeleteOneAsync(u => u.UserID == user.UserID);
            await programContext.Users.UpdateManyAsync(
                Builders<User>.Filter.Gt("UserID", user.UserID),
                Builders<User>.Update.Inc("UserID", -1));
            
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Изменение информации о пользователе в бд Users.
        /// </summary>
        /// <param name="user">Изменяемый пользователь</param>
        public async Task<ActionResult> EditUser(User user)
        {
            var programContext = new Context();

            await programContext.Users.UpdateOneAsync(
                Builders<User>.Filter.Eq("UserID", user.UserID),
                Builders<User>.Update.Set("UserName", user.UserName));

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Открытие нового View для добавления нового Department.
        /// </summary>
        [HttpGet]
        public ActionResult NewDepartment()
        {
            return View(new Department());
        }

        /// <summary>
        /// Добавление нового департамента в бд Departments.
        /// </summary>
        /// <param name="dep">Новый департамент</param>
        [HttpPost]
        public async Task<ActionResult> NewDepartment(Department dep)
        {
            var programContext = new Context();

            var count = programContext.Departments.Find(_ => true).Count();

            var newDep = new Department()
            {
                Title = dep.Title,
                DepartmentId = (int)count++
            };

            await programContext.Departments.InsertOneAsync(newDep);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Вывод листа департаментов во View.
        /// </summary>
        public async Task<ActionResult> DepartmentsList()
        {
            var programContext = new Context();

            var allDeps = await programContext.Departments.Find(_ => true).ToListAsync();

            return View(allDeps);
        }

        /// <summary>
        /// Изменение данных о департаменте в бд Departments.
        /// </summary>
        /// <param name="dep">Изменяемый департамент</param>
        public async Task<ActionResult> EditDepartment(Department dep)
        {
            var programContext = new Context();

            await programContext.Departments.UpdateOneAsync(
                Builders<Department>.Filter.Eq("DepartmentId", dep.DepartmentId),
                Builders<Department>.Update.Set("Title", dep.Title));

            return RedirectToAction("DepartmentsList");
        }

        /// <summary>
        /// Удаление департамента из бд Departments.
        /// </summary>
        /// <param name="dep">Удаляемый департамент</param>
        public async Task<ActionResult> DeleteDepartment(Department dep)
        {
            var programContext = new Context();

            await programContext.Departments.DeleteOneAsync(u => u.DepartmentId == dep.DepartmentId);

            await programContext.Departments.UpdateManyAsync(
                    Builders<Department>.Filter.Gt("DepartmentId", dep.DepartmentId),
                    Builders<Department>.Update.Inc("DepartmentId", -1));

            return RedirectToAction("DepartmentsList");
        }
    }
}
