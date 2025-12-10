using System.Net.Http.Json;
using System.Text.Json;

// Day 08 Endpoint Integration Tests

namespace EndpointTests;

class Program
{
    static async Task Main(string[] args)
    {
        const string baseUrl = "https://localhost:5001";
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true;
        var client = new HttpClient(handler) { BaseAddress = new Uri(baseUrl), Timeout = TimeSpan.FromSeconds(10) };

        Console.WriteLine("\n🚀 Day 08 Endpoint Integration Tests");
        Console.WriteLine("====================================\n");

        var testsPassed = 0;
        var testsFailed = 0;

        try
        {
            // Test 1: Health Check
            Console.WriteLine("Test 1: GET /health");
            try
            {
                var response = await client.GetAsync("/health");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("ok"))
                    {
                        Console.WriteLine("   ✅ PASS - Health check working\n");
                        testsPassed++;
                    }
                    else
                    {
                        Console.WriteLine("   ❌ FAIL - Unexpected response\n");
                        testsFailed++;
                    }
                }
                else
                {
                    Console.WriteLine($"   ❌ FAIL - Status: {response.StatusCode}\n");
                    testsFailed++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ❌ FAIL - {ex.Message}\n");
                testsFailed++;
            }

            // Test 2: Get All Customers
            Console.WriteLine("Test 2: GET /customers");
            try
            {
                var response = await client.GetAsync("/customers");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var doc = JsonDocument.Parse(content);
                    if (doc.RootElement.ValueKind == JsonValueKind.Array && doc.RootElement.GetArrayLength() > 0)
                    {
                        Console.WriteLine($"   ✅ PASS - Found {doc.RootElement.GetArrayLength()} customers\n");
                        testsPassed++;
                    }
                    else
                    {
                        Console.WriteLine("   ❌ FAIL - No customers found\n");
                        testsFailed++;
                    }
                }
                else
                {
                    Console.WriteLine($"   ❌ FAIL - Status: {response.StatusCode}\n");
                    testsFailed++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ❌ FAIL - {ex.Message}\n");
                testsFailed++;
            }

            // Test 3: Get Customer by ID
            Console.WriteLine("Test 3: GET /customers/1");
            try
            {
                var response = await client.GetAsync("/customers/1");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("Alice"))
                    {
                        Console.WriteLine("   ✅ PASS - Retrieved Alice Johnson\n");
                        testsPassed++;
                    }
                    else
                    {
                        Console.WriteLine("   ❌ FAIL - Wrong customer\n");
                        testsFailed++;
                    }
                }
                else
                {
                    Console.WriteLine($"   ❌ FAIL - Status: {response.StatusCode}\n");
                    testsFailed++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ❌ FAIL - {ex.Message}\n");
                testsFailed++;
            }

            // Test 4: Create Customer
            Console.WriteLine("Test 4: POST /customers (create new)");
            try
            {
                var newCustomer = new { name = "Test User", email = "test@example.com" };
                var response = await client.PostAsJsonAsync("/customers", newCustomer);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("Test User"))
                    {
                        Console.WriteLine("   ✅ PASS - Customer created successfully\n");
                        testsPassed++;
                    }
                    else
                    {
                        Console.WriteLine("   ❌ FAIL - Customer not in response\n");
                        testsFailed++;
                    }
                }
                else
                {
                    Console.WriteLine($"   ❌ FAIL - Status: {response.StatusCode}\n");
                    testsFailed++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ❌ FAIL - {ex.Message}\n");
                testsFailed++;
            }

            // Test 5: Get All Work Orders
            Console.WriteLine("Test 5: GET /workorders");
            try
            {
                var response = await client.GetAsync("/workorders");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var doc = JsonDocument.Parse(content);
                    if (doc.RootElement.ValueKind == JsonValueKind.Array && doc.RootElement.GetArrayLength() > 0)
                    {
                        Console.WriteLine($"   ✅ PASS - Found {doc.RootElement.GetArrayLength()} work orders\n");
                        testsPassed++;
                    }
                    else
                    {
                        Console.WriteLine("   ❌ FAIL - No work orders found\n");
                        testsFailed++;
                    }
                }
                else
                {
                    Console.WriteLine($"   ❌ FAIL - Status: {response.StatusCode}\n");
                    testsFailed++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ❌ FAIL - {ex.Message}\n");
                testsFailed++;
            }

            // Test 6: Get Work Order by ID
            Console.WriteLine("Test 6: GET /workorders/1");
            try
            {
                var response = await client.GetAsync("/workorders/1");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("Gutter"))
                    {
                        Console.WriteLine("   ✅ PASS - Retrieved Gutter Cleaning work order\n");
                        testsPassed++;
                    }
                    else
                    {
                        Console.WriteLine("   ❌ FAIL - Wrong work order\n");
                        testsFailed++;
                    }
                }
                else
                {
                    Console.WriteLine($"   ❌ FAIL - Status: {response.StatusCode}\n");
                    testsFailed++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ❌ FAIL - {ex.Message}\n");
                testsFailed++;
            }

            // Test 7: Create Work Order
            Console.WriteLine("Test 7: POST /workorders (create new)");
            try
            {
                var newOrder = new { customerId = 1, description = "Integration Test Order" };
                var response = await client.PostAsJsonAsync("/workorders", newOrder);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("Integration Test"))
                    {
                        Console.WriteLine("   ✅ PASS - Work order created successfully\n");
                        testsPassed++;
                    }
                    else
                    {
                        Console.WriteLine("   ❌ FAIL - Work order not in response\n");
                        testsFailed++;
                    }
                }
                else
                {
                    Console.WriteLine($"   ❌ FAIL - Status: {response.StatusCode}\n");
                    testsFailed++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ❌ FAIL - {ex.Message}\n");
                testsFailed++;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Fatal Error: {ex.Message}\n");
            Console.WriteLine("Make sure the Day 08 API is running:");
            Console.WriteLine("  cd days/Day08-Classes-And-Objects/Day08-Complete");
            Console.WriteLine("  dotnet run\n");
        }

        // Final Summary
        Console.WriteLine("====================================");
        Console.WriteLine($"📊 Test Results: {testsPassed} Passed, {testsFailed} Failed");
        Console.WriteLine("====================================\n");

        if (testsFailed == 0)
        {
            Console.WriteLine("🎉 All Day 08 endpoints are working correctly!");
        }
        else
        {
            Console.WriteLine($"⚠️ {testsFailed} test(s) failed. Check the API is running and responding.");
        }
    }
}
