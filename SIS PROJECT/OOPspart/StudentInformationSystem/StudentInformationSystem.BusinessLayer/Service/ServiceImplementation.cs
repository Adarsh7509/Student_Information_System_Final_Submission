using StudentInformationSystem.BusinessLayer.Exceptions;
using StudentInformationSystem.BusinessLayer.Repository;
using StudentInformationSystem.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.BusinessLayer.Service
{
    public class ServiceImplementation : ISISservice
    {
        public readonly IStudentRepository _studentRepository;
        public readonly ICourseRepository _courseRepository;
        public readonly ITeacherRepository _teacherRepository;
        public readonly IEnrollmentRepository _enrollmentRepository;
        public readonly IPaymentRepository _paymentRepository;

        public ServiceImplementation(
        IStudentRepository studentRepository,
        ICourseRepository courseRepository,
        ITeacherRepository teacherRepository,
        IEnrollmentRepository enrollmentRepository,
        IPaymentRepository paymentRepository)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            _teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
            _enrollmentRepository = enrollmentRepository ?? throw new ArgumentNullException(nameof(enrollmentRepository));
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        }
        // Enrollment methods
        public void EnrollStudentInCourse(int studentId, int courseId)
        {
            var student = _studentRepository.GetStudentById(studentId);
            var course = _courseRepository.GetCourseById(courseId);

            if (student == null)
            {
                throw new StudentNotFoundException("Student not found.");
            }

            if (course == null)
            {
                throw new CourseNotFoundException("Course not found."); // Throw an exception if course is not found
            }

            if (_enrollmentRepository.Exists(studentId, courseId))
            {
                throw new DuplicateEnrollmentException("Student is already enrolled in this course.");
            }

            // Create the Enrollment object without specifying EnrollmentId
            var enrollment = new Enrollment(student, course, DateTime.Now); // No need to specify EnrollmentId

            // Add the enrollment to the repository and get the generated EnrollmentId
            int enrollmentId = _enrollmentRepository.AddEnrollment(enrollment);

            // Update the EnrollmentId property with the returned value
            enrollment.EnrollmentId = enrollmentId;

            // Add the enrollment to the student's collection
            student.Enrollments.Add(enrollment);
            _studentRepository.UpdateStudent(student);

            // Add the enrollment to the course's collection
            course.Enrollments.Add(enrollment);
            _courseRepository.UpdateCourse(course);
        }

        public void AssignTeacherToCourse(int teacherId, int courseId)
        {
            var teacher = _teacherRepository.GetTeacherById(teacherId);
            var course = _courseRepository.GetCourseById(courseId);

            if (teacher == null || course == null)
            {
                throw new TeacherNotFoundException("Teacher or course not found.");
            }

            course.AssignedTeacher = teacher;
            teacher.AssignedCourses.Add(course);

            _courseRepository.UpdateCourse(course);
            _teacherRepository.UpdateTeacher(teacher);
        }

        // Payment methods
        public void RecordPayment(int studentId, decimal amount, DateTime paymentDate)
        {
            var student = _studentRepository.GetStudentById(studentId);
            if (student == null)
            {
                throw new StudentNotFoundException($"Student with ID {studentId} not found.");
            }

            // Call the constructor without the PaymentId since it's optional
            var payment = new Payment(student, amount, paymentDate); // No ID needed here

            // Add the payment to the database, assuming it handles ID generation
            int paymentId = _paymentRepository.AddPayment(payment);
            payment.PaymentId = paymentId; // Set the generated ID
            student.Payments.Add(payment);
            _studentRepository.UpdateStudent(student);
        }


        // Reporting methods
        public void GenerateEnrollmentReport(int courseId)
        {
            var course = _courseRepository.GetCourseById(courseId);
            if (course == null)
            {
                throw new CourseNotFoundException($"Course with ID {courseId} not found.");
            }

            Console.WriteLine($"Enrollment Report for {course.CourseName} ({course.CourseId})");
            foreach (var enrollment in course.Enrollments)
            {
                Console.WriteLine($"- {enrollment.Student.FirstName} {enrollment.Student.LastName}");
            }
        }

        public void GeneratePaymentReport(int studentId)
        {
            var student = _studentRepository.GetStudentById(studentId);
            if (student == null)
            {
                throw new StudentNotFoundException($"Student with ID {studentId} not found.");
            }

            Console.WriteLine($"Payment Report for {student.FirstName} {student.LastName}");
            foreach (var payment in student.Payments)
            {
                Console.WriteLine($"- Date: {payment.PaymentDate.ToShortDateString()}, Amount: ${payment.Amount}");
            }
        }

        public void CalculateCourseStatistics(int courseId)
        {
            var course = _courseRepository.GetCourseById(courseId);
            if (course == null)
            {
                throw new CourseNotFoundException($"Course with ID {courseId} not found.");
            }

            int enrollmentCount = course.Enrollments.Count;
            decimal totalPayments = course.Enrollments.Sum(e => e.Student.Payments.Sum(p => p.Amount));

            Console.WriteLine($"Course Statistics for {course.CourseName}");
            Console.WriteLine($"Number of Enrollments: {enrollmentCount}");
            Console.WriteLine($"Total Payments: ${totalPayments}");
        }

        //Extra methods
        public void AddEnrollment(int studentId, int courseId, DateTime enrollmentDate)
        {
            var student = _studentRepository.GetStudentById(studentId);
            var course = _courseRepository.GetCourseById(courseId);

            if (student == null || course == null)
            {
                throw new Exception("Student or course not found.");
            }

            var enrollment = new Enrollment(student, course, enrollmentDate);
            _enrollmentRepository.AddEnrollment(enrollment);

            // Add the enrollment to both the student's and course's lists
            student.Enrollments.Add(enrollment);
            course.Enrollments.Add(enrollment);

            _studentRepository.UpdateStudent(student);
            _courseRepository.UpdateCourse(course);
        }

        // Method to assign a course to a teacher
        public void AssignCourseToTeacher(int courseId, int teacherId)
        {
            var course = _courseRepository.GetCourseById(courseId);
            var teacher = _teacherRepository.GetTeacherById(teacherId);

            if (course == null || teacher == null)
            {
                throw new Exception("Course or teacher not found.");
            }

            course.AssignedTeacher = teacher; // Assuming AssignedTeacher is a property
            teacher.AssignedCourses.Add(course);

            _courseRepository.UpdateCourse(course);
            _teacherRepository.UpdateTeacher(teacher);
        }

        // Method to add a payment
        public void AddPayment(int studentId, decimal amount, DateTime paymentDate)
        {
            var student = _studentRepository.GetStudentById(studentId);

            if (student == null)
            {
                throw new Exception("Student not found.");
            }

            var payment = new Payment(student, amount, paymentDate); // No ID needed here

            // Add the payment to the database, assuming it handles ID generation
            int paymentId = _paymentRepository.AddPayment(payment);
            payment.PaymentId = paymentId;
            student.Payments.Add(payment);
            _studentRepository.UpdateStudent(student);
        }

        // Method to retrieve enrollments for a specific student
        public IEnumerable<Enrollment> GetEnrollmentsForStudent(int studentId)
        {
            var student = _studentRepository.GetStudentById(studentId);

            if (student == null)
            {
                throw new Exception("Student not found.");
            }

            return student.Enrollments; // Return the list of enrollments
        }

        // Method to retrieve courses for a specific teacher
        public IEnumerable<Course> GetCoursesForTeacher(int teacherId)
        {
            var teacher = _teacherRepository.GetTeacherById(teacherId);

            if (teacher == null)
            {
                throw new Exception("Teacher not found.");
            }

            return teacher.AssignedCourses; // Return the list of assigned courses
        }
        public int AddStudentFromInput()
        {
            // Prompt for user input
            Console.WriteLine("Enter first name:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Enter last name:");
            string lastName = Console.ReadLine();

            Console.WriteLine("Enter date of birth (yyyy-mm-dd):");
            DateTime dateOfBirth;
            while (!DateTime.TryParse(Console.ReadLine(), out dateOfBirth))
            {
                Console.WriteLine("Invalid date format. Please enter again (yyyy-mm-dd):");
            }

            Console.WriteLine("Enter email:");
            string email = Console.ReadLine();

            Console.WriteLine("Enter phone number:");
            string phoneNumber = Console.ReadLine();

            // Create a new Student object
            var student = new Student
            (
                firstName,
                lastName,
                dateOfBirth,
                email,
                phoneNumber
            );

            // Add the student to the repository
            _studentRepository.AddStudent(student);
            Console.WriteLine("Student added successfully.");
            return student.StudentId;
        }
    }
}
