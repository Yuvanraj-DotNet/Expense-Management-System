using Expense_Management_System.Data;
using Expense_Management_System.DTOs.Expense;
using Expense_Management_System.DTOs.Reports;
using System.Text.RegularExpressions;
using System.Text;

namespace Expense_Management_System.Services.Expense
{
    public class ExpenseService : IExpenseService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ExpenseService(
               ApplicationDbContext context,
               IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public string CreateExpense(CreateExpenseDto createExpenseDto, int userId)
        {
            // User Exists
            var user = _context.Users
             .FirstOrDefault(u => u.Id == userId);

            if (user.ManagerId == null)
            {
                return "Employee is not assigned to any Manager";
            }

            if (user == null)
            {
                return "User Not Found";
            }

            // Only Employees can create Expense
            if (user.RoleId != 1)
            {
                return "Only Employees can create expenses";
            }

            // Category Exists
            var category = _context.ExpenseCategories
                .FirstOrDefault(c => c.Id == createExpenseDto.CategoryId);

            if (category == null)
            {
                return "Expense Category Not Found";
            }

            // Amount > 0
            if (createExpenseDto.Amount <= 0)
            {
                return "Amount must be greater than 0";
            }

            // Category Limit
            if (createExpenseDto.Amount > category.MaxAllowedAmount)
            {
                return $"Maximum allowed amount for {category.Name} is {category.MaxAllowedAmount}";
            }

            // Future Date
            if (createExpenseDto.ExpenseDate.Date > DateTime.Today)
            {
                return "Expense Date cannot be in the future";
            }

            // Receipt Validation
            if (createExpenseDto.Receipt == null || createExpenseDto.Receipt.Length == 0)
            {
                return "Receipt is required";
            }

            // Allowed File Types
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };

            var extension = Path.GetExtension(createExpenseDto.Receipt.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                return "Only JPG, JPEG, PNG and PDF files are allowed";
            }

            // Maximum File Size (5 MB)
            if (createExpenseDto.Receipt.Length > 5 * 1024 * 1024)
            {
                return "Receipt file size cannot exceed 5 MB";
            }

            // Create Receipts Folder
            var receiptsFolder = Path.Combine(_environment.WebRootPath, "Receipts");

            if (!Directory.Exists(receiptsFolder))
            {
                Directory.CreateDirectory(receiptsFolder);
            }

            // Generate Unique File Name
            var fileName = Guid.NewGuid().ToString() +
            Path.GetExtension(createExpenseDto.Receipt.FileName);



            // Full File Path
            var filePath = Path.Combine(receiptsFolder, fileName);

            // Save Receipt File
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                createExpenseDto.Receipt.CopyTo(stream);
            }

            var expense = new Models.Expense
            {
                UserId = userId,
                CategoryId = createExpenseDto.CategoryId,
                Title = createExpenseDto.Title.Trim(),
                Amount = createExpenseDto.Amount,
                ExpenseDate = createExpenseDto.ExpenseDate,
                Description = createExpenseDto.Description.Trim(),
                ReceiptPath = fileName
            };

            _context.Expenses.Add(expense);
            _context.SaveChanges();

