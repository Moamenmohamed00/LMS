# PRD — LMS + Online Exams (V1)

## 1. Overview

**Project Name:** LMS + Online Exams

**Goal:**
Build a learning management system that allows instructors to publish courses and content, and allows students to consume content, take quizzes/exams, submit assignments, track progress, and receive certificates after completion.

**Primary Platforms:**

* Web application
* Admin panel
* Instructor dashboard
* Student dashboard

**Target Users:**

* Admin
* Instructor
* Student

---

## 2. Problem Statement

Educational content is often scattered across multiple tools: file sharing, video links, quizzes, and grading platforms. This project centralizes the full learning workflow in one system.

---

## 3. Product Goals

* Provide a structured course delivery experience.
* Support multiple content types: PDF, video, YouTube links.
* Enable assessment through quizzes, exams, and assignments.
* Track student progress across lessons and courses.
* Give admins control over users, courses, and reports.
* Support both free and paid courses through Stripe/Paymob integration.

---

## 4. Success Metrics

* Number of registered users
* Number of created courses
* Number of enrolled students per course
* Lesson completion rate
* Quiz/exam completion rate
* Assignment submission rate
* Payment success rate
* Certificate generation count

---

## 5. User Roles

### 5.1 Admin

Can manage users, courses, reports, and system settings.

### 5.2 Instructor

Can create and manage courses, modules, lessons, quizzes, exams, assignments, and grades.

### 5.3 Student

Can enroll in courses, watch content, take quizzes/exams, submit assignments, track progress, and download certificates.

---

## 6. Scope (V1)

### 6.1 Authentication & Roles

**In scope:**

* Register (Students register directly; Instructors register and require admin approval)
* Login
* Logout
* Forgot password (via email)
* Email verification on registration
* Role-based access control

**Roles in V1:**

* Admin
* Instructor (admin-approved)
* Student

**Instructor Onboarding Flow:**

User registers as Instructor → Account created in pending state → Admin reviews and approves/rejects → Instructor gains access to create courses

---

### 6.2 Course Management

**Instructor can:**

* Create a new course
* Edit course details
* Delete course (soft delete)
* Add categories (flat structure)
* Add modules
* Add lessons
* Attach course thumbnail
* Set course price (0 for free courses)
* Submit course for admin approval

**Admin can:**

* Approve or reject submitted courses
* Unpublish a published course

**Course State Machine:**

Draft → Pending Approval → Approved/Rejected → Published → Unpublished

* Instructor creates course in **Draft** state
* Instructor submits for review → moves to **Pending Approval**
* Admin approves → **Approved** (auto-publishes) or rejects → **Rejected** (with feedback)
* Instructor can edit a rejected course and resubmit
* Admin can unpublish a published course

**Course content types:**

* PDF files
* Video uploads (stored on Cloudinary)
* YouTube links

---

### 6.3 Learning Experience

**Student can:**

* View enrolled courses
* Open lessons (sequential/locked — must complete previous lesson to unlock next)
* Watch videos
* Read PDFs inside the platform
* Resume content from last progress point
* View lesson completion status
* View course progress percentage

**Lesson Completion Criteria:**

* **Video lessons:** Automatically marked complete when the student watches **85%** of the video
* **PDF lessons:** Marked complete when the student opens/views the PDF
* Lessons are **sequential and locked** — a student must complete the current lesson before the next one unlocks

---

### 6.4 Quiz Inside Lesson

**Instructor can:**

* Add a mini quiz inside a lesson
* Define quiz questions and answers

**Student can:**

* Answer quiz questions after lesson content
* See quiz result immediately or after submission, depending on configuration

**Note:** Quiz scores are recorded but are not required for certificate eligibility. Quizzes serve as self-assessment tools.

---

### 6.5 Exams Management

**Instructor can:**

* Create an exam
* Assign exam to a course
* Add questions
* Set passing grade
* Set time limit
* Set attempt limit
* Enable or disable question/random choice shuffle

**Supported question types in V1:**

* Multiple Choice (MCQ)
* True / False

**Student can:**

* Start exam
* See countdown timer
* Submit answers
* Receive result based on grading rules
* Auto-submit when time ends

---

### 6.6 Assignments

**Instructor can:**

* Create assignment
* Set deadline
* Attach assignment instructions/files
* Review submissions
* Add grade and feedback

**Student can:**

* Submit assignment file (one submission only — no re-submission allowed)
* See submission status
* Receive grade and feedback

**Note:** Students cannot re-submit assignments after initial submission. Instructors may handle exceptions manually.

---

### 6.7 Communication

**In scope:**

* Lesson comments
* Instructor replies
* Pin important comments

**Note:**
A full discussion forum is out of scope for V1.

---

### 6.8 Certificates

**In scope:**

* Generate PDF certificate after course completion and success criteria are met

**Certificate Eligibility Criteria:**

* Student must complete **all lessons** in the course
* Student must **pass the final exam** (meet or exceed passing grade)
* Both conditions must be met to unlock certificate generation

**Certificate includes:**

  * Unique certificate ID/number
  * Student name
  * Course name
  * Completion date
  * Instructor name or signature representation
  * QR code for verification (links to public verification page)

---

### 6.9 Admin Dashboard

