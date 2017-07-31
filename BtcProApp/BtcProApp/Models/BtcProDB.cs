namespace BtcProApp.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    public class BtcProDB : DbContext
    {
        public BtcProDB()
            : base("name=BtcProDB")
        {

        }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Package> Packages { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<Register> Registrations { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<SubLedger> SubLedgers { get; set; }
        public virtual DbSet<Ledger> Ledgers { get; set; }
        public virtual DbSet<WeeklyIncome> WeeklyIncomes { get; set; }
        public virtual DbSet<BinaryIncome> BinaryIncomes { get; set; }
        public virtual DbSet<SponsorIncome> SponsorIncomes { get; set; }
        public virtual DbSet<GenerationIncome> GenerationIncomes { get; set; }
        public virtual DbSet<LibraryDocument> LibraryDocuments { get; set; }
        public virtual DbSet<KYCDocument> KYCDocuments { get; set; }
        public System.Data.Entity.DbSet<BtcProApp.Models.Ticket> Tickets { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<WithdrawalRequest> WithdrawalRequests { get; set; }
        public virtual DbSet<BinaryOpening> BinaryOpenings { get; set; }
        public virtual DbSet<NewsReport> NewsReports { get; set; }
        public virtual DbSet<ipn> Ipns { get; set; }
        public virtual DbSet<CryptoCurrrency> CryptoCurrencies { get; set; }
        public virtual DbSet<PayoutProcess> PayoutProcess { get; set; }
    }

    public class Member
    {
        public long Id { get; set; }
        public DateTime Doj { get; set; }
        public DateTime? Activationdate { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Displayname { get; set; }
        public string Emailid { get; set; }
        public string Addressline1 { get; set; }
        public string Addressline2 { get; set; }
        public string City { get; set; }
        public long? Postcode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Mobileno { get; set; }
        public string Preferredlanguage { get; set; }
        public string Secretquestion { get; set; }
        public string Secretpassword { get; set; }
        public bool Isactive { get; set; }
        public long ReferrerRegistrationId { get; set; }
        public string Referrerusername { get; set; }
        public long? UplineRegistrationId { get; set; }
        public string Uplineusername { get; set; }
        public string BinaryPosition { get; set; }
        public long? Totalmembers { get; set; }
        public long? Leftmembers { get; set; }
        public long? Rightmembers { get; set; }
        public string Transactionpassword { get; set; }
        public bool Kycstatus { get; set; }
        public string Kycdocuments { get; set; }
        public int? Level { get; set; }
        public int? Defaultpackagecode { get; set; }
        public long RegistrationId { get; set; }
    }

    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Minamount { get; set; }
        public double Maxamount { get; set; }
        public int Gauranteedreturn_percent { get; set; }
        public int Targetdays { get; set; }
        public bool Isactive { get; set; }
    }

    public class Purchase
    {
        public long Id { get; set; }
        public DateTime Purchasedate { get; set; }
        public int Packageid { get; set; }
        public long RegistrationId { get; set; }
        public double Amount { get; set; }
        public string Payreferenceno { get; set; }
        public bool Isapproved { get; set; }
    }

    public class Register
    {
        public long Id { get; set; }
        [Required]
        public string ReferrerName { get; set; }
        [Required]
        public long ReferrerId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string EmailId { get; set; }
        [Required]
        public string UserName {get; set; }
        [Required]
        public string Password { get; set; }
        public string TrxPassword { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public string BinaryPosition { get; set; }
        public Boolean Joined { get; set; }
        public string CountryCode { get; set; }
        public string MyWalletAccount { get; set; }
        public string WorkingLeg { get; set; }
        public Boolean isBlocked { get; set; }

        [NotMapped]
        public string ConfirmPassword { get; set; }
    }

    public class Wallet
    {
        public int Id { get; set; }
        public string WalletName { get; set; }
        public bool Show { get; set; }
    }

    public class TransactionType
    {
        public int Id { get; set; }
        public string TransactionName { get; set; }
    }

    public class SubLedger
    {
        public int Id { get; set; }
        public string SubLedgerName { get; set; }
    }

    public class Ledger
    {
        public long Id { get; set; }
        public long RegistrationId { get; set; }
        public int WalletId { get; set; }
        public DateTime? WeekStartDate { get; set; }
        public DateTime? WeekEndDate { get; set; }
        public DateTime Date { get; set; }
        public double Deposit { get; set; }
        public double Withdraw { get; set; }
        public int TransactionTypeId { get; set; }
        public long TransactionId { get; set; }
        public int SubLedgerId { get; set; }
        public long ToFromUser { get; set; }
        public string BatchNo { get; set; }
        public string ProcessId { get; set; }
        public double? Leftside_cd { get; set; }
        public double? Rightside_cd { get; set; }
        public string Comment { get; set; }
    }

    public class WithdrawalRequest
    {
        public long Id { get; set; }
        public long RegistrationId { get; set; }
        public int WalletId { get; set; }
        public DateTime Date { get; set; }
        public string sDate { get; set; }
        public double Amount { get; set; }
        public DateTime? Approved_Date { get; set; }
        public string sApproved_Date { get; set; }
        public string Status { get; set; }
        public double PaidOutAmount { get; set; }
        public double ServiceCharge { get; set; }
        public string BatchNo { get; set; }
        public string ReferenceNo { get; set; }
        public string PaidBitCoinAccount { get; set; }
        public string Comment { get; set; }
        [NotMapped]
        public string Username;
        [NotMapped]
        public string Walletname;
        [NotMapped]
        public string BitcoinAcNo { get; set; }
        [NotMapped]
        public bool isApproved { get; set; }
    }

    public class WeeklyIncome
    {
        public int Id { get; set; }
        public long RegistrationId { get; set; }
        public int PackageId { get; set; }
        public int WeekNo { get; set; }
        public int Days { get; set; }
        public double Pc { get; set; }
        public double Income { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public double? CashWallet { get; set; }
        public double? ReserveWallet { get; set; }
        public double? FixedIncomeWallet { get; set; }
        public double? FrozenWallet { get; set; }
        public DateTime? PaidDate { get; set; }
        public long? Ledger_PaymentReferenceId { get; set; }
        public string BatchNo { get; set; }
        public string ProcessId { get; set; }
    }

    public class BinaryIncome
    {
        public int Id { get; set; }
        public long RegistrationId { get; set; }
        public int WeekNo { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public long LeftNewJoining { get; set; }
        public long RightNewJoining { get; set; }
        public long LeftNewBusinessCount { get; set; }
        public long RightNewBusinessCount { get; set; }
        public double LeftNewBusinessAmount { get; set; }
        public double RightNewBusinessAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public long PurchaserRegistrationId { get; set; }
        public int PackageId { get; set; }
        public double? CashWallet { get; set; }
        public double? ReserveWallet { get; set; }
        public double? FixedIncomeWallet { get; set; }
        public double? FrozenWallet { get; set; }
        public DateTime? PaidDate { get; set; }
        public long? Ledger_PaymentReferenceId { get; set; }
        public string BatchNo { get; set; }
        public string ProcessId { get; set; }
    }

    public class SponsorIncome
    {
        public int Id { get; set; }
        public long RegistrationId { get; set; }
        public int WeekNo { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public long LeftNewJoining { get; set; }
        public long RightNewJoining { get; set; }
        public long LeftNewBusinessCount { get; set; }
        public long RightNewBusinessCount { get; set; }
        public double LeftNewBusinessAmount { get; set; }
        public double RightNewBusinessAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public long PurchaserRegistrationId { get; set; }
        public int PackageId { get; set; }
        public double IncomeAmount { get; set; }
        public double? CashWallet { get; set; }
        public double? ReserveWallet { get; set; }
        public double? FixedIncomeWallet { get; set; }
        public double? FrozenWallet { get; set; }
        public DateTime? PaidDate { get; set; }
        public long? Ledger_PaymentReferenceId { get; set; }
        public string BatchNo { get; set; }
        public string ProcessId { get; set; }
    }

    public class GenerationIncome
    {
        public int Id { get; set; }
        public long RegistrationId { get; set; }
        public int WeekNo { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public long LeftNewJoining { get; set; }
        public long RightNewJoining { get; set; }
        public long LeftNewBusinessCount { get; set; }
        public long RightNewBusinessCount { get; set; }
        public double LeftNewBusinessAmount { get; set; }
        public double RightNewBusinessAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public long PurchaserRegistrationId { get; set; }
        public int PackageId { get; set; }
        public double IncomeAmount { get; set; }
        public int IncomefromDownlineLevel { get; set; }
        public double? CashWallet { get; set; }
        public double? ReserveWallet { get; set; }
        public double? FixedIncomeWallet { get; set; }
        public double? FrozenWallet { get; set; }
        public DateTime? PaidDate { get; set; }
        public long? Ledger_PaymentReferenceId { get; set; }
        public string BatchNo { get; set; }
        public string ProcessId { get; set; }

    }

    public class LibraryDocument
    {
        public long Id { get; set; }
        public string Module { get; set; }                    //CONTRACT / BID / CAPABILITY
        public Int32 ModuleId { get; set; }                   //contractId / BidId / CapabilityId
        public string SubModule { get; set; }                 //CONTRACT-RESEARCH / BID-RESEARCH / CAPABILITY-RESEARCH
        public Int32 SubModuleId { get; set; }                //contract_researchId / Bid_ResearchId / 
        public string OriginalImageName { get; set; }         //Image path
        public string UniqueImageName { get; set; }           //System generated image name
        public string ImageType { get; set; }                 //jpg,png,tiff,doc,xls etc
        public DateTime UploadDate { get; set; }              //date of upload
        public string UploadedBy { get; set; }                //uploaded by
        public string Remarks { get; set; }                   //remarks about image
    }

    public class KYCDocument
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public long RegistrationId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public string Comments { get; set; }
        public long LibraryId { get; set; }
        [NotMapped]
        public string LibraryFilename { get; set; }
        public string DateString { get; set; }
        public string Status { get; set; }
        public Boolean isApproved { get; set; }
        public DateTime? ApprovedWhen { get; set; }
        public string Comment { get; set; }
    }

    public class Ticket
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public long RegistrationId { get; set; }
        public string TicketCategory { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Priority { get; set; }
        public long LibraryId { get; set; }
        [NotMapped]
        public string LibraryFilename { get; set; }
        public string DateString { get; set; }
        public string Status { get; set; }
        public Boolean isApproved { get; set; }
        public DateTime? ApprovedWhen { get; set; }
        public string Comment { get; set; }

    }

    public class Country
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CountryId { get; set; }
        public string Mapimagename { get; set; }

    }

    public class BinaryOpening
    {
        public long Id { get; set; }
        public long RegistrationId { get; set; }
        public double LeftSideOp { get; set; }
        public double RightSideOp { get; set; }
        public double LeftSideCurrent { get; set; }
        public double RightSideCurrent { get; set; }
        public double LeftSideGrandTotal { get; set; }
        public double RightSideGrandTotal { get; set; }
        public double Binary { get; set; }
        public double CappingAmount { get; set; }
        public double Income { get; set; }
        public double LeftSideCd { get; set; }
        public double RightSideCd { get; set; }
        public string ProcessId { get; set; }
        public string Week { get; set; }
        public Boolean IsCurrent { get; set; }
    }

    public class PayoutProcess
        {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime Date { get; set; }
        public string ProcessNo { get; set; }
        }

    public class NewsReport
    {
        public long Id { get; set; }
        public string NewsItemTitle { get; set; }
        public string NewsItemBody { get; set; }
        public string ImageFileName { get; set; }
        public string NewsAuthor { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public string UpdatedByUser { get; set; }
        [NotMapped]
        public Boolean Add { get; set; }     //flag to indicate whether record is for ADD?
        [NotMapped]
        public Boolean Edit { get; set; }    //flag to indicate whether record is for EDIT?
    }

    public class ipn
    {
        public long id { get; set; }
        public decimal ipn_version { get; set; }
        public string ipn_type { get; set; }
        public string ipn_mode { get; set; }
        public string ipn_id { get; set; }
        public string merchant { get; set; }
        public string address { get; set; }
        public string txn_id { get; set; }
        public string status { get; set; }
        public string status_text { get; set; }
        public string currency { get; set; }
        public int confirms { get; set; }
        public decimal amount { get; set; }
        public decimal amounti { get; set; }
        public decimal fee { get; set; }

        public decimal feei { get; set; }
    }

    public class CryptoCurrrency
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime Transactiondate { get; set; }
        public int PackageId { get; set; }
        public double PackageAmt { get; set; }
        public long UplineId { get; set; }
        public string Paying_Currency { get; set; }
        public string Target_Currency { get; set; }
        public string TargetWalletAccount { get; set; }
        public string ConvertedAmount { get; set; }
        public string Transactionid { get; set; }
        public string Status_url { get; set; }
        public string Qrcode_url { get; set; }
        public int Paymentstatus { get; set; }
        public string StatusRemarks { get; set; }
        public Boolean FullyPaid { get; set; }
        public DateTime? PaymentDate { get; set; }

    }
}