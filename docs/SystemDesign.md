# LMS + Online Exams — System Design

## 1. High-Level System Architecture

```mermaid
graph TB
    subgraph Client["Client Layer (Vanilla JS)"]
        WEB["Web Application<br/>HTML / CSS / JS"]
    end

    subgraph API["API Layer (ASP.NET Core)"]
        GW["API Gateway / Middleware"]
        AUTH["Auth Controller"]
        COURSE["Course Controller"]
        LESSON["Lesson Controller"]
        ENROLL["Enrollment Controller"]
        EXAM["Exam Controller"]
        QUIZ["Quiz Controller"]
        ASSIGN["Assignment Controller"]
        CERT["Certificate Controller"]
        PAY["Payment Controller"]
        ADMIN["Admin Controller"]
        COMMENT["Comment Controller"]
        NOTIFY["Notification Controller"]
    end

    subgraph Services["Service / Application Layer"]
        AUTH_SVC["Auth Service"]
        COURSE_SVC["Course Service"]
        LESSON_SVC["Lesson Service"]
        ENROLL_SVC["Enrollment Service"]
        EXAM_SVC["Exam Service"]
        QUIZ_SVC["Quiz Service"]
        ASSIGN_SVC["Assignment Service"]
        CERT_SVC["Certificate Service"]
        PAY_SVC["Payment Service"]
        PROGRESS_SVC["Progress Service"]
        ADMIN_SVC["Admin Service"]
        COMMENT_SVC["Comment Service"]
        NOTIFY_SVC["Notification Service"]
        AUDIT_SVC["Audit Service"]
    end

    subgraph Data["Data Layer"]
        DB[("SQL Server<br/>EF Core")]
        CLOUD["Cloudinary<br/>(Videos / Images)"]
        FILE["File Storage<br/>(PDFs / Docs)"]
    end

    subgraph External["External Services"]
        STRIPE["Stripe API"]
        PAYMOB["Paymob API"]
        EMAIL["Email Service<br/>(SMTP / SendGrid)"]
    end

    WEB <-->|HTTP / REST| GW
    GW --> AUTH & COURSE & LESSON & ENROLL & EXAM & QUIZ & ASSIGN & CERT & PAY & ADMIN & COMMENT & NOTIFY

    AUTH --> AUTH_SVC
    COURSE --> COURSE_SVC
    LESSON --> LESSON_SVC
    ENROLL --> ENROLL_SVC
    EXAM --> EXAM_SVC
    QUIZ --> QUIZ_SVC
    ASSIGN --> ASSIGN_SVC
    CERT --> CERT_SVC
    PAY --> PAY_SVC
    ADMIN --> ADMIN_SVC
    COMMENT --> COMMENT_SVC
    NOTIFY --> NOTIFY_SVC

    AUTH_SVC & COURSE_SVC & LESSON_SVC & ENROLL_SVC & EXAM_SVC & QUIZ_SVC & ASSIGN_SVC & CERT_SVC & PAY_SVC & PROGRESS_SVC & ADMIN_SVC & COMMENT_SVC & NOTIFY_SVC & AUDIT_SVC --> DB

    LESSON_SVC --> CLOUD
    COURSE_SVC --> CLOUD
    ASSIGN_SVC --> FILE
    LESSON_SVC --> FILE
    PAY_SVC --> STRIPE & PAYMOB
    AUTH_SVC --> EMAIL
    NOTIFY_SVC --> EMAIL
    CERT_SVC --> FILE

    style Client fill:#1e293b,stroke:#38bdf8,color:#f8fafc
    style API fill:#1e293b,stroke:#818cf8,color:#f8fafc
    style Services fill:#1e293b,stroke:#a78bfa,color:#f8fafc
    style Data fill:#1e293b,stroke:#34d399,color:#f8fafc
    style External fill:#1e293b,stroke:#fb923c,color:#f8fafc
```

---

## 2. Entity Relationship Diagram

