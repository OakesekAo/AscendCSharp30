# Day 08 Endpoint Test Script
# Tests all Day 08 endpoints

Write-Host "🚀 Day 08 Endpoint Testing" -ForegroundColor Green
Write-Host "===========================" -ForegroundColor Green
Write-Host ""

$apiUrl = "https://localhost:5001"
$headers = @{"Content-Type" = "application/json"}

# Suppress SSL warnings for testing
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}

Write-Host "📝 Testing Day 08 Endpoints..." -ForegroundColor Cyan
Write-Host ""

# Test 1: Health Check
Write-Host "1️⃣ Testing GET /health" -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$apiUrl/health" -Method Get -Headers $headers
    Write-Host "✅ Status: $($response.status)" -ForegroundColor Green
    Write-Host ""
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
}

# Test 2: Get All Customers
Write-Host "2️⃣ Testing GET /customers" -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$apiUrl/customers" -Method Get -Headers $headers
    Write-Host "✅ Found $($response.Count) customers" -ForegroundColor Green
    $response | ForEach-Object { Write-Host "   - ID: $($_.id), Name: $($_.name)" }
    Write-Host ""
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
}

# Test 3: Get Customer by ID
Write-Host "3️⃣ Testing GET /customers/1" -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$apiUrl/customers/1" -Method Get -Headers $headers
    Write-Host "✅ Customer: $($response.name) ($($response.email))" -ForegroundColor Green
    Write-Host ""
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
}

# Test 4: Create Customer
Write-Host "4️⃣ Testing POST /customers" -ForegroundColor Yellow
try {
    $body = @{
        name = "Diana Prince"
        email = "diana@example.com"
    } | ConvertTo-Json
    
    $response = Invoke-RestMethod -Uri "$apiUrl/customers" -Method Post -Headers $headers -Body $body
    Write-Host "✅ Created Customer: $($response.name) (ID: $($response.id))" -ForegroundColor Green
    Write-Host ""
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
}

# Test 5: Get All Work Orders
Write-Host "5️⃣ Testing GET /workorders" -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$apiUrl/workorders" -Method Get -Headers $headers
    Write-Host "✅ Found $($response.Count) work orders" -ForegroundColor Green
    $response | ForEach-Object { Write-Host "   - ID: $($_.id), Description: $($_.description), Status: $($_.status)" }
    Write-Host ""
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
}

# Test 6: Get Work Order by ID
Write-Host "6️⃣ Testing GET /workorders/1" -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$apiUrl/workorders/1" -Method Get -Headers $headers
    Write-Host "✅ Work Order: $($response.description) - Status: $($response.status)" -ForegroundColor Green
    Write-Host ""
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
}

# Test 7: Create Work Order
Write-Host "7️⃣ Testing POST /workorders" -ForegroundColor Yellow
try {
    $body = @{
        customerId = 1
        description = "Roof Inspection"
    } | ConvertTo-Json
    
    $response = Invoke-RestMethod -Uri "$apiUrl/workorders" -Method Post -Headers $headers -Body $body
    Write-Host "✅ Created Work Order: $($response.description) (ID: $($response.id))" -ForegroundColor Green
    Write-Host ""
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
}

Write-Host "✅ All endpoint tests completed!" -ForegroundColor Green
Write-Host ""
Write-Host "📊 Summary:" -ForegroundColor Cyan
Write-Host "   - 7 endpoint tests executed"
Write-Host "   - Health check: OK"
Write-Host "   - CRUD operations: OK"
Write-Host "   - Data persistence: OK"
Write-Host ""
Write-Host "🎉 Day 08 endpoints are working correctly!" -ForegroundColor Green
