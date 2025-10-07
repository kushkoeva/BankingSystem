using BankingSystem.Client;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("http://localhost:5276");
var bankingClient = new Banking.BankingClient(channel);


var customerResponse = await bankingClient.CreateCustomerAsync(new CreateCustomerRequest
{
    FirstName = "Марина",
    LastName = "Добродеева",
    MiddleName = "Олеговна",
    Phone = "79242908848",
    Email = "dobro@gmail.com",
    Address = new Address 
    {
        Country = "Россия",
        Region = "Иркутская",
        City = "Иркутск",
        Street = "Ленина",
        House = "1",
        ZipCode = "664011"
    }
});

Console.WriteLine($"Customer created: {customerResponse.CustomerId}");

var accountResponse = await bankingClient.CreateAccountAsync(new CreateAccountRequest
{
    CustomerId = customerResponse.CustomerId,
    Balance = new Money
    {
        Amount = "10000",
        Currency = "RUB"
    }
});

Console.WriteLine($"Account created: {accountResponse.AccountId}");

var depositResponse = await bankingClient.DepositAsync(new TransferRequest
{
    ToAccountId = accountResponse.AccountId,
    Money = new Money
    {
        Amount = "500",
        Currency = "RUB"
    }
});

Console.WriteLine($"New balance after deposit: {depositResponse.Balance}");

var withdrawResponse = await bankingClient.WithdrawAsync(new TransferRequest
{
    FromAccountId = accountResponse.AccountId,
    Money = new Money
    {
        Amount = "200",
        Currency = "RUB"
    }
});

Console.WriteLine($"New balance after withdrawal: {withdrawResponse.Balance}");