```mermaid
erDiagram
    User ||--o{ Enrollment : "enrolls in"
    User ||--o{ Course : "teaches"
    User ||--o{ Comment : "writes"
    User ||--o{ Payment : "makes"
    User ||--o{ Certificate : "earns"
    User ||--o{ Notification : "receives"
    User ||--o{ ExamAttempt : "takes"
    User ||--o{ QuizAttempt : "takes"
    User ||--o{ Submission : "submits"
    User }o--|| Role : "has"

    Category ||--o{ Course : "contains"

    Course ||--o{ Module : "has"
    Course ||--o{ Enrollment : "has"
    Course ||--o{ Exam : "has"
    Course ||--o{ Certificate : "awards"
    Course ||--o{ Payment : "paid for"

    Module ||--o{ Lesson : "contains"

    Lesson ||--o{ LessonContent : "has"
    Lesson ||--o{ Comment : "has"
    Lesson ||--o{ LessonProgress : "tracked by"
    Lesson ||--o| Quiz : "may have"
    Lesson ||--o| Assignment : "may have"

    Quiz ||--o{ Question : "has"
    Quiz ||--o{ QuizAttempt : "attempted in"

    Exam ||--o{ Question : "has"
    Exam ||--o{ ExamAttempt : "attempted in"

    ExamAttempt ||--o{ StudentAnswer : "contains"
    QuizAttempt ||--o{ StudentAnswer : "contains"

    Question ||--o{ StudentAnswer : "answered in"

    Assignment ||--o{ Submission : "receives"

    Submission ||--o| Grade : "graded as"
    ExamAttempt ||--o| Grade : "graded as"

    Enrollment ||--o{ LessonProgress : "tracks"

    User {
        guid Id PK
        string FirstName
        string LastName
        string Email
        string PasswordHash
        string ProfileImageUrl
        string Status "Active/Pending/Suspended"
        datetime CreatedAt
        datetime UpdatedAt
    }

    Role {
        int Id PK
        string Name "Admin/Instructor/Student"
    }

    Category {
        int Id PK
        string Name
        string Description
        string ImageUrl
        bool IsActive
    }

    Course {
        guid Id PK
        guid InstructorId FK
        int CategoryId FK
        string Title
        string Description
        string ThumbnailUrl
        decimal Price "0 for free"
        string Status "Draft/Pending/Approved/Rejected/Published/Unpublished"
        string RejectionReason
        int TotalLessons
        datetime CreatedAt
        datetime PublishedAt
    }

    Module {
        int Id PK
        guid CourseId FK
        string Title
        string Description
        int OrderIndex
    }

    Lesson {
        guid Id PK
        int ModuleId FK
        string Title
        string Description
        int OrderIndex
        string ContentType "Video/PDF/YouTube"
        int DurationMinutes
    }

    LessonContent {
        int Id PK
        guid LessonId FK
        string Type "Video/PDF/YouTube"
        string Url "Cloudinary or file URL"
        string FileName
        long FileSize
    }

    LessonProgress {
        int Id PK
        guid EnrollmentId FK
        guid LessonId FK
        decimal WatchPercentage
        bool IsCompleted
        datetime CompletedAt
        datetime LastAccessedAt
    }

    Quiz {
        int Id PK
        guid LessonId FK
        string Title
        bool ShowResultImmediately
    }

    Exam {
        guid Id PK
        guid CourseId FK
        string Title
        int TimeLimitMinutes
        decimal PassingGrade
        int MaxAttempts
        bool ShuffleQuestions
        bool ShuffleChoices
    }

    Question {
        guid Id PK
        int QuizId FK "nullable"
        guid ExamId FK "nullable"
        string Text
        string Type "MCQ/TrueFalse"
        string ImageUrl "nullable"
        int OrderIndex
        decimal Points
    }

    QuizAttempt {
        guid Id PK
        int QuizId FK
        guid StudentId FK
        decimal Score
        datetime StartedAt
        datetime CompletedAt
    }

    ExamAttempt {
        guid Id PK
        guid ExamId FK
        guid StudentId FK
        int AttemptNumber
        decimal Score
        bool Passed
        datetime StartedAt
        datetime SubmittedAt
        bool AutoSubmitted
    }

    StudentAnswer {
        int Id PK
        guid QuizAttemptId FK "nullable"
        guid ExamAttemptId FK "nullable"
        guid QuestionId FK
        string SelectedAnswer
        bool IsCorrect
    }

    Assignment {
        guid Id PK
        guid LessonId FK
        string Title
        string Instructions
        string AttachmentUrl
        datetime Deadline
        decimal MaxGrade
    }

    Submission {
        guid Id PK
        guid AssignmentId FK
        guid StudentId FK
        string FileUrl
        string FileName
        datetime SubmittedAt
    }

    Grade {
        int Id PK
        guid SubmissionId FK "nullable"
        guid ExamAttemptId FK "nullable"
        guid GradedById FK
        decimal Score
        decimal MaxScore
        string Feedback
        datetime GradedAt
    }

    Comment {
        guid Id PK
        guid LessonId FK
        guid UserId FK
        guid ParentCommentId FK "nullable - for replies"
        string Content
        bool IsPinned
        datetime CreatedAt
    }

    Enrollment {
        guid Id PK
        guid StudentId FK
        guid CourseId FK
        guid PaymentId FK "nullable - null for free"
        decimal ProgressPercentage
        bool IsCompleted
        datetime EnrolledAt
        datetime CompletedAt
    }

    Payment {
        guid Id PK
        guid StudentId FK
        guid CourseId FK
        decimal Amount
        string Currency
        string Provider "Stripe/Paymob"
        string ProviderTransactionId
        string Status "Pending/Success/Failed"
        datetime CreatedAt
    }

    Certificate {
        guid Id PK
        guid StudentId FK
        guid CourseId FK
        string CertificateNumber "unique"
        string PdfUrl
        string QrCodeData
        datetime IssuedAt
    }

    Notification {
        int Id PK
        guid UserId FK
        string Title
        string Message
        string Type "Enrollment/Grade/CourseApproval/etc"
        bool IsRead
        datetime CreatedAt
    }

    AuditLog {
        long Id PK
        guid UserId FK "nullable"
        string Action
        string EntityType
        string EntityId
        string OldValues "JSON"
        string NewValues "JSON"
        datetime Timestamp
    }
```

