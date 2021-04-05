using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceDesk.Web.ApiModels;
using ServiceDesk.Web.Controllers;
using ServiceDesk.Web.Data;
using ServiceDesk.Web.Infrastructure;
using Xunit;

namespace ServiceDesk.IntegrationTests.Controllers
{
    public class TicketsApiControllerTests
    {
        private readonly Mock<ILogger<TicketsApiController>> _loggerStub = new();
        private readonly Mock<IEmailSender> _emailSenderStub = new();
        private readonly ApplicationDbContext _dbContext;

        public TicketsApiControllerTests()
        {
            _dbContext = MakeApplicationDbContext();
        }

        [Fact]
        public async Task Post_Is_Successful()
        {
            var controller = new TicketsApiController(_dbContext, _emailSenderStub.Object, _loggerStub.Object);

            var input = new TicketCreateDto
            {
                Title = $"Test Title - {Guid.NewGuid()}",
                Details = $"Test Details - {Guid.NewGuid()}",
                SubmitterEmail = "test@example.com",
                SubmitterName = "Test User"
            };

            var result = await controller.CreateTicket(input, CancellationToken.None);

            result.Should().BeOfType<CreatedAtRouteResult>();
            var createdTicket = (result as CreatedAtRouteResult)?.Value as TicketDetailsDto;
            Assert.NotNull(createdTicket);
            createdTicket.Should().BeEquivalentTo(input, x => x.ExcludingMissingMembers());
            createdTicket.Id.Should().NotBe(0);
            createdTicket.Created.Should().BeCloseTo(DateTime.UtcNow, 1000);
        }

        [Fact]
        public async Task GetTicketById_NonExistingId_ReturnsNotFound()
        {
            var controller = new TicketsApiController(_dbContext, _emailSenderStub.Object, _loggerStub.Object);

            var badTicketId = long.MaxValue;
            var result = await controller.GetTicketById(badTicketId);

            result.Should().BeOfType<NotFoundResult>();
        }

        private static ApplicationDbContext MakeApplicationDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase($"{nameof(TicketsApiControllerTests)}-{Guid.NewGuid()}").Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            return dbContext;
        }
    }

    public class TicketsCommentsApiControllerTests
    {
        private readonly Mock<ILogger<TicketCommentsApiController>> _loggerStub = new();
        private readonly Mock<IEmailSender> _emailSenderStub = new();
        private readonly ApplicationDbContext _dbContext;

        public TicketsCommentsApiControllerTests()
        {
            _dbContext = MakeApplicationDbContext();
        }


        [Fact]
        public async Task AddComment_NonExistingId_ReturnsNotFound()
        {
            var controller = new TicketCommentsApiController(_dbContext, _emailSenderStub.Object, _loggerStub.Object);

            var badTicketId = long.MaxValue;
            var result = await controller.AddComment(badTicketId, new AddCommentDto());

            result.Should().BeOfType<NotFoundResult>();
        }

        private static ApplicationDbContext MakeApplicationDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase($"{nameof(TicketsCommentsApiControllerTests)}-{Guid.NewGuid()}").Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            return dbContext;
        }
    }
}
