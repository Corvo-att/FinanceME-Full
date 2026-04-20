
# FinanceME Back-End: ASP.NET MVC Implementation Guide

---

## Table of Contents

1. [Project Creation (Visual Studio GUI)](https://playground.outlier.ai/conversation/69dede9f385239492963da62#1-project-creation)
2. [Solution Structure Overview](https://playground.outlier.ai/conversation/69dede9f385239492963da62#2-solution-structure-overview)
3. [NuGet Packages to Install](https://playground.outlier.ai/conversation/69dede9f385239492963da62#3-nuget-packages-to-install)
4. [Entity Definitions & Relationships](https://playground.outlier.ai/conversation/69dede9f385239492963da62#4-entity-definitions--relationships)
5. [Entity Relationship Diagram (Textual)](https://playground.outlier.ai/conversation/69dede9f385239492963da62#5-entity-relationship-diagram)
6. [DbContext Configuration](https://playground.outlier.ai/conversation/69dede9f385239492963da62#6-dbcontext-configuration)
7. [Connection String Setup](https://playground.outlier.ai/conversation/69dede9f385239492963da62#7-connection-string-setup)
8. [Identity Integration](https://playground.outlier.ai/conversation/69dede9f385239492963da62#8-identity-integration)
9. [Migrations & Database Creation](https://playground.outlier.ai/conversation/69dede9f385239492963da62#9-migrations--database-creation)
10. [Folder/File Organization](https://playground.outlier.ai/conversation/69dede9f385239492963da62#10-folderfile-organization)
11. [Controller Scaffolding Strategy](https://playground.outlier.ai/conversation/69dede9f385239492963da62#11-controller-scaffolding-strategy)
12. [Authentication & Authorization Setup](https://playground.outlier.ai/conversation/69dede9f385239492963da62#12-authentication--authorization-setup)
13. [Seed Data Strategy](https://playground.outlier.ai/conversation/69dede9f385239492963da62#13-seed-data-strategy)
14. [View Integration Plan (Your Existing Front-End)](https://playground.outlier.ai/conversation/69dede9f385239492963da62#14-view-integration-plan)
15. Step-by-Step Execution Checklist
16. [[Work Flow]]

---

## 1. Project Creation

Everything through Visual Studio's graphical interface. No terminal commands.

### Step-by-step:

1. **Open Visual Studio 2022** (Community, Professional, or Enterprise)
    
2. **Click** "Create a new project"
    
3. **In the search bar**, type ASP.NET Core Web App (Model-View-Controller)
    
4. **Select** the template that says:
    
    > ASP.NET Core Web App (Model-View-Controller) — with the C# tag
    
5. **Click** Next
    
6. **Configure your project:**
    
    - **Project name:** FinanceME
    - **Location:** Choose your preferred folder
    - **Solution name:** FinanceME (leave as-is or rename to FinanceME.Solution)
    - ☑ Check **"Place solution and project in the same directory"** only if you don't plan multiple projects in this solution (leave unchecked if you think you may add a class library later)
7. **Click** Next
    
8. **Additional information screen:**
    
    - **Framework:** .NET 8.0 (Long Term Support) — or .NET 9.0 if available and preferred
    - **Authentication type:** Individual Accounts
        
        > This is critical — it scaffolds ASP.NET Core Identity for you, giving you User registration, login, logout, password reset, and the full ApplicationDbContext wired to Identity out of the box.
        
    - **Configure for HTTPS:** ☑ Yes
    - **Enable Docker:** ☐ No (not needed)
    - **Do not use top-level statements:** Leave default (unchecked is fine)
9. **Click** Create
    

### What you now have:

Visual Studio generates a working MVC project with:

- A Data/ApplicationDbContext.cs pre-wired to Identity
- A Controllers/HomeController.cs
- A Views/ folder with Razor layout scaffolding
- Program.cs with Identity and EF Core services registered
- A connection string placeholder in appsettings.json
- Identity pages via the default Identity UI library

---

## 2. Solution Structure Overview

After creation, and after you organize your code, your project should look like this:

text

```text
FinanceME/
│
├── Controllers/
│   ├── HomeController.cs
│   ├── DashboardController.cs
│   ├── AccountsController.cs
│   ├── TransactionsController.cs
│   ├── BudgetsController.cs
│   ├── GoalsController.cs
│   ├── BillsController.cs
│   ├── ForecastController.cs
│   ├── DebtsController.cs
│   ├── SubscriptionsController.cs
│   ├── NotificationsController.cs
│   ├── AlertRulesController.cs
│   ├── ReportsController.cs
│   └── SettingsController.cs
│
├── Models/
│   ├── Entities/
│   │   ├── User.cs          (extends IdentityUser)
│   │   ├── Account.cs
│   │   ├── Category.cs
│   │   ├── Transaction.cs
│   │   ├── Budget.cs
│   │   ├── Goal.cs
│   │   ├── GoalContribution.cs
│   │   ├── Bill.cs
│   │   ├── Forecast.cs
│   │   ├── Debt.cs
│   │   ├── DebtPayment.cs       (added)
│   │   ├── Subscription.cs
│   │   ├── Notification.cs
│   │   └── AlertRule.cs
│   │
│   └── ViewModels/
│       ├── DashboardViewModel.cs
│       ├── AccountSummaryViewModel.cs
│       ├── TransactionFilterViewModel.cs
│       ├── BudgetOverviewViewModel.cs
│       ├── GoalOverviewViewModel.cs
│       └── ReportViewModel.cs
│
├── Data/
│   ├── ApplicationDbContext.cs
│   └── SeedData.cs
│
├── Views/
│   ├── Shared/
│   │   ├── _Layout.cshtml
│   │   ├── _Sidebar.cshtml
│   │   ├── _Header.cshtml
│   │   └── _Footer.cshtml
│   ├── Home/
│   ├── Dashboard/
│   ├── Accounts/
│   ├── Transactions/
│   ├── Budgets/
│   ├── Goals/
│   ├── Bills/
│   ├── Forecast/
│   ├── Debts/
│   ├── Subscriptions/
│   ├── Notifications/
│   ├── AlertRules/
│   ├── Reports/
│   └── Settings/
│
├── wwwroot/
│   ├── css/          ← your existing CSS files go here
│   ├── js/           ← your existing JS files go here
│   ├── Logo/         ← your existing logos
│   └── pictures/     ← your existing images
│
├── Enums/
│   ├── AccountType.cs
│   ├── TransactionType.cs
│   ├── BudgetPeriod.cs
│   ├── GoalStatus.cs
│   ├── BillFrequency.cs
│   ├── DebtType.cs
│   ├── SubscriptionStatus.cs
│   ├── NotificationType.cs
│   └── AlertCondition.cs
│
├── Program.cs
├── appsettings.json
└── appsettings.Development.json
```

---

## 3. NuGet Packages to Install

All through Visual Studio GUI:

**Right-click the project → Manage NuGet Packages → Browse tab**

Install these packages (search by name):

|Package|Purpose|
|---|---|
|Microsoft.AspNetCore.Identity.EntityFrameworkCore|Already included by template|
|Microsoft.EntityFrameworkCore.SqlServer|SQL Server provider|
|Microsoft.EntityFrameworkCore.Tools|Enables migrations via Package Manager Console or GUI|
|Microsoft.AspNetCore.Identity.UI|Already included by template — default Identity Razor pages|
|Microsoft.EntityFrameworkCore.Design|Design-time support for EF tools|

> **Note:** If you prefer SQLite for development simplicity, install Microsoft.EntityFrameworkCore.Sqlite instead of SqlServer and adjust the connection string accordingly.

---

## 4. Entity Definitions & Relationships

Below is every entity in full. Each property is intentional and maps directly to the finance features in your front-end.

---

### Enums (Create each as a separate file in Enums/ folder)

csharp

```csharp
// Enums/AccountType.cs
namespace FinanceME.Enums
{
    public enum AccountType
    {
        Checking,
        Savings,
        CreditCard,
        Investment,
        Loan,
        Cash,
        Other
    }
}
```

csharp

```csharp
// Enums/TransactionType.cs
namespace FinanceME.Enums
{
    public enum TransactionType
    {
        Income,
        Expense,
        Transfer
    }
}
```

csharp

```csharp
// Enums/BudgetPeriod.cs
namespace FinanceME.Enums
{
    public enum BudgetPeriod
    {
        Weekly,
        BiWeekly,
        Monthly,
        Quarterly,
        Yearly
    }
}
```

csharp

```csharp
// Enums/GoalStatus.cs
namespace FinanceME.Enums
{
    public enum GoalStatus
    {
        Active,
        Paused,
        Completed,
        Cancelled
    }
}
```

csharp

```csharp
// Enums/BillFrequency.cs
namespace FinanceME.Enums
{
    public enum BillFrequency
    {
        OneTime,
        Weekly,
        BiWeekly,
        Monthly,
        Quarterly,
        SemiAnnually,
        Annually
    }
}
```

csharp

```csharp
// Enums/DebtType.cs
namespace FinanceME.Enums
{
    public enum DebtType
    {
        CreditCard,
        StudentLoan,
        Mortgage,
        AutoLoan,
        PersonalLoan,
        MedicalDebt,
        Other
    }
}
```

csharp

```csharp
// Enums/SubscriptionStatus.cs
namespace FinanceME.Enums
{
    public enum SubscriptionStatus
    {
        Active,
        Paused,
        Cancelled,
        Trial,
        Expired
    }
}
```

csharp

```csharp
// Enums/NotificationType.cs
namespace FinanceME.Enums
{
    public enum NotificationType
    {
        Info,
        Warning,
        Alert,
        Success,
        Reminder
    }
}
```

csharp

```csharp
// Enums/AlertCondition.cs
namespace FinanceME.Enums
{
    public enum AlertCondition
    {
        BudgetExceeded,
        BudgetNearLimit,
        LowBalance,
        LargeTransaction,
        BillDueSoon,
        GoalMilestone,
        UnusualSpending,
        SubscriptionRenewal
    }
}
```

---

### Entity: User

csharp

```csharp
// Models/Entities/User.cs
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FinanceME.Models.Entities
{
    public class User : IdentityUser
    {
        // IdentityUser already gives you:
        // - Id (string, GUID-based)
        // - UserName
        // - Email
        // - PasswordHash
        // - PhoneNumber
        // - EmailConfirmed
        // - TwoFactorEnabled
        // - etc.

        // Extended profile fields
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(10)]
        public string PreferredCurrency { get; set; } = "EGP";

        [MaxLength(50)]
        public string Locale { get; set; } = "ar-EG";

        [MaxLength(50)]
        public string Timezone { get; set; } = "UTC";

        public string? ProfilePictureUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties — a User owns everything
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
        public ICollection<Bill> Bills { get; set; } = new List<Bill>();
        public ICollection<Forecast> Forecasts { get; set; } = new List<Forecast>();
        public ICollection<Debt> Debts { get; set; } = new List<Debt>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<AlertRule> AlertRules { get; set; } = new List<AlertRule>();
    }
}
```

**Why extend IdentityUser instead of building from scratch?**  
Identity gives you hashed passwords, email confirmation, two-factor auth, claims, roles, lockout — everything your auth pages need — for free. You just add your finance-specific profile fields on top.

---

### Entity: Account

csharp

```csharp
// Models/Entities/Account.cs
using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Account
    {
        public int Id { get; set; }

        // Foreign key to User
        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public AccountType Type { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        [MaxLength(10)]
        public string Currency { get; set; } = "EGP";

        [MaxLength(200)]
        public string? Institution { get; set; }

        [MaxLength(20)]
        public string? AccountNumber { get; set; } // Last 4 digits or masked

        public string? Color { get; set; } // For UI card color coding
        public string? Icon { get; set; }  // For UI icon display

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
```

---

### Entity: Category

csharp

```csharp
// Models/Entities/Category.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Icon { get; set; }
        public string? Color { get; set; }

        // Self-referencing for subcategories
        public int? ParentCategoryId { get; set; }
        [ForeignKey("ParentCategoryId")]
        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();

        public bool IsDefault { get; set; } = false; // System-provided vs user-created
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    }
}
```

**Why self-referencing?** Your front-end has category management (categories-manager.html) which implies parent/child category grouping (e.g., "Food" → "Groceries", "Restaurants").

---

### Entity: Transaction

csharp

```csharp
// Models/Entities/Transaction.cs
using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; } = null!;

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        // For transfers: the destination account
        public int? TransferToAccountId { get; set; }
        [ForeignKey("TransferToAccountId")]
        public Account? TransferToAccount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        [Required]
        [MaxLength(300)]
        public string Description { get; set; } = string.Empty;

        public string? Notes { get; set; }

        [Required]
        public DateTime Date { get; set; }

        // Recurring transaction support
        public bool IsRecurring { get; set; } = false;
        public BillFrequency? RecurringFrequency { get; set; }
        public int? RecurringSourceId { get; set; } // Links to the original recurring transaction

        // Auto-categorization support
        public bool IsAutoCategorized { get; set; } = false;

        public string? Tags { get; set; } // Comma-separated or JSON string

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
```

**Design decisions:**

- TransferToAccountId handles the transfer case without a separate entity
- RecurringSourceId self-references to link auto-generated recurring instances back to the template transaction
- Tags is a simple string for now — no need for a full Tags entity at this stage

---

### Entity: Budget

csharp

```csharp
// Models/Entities/Budget.cs
using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Budget
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal LimitAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SpentAmount { get; set; } = 0; // Can be computed, but cached here for performance

        [Required]
        public BudgetPeriod Period { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        [Column(TypeName = "decimal(5,2)")]
        public decimal WarningThreshold { get; set; } = 80.00m; // Percentage — triggers near-limit alert

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
```

**Why store SpentAmount?** It can be computed by summing transactions, but caching it avoids expensive queries on dashboards. You update it when transactions are created/modified/deleted.

---

### Entity: Goal

csharp

```csharp
// Models/Entities/Goal.cs
using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Goal
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TargetAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentAmount { get; set; } = 0;

        public string? Icon { get; set; }
        public string? Color { get; set; }

        [Required]
        public GoalStatus Status { get; set; } = GoalStatus.Active;

        public DateTime? TargetDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<GoalContribution> Contributions { get; set; } = new List<GoalContribution>();
    }
}
```

---

### Entity: GoalContribution

csharp

```csharp
// Models/Entities/GoalContribution.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class GoalContribution
    {
        public int Id { get; set; }

        [Required]
        public int GoalId { get; set; }
        [ForeignKey("GoalId")]
        public Goal Goal { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public string? Note { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
```

**Why a separate entity?** Each contribution is a discrete event. Your front-end's goal-detail page shows contribution history. This also lets you calculate trends, average monthly contributions, and projected completion dates.

---

### Entity: Bill

csharp

```csharp
// Models/Entities/Bill.cs
using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Bill
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public BillFrequency Frequency { get; set; }

        [Required]
        public DateTime DueDate { get; set; } // Next due date

        public bool IsPaid { get; set; } = false;
        public DateTime? LastPaidDate { get; set; }

        public bool AutoPay { get; set; } = false;
        public bool ReminderEnabled { get; set; } = true;
        public int ReminderDaysBefore { get; set; } = 3;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
```

---

### Entity: Forecast

csharp

```csharp
// Models/Entities/Forecast.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Forecast
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public DateTime ForecastMonth { get; set; } // The month this forecast is for

        [Column(TypeName = "decimal(18,2)")]
        public decimal ProjectedIncome { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ProjectedExpenses { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ProjectedSavings { get; set; }

        // Actual values once the month passes — for comparison
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualIncome { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualExpenses { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualSavings { get; set; }

        public string? Assumptions { get; set; } // Free text or JSON describing forecast basis

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
```

---

### Entity: Debt

csharp

```csharp
// Models/Entities/Debt.cs
using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Debt
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DebtType Type { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentBalance { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal InterestRate { get; set; } // Annual percentage

        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumPayment { get; set; }

        public DateTime? DueDate { get; set; } // Monthly due date

        [MaxLength(200)]
        public string? Lender { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<DebtPayment> Payments { get; set; } = new List<DebtPayment>();
    }
}
```

---

### Entity: DebtPayment (Added — necessary for debt payoff tracking)

csharp

```csharp
// Models/Entities/DebtPayment.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class DebtPayment
    {
        public int Id { get; set; }

        [Required]
        public int DebtId { get; set; }
        [ForeignKey("DebtId")]
        public Debt Debt { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrincipalPortion { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal InterestPortion { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
```

**Why add this?** Your front-end has a debt-payoff.html page. To show payoff progress, amortization schedules, and snowball/avalanche comparisons, you need individual payment records.

---

### Entity: Subscription

csharp

```csharp
// Models/Entities/Subscription.cs
using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Subscription
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public BillFrequency BillingCycle { get; set; }

        [Required]
        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;

        public DateTime StartDate { get; set; }
        public DateTime? NextBillingDate { get; set; }
        public DateTime? CancellationDate { get; set; }

        public string? Provider { get; set; } // e.g., "Netflix", "Spotify"
        public string? LogoUrl { get; set; }

        public bool ReminderEnabled { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
```

---

### Entity: Notification

csharp

```csharp
// Models/Entities/Notification.cs
using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        // Optional link to the rule that generated this notification
        public int? AlertRuleId { get; set; }
        [ForeignKey("AlertRuleId")]
        public AlertRule? AlertRule { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        [Required]
        public NotificationType Type { get; set; }

        public bool IsRead { get; set; } = false;
        public bool IsDismissed { get; set; } = false;

        // Optional deep link to relevant page/entity
        public string? ActionUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReadAt { get; set; }
    }
}
```

---

### Entity: AlertRule

csharp

```csharp
// Models/Entities/AlertRule.cs
using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class AlertRule
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public AlertCondition Condition { get; set; }

        // Threshold value — meaning depends on Condition
        // e.g., for LowBalance: the dollar amount; for BudgetNearLimit: the percentage
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ThresholdValue { get; set; }

        // Optional: scope the rule to a specific entity
        public int? AccountId { get; set; }
        public int? BudgetId { get; set; }
        public int? GoalId { get; set; }

        public bool IsEnabled { get; set; } = true;

        public bool NotifyByEmail { get; set; } = false;
        public bool NotifyInApp { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Notification> GeneratedNotifications { get; set; } = new List<Notification>();
    }
}
```

---

## 5. Entity Relationship Diagram

text

```text
┌──────────────────────────────────────────────────────────────────┐
│                              USER                                │
│                        (extends IdentityUser)                    │
└──────┬───┬───┬───┬───┬───┬───┬───┬───┬───┬──────────────────────┘
       │   │   │   │   │   │   │   │   │   │
       │   │   │   │   │   │   │   │   │   └──── AlertRule ──→ Notification
       │   │   │   │   │   │   │   │   │
       │   │   │   │   │   │   │   │   └──── Notification
       │   │   │   │   │   │   │   │
       │   │   │   │   │   │   │   └──── Subscription ──→ Category?
       │   │   │   │   │   │   │
       │   │   │   │   │   │   └──── Debt ──→ DebtPayment (1:N)
       │   │   │   │   │   │
       │   │   │   │   │   └──── Forecast
       │   │   │   │   │
       │   │   │   │   └──── Bill ──→ Category?
       │   │   │   │
       │   │   │   └──── Goal ──→ GoalContribution (1:N)
       │   │   │
       │   │   └──── Budget ──→ Category (required)
       │   │
       │   └──── Category ──→ Category? (self-ref: parent/child)
       │
       └──── Account ──→ Transaction (1:N)
                              │
                              ├──→ Category?
                              └──→ Account? (TransferToAccount)
```

### Relationship Summary Table

|From|To|Cardinality|FK Location|Required?|
|---|---|---|---|---|
|User|Account|1 : N|Account.UserId|Yes|
|User|Category|1 : N|Category.UserId|Yes|
|User|Transaction|1 : N|Transaction.UserId|Yes|
|User|Budget|1 : N|Budget.UserId|Yes|
|User|Goal|1 : N|Goal.UserId|Yes|
|User|Bill|1 : N|Bill.UserId|Yes|
|User|Forecast|1 : N|Forecast.UserId|Yes|
|User|Debt|1 : N|Debt.UserId|Yes|
|User|Subscription|1 : N|Subscription.UserId|Yes|
|User|Notification|1 : N|Notification.UserId|Yes|
|User|AlertRule|1 : N|AlertRule.UserId|Yes|
|Account|Transaction|1 : N|Transaction.AccountId|Yes|
|Account|Transaction|1 : N|Transaction.TransferToAccountId|No|
|Category|Transaction|1 : N|Transaction.CategoryId|No|
|Category|Budget|1 : N|Budget.CategoryId|Yes|
|Category|Bill|1 : N|Bill.CategoryId|No|
|Category|Subscription|1 : N|Subscription.CategoryId|No|
|Category|Category|1 : N|Category.ParentCategoryId|No (self-ref)|
|Goal|GoalContribution|1 : N|GoalContribution.GoalId|Yes|
|Debt|DebtPayment|1 : N|DebtPayment.DebtId|Yes|
|AlertRule|Notification|1 : N|Notification.AlertRuleId|No|

---

## 6. DbContext Configuration

csharp

```csharp
// Data/ApplicationDbContext.cs
using FinanceME.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceME.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<GoalContribution> GoalContributions { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Forecast> Forecasts { get; set; }
        public DbSet<Debt> Debts { get; set; }
        public DbSet<DebtPayment> DebtPayments { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AlertRule> AlertRules { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // CRITICAL: Must call base for Identity tables

            // ============================================================
            // Transaction: Multiple FK to Account (need to clarify cascade)
            // ============================================================
            builder.Entity<Transaction>(entity =>
            {
                entity.HasOne(t => t.Account)
                    .WithMany(a => a.Transactions)
                    .HasForeignKey(t => t.AccountId)
                    .OnDelete(DeleteBehavior.Restrict); // Don't cascade-delete transactions when account deleted

                entity.HasOne(t => t.TransferToAccount)
                    .WithMany()
                    .HasForeignKey(t => t.TransferToAccountId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);

                entity.HasOne(t => t.Category)
                    .WithMany(c => c.Transactions)
                    .HasForeignKey(t => t.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);

                entity.HasOne(t => t.User)
                    .WithMany(u => u.Transactions)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(t => t.Date);
                entity.HasIndex(t => new { t.UserId, t.Date });
                entity.HasIndex(t => new { t.UserId, t.CategoryId });
            });

            // ============================================================
            // Account
            // ============================================================
            builder.Entity<Account>(entity =>
            {
                entity.HasOne(a => a.User)
                    .WithMany(u => u.Accounts)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(a => new { a.UserId, a.IsActive });
            });

            // ============================================================
            // Category: Self-referencing
            // ============================================================
            builder.Entity<Category>(entity =>
            {
                entity.HasOne(c => c.ParentCategory)
                    .WithMany(c => c.SubCategories)
                    .HasForeignKey(c => c.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                entity.HasOne(c => c.User)
                    .WithMany(u => u.Categories)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(c => new { c.UserId, c.Name });
            });

            // ============================================================
            // Budget
            // ============================================================
            builder.Entity<Budget>(entity =>
            {
                entity.HasOne(b => b.User)
                    .WithMany(u => u.Budgets)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(b => b.Category)
                    .WithMany(c => c.Budgets)
                    .HasForeignKey(b => b.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(b => new { b.UserId, b.IsActive });
            });

            // ============================================================
            // Goal & GoalContribution
            // ============================================================
            builder.Entity<Goal>(entity =>
            {
                entity.HasOne(g => g.User)
                    .WithMany(u => u.Goals)
                    .HasForeignKey(g => g.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<GoalContribution>(entity =>
            {
                entity.HasOne(gc => gc.Goal)
                    .WithMany(g => g.Contributions)
                    .HasForeignKey(gc => gc.GoalId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ============================================================
            // Bill
            // ============================================================
            builder.Entity<Bill>(entity =>
            {
                entity.HasOne(b => b.User)
                    .WithMany(u => u.Bills)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(b => b.Category)
                    .WithMany()
                    .HasForeignKey(b => b.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);
            });

            // ============================================================
            // Forecast
            // ============================================================
            builder.Entity<Forecast>(entity =>
            {
                entity.HasOne(f => f.User)
                    .WithMany(u => u.Forecasts)
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(f => new { f.UserId, f.ForecastMonth });
            });

            // ============================================================
            // Debt & DebtPayment
            // ============================================================
            builder.Entity<Debt>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(u => u.Debts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<DebtPayment>(entity =>
            {
                entity.HasOne(dp => dp.Debt)
                    .WithMany(d => d.Payments)
                    .HasForeignKey(dp => dp.DebtId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ============================================================
            // Subscription
            // ============================================================
            builder.Entity<Subscription>(entity =>
            {
                entity.HasOne(s => s.User)
                    .WithMany(u => u.Subscriptions)
                    .HasForeignKey(s => s.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.Category)
                    .WithMany()
                    .HasForeignKey(s => s.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);
            });

            // ============================================================
            // Notification
            // ============================================================
            builder.Entity<Notification>(entity =>
            {
                entity.HasOne(n => n.User)
                    .WithMany(u => u.Notifications)
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(n => n.AlertRule)
                    .WithMany(ar => ar.GeneratedNotifications)
                    .HasForeignKey(n => n.AlertRuleId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);

                entity.HasIndex(n => new { n.UserId, n.IsRead });
                entity.HasIndex(n => new { n.UserId, n.CreatedAt });
            });

            // ============================================================
            // AlertRule
            // ============================================================
            builder.Entity<AlertRule>(entity =>
            {
                entity.HasOne(ar => ar.User)
                    .WithMany(u => u.AlertRules)
                    .HasForeignKey(ar => ar.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
```

---

## 7. Connection String Setup

### For SQL Server (LocalDB — built into Visual Studio):

Open appsettings.json and update:

json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FinanceMEDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### For SQLite alternative:

json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=FinanceME.db"
  }
}
```

And in Program.cs, change the database provider line:

csharp

```csharp
// Change from:
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// To:
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
```

---

## 8. Identity Integration

### Program.cs Updates

The template already generates most of this. Verify and adjust to use your custom User class:

csharp

```csharp
// Program.cs
using FinanceME.Data;
using FinanceME.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity with YOUR custom User class
builder.Services.AddDefaultIdentity<User>(options =>
{
    // Password settings — adjust to your preference
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;

    // Lockout
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;

    // User
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false; // Set true for production
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Configure cookie for auth redirects
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Needed for Identity UI pages

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages(); // Maps Identity UI pages

app.Run();
```

---

## 9. Migrations & Database Creation

### Using Visual Studio GUI (Package Manager Console):

1. **Open:** Tools → NuGet Package Manager → Package Manager Console
    
2. **Run:**
    
    text
    
    ```text
    Add-Migration InitialCreate
    ```
    
    This reads all your entities and DbContext configuration, then generates a migration file.
    
3. **Review** the generated migration file in Migrations/ folder. It should create tables for all your entities plus Identity tables.
    
4. **Apply:**
    
    text
    
    ```text
    Update-Database
    ```
    

### When you change entities later:

text

```text
Add-Migration DescriptiveNameOfChange
Update-Database
```

### View the database:

- **Visual Studio:** View → SQL Server Object Explorer → expand (localdb)\MSSQLLocalDB → Databases → FinanceMEDb

---

## 10. Folder/File Organization

### Create these folders manually in Visual Studio:

Right-click project → Add → New Folder for each:

text

```text
Models/
  Entities/        ← All entity .cs files
  ViewModels/      ← View-specific data shapes
Enums/             ← All enum .cs files
Data/              ← DbContext + SeedData
Controllers/       ← All controllers
Views/
  Shared/          ← _Layout, _Sidebar, _Header, _Footer partials
  Home/            ← Landing page view
  Dashboard/       ← Dashboard views
  Accounts/        ← Account views
  Transactions/    ← Transaction views
  Budgets/         ← Budget views
  Goals/           ← Goal views
  Bills/           ← Bill views
  Forecast/        ← Forecast views
  Debts/           ← Debt views
  Subscriptions/   ← Subscription views
  Notifications/   ← Notification views
  AlertRules/      ← Alert rule views
  Reports/         ← Report views
  Settings/        ← Settings views
wwwroot/
  css/             ← Your existing CSS files
  js/              ← Your existing JS files
  Logo/            ← Your existing logo assets
  pictures/        ← Your existing image assets
  lib/             ← (auto-generated by VS for client libraries)
```

### Moving your existing front-end assets:

1. Copy all files from your current css/ into wwwroot/css/
2. Copy all files from your current js/ into wwwroot/js/
3. Copy Logo/ into wwwroot/Logo/
4. Copy pictures/ into wwwroot/pictures/

---

## 11. Controller Scaffolding Strategy

### How to scaffold controllers via GUI:

1. **Right-click** the Controllers/ folder
2. **Select** Add → Controller...
3. **Choose** MVC Controller with views, using Entity Framework
4. **Configure:**
    - Model class: (select your entity, e.g., Account)
    - Data context class: ApplicationDbContext
    - ☑ Generate views
    - ☑ Reference script libraries
    - ☑ Use a layout page
5. **Click** Add

This generates a full CRUD controller and matching views (Index, Create, Edit, Details, Delete) for that entity.

### Do this for these entities (in this order):

|Order|Entity|Controller Name|Notes|
|---|---|---|---|
|1|Account|AccountsController|Core entity, no dependencies beyond User|
|2|Category|CategoriesController|Also no complex dependencies|
|3|Transaction|TransactionsController|Depends on Account + Category|
|4|Budget|BudgetsController|Depends on Category|
|5|Goal|GoalsController|Standalone under User|
|6|GoalContribution|—|Handle inside GoalsController (nested)|
|7|Bill|BillsController|Optional Category link|
|8|Forecast|ForecastsController|Standalone under User|
|9|Debt|DebtsController|Standalone under User|
|10|DebtPayment|—|Handle inside DebtsController (nested)|
|11|Subscription|SubscriptionsController|Optional Category link|
|12|Notification|NotificationsController|Linked to AlertRule optionally|
|13|AlertRule|AlertRulesController|Standalone under User|

### Controllers you create manually (not scaffolded):

csharp

```csharp
// Controllers/DashboardController.cs
using FinanceME.Data;
using FinanceME.Models.Entities;
using FinanceME.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceME.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var viewModel = new DashboardViewModel
            {
                Accounts = await _context.Accounts
                    .Where(a => a.UserId == userId && a.IsActive)
                    .ToListAsync(),

                RecentTransactions = await _context.Transactions
                    .Where(t => t.UserId == userId)
                    .OrderByDescending(t => t.Date)
                    .Take(10)
                    .Include(t => t.Account)
                    .Include(t => t.Category)
                    .ToListAsync(),

                ActiveBudgets = await _context.Budgets
                    .Where(b => b.UserId == userId && b.IsActive)
                    .Include(b => b.Category)
                    .ToListAsync(),

                ActiveGoals = await _context.Goals
                    .Where(g => g.UserId == userId && g.Status == Enums.GoalStatus.Active)
                    .ToListAsync(),

                UpcomingBills = await _context.Bills
                    .Where(b => b.UserId == userId && b.IsActive && !b.IsPaid)
                    .Where(b => b.DueDate <= DateTime.UtcNow.AddDays(30))
                    .OrderBy(b => b.DueDate)
                    .Take(5)
                    .ToListAsync(),

                UnreadNotificationCount = await _context.Notifications
                    .Where(n => n.UserId == userId && !n.IsRead)
                    .CountAsync(),

                TotalBalance = await _context.Accounts
                    .Where(a => a.UserId == userId && a.IsActive)
                    .SumAsync(a => a.Balance),

                TotalDebt = await _context.Debts
                    .Where(d => d.UserId == userId && d.IsActive)
                    .SumAsync(d => d.CurrentBalance)
            };

            return View(viewModel);
        }
    }
}
```

csharp

```csharp
// Controllers/ReportsController.cs
using FinanceME.Data;
using FinanceME.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceME.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ReportsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index() => View();

        public async Task<IActionResult> SpendingAnalysis(int? months)
        {
            var userId = _userManager.GetUserId(User);
            var period = months ?? 6;
            var startDate = DateTime.UtcNow.AddMonths(-period);

            var transactions = await _context.Transactions
                .Where(t => t.UserId == userId
                    && t.Type == Enums.TransactionType.Expense
                    && t.Date >= startDate)
                .Include(t => t.Category)
                .ToListAsync();

            return View(transactions);
        }

        public async Task<IActionResult> IncomeVsExpenses(int? months)
        {
            var userId = _userManager.GetUserId(User);
            var period = months ?? 12;
            var startDate = DateTime.UtcNow.AddMonths(-period);

            var transactions = await _context.Transactions
                .Where(t => t.UserId == userId && t.Date >= startDate)
                .ToListAsync();

            return View(transactions);
        }

        public async Task<IActionResult> NetWorthTrends()
        {
            var userId = _userManager.GetUserId(User);

            var accounts = await _context.Accounts
                .Where(a => a.UserId == userId && a.IsActive)
                .ToListAsync();
            var debts = await _context.Debts
                .Where(d => d.UserId == userId && d.IsActive)
                .ToListAsync();

            ViewBag.TotalAssets = accounts.Sum(a => a.Balance);
            ViewBag.TotalDebts = debts.Sum(d => d.CurrentBalance);

            return View();
        }

        public async Task<IActionResult> CashFlowReport(int? months)
        {
            var userId = _userManager.GetUserId(User);
            var period = months ?? 6;
            var startDate = DateTime.UtcNow.AddMonths(-period);

            var transactions = await _context.Transactions
                .Where(t => t.UserId == userId && t.Date >= startDate)
                .OrderBy(t => t.Date)
                .ToListAsync();

            return View(transactions);
        }

        public IActionResult CategoryTrends() => View();
        public IActionResult TaxSummary() => View();
    }
}
```

csharp

```csharp
// Controllers/SettingsController.cs
using FinanceME.Data;
using FinanceME.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinanceME.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public SettingsController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(string firstName, string lastName, string preferredCurrency, string locale, string timezone)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.FirstName = firstName;
            user.LastName = lastName;
            user.PreferredCurrency = preferredCurrency;
            user.Locale = locale;
            user.Timezone = timezone;
            user.UpdatedAt = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);
            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Profile));
        }

        public IActionResult Security() => View();
        public IActionResult Integrations() => View();
        public IActionResult CurrencyLocale() => View();
        public IActionResult DataExport() => View();
        public IActionResult DeleteAccount() => View();
    }
}
```

---

### Important: Add [Authorize] to all scaffolded controllers

After scaffolding, open every controller and add:

csharp

```csharp
using Microsoft.AspNetCore.Authorization;

namespace FinanceME.Controllers
{
    [Authorize]  // ← Add this
    public class AccountsController : Controller
    {
        // ...
    }
}
```

Also, **add user filtering to every query** in the scaffolded controllers. The scaffolder generates generic CRUD that shows ALL records. You must scope everything to the logged-in user.

**Example modification** for the scaffolded AccountsController.Index:

csharp

```csharp
// BEFORE (scaffolded — shows everyone's accounts):
public async Task<IActionResult> Index()
{
    return View(await _context.Accounts.ToListAsync());
}
```
---
---
# EF Core Migrations

Migrations generate SQL scripts from your C# models — your schema lives in code, not in the database tool.

Run these commands from the solution root

## Initial migration — creates all tables

`dotnet ef migrations add InitialCreate \
`  --project FinanceME.Infrastructure \`
`  --startup-project FinanceME.API`
`
## Apply to database

`dotnet ef database update \`
`  --project FinanceME.Infrastructure \`
`  --startup-project FinanceME.API
`
Workflow for future changes

## 1. Change your model (e.g. add a field to Transaction)
## 2. Add a new migration

`dotnet ef migrations add AddMerchantToTransaction \`
`  --project FinanceME.Infrastructure \`
`  --startup-project FinanceME.API`

## 3. Apply it

`dotnet ef database update \`
`  --project FinanceME.Infrastructure \`
`  --startup-project FinanceME.API`

`Seed initial data — in ApplicationDbContext
`
protected override void OnModelCreating(ModelBuilder builder)
{
    // ... other config ...

    // Seed default categories
    builder.Entity<Category>().HasData(
        new Category { Id = 1, Name = "Food & Dining", Icon = "utensils", Color = "#E05C5C" },
        new Category { Id = 2, Name = "Transportation", Icon = "car", Color = "#4C8EF5" },
        new Category { Id = 3, Name = "Housing", Icon = "home", Color = "#C9A84C" },
        new Category { Id = 4, Name = "Salary", Icon = "briefcase", Color = "#2ECC8A" }
    );
}


