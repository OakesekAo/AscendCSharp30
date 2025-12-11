// Day 22 - Unit Testing with xUnit (Complete)
using Xunit;

namespace ServiceHub.Day22.Tests;

public class CustomerServiceTests
{
    [Fact]
    public async Task CreateCustomer_WithValidData_ReturnsCustomer()
    {
        var repo = new MockCustomerRepository();
        var service = new CustomerService(repo);
        var result = await service.CreateAsync("John", "john@example.com");
        Assert.NotNull(result);
        Assert.Equal("John", result.Name);
    }

    [Fact]
    public async Task CreateCustomer_WithEmptyName_ThrowsException()
    {
        var repo = new MockCustomerRepository();
        var service = new CustomerService(repo);
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync("", "test@example.com"));
    }

    [Fact]
    public async Task GetCustomer_WithValidId_ReturnsCustomer()
    {
        var repo = new MockCustomerRepository();
        var service = new CustomerService(repo);
        var result = await service.GetAsync(1);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetAllCustomers_ReturnsAllCustomers()
    {
        var repo = new MockCustomerRepository();
        var service = new CustomerService(repo);
        var result = await service.GetAllAsync();
        Assert.NotEmpty(result);
        Assert.True(result.Count > 0);
    }
}

public class WorkOrderServiceTests
{
    [Fact]
    public async Task CreateWorkOrder_WithValidData_ReturnsWorkOrder()
    {
        var repo = new MockWorkOrderRepository();
        var service = new WorkOrderService(repo);
        var result = await service.CreateAsync(1, "Test task");
        Assert.NotNull(result);
        Assert.Equal("Test task", result.Description);
    }

    [Fact]
    public async Task GetAllWorkOrders_ReturnsAllOrders()
    {
        var repo = new MockWorkOrderRepository();
        var service = new WorkOrderService(repo);
        var result = await service.GetAllAsync();
        Assert.NotEmpty(result);
    }
}

public class ValidationServiceTests
{
    [Fact]
    public void ValidateCustomer_WithValidData_ReturnsNoErrors()
    {
        var service = new ValidationService();
        var errors = service.ValidateCustomer(new CreateCustomerRequest("John", "john@example.com"));
        Assert.Empty(errors);
    }

    [Fact]
    public void ValidateCustomer_WithEmptyName_ReturnsError()
    {
        var service = new ValidationService();
        var errors = service.ValidateCustomer(new CreateCustomerRequest("", "john@example.com"));
        Assert.NotEmpty(errors);
        Assert.Contains("Name required", errors);
    }

    [Fact]
    public void ValidateCustomer_WithInvalidEmail_ReturnsError()
    {
        var service = new ValidationService();
        var errors = service.ValidateCustomer(new CreateCustomerRequest("John", "invalid-email"));
        Assert.NotEmpty(errors);
    }

    [Fact]
    public void ValidateWorkOrder_WithValidData_ReturnsNoErrors()
    {
        var service = new ValidationService();
        var errors = service.ValidateWorkOrder(new CreateWorkOrderRequest(1, "Task"));
        Assert.Empty(errors);
    }

    [Fact]
    public void ValidateWorkOrder_WithInvalidCustomerId_ReturnsError()
    {
        var service = new ValidationService();
        var errors = service.ValidateWorkOrder(new CreateWorkOrderRequest(0, "Task"));
        Assert.NotEmpty(errors);
    }
}

// Models and Services
public record Customer(int Id, string Name, string Email);
public record WorkOrder(int Id, int CustomerId, string Description, string Status);
public record CreateCustomerRequest(string Name, string Email);
public record CreateWorkOrderRequest(int CustomerId, string Description);

public interface ICustomerRepository
{
    Task<Customer?> GetAsync(int id);
    Task<List<Customer>> GetAllAsync();
    Task AddAsync(Customer c);
}

public interface IWorkOrderRepository
{
    Task<WorkOrder?> GetAsync(int id);
    Task<List<WorkOrder>> GetAllAsync();
    Task AddAsync(WorkOrder o);
}

public class ValidationService
{
    public List<string> ValidateCustomer(CreateCustomerRequest r)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(r.Name)) errors.Add("Name required");
        if (string.IsNullOrWhiteSpace(r.Email) || !r.Email.Contains("@")) errors.Add("Valid email required");
        return errors;
    }

    public List<string> ValidateWorkOrder(CreateWorkOrderRequest r)
    {
        var errors = new List<string>();
        if (r.CustomerId <= 0) errors.Add("Valid customer required");
        if (string.IsNullOrWhiteSpace(r.Description)) errors.Add("Description required");
        return errors;
    }
}

public class CustomerService
{
    private readonly ICustomerRepository _repo;
    public CustomerService(ICustomerRepository r) => _repo = r;
    public async Task<Customer> CreateAsync(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email required");
        var c = new Customer(0, name, email);
        await _repo.AddAsync(c);
        return c;
    }
    public async Task<Customer?> GetAsync(int id) => await _repo.GetAsync(id);
    public async Task<List<Customer>> GetAllAsync() => await _repo.GetAllAsync();
}

public class WorkOrderService
{
    private readonly IWorkOrderRepository _repo;
    public WorkOrderService(IWorkOrderRepository r) => _repo = r;
    public async Task<WorkOrder> CreateAsync(int cid, string desc)
    {
        var o = new WorkOrder(0, cid, desc, "Scheduled");
        await _repo.AddAsync(o);
        return o;
    }
    public async Task<List<WorkOrder>> GetAllAsync() => await _repo.GetAllAsync();
}

// Mock Repositories for Testing
public class MockCustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _list = new() { new(1, "Alice", "a@ex.com"), new(2, "Bob", "b@ex.com") };
    public Task<Customer?> GetAsync(int id) => Task.FromResult(_list.FirstOrDefault(x => x.Id == id));
    public Task<List<Customer>> GetAllAsync() => Task.FromResult(_list);
    public Task AddAsync(Customer c) { _list.Add(c); return Task.CompletedTask; }
}

public class MockWorkOrderRepository : IWorkOrderRepository
{
    private readonly List<WorkOrder> _list = new() { new(1, 1, "Task", "Scheduled") };
    public Task<WorkOrder?> GetAsync(int id) => Task.FromResult(_list.FirstOrDefault(x => x.Id == id));
    public Task<List<WorkOrder>> GetAllAsync() => Task.FromResult(_list);
    public Task AddAsync(WorkOrder o) { _list.Add(o); return Task.CompletedTask; }
}
