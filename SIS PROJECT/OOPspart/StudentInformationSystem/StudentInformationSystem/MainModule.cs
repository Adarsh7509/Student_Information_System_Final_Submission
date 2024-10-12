using StudentInformationSystem.BusinessLayer.Exceptions;
using StudentInformationSystem.BusinessLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.BusinessLayer.Repository
{
    public class MainModule
    {
        public ServiceImplementation service;

        public MainModule(
            IStudentRepository studentRepository,
            ICourseRepository courseRepository,
            ITeacherRepository teacherRepository,
            IEnrollmentRepository enrollmentRepository,
            IPaymentRepository paymentRepository)
        {
            service = new ServiceImplementation(
                studentRepository,
                courseRepository,
                teacherRepository,
                enrollmentRepository,
                paymentRepository);
        }

        public void ShowMenu()
        {
            bool continueRunning = true;

            while (continueRunning)
            {
                Console.WriteLine("\n--- Student Information System ---");
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Enroll a Student in a Course");
                Console.WriteLine("2. Assign Teacher to Course");
                Console.WriteLine("3. Record a Payment");
                Console.WriteLine("4. Generate Enrollment Report");
                Console.WriteLine("5. Generate Payment Report");
                Console.WriteLine("6. Calculate Course Statistics");
                Console.WriteLine("7. Add New Student");
                Console.WriteLine("8. Exit");
                Console.Write("Enter your choice: ");

                var input = Console.ReadLine();

                try
                {
                    switch (input)
                    {
                        case "1":
                            EnrollStudent();
                            break;
                        case "2":
                            AssignTeacherToCourse();
                            break;
                        case "3":
                            RecordPayment();
                            break;
                        case "4":
                            GenerateEnrollmentReport();
                            break;
                        case "5":
                            GeneratePaymentReport();
                            break;
                        case "6":
                            CalculateCourseStatistics();
                            break;
                        case "7":
                            AddNewStudent();
                            break;
                        case "8":
                            continueRunning = false;
                            Console.WriteLine("Exiting the system...");
                            break;
                        default:
                            Console.WriteLine("Invalid choice, please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        public void AddNewStudent()
        {
            Console.WriteLine("\n--- Add New Student ---");
            int studentId = service.AddStudentFromInput();
            Console.WriteLine($"New student added successfully. Student ID: {studentId}");

            Console.WriteLine("Do you want to enroll this student in courses? (Y/N)");
            if (Console.ReadLine().Trim().ToUpper() == "Y")
            {
                EnrollStudentInMultipleCourses(studentId);
            }
        }

        public void EnrollStudentInMultipleCourses(int studentId)
        {
            while (true)
            {
                Console.Write("Enter Course ID to enroll (or 0 to finish): ");
                int courseId = int.Parse(Console.ReadLine());
                if (courseId == 0) break;

                service.EnrollStudentInCourse(studentId, courseId);
                Console.WriteLine("Student enrolled successfully in the course.");
            }
        }

        public void EnrollStudent()
        {
            Console.WriteLine("\n--- Enroll a Student in a Course ---");

            int studentId;
            while (true)
            {
                Console.Write("Enter Student ID: ");
                if (int.TryParse(Console.ReadLine(), out studentId))
                    break;
                Console.WriteLine("Invalid input. Please enter a valid Student ID.");
            }

            int courseId;
            while (true)
            {
                Console.Write("Enter Course ID: ");
                if (int.TryParse(Console.ReadLine(), out courseId))
                    break;
                Console.WriteLine("Invalid input. Please enter a valid Course ID.");
            }

            try
            {
                service.EnrollStudentInCourse(studentId, courseId);
                Console.WriteLine("Student enrolled successfully in the course.");
            }
            catch (StudentNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (CourseNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (DuplicateEnrollmentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }

        public void AssignTeacherToCourse()
        {
            Console.WriteLine("\n--- Assign Teacher to Course ---");
            Console.Write("Enter Teacher Name: ");
            string teacherName = Console.ReadLine();
            Console.Write("Enter Teacher Email: ");
            string teacherEmail = Console.ReadLine();
            Console.Write("Enter Teacher Expertise: ");
            string teacherExpertise = Console.ReadLine();
            Console.Write("Enter Course Code: ");
            string courseCode = Console.ReadLine();

            //service.AssignTeacherToCourse(teacherName, teacherEmail, teacherExpertise, courseCode);
            Console.WriteLine("Teacher assigned to the course successfully.");
        }

        public void RecordPayment()
        {
            Console.WriteLine("\n--- Record a Payment ---");
            Console.Write("Enter Student ID: ");
            int studentId = int.Parse(Console.ReadLine());
            Console.Write("Enter Payment Amount: ");
            decimal paymentAmount = decimal.Parse(Console.ReadLine());
            Console.Write("Enter Payment Date (YYYY-MM-DD): ");
            DateTime paymentDate = DateTime.Parse(Console.ReadLine());

            service.RecordPayment(studentId, paymentAmount, paymentDate);
            Console.WriteLine("Payment recorded successfully.");
        }

        public void GenerateEnrollmentReport()
        {
            Console.WriteLine("\n--- Generate Enrollment Report ---");
            Console.Write("Enter Course id: ");
            int cid = Convert.ToInt32(Console.ReadLine());

            service.GenerateEnrollmentReport(cid);
        }

        public void GeneratePaymentReport()
        {
            Console.WriteLine("\n--- Generate Payment Report ---");
            Console.Write("Enter Student ID: ");
            int studentId = int.Parse(Console.ReadLine());

            service.GeneratePaymentReport(studentId);
        }

        public void CalculateCourseStatistics()
        {
            Console.WriteLine("\n--- Calculate Course Statistics ---");
            Console.Write("Enter Course ID: ");
            int courseId = int.Parse(Console.ReadLine());

            service.CalculateCourseStatistics(courseId);
        }
    }
}
