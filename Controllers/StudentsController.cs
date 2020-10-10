using System.Collections.Generic;
using AspNetiTextSharp.Model;
using AspNetiTextSharp.Reports;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AspNetiTextSharp.Controllers
{       
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnviroment;

        public StudentsController(IWebHostEnvironment webHostEnviroment)
        {
            _webHostEnviroment = webHostEnviroment;
        }

        [HttpGet]
        public ActionResult Get()
        {
            List<Student> students = new List<Student>();
            for (int i = 0; i < 100; i++)
            {
                Student student = new Student();
                student.Id = i;
                student.Name = "Student សិស្សសាលារៀន " + i;
                student.Address = "Address អាសយដ្ឋាន " + i;
                students.Add(student);
            }

            StudentReport rpt = new StudentReport(_webHostEnviroment);

            return File(rpt.Report(students), "application/pdf");
        }
    }
}