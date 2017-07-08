using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BtcProApp.Models
{
    public class WeekModel
    {
        public int WeekNo { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
    }

    public class MemberTransferVM
    {
        public long Id { get; set; }
        public DateTime DateD { get; set; }
        public string Date { get; set; }
        public double Deposit { get; set; }
        public double Withdraw { get; set; }
        public string Transfer { get; set; }
        public string Walletname { get; set; }
        public double Balance { get; set; }
        public string Comment { get; set; }
    }

    public class MemberWithdrawVM
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string sDate { get; set; }
        public string WalletName { get; set; }
        public string BitCoinAccount { get; set; }
        public double Amount { get; set; }
        public double Payable { get; set; }
        public double AdministrativeChg { get; set; }
        public DateTime? Approved_Date { get; set; }
        public string sApproved_Date { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
    }

    public class LedgerVM
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string sDate { get; set; }
        public double Deposit { get; set; }
        public double Withdraw { get; set; }
        public double Balance { get; set; }
        public string TransactionType { get; set; }
        public string Ledger { get; set; }
        public string Remarks { get; set; }
        public string BatchNo { get; set; }
    }
    public class FixedIncomeLedgerVM
    {
        public int WeekNo { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public DateTime DueDate { get; set; }
        public double Amount { get; set; }
        public string Package { get; set; }
        public string sWeekStartDate { get; set; }
        public string sWeekEndDate { get; set; }
        public string sDueDate { get; set; }
        public double Total { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string sPaymentDate { get; set; }
    }

    public class BinaryIncomeLedgerVM
    {
        public int Id { get; set; }
        public long RegistrationId { get; set; }
        public int WeekNo { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public DateTime Date { get; set; }
        public string Purchaser { get; set; }
        public string Package { get; set; }
        public double LeftSideAmount { get; set; }
        public double RightSideAmount { get; set; }
        public string sWeekStartDate { get; set; }
        public string sWeekEndDate { get; set; }
        public string sDate { get; set; }
        public double Total { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string sPaymentDate { get; set; }
        public double WalletAmount { get; set; }
        public int Level { get; set; }
    }

    public class BinaryIncomeLedgerVMTotals
    {
        public double opLeftAmount { get; set; }
        public double opRightAmount { get; set; }
        public double CurrentLeftAmount { get; set; }
        public double CurrentRightAmount { get; set; }
        public double TotalLeftAmount { get; set; }
        public double TotalRightAmount { get; set; }
        public double ConsiderationAmount { get; set; }
        public double IncomeAmount { get; set; }
        public double cdLeftAmount { get; set; }
        public double cdRightAmount { get; set; }
    }

    public class DashboardModel
    {
        public double TotalTeamMembers { get; set; }
        public double TotalTeamBusiness { get; set; }
        public double InitialInvestment { get; set; }
        public double TotalRepurchase { get; set; }
        public double PlutoPurchasePc { get; set; }
        public double JupiterPurchasePc { get; set; }
        public double EarthPurchasePc { get; set; }
        public double MercuryPurchasePc { get; set; }
        public long TotalPkgPurchaseCount { get; set; }
        public long MyLeftCount { get; set; }
        public long MyRightCount { get; set; }
        public double CashWalletBalance { get; set; }
        public double ReserveWalletBalance { get; set; }
        public double ReturnWalletBalance { get; set; }
        public double FrozenWalletBalance { get; set; }
    }

    public class UploadResult
    {
        public string fileName { get; set; }
        public long fileId { get; set; }
        public string fileInternalName { get; set; }
        public string fileType { get; set; }
        public string filePath { get; set; }
        public string filesubModule { get; set; }
        public long filesubModuleId { get; set; }
    }

    public class Treemodel
    {
        public long id { get; set; }
        public long? parentId { get; set; }
        public string Name { get; set; }
        public string pic { get; set; }
        public Boolean isMember { get; set; }
        public string position { get; set; }
        public string fullname { get; set; }
        public string username { get; set; }
        public string sponsorname { get; set; }
        public string package { get; set; }
        public long totalleft { get; set; }
        public long totalright { get; set; }
        public double totalbusinessleft { get; set; }
        public double totalbusinessright { get; set; }
        public double businessleft { get; set; }
        public double businessright { get; set; }
        public string achievement { get; set; }
        public long UplineRegistrationId { get; set; }
    }
    public class UploadResult2
    {
        public string url { get; set; }
        public List<UploadResult> uploadresults { get; set; }
    }
    public class WithdrawalRequestVM
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
        public string Username;
        public string Walletname;
        public string BitcoinAcNo { get; set; }
        public bool isApproved { get; set; }
        public string Comment { get; set; }
    }

    public class WeeklyIncomeVM
    {
        public int Id { get; set; }
        public long RegistrationId { get; set; }
        public string Username { get; set; }
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public double InvestmentAmount { get; set; }
        public int WeekNo { get; set; }
        public int Days { get; set; }
        public double Pc { get; set; }
        public double Income { get; set; }
        public DateTime DueDate { get; set; }
        public string sDueDate { get; set; }
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
        public string Status { get; set; }
    }

}