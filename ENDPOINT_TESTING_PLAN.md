# 🧪 ENDPOINT TESTING PLAN

## Status: READY FOR TESTING

---

## **How to Test Each Day**

### **Quick Start Template**

For any day:

```bash
cd days/Day[XX]-[Topic]/Day[XX]-Complete
dotnet run
```

Then:
1. Wait for "Now listening on: https://localhost:5001"
2. Open browser to `https://localhost:5001/swagger/index.html`
3. Click any endpoint
4. Click "Try it out"
5. Click "Execute"
6. See response

---

## **Day 08 Test Cases**

### **Setup**
```bash
cd days/Day08-Classes-And-Objects/Day08-Complete
dotnet run
# Keep running in terminal
```

### **Test 1: Health Check** ✅
```
GET /health
Expected: { "status": "ok" }
```

### **Test 2: Get All Customers**
```
GET /customers
Expected: Array of 3 customers (Alice, Bob, Charlie)
```

### **Test 3: Get Customer by ID**
```
GET /customers/1
Expected: Alice Johnson's customer object
```

### **Test 4: Create Customer** 
```
POST /customers
Body: { "name": "Diana Prince", "email": "diana@example.com" }
Expected: 201 Created with customer ID
```

### **Test 5: Get All Work Orders**
```
GET /workorders
Expected: Array of 3 work orders (Gutter, Lawn, Window)
```

### **Test 6: Get Work Order by ID**
```
GET /workorders/1
Expected: Gutter Cleaning work order
```

### **Test 7: Create Work Order**
```
POST /workorders
Body: { "customerId": 1, "description": "Roof Inspection" }
Expected: 201 Created with work order ID
```

---

## **Day 09 Test Cases** (Same as Day 08, better organized)

### **Differences from Day 08**
- Endpoints now return **DTOs** instead of domain models
- Same functionality, cleaner structure
- Organized endpoint files

### **Test Same 7 Endpoints**
- Health check
- CRUD for customers
- CRUD for work orders

---

## **Day 10 Test Cases** (Day 09 + Error Handling)

### **New Endpoints**
```
GET /workorders/customer/{customerId}
PUT /workorders/{id}/status
```

### **Error Validation Tests**
```
POST /customers (with invalid data)
Expected: 400 Bad Request with error details

POST /workorders (with missing customerId)
Expected: 400 Bad Request with validation errors

GET /customers/999
Expected: 404 Not Found
```

---

## **Day 11 Test Cases** (Day 10 + Search)

### **New Search Endpoints**
```
GET /customers/search/alice
Expected: Alice Johnson customer

GET /workorders/search/gutter
Expected: Gutter Cleaning work order

GET /workorders/status/Scheduled
Expected: All scheduled work orders
```

---

## **Testing Checklist**

### **For Each Day:**
- [ ] Project compiles: `dotnet build`
- [ ] API starts: `dotnet run`
- [ ] Swagger UI loads: https://localhost:5001/swagger
- [ ] All endpoints visible in Swagger
- [ ] Health check responds
- [ ] GET endpoints return data
- [ ] POST endpoints create data
- [ ] Seed data present
- [ ] Error handling works

---

## **What We Haven't Done Yet**

✅ **Compilation**: All 4 days compile
❌ **Execution**: Haven't actually run the apps
❌ **Endpoint Testing**: Haven't tested live endpoints
❌ **Data Validation**: Haven't verified response structure
❌ **Error Cases**: Haven't tested error scenarios

---

## **Next Steps**

To properly test:

1. **Run Day 08**
   ```bash
   cd days/Day08-Classes-And-Objects/Day08-Complete
   dotnet run
   ```

2. **Open Swagger**
   - Navigate to https://localhost:5001/swagger

3. **Test Each Endpoint**
   - Click endpoint
   - Try it out
   - Verify response

4. **Document Results**
   - Success: ✅
   - Failure: ❌ (and why)

5. **Move to Next Day**
   - Ctrl+C to stop
   - `cd ..` to parent
   - `cd ../Day09-*/Day09-Complete`
   - Repeat

---

## **Expected Results**

All endpoints should:
- ✅ Return correct HTTP status codes
- ✅ Return valid JSON responses
- ✅ Use seed data properly
- ✅ Create new data correctly
- ✅ Handle errors gracefully

---

## **Scripts Available**

- `test-day08-endpoints.ps1` - PowerShell test script
- `test-day08-endpoints.sh` - Bash test script (if on Linux/Mac)

---

**Ready to test?** Let's start with Day 08! 🚀
