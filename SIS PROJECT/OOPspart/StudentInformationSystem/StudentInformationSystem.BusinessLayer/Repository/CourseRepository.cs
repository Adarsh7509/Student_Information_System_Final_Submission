using StudentInformationSystem.Entity;
using StudentInformationSystem.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.BusinessLayer.Repository
{
    public class CourseRepository : ICourseRepository
    {
        public readonly string _connectionString;

        public CourseRepository()
        {
            _connectionString = DbConnUtil.GetConnString();
        }

        // Method to get a course by its ID
        public Course GetCourseById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT course_id, course_name, credits FROM Courses WHERE course_id = @CourseId", connection))
                {
                    command.Parameters.AddWithValue("@CourseId", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Course(

                                reader.GetString(reader.GetOrdinal("course_name")),
                                reader.GetInt32(reader.GetOrdinal("credits"))// Changed to GetInt32 for credits
                            );
                        }
                    }
                }
            }
            return null; // Return null if no course found
        }

        // Method to get all courses
        public IEnumerable<Course> GetAllCourses()
        {
            var courses = new List<Course>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT Courses.course_id, Courses.course_name, Courses.credits, Teacher.teacher_id, Teacher.first_name, Teacher.last_name, Teacher.email FROM Courses LEFT JOIN Teacher ON Courses.teacher_id = Teacher.teacher_id", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Retrieve teacherId safely
                            int? teacherId = reader["teacher_id"] != DBNull.Value
                                ? reader.GetInt32(reader.GetOrdinal("teacher_id"))
                                : (int?)null;

                            // Create the Teacher object if teacherId is available
                            Teacher assignedTeacher = null;
                            if (teacherId.HasValue)
                            {
                                assignedTeacher = new Teacher(
                                    teacherId.Value,
                                    reader.GetString(reader.GetOrdinal("first_name")),
                                    reader.GetString(reader.GetOrdinal("last_name")),
                                    reader.GetString(reader.GetOrdinal("email"))
                                );
                            }

                            // Create and add the Course object to the list
                            var course = new Course(
                                reader.GetInt32(reader.GetOrdinal("course_id")),
                                reader.GetString(reader.GetOrdinal("course_name")),
                                reader.GetInt32(reader.GetOrdinal("credits")),
                                assignedTeacher
                            );

                            courses.Add(course);
                        }
                    }
                }
            }
            return courses; // Return the full list of courses
        }

        // Method to add a new course
        public void AddCourse(Course course)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO Courses (course_name, credits, teacher_id) VALUES (@CourseName, @Credits, @TeacherId)", connection))
                {
                    command.Parameters.AddWithValue("@CourseName", course.CourseName);
                    command.Parameters.AddWithValue("@Credits", course.Credits);
                    command.Parameters.AddWithValue("@TeacherId", course.AssignedTeacher?.TeacherId ?? (object)DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Method to update an existing course
        public void UpdateCourse(Course course)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE Courses SET course_name = @CourseName, credits = @Credits, teacher_id = @TeacherId WHERE course_id = @CourseId", connection))
                {
                    command.Parameters.AddWithValue("@CourseId", course.CourseId);
                    command.Parameters.AddWithValue("@CourseName", course.CourseName);
                    command.Parameters.AddWithValue("@Credits", course.Credits);
                    command.Parameters.AddWithValue("@TeacherId", course.AssignedTeacher?.TeacherId ?? (object)DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Method to delete a course by its ID
        public void DeleteCourse(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM Courses WHERE course_id = @CourseId", connection))
                {
                    command.Parameters.AddWithValue("@CourseId", id);
                    command.ExecuteNonQuery();
                }
            }
        }
        //// Method to GetCourseEnrollments
        public IEnumerable<Enrollment> GetCourseEnrollments(int courseId)
        {
            return null;
        }
    }
}