            return "Expense Created Successfully";
        }

        public string UpdateExpense(int id, int userId, UpdateExpenseDto updateExpenseDto)
        {
            // Expense Exists
            var expense = _context.Expenses
                .FirstOrDefault(e => e.Id == id);

            if (expense == null)
            {
                return "Expense Not Found";
            }

            // Only the owner can update the expense
            if (expense.UserId != userId)
            {
                return "You are not allowed to update this expense";
            }

            // Only Draft Expenses Can Be Updated
            if (expense.Status != "Draft")
            {
                return "Only Draft Expenses Can Be Updated";
            }

            // Category Exists
            var category = _context.ExpenseCategories
                .FirstOrDefault(c => c.Id == updateExpenseDto.CategoryId);

            if (category == null)
            {
                return "Expense Category Not Found";
            }

            // Amount Validation
            if (updateExpenseDto.Amount <= 0)
            {
                return "Amount must be greater than 0";
            }

            // Category Maximum Limit
            if (updateExpenseDto.Amount > category.MaxAllowedAmount)
            {
                return $"Maximum allowed amount for {category.Name} is {category.MaxAllowedAmount}";
            }

            // Future Date Validation
            if (updateExpenseDto.ExpenseDate.Date > DateTime.Today)
            {
                return "Expense Date cannot be in the future";
            }

            // ===========================
            // Receipt Upload (Optional)
            // ===========================

            if (updateExpenseDto.Receipt != null)
            {
                // Allowed File Types
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };

                var extension = Path.GetExtension(updateExpenseDto.Receipt.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    return "Only JPG, JPEG, PNG and PDF files are allowed";
                }

                // Maximum File Size (5 MB)
                if (updateExpenseDto.Receipt.Length > 5 * 1024 * 1024)
                {
                    return "Receipt file size cannot exceed 5 MB";
                }

                // Receipts Folder
                var receiptsFolder = Path.Combine(_environment.WebRootPath!, "Receipts");

                if (!Directory.Exists(receiptsFolder))
                {
                    Directory.CreateDirectory(receiptsFolder);
                }

                // Delete Old Receipt
                if (!string.IsNullOrWhiteSpace(expense.ReceiptPath))
                {
                    var oldFile = Path.Combine(receiptsFolder, expense.ReceiptPath);

                    if (File.Exists(oldFile))
                    {
                        File.Delete(oldFile);
                    }
                }

                // Generate New File Name
                var fileName = Guid.NewGuid().ToString() +
                    Path.GetExtension(updateExpenseDto.Receipt.FileName);

                // Save New Receipt
                var filePath = Path.Combine(receiptsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    updateExpenseDto.Receipt.CopyTo(stream);
                }

                expense.ReceiptPath = fileName;
            }

            // ===========================
            // Update Expense
            // ===========================

            expense.CategoryId = updateExpenseDto.CategoryId;
            expense.Title = updateExpenseDto.Title.Trim();
            expense.Amount = updateExpenseDto.Amount;
            expense.ExpenseDate = updateExpenseDto.ExpenseDate;
            expense.Description = updateExpenseDto.Description.Trim();

            _context.SaveChanges();

            return "Expense Updated Successfully";
        }

        public List<ExpenseResponseDto> GetMyExpenses(int userId)
        {
            var expenses = (from expense in _context.Expenses

                            join approval in _context.ExpenseApprovals
                            on expense.Id equals approval.ExpenseId into approvalGroup

                            from approval in approvalGroup
                                .OrderByDescending(a => a.ActionDate)
                                .Take(1)
                                .DefaultIfEmpty()

                            where expense.UserId == userId

                            select new ExpenseResponseDto
                            {
                                Id = expense.Id,
                                Title = expense.Title,
                                Amount = expense.Amount,
                                Status = expense.Status,
                                ExpenseDate = expense.ExpenseDate,

                                ManagerComment = approval != null
                                    ? approval.Comment
                                    : null,

                                ActionDate = approval != null
                                    ? approval.ActionDate
                                    : null
                            }).ToList();

            return expenses;
        }

        public List<ExpenseListResponseDto> GetAllExpenses
             (
                string? search,
               int pageNumber,
               int pageSize,
               out int totalRecords
             )

        {
            var query =

                from expense in _context.Expenses

                join user in _context.Users
                    on expense.UserId equals user.Id

                join category in _context.ExpenseCategories
                    on expense.CategoryId equals category.Id

                select new ExpenseListResponseDto
                {
                    Id = expense.Id,

                    EmployeeName = user.Name,

                    CategoryName = category.Name,

                    Title = expense.Title,

                    Amount = expense.Amount,

                    ExpenseDate = expense.ExpenseDate,

                    Status = expense.Status
                };

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                query = query.Where(e =>

                    e.Id.ToString().Contains(search) ||

                    e.EmployeeName.ToLower().Contains(search) ||

                    e.CategoryName.ToLower().Contains(search) ||

                    e.Title.ToLower().Contains(search) ||

                    e.Status.ToLower().Contains(search) ||

                    e.Amount.ToString().Contains(search)
                );
            }

            // Total Records
            totalRecords = query.Count();

            // Pagination
            var expenses = query

               .OrderBy(e => e.Id)

                .Skip((pageNumber - 1) * pageSize)

                .Take(pageSize)

                .ToList();

            return expenses;
        }



        public string SubmitExpense(int id, int userId)
        {
            var expense = _context.Expenses.FirstOrDefault(e => e.Id == id);

            if (expense == null)
            {
                return "Expense Not Found";
            }

            if (expense.UserId != userId)
            {
                return "You are not allowed to submit this expense";
            }

            if (expense.Status != "Draft")
            {
                return "Only Draft Expenses Can Be Submitted";
            }

            if (string.IsNullOrWhiteSpace(expense.ReceiptPath))
            {
                return "Receipt is required before submitting.";
            }

            expense.Status = "Submitted";

            _context.SaveChanges();

            return "Expense Submitted Successfully";
        }

        public List<ExpenseResponseDto> GetPendingApprovals(int managerId)
        {
            var manager = _context.Users
                .FirstOrDefault(u => u.Id == managerId);

            // Manager must exist
            if (manager == null)
            {
                return new List<ExpenseResponseDto>();
            }

            // Only Managers can view pending approvals
            if (manager.RoleId != 2)
            {
                return new List<ExpenseResponseDto>();
            }

            var expenses = (from expense in _context.Expenses
                            join user in _context.Users
                                on expense.UserId equals user.Id
                            where expense.Status == "Submitted"
                                  && user.DepartmentId == manager.DepartmentId
                            select new ExpenseResponseDto
                            {
                                Id = expense.Id,
                                Title = expense.Title,
                                Amount = expense.Amount,
                                Status = expense.Status,
                                ExpenseDate = expense.ExpenseDate
                            }).ToList();

            return expenses;
        }

        public string ApproveExpense(int id, int managerId, ApproveExpenseDto approveExpenseDto)
        {
            // Expense Exists
            var expense = _context.Expenses
                .FirstOrDefault(e => e.Id == id);

            if (expense == null)
            {
                return "Expense Not Found";
            }

            // Expense Status Validation
            if (expense.Status == "Approved")
            {
                return "Expense is already approved";
            }

            if (expense.Status == "Rejected")
            {
                return "Rejected expenses cannot be approved";
            }

            if (expense.Status == "Reimbursed")
            {
                return "Reimbursed expenses cannot be approved";
            }

            if (expense.Status != "Submitted")
            {
                return "Only submitted expenses can be approved";
            }

            // Manager Exists
            var manager = _context.Users
                  .FirstOrDefault(u => u.Id == managerId);

            if (manager == null)
            {
                return "Manager Not Found";
            }

            // Only Department Managers
            if (manager.RoleId != 2)
            {
                return "Only Department Managers Can Approve Expenses";
            }

            // Employee Exists
            var employee = _context.Users
                .FirstOrDefault(u => u.Id == expense.UserId);

            if (employee == null)
            {
                return "Employee Not Found";
            }

            // Same Department Validation
            if (employee.DepartmentId != manager.DepartmentId)
            {
                return "You are not authorized to approve this employee's expense";
            }

            // Approve Expense
            expense.Status = "Approved";


            // Save Approval History
            var approval = new Models.ExpenseApproval
            {
                ExpenseId = expense.Id,
                ApproverId = managerId,
                Action = "Approved",
                Comment = string.IsNullOrWhiteSpace(approveExpenseDto.Comment)
                    ? "Approved by Department Manager"
                    : approveExpenseDto.Comment.Trim(),
                ActionDate = DateTime.Now
            };

            _context.ExpenseApprovals.Add(approval);

            _context.SaveChanges();

            return "Expense Approved Successfully";
        }


        public string RejectExpense(
                                    int id,
                                    int managerId,
                             RejectExpenseDto rejectExpenseDto)
        {
            // Expense Exists
            var expense = _context.Expenses
                .FirstOrDefault(e => e.Id == id);

            if (expense == null)
            {
                return "Expense Not Found";
            }

            // Expense Status Validation
            if (expense.Status == "Rejected")
            {
                return "Expense is already rejected";
            }

            if (expense.Status == "Approved")
            {
                return "Approved expenses cannot be rejected";
            }

            if (expense.Status == "Reimbursed")
            {
                return "Reimbursed expenses cannot be rejected";
            }

            if (expense.Status != "Submitted")
            {
                return "Only submitted expenses can be rejected";
            }

            // Reject Reason Mandatory
            if (string.IsNullOrWhiteSpace(rejectExpenseDto.Comment))
            {
                return "Reject Reason is Required";
            }

            // Manager Exists
            var manager = _context.Users
            .FirstOrDefault(u => u.Id == managerId);

            if (manager == null)
            {
                return "Manager Not Found";
            }

            // Only Department Managers
            if (manager.RoleId != 2)
            {
                return "Only Department Managers Can Reject Expenses";
            }

            // Employee Exists
            var employee = _context.Users
                .FirstOrDefault(u => u.Id == expense.UserId);

            if (employee == null)
            {
                return "Employee Not Found";
            }

            // Same Department Validation
            if (employee.DepartmentId != manager.DepartmentId)
            {
                return "You are not authorized to reject this employee's expense";
            }

            // Reject Expense
            expense.Status = "Rejected";

            // Save Approval History
            var approval = new Models.ExpenseApproval
            {
                ExpenseId = expense.Id,
                ApproverId = managerId,
                Action = "Rejected",
                Comment = rejectExpenseDto.Comment.Trim(),
                ActionDate = DateTime.Now
            };

            _context.ExpenseApprovals.Add(approval);

            _context.SaveChanges();

            return "Expense Rejected Successfully";
        }


        public List<ApprovedExpenseDto> GetApprovedExpenses()
        {
            var approvedExpenses = (from expense in _context.Expenses
                                    join user in _context.Users
                                        on expense.UserId equals user.Id
                                    join category in _context.ExpenseCategories
                                        on expense.CategoryId equals category.Id
                                    join approval in _context.ExpenseApprovals
                                        on expense.Id equals approval.ExpenseId
                                    where expense.Status == "Approved"
                                      && approval.Action == "Approved"
                                    select new ApprovedExpenseDto
                                    {
                                        ExpenseId = expense.Id,
                                        ApproverId = approval.ApproverId,
                                        EmployeeName = user.Name,
                                        CategoryName = category.Name,
                                        Amount = expense.Amount,
                                        ExpenseDate = expense.ExpenseDate,
                                        Status = expense.Status
                                    }).ToList();

            return approvedExpenses;
        }


        public List<ReimbursementResponseDto> GetAllReimbursements
             (
                   string? search,
                   int pageNumber,
                   int pageSize,
                   out int totalRecords
             )

        {
            var query =
                from reimbursement in _context.Reimbursements

                join expense in _context.Expenses
                    on reimbursement.ExpenseId equals expense.Id

                join employee in _context.Users
                    on expense.UserId equals employee.Id

                join finance in _context.Users
                    on reimbursement.ProcessedBy equals finance.Id

                select new ReimbursementResponseDto
                {
                    ReimbursementId = reimbursement.Id,
                    ExpenseId = reimbursement.ExpenseId,
                    EmployeeName = employee.Name,
                    Amount = reimbursement.Amount,
                    PaymentDate = reimbursement.PaymentDate,
                    ReferenceNumber = reimbursement.ReferenceNumber,
                    ProcessedBy = finance.Name
                };

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                query = query.Where(r =>
                    r.ReimbursementId.ToString().Contains(search) ||
                    r.ExpenseId.ToString().Contains(search) ||
                    r.EmployeeName.ToLower().Contains(search) ||
                    r.ReferenceNumber.ToLower().Contains(search) ||
                    r.Amount.ToString().Contains(search) ||
                    r.ProcessedBy.ToLower().Contains(search));
            }

            // Total Records
            totalRecords = query.Count();

            // Pagination
            return query
                .OrderBy(r => r.ReimbursementId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public string ReimburseExpense( int id, ReimburseExpenseDto reimburseExpenseDto,
                                        int financeUserId)
        {
            var expense = _context.Expenses
                .FirstOrDefault(e => e.Id == id);

            if (expense == null)
            {
                return "Expense Not Found";
            }

            // Expense Status Validation
            if (expense.Status == "Reimbursed")
            {
                return "Expense has already been reimbursed";
            }

            if (expense.Status == "Rejected")
            {
                return "Rejected expenses cannot be reimbursed";
            }

            if (expense.Status == "Submitted")
            {
                return "Expense must be approved before reimbursement";
            }

            if (expense.Status == "Draft")
            {
                return "Draft expenses cannot be reimbursed";
            }

            if (expense.Status != "Approved")
            {
                return "Only approved expenses can be reimbursed";
            }

            // Finance User Exists
            var financeUser = _context.Users
              .FirstOrDefault(u => u.Id == financeUserId);

            if (financeUser == null)
            {
                return "Finance User Not Found";
            }

            // Only Finance Can Reimburse
            if (financeUser.RoleId != 3)
            {
                return "Only Finance Users Can Reimburse Expenses";
            }

            // Reference Number Validation
            if (string.IsNullOrWhiteSpace(reimburseExpenseDto.ReferenceNumber))
            {
                return "Reference Number is Required";
            }

            if (!Regex.IsMatch(reimburseExpenseDto.ReferenceNumber,
                @"^(BANK|NEFT|RTGS|IMPS|UPI)\d{6,12}$"))
            {
                return "Enter a valid payment reference number";
            }

            var reimbursement = new Models.Reimbursement
            {
                ExpenseId = expense.Id,
                ProcessedBy = financeUserId,
                PaymentDate = DateTime.Now,
                ReferenceNumber = reimburseExpenseDto.ReferenceNumber.Trim(),
                Amount = expense.Amount
            };

            _context.Reimbursements.Add(reimbursement);

            expense.Status = "Reimbursed";

            _context.SaveChanges();

            return "Expense Reimbursed Successfully";
        }

        public List<MonthlyReportDto> GetMonthlyReport(int month, int year)
        {
            if (month < 1 || month > 12)
            {
                return new List<MonthlyReportDto>();
            }

            if (year < 2025 || year > DateTime.Now.Year)
            {
                return new List<MonthlyReportDto>();
            }

            var monthlyReport =
                (from expense in _context.Expenses

                 join user in _context.Users
                     on expense.UserId equals user.Id

                 join department in _context.Departments
                     on user.DepartmentId equals department.Id

                 where expense.ExpenseDate.Month == month
                    && expense.ExpenseDate.Year == year
                    && expense.Status != "Draft"

                 group expense by new
                 {
                     department.Id,
                     department.Name
                 } into departmentGroup

                 select new MonthlyReportDto
                 {
                     Month = new DateTime(year, month, 1).ToString("MMMM yyyy"),

                     DepartmentName = departmentGroup.Key.Name,

                     TotalExpenses = departmentGroup.Count(),

                     TotalAmount = departmentGroup.Sum(e => e.Amount),

                     ApprovedExpenses = departmentGroup.Count(e => e.Status == "Approved"),

                     RejectedExpenses = departmentGroup.Count(e => e.Status == "Rejected"),

                     ReimbursedExpenses = departmentGroup.Count(e => e.Status == "Reimbursed")
                 }).ToList();

            return monthlyReport;
        }

        public byte[] ExportMonthlyReport(int month, int year)
        {
            var report = GetMonthlyReport(month, year);

            if (report.Count == 0)
            {
                return Array.Empty<byte>();
            }

            var csv = new StringBuilder();

            csv.AppendLine("Month,Department,TotalExpenses,TotalAmount,ApprovedExpenses,RejectedExpenses,ReimbursedExpenses");

            foreach (var item in report)
            {
                csv.AppendLine(
                    $"{item.Month}," +
                    $"{item.DepartmentName}," +
                    $"{item.TotalExpenses}," +
                    $"{item.TotalAmount}," +
                    $"{item.ApprovedExpenses}," +
                    $"{item.RejectedExpenses}," +
                    $"{item.ReimbursedExpenses}");
            }

            return Encoding.UTF8.GetBytes(csv.ToString());
        }
    }
}
