#!/bin/bash
# Day 08 Endpoint Test Script
# Tests all endpoints for Day 08

echo "🚀 Starting Day 08 API..."
cd days/Day08-Classes-And-Objects/Day08-Complete

# Start the API in background
dotnet run &
API_PID=$!

# Wait for API to start
sleep 5

echo "✅ API Started (PID: $API_PID)"
echo ""
echo "📝 Testing Endpoints..."
echo ""

# Test 1: Health check
echo "1️⃣ Testing GET /health"
curl -s -k https://localhost:5001/health | jq .
echo ""

# Test 2: Get all customers
echo "2️⃣ Testing GET /customers"
curl -s -k https://localhost:5001/customers | jq .
echo ""

# Test 3: Get customer by ID
echo "3️⃣ Testing GET /customers/1"
curl -s -k https://localhost:5001/customers/1 | jq .
echo ""

# Test 4: Create customer
echo "4️⃣ Testing POST /customers"
curl -s -k -X POST https://localhost:5001/customers \
  -H "Content-Type: application/json" \
  -d '{"name":"Diana Prince","email":"diana@example.com"}' | jq .
echo ""

# Test 5: Get all work orders
echo "5️⃣ Testing GET /workorders"
curl -s -k https://localhost:5001/workorders | jq .
echo ""

# Test 6: Get work order by ID
echo "6️⃣ Testing GET /workorders/1"
curl -s -k https://localhost:5001/workorders/1 | jq .
echo ""

# Test 7: Create work order
echo "7️⃣ Testing POST /workorders"
curl -s -k -X POST https://localhost:5001/workorders \
  -H "Content-Type: application/json" \
  -d '{"customerId":1,"description":"Roof Inspection"}' | jq .
echo ""

echo "✅ All tests completed!"
echo ""
echo "🛑 Stopping API..."
kill $API_PID

echo "✅ Done!"