**Admin can:**

* View system overview
* Manage users
* Manage courses
* View reports

**Reports may include:**

* Number of students
* Number of instructors
* Number of courses
* Course enrollments
* Progress statistics
* Exam results summary
* Payment summary

---

### 6.10 Payments & Enrollment

**In scope:**

* Stripe and Paymob integration
* Purchase course access (one-time purchase model)
* Payment history
* Payment success/failure handling
* Webhook handling for payment provider callbacks

**Enrollment Flows:**

* **Free courses:** Student clicks "Enroll" → enrolled directly (no payment step)
* **Paid courses:** Student clicks "Enroll" → redirected to Stripe/Paymob checkout → payment success webhook → student enrolled

**Outcome:**
After successful payment (or direct enrollment for free courses), the student is enrolled in the course.

---

## 7. Out of Scope for V1

The following are not part of the first release:

* AI Q&A assistant
* AI summary generation
* AI exam generation
* Webcam-based anti-cheat
* Face recognition
* Full discussion forum
* Mobile app
* Live streaming classes
* Refund system
* Coupons and discounts
* Multi-vendor marketplace features

---

## 8. Core User Flows

### 8.1 Instructor Flow

Login → Create course → Add modules → Add lessons → Upload content → Create quiz/exam/assignment → Publish course

### 8.2 Student Flow

Register/Login → Browse course → Pay/enroll → Study content → Take quizzes/exams → Submit assignments → Track progress → Download certificate

### 8.3 Admin Flow

Login → View dashboard → Manage users/courses → Review reports

---

## 9. Functional Requirements

### Authentication

* The system must support user registration and login.
* The system must enforce role-based permissions.

### Courses

* The system must allow course creation and management.
* The system must support categories, modules, and lessons.

### Content

* The system must allow PDF upload and viewing.
* The system must allow video upload and YouTube links.

### Learning

* The system must track lesson completion and course progress.
* The system must support resume learning.

### Assessment

* The system must support quizzes, exams, and assignments.
* The system must store grades and feedback.

### Communication

* The system must support lesson-level comments.

### Certificates

* The system must generate downloadable PDF certificates.

### Admin

* The system must provide dashboards and reports.

### Payments

* The system must support paid course checkout through Stripe and Paymob.
* The system must support free course enrollment without payment.

---

## 10. Non-Functional Requirements

* Secure authentication and authorization
* Responsive UI for desktop and mobile browsers
* Clean and maintainable architecture
* File storage: Cloudinary for videos, cloud storage for PDFs and documents
* Auditability for grades and payment actions (via audit logs)
* Fast enough loading for course browsing and lesson access
* Email service for password reset, email verification, and key notifications

---

## 11. Data Entities (High-Level)

* User
* Role
* Category
* Course
* Module
* Lesson
* LessonContent
* LessonProgress (per-student, per-lesson completion tracking)
* Quiz
* QuizAttempt
* Exam
* ExamAttempt (tracks each attempt with individual answers)
* Question
* StudentAnswer (individual question responses within an attempt)
* Assignment
* Submission
* Grade (for both exams and assignments)
* Comment
* Enrollment
* Payment
* Certificate
* Notification
* AuditLog

---

## 12. Acceptance Criteria (V1)

The project is considered successful when:

* Users can register and log in.
* Instructors can create courses and add content.
* Students can access lessons and track progress.
* Exams and assignments work correctly.
* Certificates can be generated after completion.
* Admin can monitor the system.
* Stripe/Paymob payment flow can enroll a student in a paid course.
* Free course enrollment works without payment.

---

## 13. Suggested V1 Priority Order

1. Authentication & Roles
2. Course Management
3. Lesson Content (PDF / Video / YouTube)
4. Student Enrollment
5. Progress Tracking
6. Quizzes and Exams
7. Assignments and Grades
8. Certificates
9. Admin Dashboard
10. Payments

---

## 14. Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core Web API |
| Frontend | Vanilla JavaScript (HTML/CSS/JS) |
| Database | SQL Server with Entity Framework Core |
| Auth | ASP.NET Identity + JWT |
| File Storage | Cloudinary (videos, images), cloud storage for PDFs |
| Payments | Stripe + Paymob |

---

## 15. Resolved Decisions

* ✅ Instructors require **admin approval** to publish courses
* ✅ Instructor accounts require **admin approval** after registration
* ✅ Certificates require **all lessons completed + final exam passed**
* ✅ Lesson comments visible to **all students** (not just enrolled)
* ✅ Course access is **one-time purchase** (no subscription in V1)
* ✅ Both **free and paid** courses are supported
* ✅ Payment via **Stripe and Paymob**
* ✅ Videos stored on **Cloudinary**
* ✅ Grade entity covers **both exams and assignments**
* ✅ Tech stack: **ASP.NET Core + Vanilla JS + SQL Server**
* ✅ Lesson completion: **85% video watch** / PDF view
* ✅ **No assignment re-submission** allowed
* ✅ Lessons are **sequential/locked** within modules

---

## 16. Remaining Open Questions

* What scoring policy for multiple exam attempts? (Best score / Last score / Average)
* Max file upload sizes for PDFs and assignment submissions?
* Accepted file types for assignment submissions?