---

## 3. Course State Machine

```mermaid
stateDiagram-v2
    [*] --> Draft : Instructor creates course

    Draft --> PendingApproval : Instructor submits for review
    Draft --> Draft : Instructor edits

    PendingApproval --> Published : Admin approves
    PendingApproval --> Rejected : Admin rejects (with feedback)

    Rejected --> Draft : Instructor edits and resubmits

    Published --> Unpublished : Admin unpublishes
    Unpublished --> PendingApproval : Instructor resubmits

    Published --> [*] : Active course

    state Draft {
        [*] --> Editing
        Editing --> AddingModules
        AddingModules --> AddingLessons
        AddingLessons --> UploadingContent
        UploadingContent --> Ready
    }

    note right of PendingApproval
        Admin reviews course content,
        structure, and quality
    end note

    note right of Rejected
        Includes rejection reason
        for instructor feedback
    end note
```

---

## 4. Instructor Account Approval Flow

```mermaid
stateDiagram-v2
    [*] --> Registered : User registers as Instructor

    Registered --> PendingApproval : Account created in pending state

    PendingApproval --> Approved : Admin approves
    PendingApproval --> Rejected : Admin rejects

    Approved --> Active : Instructor gains full access
    Rejected --> [*] : Account rejected

    Active --> Suspended : Admin suspends
    Suspended --> Active : Admin reactivates

    note right of PendingApproval
        Instructor can log in but
        cannot create courses
    end note

    note right of Active
        Full access to create
        and manage courses
    end note
```

---

## 5. Student Enrollment & Learning Flow

