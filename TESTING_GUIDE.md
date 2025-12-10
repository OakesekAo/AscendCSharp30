# 🧪 COMPREHENSIVE ENDPOINT TESTING GUIDE

## How to Actually Test All Endpoints

This guide walks through testing **Day 08, 09, 10, and 11** endpoints in detail.

---

## **Quick Setup for Any Day**

### **Terminal 1: Run the API**
```bash
cd days/Day08-Classes-And-Objects/Day08-Complete
dotnet run
```

You should see:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
```

Keep this running.

### **Terminal 2 (or Browser): Test the API**

Either:
- **Option A**: Use Swagger UI (browser)
- **Option B**: Use the provided test project (console)
- **Option C**: Use curl/Postman manually

---

## **Option A: Swagger UI (Easiest)**

1. **Open browser** to: `https://localhost:5001/swagger/index.html`

2. **You'll see all endpoints listed**

3. **Click any endpoint** (e.g., `GET /health`)

4. **Click "Try it out"**

5. **Click "Execute"**

6. **See the response!**

---

## **Option B: Use the Test Project (Automated)**

### **Run the integration test:**

```bash
cd EndpointTests
dotnet run
```

This will:
- ✅ Test all 7 Day 08 endpoints
- ✅ Verify responses
- ✅ Create test data
- ✅ Show pass/fail for each
- ✅ Display summary

---

## **Option C: Manual Testing (curl)**

### **Test 1: Health Check**
```bash
curl -k https://localhost:5001/health
```
**Expected Response:**
```json
{"status":"ok"}
```

### **Test 2: Get All Customers**
```bash
curl -k https://localhost:5001/customers
```
**Expected Response:**
```json
[
  {"id":1,"name":"Alice Johnson","email":"alice@example.com"},
  {"id":2,"name":"Bob Smith","email":"bob@example.com"},
  {"id":3,"name":"Charlie Brown","email":"charlie@example.com"}
]
```

### **Test 3: Get Customer by ID**
```bash
curl -k https://localhost:5001/customers/1
```
**Expected Response:**
```json
{"id":1,"name":"Alice Johnson","email":"alice@example.com"}
```

### **Test 4: Create Customer**
```bash
curl -k -X POST https://localhost:5001/customers \
  -H "Content-Type: application/json" \
  -d '{"name":"Diana Prince","email":"diana@example.com"}'
```
**Expected Response:** `201 Created`
```json
{"id":4,"name":"Diana Prince","email":"diana@example.com"}
```

### **Test 5: Get All Work Orders**
```bash
curl -k https://localhost:5001/workorders
```
**Expected Response:**
```json
[
  {"id":1,"customerId":1,"description":"Gutter Cleaning","status":"Scheduled"},
  {"id":2,"customerId":2,"description":"Lawn Mowing","status":"Scheduled"},
  {"id":3,"customerId":1,"description":"Window Washing","status":"InProgress"}
]
```

### **Test 6: Get Work Order by ID**
```bash
curl -k https://localhost:5001/workorders/1
```
**Expected Response:**
```json
{"id":1,"customerId":1,"description":"Gutter Cleaning","status":"Scheduled"}
```

### **Test 7: Create Work Order**
```bash
curl -k -X POST https://localhost:5001/workorders \
  -H "Content-Type: application/json" \
  -d '{"customerId":1,"description":"Roof Inspection"}'
```
**Expected Response:** `201 Created`
```json
{"id":4,"customerId":1,"description":"Roof Inspection","status":"Scheduled"}
```

---

## **Day 09 Testing**

Same endpoints as Day 08, but with:
- ✅ Organized endpoint files
- ✅ DTOs in requests/responses
- ✅ Mapper extension methods

### **Run Day 09:**
```bash
cd days/Day09-Interfaces-And-Abstraction/Day09-Complete
dotnet run
```

**All 7 endpoints work identically to Day 08!**

---

## **Day 10 Testing (Error Handling)**

Everything from Day 09, PLUS:

### **New Endpoint 1: Get Work Orders by Customer**
```bash
curl -k https://localhost:5001/workorders/customer/1
```
**Expected Response:** Array of work orders for customer 1

### **New Endpoint 2: Update Work Order Status**
```bash
curl -k -X PUT https://localhost:5001/workorders/1/status \
  -H "Content-Type: application/json" \
  -d '{"status":"Completed"}'
```
**Expected Response:** Updated work order with new status

### **Test Error Handling**

**Invalid customer creation (missing email):**
```bash
curl -k -X POST https://localhost:5001/customers \
  -H "Content-Type: application/json" \
  -d '{"name":"Test"}'
```
**Expected Response:** `400 Bad Request`
```json
{"error":"Validation failed","details":"Email is required"}
```

**Invalid work order (no customer ID):**
```bash
curl -k -X POST https://localhost:5001/workorders \
  -H "Content-Type: application/json" \
  -d '{"description":"Test"}'
```
**Expected Response:** `400 Bad Request`
```json
{"error":"Validation failed","details":"CustomerId must be greater than 0"}
```

### **Run Day 10:**
```bash
cd days/Day10-Inheritance-And-Polymorphism/Day10-Complete
dotnet run
```

---

## **Day 11 Testing (Search & Filtering)**

Everything from Day 10, PLUS:

### **New Endpoint 1: Search Customers**
```bash
curl -k "https://localhost:5001/customers/search/alice"
```
**Expected Response:** Customers matching "alice"

### **New Endpoint 2: Search Work Orders**
```bash
curl -k "https://localhost:5001/workorders/search/gutter"
```
**Expected Response:** Work orders matching "gutter"

### **New Endpoint 3: Filter by Status**
```bash
curl -k "https://localhost:5001/workorders/status/Scheduled"
```
**Expected Response:** All scheduled work orders

### **Run Day 11:**
```bash
cd days/Day11-Polymorphism-Advanced/Day11-Complete
dotnet run
```

---

## **Testing Checklist**

### **For Each Day:**

- [ ] API starts without errors
- [ ] Swagger UI loads
- [ ] Health check responds
- [ ] GET /customers returns array with 3+ items
- [ ] GET /customers/{id} returns single customer
- [ ] POST /customers creates new customer
- [ ] GET /workorders returns array with 3+ items
- [ ] GET /workorders/{id} returns single work order
- [ ] POST /workorders creates new work order
- [ ] Error responses work (400, 404, etc.)

### **Day 10 Additional:**
- [ ] GET /workorders/customer/{id} works
- [ ] PUT /workorders/{id}/status updates status
- [ ] Validation errors return proper messages

### **Day 11 Additional:**
- [ ] GET /customers/search/{term} searches names/emails
- [ ] GET /workorders/search/{term} searches descriptions
- [ ] GET /workorders/status/{status} filters correctly

---

## **Troubleshooting**

### **"Connection refused"**
- Is the API running? Check Terminal 1
- Is it running on localhost:5001?
- Try `dotnet run` again

### **"SSL certificate problem"**
- Use `-k` flag with curl (skip SSL verification)
- Or accept the certificate in browser first

### **"404 Not Found"**
- Check the endpoint URL spelling
- Verify you're on the right Day project
- Check the Program.cs for mapped endpoints

### **"400 Bad Request"**
- Check your JSON body format
- Verify all required fields are present
- Look at the error message for details

---

## **Success Indicators**

✅ **Day 08 Success:**
```
Test 1: Health - PASS
Test 2: Get Customers - PASS
Test 3: Get Customer - PASS
Test 4: Create Customer - PASS
Test 5: Get Work Orders - PASS
Test 6: Get Work Order - PASS
Test 7: Create Work Order - PASS

All tests passed! ✅
```

---

## **Next Steps After Testing**

1. ✅ Test Day 08 (basic CRUD)
2. ✅ Test Day 09 (organized + DTOs)
3. ✅ Test Day 10 (error handling + new endpoints)
4. ✅ Test Day 11 (search + filtering)
5. Move on to complete Days 12-14

---

**Ready to test? Start with Day 08!** 🚀
