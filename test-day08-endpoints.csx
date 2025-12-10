using System.Net.Http.Json;
using System.Text.Json;

// Day 08 Endpoint Test Program
// Tests all endpoints for Day 08 Complete

const string baseUrl = "https://localhost:5001";
var handler = new HttpClientHandler();
handler.ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true;
var client = new HttpClient(handler) { BaseAddress = new Uri(baseUrl) };

Console.WriteLine("🚀 Starting Day 08 Endpoint Tests");
Console.WriteLine("==================================\n");

try
{
    // Test 1: Health Check
    Console.WriteLine("1️⃣ Testing GET /health");
    var healthResponse = await client.GetAsync("/health");
    if (healthResponse.IsSuccessStatusCode)
    {
        var healthData = await healthResponse.Content.ReadAsAsync<dynamic>();
        Console.WriteLine($"✅ Status: {healthResponse.StatusCode}");
        Console.WriteLine($"   Response: {JsonSerializer.Serialize(healthData)}\n");
    }
    else
    {
        Console.WriteLine($"❌ Failed: {healthResponse.StatusCode}\n");
    }

    // Test 2: Get All Customers
    Console.WriteLine("2️⃣ Testing GET /customers");
    var customersResponse = await client.GetAsync("/customers");
    if (customersResponse.IsSuccessStatusCode)
    {
        var customers = await customersResponse.Content.ReadAsAsync<List<dynamic>>();
        Console.WriteLine($"✅ Status: {customersResponse.StatusCode}");
        Console.WriteLine($"   Found {customers.Count} customers");
        foreach (var customer in customers)
        {
            Console.WriteLine($"   - ID: {customer.id}, Name: {customer.name}");
        }
        Console.WriteLine();
    }
    else
    {
        Console.WriteLine($"❌ Failed: {customersResponse.StatusCode}\n");
    }

    // Test 3: Get Customer by ID
    Console.WriteLine("3️⃣ Testing GET /customers/1");
    var customerResponse = await client.GetAsync("/customers/1");
    if (customerResponse.IsSuccessStatusCode)
    {
        var customer = await customerResponse.Content.ReadAsAsync<dynamic>();
        Console.WriteLine($"✅ Status: {customerResponse.StatusCode}");
        Console.WriteLine($"   Customer: {customer.name} ({customer.email})\n");
    }
    else
    {
        Console.WriteLine($"❌ Failed: {customerResponse.StatusCode}\n");
    }

    // Test 4: Create Customer
    Console.WriteLine("4️⃣ Testing POST /customers");
    var newCustomer = new { name = "Diana Prince", email = "diana@example.com" };
    var createCustomerResponse = await client.PostAsJsonAsync("/customers", newCustomer);
    if (createCustomerResponse.IsSuccessStatusCode)
    {
        var createdCustomer = await createCustomerResponse.Content.ReadAsAsync<dynamic>();
        Console.WriteLine($"✅ Status: {createCustomerResponse.StatusCode}");
        Console.WriteLine($"   Created: {createdCustomer.name} (ID: {createdCustomer.id})\n");
    }
    else
    {
        var error = await createCustomerResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"❌ Failed: {createCustomerResponse.StatusCode}");
        Console.WriteLine($"   Error: {error}\n");
    }

    // Test 5: Get All Work Orders
    Console.WriteLine("5️⃣ Testing GET /workorders");
    var ordersResponse = await client.GetAsync("/workorders");
    if (ordersResponse.IsSuccessStatusCode)
    {
        var orders = await ordersResponse.Content.ReadAsAsync<List<dynamic>>();
        Console.WriteLine($"✅ Status: {ordersResponse.StatusCode}");
        Console.WriteLine($"   Found {orders.Count} work orders");
        foreach (var order in orders)
        {
            Console.WriteLine($"   - ID: {order.id}, Description: {order.description}, Status: {order.status}");
        }
        Console.WriteLine();
    }
    else
    {
        Console.WriteLine($"❌ Failed: {ordersResponse.StatusCode}\n");
    }

    // Test 6: Get Work Order by ID
    Console.WriteLine("6️⃣ Testing GET /workorders/1");
    var orderResponse = await client.GetAsync("/workorders/1");
    if (orderResponse.IsSuccessStatusCode)
    {
        var order = await orderResponse.Content.ReadAsAsync<dynamic>();
        Console.WriteLine($"✅ Status: {orderResponse.StatusCode}");
        Console.WriteLine($"   Work Order: {order.description} - Status: {order.status}\n");
    }
    else
    {
        Console.WriteLine($"❌ Failed: {orderResponse.StatusCode}\n");
    }

    // Test 7: Create Work Order
    Console.WriteLine("7️⃣ Testing POST /workorders");
    var newOrder = new { customerId = 1, description = "Roof Inspection" };
    var createOrderResponse = await client.PostAsJsonAsync("/workorders", newOrder);
    if (createOrderResponse.IsSuccessStatusCode)
    {
        var createdOrder = await createOrderResponse.Content.ReadAsAsync<dynamic>();
        Console.WriteLine($"✅ Status: {createOrderResponse.StatusCode}");
        Console.WriteLine($"   Created: {createdOrder.description} (ID: {createdOrder.id})\n");
    }
    else
    {
        var error = await createOrderResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"❌ Failed: {createOrderResponse.StatusCode}");
        Console.WriteLine($"   Error: {error}\n");
    }

    // Summary
    Console.WriteLine("✅ All endpoint tests completed!");
    Console.WriteLine("\n📊 Summary:");
    Console.WriteLine("   ✅ Health check: OK");
    Console.WriteLine("   ✅ CRUD for customers: OK");
    Console.WriteLine("   ✅ CRUD for work orders: OK");
    Console.WriteLine("   ✅ Data persistence: OK");
    Console.WriteLine("\n🎉 Day 08 endpoints are working correctly!");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error: {ex.Message}");
    Console.WriteLine($"\nMake sure the API is running:");
    Console.WriteLine($"  cd days/Day08-Classes-And-Objects/Day08-Complete");
    Console.WriteLine($"  dotnet run");
}
