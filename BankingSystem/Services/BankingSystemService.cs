using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Interfaces;
using BankingSystem.Domain.ValueObjects;
using BankingSystem.Grpc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Serilog;
using Account = BankingSystem.Domain.Entities.Account;
using Address = BankingSystem.Domain.ValueObjects.Address;
using Money = BankingSystem.Domain.ValueObjects.Money;
using Transaction = BankingSystem.Domain.Entities.Transaction;

namespace BankingSystem.API.Services
{
    public class BankingSystemService: Banking.BankingBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger _logger;

        public BankingSystemService(IAccountRepository accountService, ICustomerRepository customerService, ILogger logger)
        {
            _accountRepository = accountService;
            _customerRepository = customerService;
            _logger = logger;
        }

        public override async Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
        {
            var customer = new Customer(
                new FullName(request.FirstName, request.LastName, request.MiddleName),
                new Email(request.Email),
                new PhoneNumber(request.Phone),
                new Address(request.Address.Country, request.Address.Region, request.Address.City, request.Address.Street, request.Address.House, request.Address.ZipCode));

            var customerId = await _customerRepository.AddAsync(customer);

            return new CreateCustomerResponse { CustomerId = customerId.ToString() };
        }

        public override async Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request, ServerCallContext context)
        {
            var account = new Account(
                Guid.Parse(request.CustomerId),
                new Money(decimal.Parse(request.Balance.Amount), request.Balance.Currency));

            var accountId = await _accountRepository.AddAsync(account);

            return new CreateAccountResponse { AccountId = accountId.ToString() };
        }
        
        public override async Task<GetAccountResponse> GetAccount(GetAccountRequest request, ServerCallContext context)
        {
            var account = await _accountRepository.GetByIdAsync(Guid.Parse(request.AccountId));
            return new GetAccountResponse { 
                Account = new Grpc.Account()
                {
                    AccountId = account.AccountId.ToString(),
                    OwnerName = account.Customer.Name.ToString(),
                    Balance = new Grpc.Money() { Amount = account.Balance.Amount.ToString(), Currency = account.Balance.Currency },
                    OwnerStatus = account.Customer.Status.ToString(),
                }
            };
        }

        public override async Task<TransferResponse> Transfer(TransferRequest request, ServerCallContext context)
        {
            try
            {
                var accountFrom = await _accountRepository.GetByIdAsync(Guid.Parse(request.FromAccountId));
                var accountTo = await _accountRepository.GetByIdAsync(Guid.Parse(request.ToAccountId));
                accountTo.Transfer(accountTo, new Money(decimal.Parse(request.Money.Amount), request.Money.Currency));
                _accountRepository.UpdateAsync(accountFrom);
                _accountRepository.UpdateAsync(accountTo);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new TransferResponse
                {
                    Success = false,
                };
            }

            return new TransferResponse
            {
                Success = true,
            };
        }

        public override async Task<TransferResponse> Deposit(TransferRequest request, ServerCallContext context)
        {
            try
            {
                var account = await _accountRepository.GetByIdAsync(Guid.Parse(request.ToAccountId));
                account.Deposit(new Money(decimal.Parse(request.Money.Amount), request.Money.Currency));
                _accountRepository.UpdateAsync(account);

                return new TransferResponse
                {
                    Success = true,
                    Balance = account.Balance.Amount.ToString(),
                };
            }
            catch (Exception ex) 
            {
                _logger.Error(ex.Message);
                return new TransferResponse
                {
                    Success = false,
                };
            }

           
        }

        public override async Task<TransferResponse> Withdraw(TransferRequest request, ServerCallContext context)
        {
            try
            {
                var account = await _accountRepository.GetByIdAsync(Guid.Parse(request.FromAccountId));
                account.Withdraw(new Money(decimal.Parse(request.Money.Amount), request.Money.Currency));
                _accountRepository.UpdateAsync(account);

                return new TransferResponse
                {
                    Success = true,
                    Balance = account.Balance.Amount.ToString(),
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new TransferResponse
                {
                    Success = false,
                };
            }
        }

        public override async Task<GetTransactionsResponse> GetTransactions(GetTransactionsRequest request, ServerCallContext context)
        {
            IReadOnlyCollection<Transaction> transactions = await _accountRepository.GetTransactionsAsync(Guid.Parse(request.AccountId), request.PageNumber, request.PageSize);
            var response = new GetTransactionsResponse();
            response.Transactions.AddRange(transactions.Select(t => new Grpc.Transaction
            {
                TransactionId = t.TransactionId.ToString(),
                Amount = t.Money.Amount.ToString(),   
                Type = t.Type.ToString(),
                CreateDate = t.CreateDate.ToTimestamp(),

            }));
            return response;
        }
    }
}
