E-Commerce API for your project. It's structured to give new developers and collaborators a clear understanding of setup, assumptions, tech stack, and workflow.
A clean, modular ASP.NET Core Web API for managing products, carts, orders, and authentication with JWT-based role authorization. Built using the Repository + UnitOfWork pattern for maintainability and scalability.

*Features

. Authentication & Authorization 
User registration & login with ASP.NET Identity 
JWT token issuance with role claims 
Role-based access control (User, Admin)

. Products
CRUD operations
Search, stock management
Product images

. Cart
Add/remove items
Clear cart
Checkout → creates an order

. Orders
View user orders
Admin management of all orders
Order items with quantity updates

*Tech Stack
.NET 8 / ASP.NET Core Web API
Entity Framework Core (SQL Server by default)
ASP.NET Identity for user management
JWT (JSON Web Tokens) for authentication
Repository + UnitOfWork Pattern
Swagger/OpenAPI for API documentation

*Assumptions
Users must be authenticated to access most endpoints.
Admin role is required for product/order management.
Cart checkout converts items into an order and clears the cart.
No seed data - first admin must be created manually via assign- role.
All responses are wrapped in a consistent ApiResponse<T> format.

