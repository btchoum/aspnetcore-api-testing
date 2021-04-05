using System;
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