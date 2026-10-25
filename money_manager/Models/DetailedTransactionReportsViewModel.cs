using money_manager.Entities;

namespace money_manager.Models;

public class DetailedTransactionReportsViewModel
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public IEnumerable<TransactionsByDate> GroupedTransactions { get; set; } = [];
    public decimal BalanceDeposits => GroupedTransactions.Sum(g => g.BalanceDeposits);
    public decimal ExpenseBalance => GroupedTransactions.Sum(g => g.ExpenseBalance);
    public decimal TotalBalance => BalanceDeposits - ExpenseBalance;

    public class TransactionsByDate
    {
        public DateTime TransactionDate { get; set; }
        public IEnumerable<TransactionEntity> Transactions { get; set; } = [];
        public decimal BalanceDeposits => Transactions.Where(t => t.OperationTypeId == OperationTypeEntity.Ingreso).Sum(t => t.Amount);
        public decimal ExpenseBalance => Transactions.Where(t => t.OperationTypeId == OperationTypeEntity.Gasto).Sum(t => t.Amount);
    }
}
