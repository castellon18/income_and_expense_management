using money_manager.Entities;
using money_manager.Models;

namespace money_manager.Mappings;

public static class TransactionMapperProfiles
{
    public static TransactionEntity ToModel(this EditTransactionViewModel viewModel)
    {
        return new TransactionEntity
        {
            TransactionId = viewModel.TransactionId,
            UserId = viewModel.UserId,
            TransactionDate = viewModel.TransactionDate,
            Amount = viewModel.Amount,
            AccountId = viewModel.AccountId,
            CategoryId = viewModel.CategoryId,
            TransactionNote = viewModel.TransactionNote
        };
    }

    public static EditTransactionViewModel ToViewModel(this TransactionEntity model)
    {
        return new EditTransactionViewModel
        {
            TransactionId = model.TransactionId,
            TransactionDate = model.TransactionDate,
            Amount = model.Amount,
            AccountId = model.AccountId,
            CategoryId = model.CategoryId,
            TransactionNote = model.TransactionNote,
            OperationTypeId = model.OperationTypeId
        };
    }
}