```mermaid
flowchart TB
    START(("Student browses courses")) --> BROWSE["View Course Catalog"]
    BROWSE --> DETAIL["View Course Details"]
    DETAIL --> CHECK{"Free or Paid?"}

    CHECK -->|Free| ENROLL_FREE["Enroll Directly"]
    CHECK -->|Paid| PAY_SELECT{"Select Payment"}

    PAY_SELECT -->|Stripe| STRIPE["Stripe Checkout"]
    PAY_SELECT -->|Paymob| PAYMOB["Paymob Checkout"]

    STRIPE --> WEBHOOK_S{"Webhook: Success?"}
    PAYMOB --> WEBHOOK_P{"Webhook: Success?"}

    WEBHOOK_S -->|Yes| ENROLL_PAID["Create Enrollment"]
    WEBHOOK_S -->|No| FAIL["Payment Failed"]
    WEBHOOK_P -->|Yes| ENROLL_PAID
    WEBHOOK_P -->|No| FAIL

    FAIL --> PAY_SELECT

    ENROLL_FREE --> ENROLLED["Student Enrolled"]
    ENROLL_PAID --> ENROLLED

    ENROLLED --> MODULE["Open Module 1"]
    MODULE --> LESSON["Open Lesson (Sequential)"]

    LESSON --> CONTENT{"Content Type?"}
    CONTENT -->|Video| VIDEO["Watch Video"]
    CONTENT -->|PDF| PDF["View PDF"]
    CONTENT -->|YouTube| YT["Watch YouTube"]

    VIDEO --> WATCH{"Watched 85%?"}
    WATCH -->|Yes| COMPLETE["Mark Lesson Complete"]
    WATCH -->|No| VIDEO

    PDF --> COMPLETE
    YT --> WATCH

    COMPLETE --> HAS_QUIZ{"Has Quiz?"}
    HAS_QUIZ -->|Yes| QUIZ["Take Quiz (Self-Assessment)"]
    HAS_QUIZ -->|No| NEXT

    QUIZ --> NEXT{"More Lessons?"}
    NEXT -->|Yes| LESSON
    NEXT -->|No| ALL_DONE["All Lessons Complete"]

    ALL_DONE --> EXAM{"Has Final Exam?"}
    EXAM -->|Yes| TAKE_EXAM["Take Final Exam"]
    TAKE_EXAM --> PASSED{"Passed?"}
    PASSED -->|Yes| CERT["🎓 Generate Certificate"]
    PASSED -->|No| RETRY{"Attempts Left?"}
    RETRY -->|Yes| TAKE_EXAM
    RETRY -->|No| LOCKED["Certificate Locked"]

    style START fill:#1e293b,stroke:#38bdf8,color:#f8fafc
    style CERT fill:#065f46,stroke:#34d399,color:#f8fafc
    style FAIL fill:#7f1d1d,stroke:#f87171,color:#f8fafc
    style LOCKED fill:#7f1d1d,stroke:#f87171,color:#f8fafc
    style ENROLLED fill:#1e3a5f,stroke:#38bdf8,color:#f8fafc
```

---

## 6. Exam Taking Flow

```mermaid
flowchart TB
    START(("Student opens exam")) --> CHECK{"Attempts remaining?"}

    CHECK -->|No| BLOCKED["Max Attempts Reached"]
    CHECK -->|Yes| BEGIN["Start Exam"]

    BEGIN --> TIMER["⏱️ Start Countdown Timer"]
    TIMER --> QUESTION["Display Question"]

    QUESTION --> SHUFFLED{"Shuffle enabled?"}
    SHUFFLED -->|Yes| RANDOMIZE["Randomize Questions & Choices"]
    SHUFFLED -->|No| ORDERED["Show in Order"]

    RANDOMIZE --> DISPLAY["Show Question"]
    ORDERED --> DISPLAY

    DISPLAY --> ANSWER["Student Selects Answer"]
    ANSWER --> SAVE["Save StudentAnswer"]

    SAVE --> MORE{"More Questions?"}
    MORE -->|Yes| QUESTION
    MORE -->|No| SUBMIT_CHECK{"Manual Submit<br/>or Time Up?"}

    SUBMIT_CHECK -->|Manual| SUBMIT["Submit Exam"]
    SUBMIT_CHECK -->|Time Up| AUTO["Auto-Submit"]

    TIMER -->|Time expires| AUTO

    SUBMIT --> GRADE["Calculate Score"]
    AUTO --> GRADE

    GRADE --> RESULT{"Score >= Passing Grade?"}
    RESULT -->|Yes| PASS["✅ Exam Passed"]
    RESULT -->|No| FAIL_EXAM["❌ Exam Failed"]

    PASS --> RECORD["Record ExamAttempt + Grade"]
    FAIL_EXAM --> RECORD

    RECORD --> NOTIFY["Send Notification"]

    style START fill:#1e293b,stroke:#38bdf8,color:#f8fafc
    style PASS fill:#065f46,stroke:#34d399,color:#f8fafc
    style FAIL_EXAM fill:#7f1d1d,stroke:#f87171,color:#f8fafc
    style BLOCKED fill:#7f1d1d,stroke:#f87171,color:#f8fafc
    style TIMER fill:#78350f,stroke:#fbbf24,color:#f8fafc
```

---

## 7. Project Folder Structure

