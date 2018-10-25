using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeltExam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BeltExam.Controllers
{
    public class HomeController : Controller
    {
        private beltexamContext dbContext;

        public HomeController(beltexamContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return RedirectToAction("LoginReg");
        }

                [HttpGet("LoginReg")]
        public IActionResult LoginReg()
        {
            ViewBag.EmailError = "";
            return View();
        }

        [HttpPost("LoginReg")]
        public IActionResult CreateAccount(User newUser)
        {
            if(ModelState.IsValid){
                if(dbContext.users.Any(u => u.Email == newUser.Email))
                    {      
                    ViewBag.EmailError = "Email Already Exists";

                    return View("LoginReg");
                    }
                else{

                        PasswordHasher<User> Hasher = new PasswordHasher<User>();
                        newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
     
                        dbContext.users.Add(newUser);
                        dbContext.SaveChanges();
                        HttpContext.Session.SetString("UserName", newUser.FName);
                        HttpContext.Session.SetString("email", newUser.Email);
                        HttpContext.Session.SetInt32("UserId", newUser.UserId);
                        return Redirect("bright_ideas");
                }
                 
            }

            return View("LoginReg");
        }
        [HttpPost("Login")]
        public IActionResult Login(LoginUser userSubmission)
        {
          if(ModelState.IsValid)
        {
            var userInDb = dbContext.users.FirstOrDefault(u => u.Email == userSubmission.Lemail);
            if(userInDb == null)
            {          
                ViewBag.EmailError = "No Email Found";

                return View("LoginReg");
            }
            var hasher = new PasswordHasher<LoginUser>();
            var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Lpassword);
            if(result == 0)
            {
                ViewBag.EmailError = "Incorrect Password";
                return View("LoginReg");
            }
            HttpContext.Session.SetString("UserName", userInDb.FName);            
            HttpContext.Session.SetString("email", userSubmission.Lemail);
            HttpContext.Session.SetInt32("UserId", userInDb.UserId);
            return Redirect("bright_ideas");

        } 

           return View("LoginReg"); 
        }
        [HttpGet("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return Redirect("LoginReg");
        }
        [HttpGet("bright_ideas")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("UserId")==null){
                return RedirectToAction("Index");
            }
            else{
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            List<Idea> ideas = dbContext.ideas.Include(i=>i.Creator).Include(i=>i.Likes).OrderByDescending(i=>i.Likes.Count).ToList();
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.Ideas = ideas;
            return View();
            }
        }
        [HttpPost("bright_ideas")]
        public IActionResult AddIdea(Idea newIdea)
        {
            if(ModelState.IsValid){

                User user = dbContext.users.SingleOrDefault(u=>u.UserId==HttpContext.Session.GetInt32("UserId"));
                
                newIdea.Creator = user;
                newIdea.CreatorId=(int)HttpContext.Session.GetInt32("UserId");
                dbContext.ideas.Add(newIdea);

                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            List<Idea> ideas = dbContext.ideas.Include(i=>i.Creator).Include(i=>i.Likes).OrderByDescending(i=>i.Likes.Count).ToList();
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.Ideas = ideas;
            return View("Dashboard");
        }
        [HttpGet("bright_ideas/{id}")]
        public IActionResult singleIdea(int id)
        {
            if(HttpContext.Session.GetInt32("UserId")==null){
                return RedirectToAction("Index");
            }
            else{
                ViewBag.Ideas = dbContext.ideas.Include(i=>i.Creator).Include(i=>i.Likes).SingleOrDefault(i=>i.IdeaId==id);
                List<Liker> likes = dbContext.likers.Include(l=>l.User).Where(l=>(l.IdeaId==id)).ToList();
                ViewBag.Likes = likes.Select(u=>u.User).Distinct();
                return View();
            }
        }
        [HttpGet("like/{id}")]
        public IActionResult Like(int id)
        {
            if(HttpContext.Session.GetInt32("UserId")==null){
                return RedirectToAction("Index");
            }
            else{
                List<Liker> validLike = dbContext.likers.Where(l=>(l.IdeaId==id)&&(l.UserId==HttpContext.Session.GetInt32("UserId"))).ToList();
                

                Liker newLiker = new Liker();
                User user = dbContext.users.SingleOrDefault(u=>u.UserId==(int)HttpContext.Session.GetInt32("UserId"));
                Idea idea = dbContext.ideas.SingleOrDefault(i=>i.IdeaId==id);
                newLiker.Idea=idea;
                newLiker.User = user;
                dbContext.Add(newLiker);
                dbContext.SaveChanges();
                

                
                return Redirect("/bright_ideas");
            }
        }
        [HttpGet("user/{id}")]
        public IActionResult SingleUser(int id)
        {
            if(HttpContext.Session.GetInt32("UserId")==null){
                return RedirectToAction("Index");
            }
            else{
                ViewBag.User = dbContext.users.SingleOrDefault(u=>u.UserId ==id);
                ViewBag.Likes = dbContext.likers.Where(l=>l.UserId==id).ToList();
                ViewBag.Ideas = dbContext.ideas.Where(l=>l.CreatorId==id).ToList();


                return View();
            }
        }
        [HttpGet("delete/{id}")]
        public IActionResult delete(int id)
        {

            if(HttpContext.Session.GetInt32("UserId")==null){
                return RedirectToAction("Index");
            }
            else{
                List<Liker> deleteLikes = dbContext.likers.Where(l=>l.IdeaId==id).ToList(); 
                Idea deleteIdea = dbContext.ideas.SingleOrDefault(w=>w.IdeaId==id);

                dbContext.likers.RemoveRange(deleteLikes);
                dbContext.ideas.Remove(deleteIdea);
                dbContext.SaveChanges();
                return Redirect("/bright_ideas");
            }
        } 
    }
}
