﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.BusinessLayer.Service
{
    public interface ISISservice
    {
        void EnrollStudentInCourse(int studentId, int courseId);
        void AssignTeacherToCourse(int teacherId, int courseId);
        void RecordPayment(int studentId, decimal amount, DateTime paymentDate);
        void GenerateEnrollmentReport(int courseId);
        void GeneratePaymentReport(int studentId);
        void CalculateCourseStatistics(int courseId);
    }
}