```mermaid
graph LR
    ROOT["LMS/"] --> SLN["LMS.sln"]

    ROOT --> API["LMS.API/"]
    ROOT --> APP["LMS.Application/"]
    ROOT --> DOMAIN["LMS.Domain/"]
    ROOT --> INFRA["LMS.Infrastructure/"]
    ROOT --> WEB["LMS.Web/"]

    subgraph Domain["LMS.Domain (Core)"]
        D_ENT["Entities/"]
        D_ENUM["Enums/"]
        D_INT["Interfaces/"]
        D_VO["ValueObjects/"]
    end

    subgraph Application["LMS.Application (Use Cases)"]
        A_SVC["Services/"]
        A_DTO["DTOs/"]
        A_INT["Interfaces/"]
        A_MAP["Mappings/"]
        A_VAL["Validators/"]
    end

    subgraph Infrastructure["LMS.Infrastructure (Data + External)"]
        I_DATA["Data/"]
        I_REPO["Repositories/"]
        I_EXT["ExternalServices/"]
        I_SEED["Seeding/"]
        I_CFG["Configurations/"]
    end

    subgraph APILayer["LMS.API (Controllers + Config)"]
        API_CTRL["Controllers/"]
        API_MID["Middleware/"]
        API_FLT["Filters/"]
    end

    subgraph WebLayer["LMS.Web (Frontend)"]
        W_CSS["css/"]
        W_JS["js/"]
        W_IMG["assets/"]
        W_HTML["pages/"]
    end

    DOMAIN --> D_ENT & D_ENUM & D_INT & D_VO
    APP --> A_SVC & A_DTO & A_INT & A_MAP & A_VAL
    INFRA --> I_DATA & I_REPO & I_EXT & I_SEED & I_CFG
    API --> API_CTRL & API_MID & API_FLT
    WEB --> W_CSS & W_JS & W_IMG & W_HTML

    style ROOT fill:#1e293b,stroke:#38bdf8,color:#f8fafc
    style Domain fill:#1e293b,stroke:#818cf8,color:#f8fafc
    style Application fill:#1e293b,stroke:#a78bfa,color:#f8fafc
    style Infrastructure fill:#1e293b,stroke:#34d399,color:#f8fafc
    style APILayer fill:#1e293b,stroke:#fb923c,color:#f8fafc
    style WebLayer fill:#1e293b,stroke:#f472b6,color:#f8fafc
```

---

## 8. API Endpoint Map

```mermaid
mindmap
    root((LMS API))
        Auth
            POST /api/auth/register
            POST /api/auth/login
            POST /api/auth/logout
            POST /api/auth/forgot-password
            POST /api/auth/reset-password
            POST /api/auth/verify-email
        Courses
            GET /api/courses
            GET /api/courses/:id
            POST /api/courses
            PUT /api/courses/:id
            DELETE /api/courses/:id
            POST /api/courses/:id/submit
            POST /api/courses/:id/approve
            POST /api/courses/:id/reject
        Modules
            POST /api/courses/:id/modules
            PUT /api/modules/:id
            DELETE /api/modules/:id
        Lessons
            POST /api/modules/:id/lessons
            PUT /api/lessons/:id
            DELETE /api/lessons/:id
            POST /api/lessons/:id/progress
        Enrollment
            POST /api/courses/:id/enroll
            GET /api/enrollments
            GET /api/enrollments/:id/progress
        Exams
            POST /api/courses/:id/exams
            GET /api/exams/:id
            POST /api/exams/:id/start
            POST /api/exam-attempts/:id/submit
        Quizzes
            POST /api/lessons/:id/quiz
            POST /api/quizzes/:id/attempt
        Assignments
            POST /api/lessons/:id/assignment
            POST /api/assignments/:id/submit
            POST /api/submissions/:id/grade
        Certificates
            GET /api/certificates/:id
            GET /api/certificates/verify/:number
        Payments
            POST /api/payments/checkout
            POST /api/payments/webhook/stripe
            POST /api/payments/webhook/paymob
            GET /api/payments/history
        Admin
            GET /api/admin/dashboard
            GET /api/admin/users
            PUT /api/admin/users/:id/status
            GET /api/admin/reports
        Comments
            GET /api/lessons/:id/comments
            POST /api/lessons/:id/comments
            PUT /api/comments/:id/pin
            DELETE /api/comments/:id
        Notifications
            GET /api/notifications
            PUT /api/notifications/:id/read
```
