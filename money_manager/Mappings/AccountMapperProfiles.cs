using money_manager.Entities;
using money_manager.Models;

namespace money_manager.Mappings;

public static class AccountMapperProfiles
{
    public static AccountEntity ToModel(this CreateAccountViewModel viewModel)
    {
        return new AccountEntity
        {
            AccountId = viewModel.AccountId,
            AccountName = viewModel.AccountName,
            AccountTypeId = viewModel.AccountTypeId,
            AccountTypeName = viewModel.AccountTypeName,
            AccountBalance = viewModel.AccountBalance,
            AccountDescription = viewModel.AccountDescription
        };
    }

    public static CreateAccountViewModel ToViewModel(this AccountEntity model)
    {
        return new CreateAccountViewModel
        {
            AccountId = model.AccountId,
            AccountName = model.AccountName,
            AccountTypeId = model.AccountTypeId,
            AccountTypeName = model.AccountTypeName,
            AccountBalance = model.AccountBalance,
            AccountDescription = model.AccountDescription
        };
    }
}
