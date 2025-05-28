using EmployeeManager.Models;
using EmployeeManagerApi.Controllers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;



namespace EmployeeManager.EmployeeManager.Tests
{
    public class EmployeesControllerTests
    {
        private ApplicationDbContext _context;
        private EmployeesController _controller;

        
        [SetUp]
        public void Setup()
        {
            // Create a fresh context for each test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
                .Options;

            _context = new ApplicationDbContext(options);

            // Seed test data
            _context.Employees.AddRange(new List<Employee>
            {
                new Employee { Id = 1, FirstName = "Alice", LastName = "tttt", Email = "alice@gmail.com", Department = "IT", DateOfBirth = new DateTime(1990, 1, 1) },
                new Employee { Id = 2, FirstName = "Bob", LastName = "tttt", Email = "bob@gmail.com", Department = "HR", DateOfBirth = new DateTime(1985, 5, 5) }
            });

            _context.SaveChanges();

            _controller = new EmployeesController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }

        [Test]
        public async Task GetEmployees_ReturnsAllEmployees()
        {
            var result = await _controller.GetEmployees();
            Assert.That(result, Is.InstanceOf<ActionResult<IEnumerable<Employee>>>());
            Assert.That(result.Value, Is.InstanceOf<IEnumerable<Employee>>());
            Assert.That(result.Value.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetEmployee_ValidId_ReturnsEmployee()
        {
            var result = await _controller.GetEmployee(1);
            Assert.That(result.Value, Is.InstanceOf<Employee>());
            Assert.That(result.Value.FirstName, Is.EqualTo("Alice"));
        }

        [Test]
        public async Task GetEmployee_InvalidId_ReturnsNotFound()
        {
            var result = await _controller.GetEmployee(99);
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task PostEmployee_AddsEmployee()
        {
            var newEmployee = new Employee { Id = 3, FirstName = "Charlie" , LastName = "vvvv", Email = "charlie@gmail.com", Department = "IT", DateOfBirth =  new DateTime(1990, 1, 1) };
            var result = await _controller.PostEmployee(newEmployee);
            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
            Assert.That(await _context.Employees.CountAsync(), Is.EqualTo(3));
        }

        [Test]
        public async Task PutEmployee_UpdatesEmployee()
        {
            var existing = await _context.Employees.FindAsync(1);
            existing.FirstName = "Alice Updated";
            var result = await _controller.PutEmployee(1, existing);
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteEmployee_RemovesEmployee()
        {
            var result = await _controller.DeleteEmployee(2);
            Assert.That(result, Is.InstanceOf<NoContentResult>());
            Assert.That(await _context.Employees.FindAsync(2), Is.Null);
        }
        
    }
}
