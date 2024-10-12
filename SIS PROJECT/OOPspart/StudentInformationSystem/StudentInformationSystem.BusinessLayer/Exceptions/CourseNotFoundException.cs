using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.BusinessLayer.Exceptions
{
    public class CourseNotFoundException : System.Exception
    {
        public CourseNotFoundException(string message) : base(message) { }
    }
}
