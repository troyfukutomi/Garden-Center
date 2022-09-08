Garden Center Capstone
A working web API built using .net and c#. Entities include Customers, Orders,
Products, and Users. Customers have a nested entity of Address. Orders have nested items, and items have nested products. And Users have nested roles. Each Customers, Orders,Products, and Users have controllers that allow for CRUD operations. The validation logic is stored seperately and called within the controllers

## Postman
A Postman collection demonstrating all 2XX and 4XX scenarios is linked below.
Postman collection:
https://www.getpostman.com/collections/1724b5db2bd55abffa22

## Logging
All Errors are logged to the Log.txt file.

## Testing
Unit tests test for all validation methods
To run unit tests, navigate to CustomerTest.cs, OrderTest.cs, ProductTest.cs, or UserTest.cs.
Each of these have their own set fo tests that can be run with the Run All Tests button near the top of the file. 

Coverage is low as the tests only cover the validation methods for each entity. 
Coverage on the validation methods are much better.

to see coverage run 
dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=lcov -p:CoverletOutput=./lcov.info

To see what lines are covered, enter ctrl + shift + p and select Coverage Gutters

All methods for each entities validation are covered.

## Linting 
To lint project use: Shift + Alt + F