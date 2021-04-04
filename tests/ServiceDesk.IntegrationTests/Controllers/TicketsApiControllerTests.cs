using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceDesk.Web.Controllers;
using ServiceDesk.Web.Data;
using ServiceDesk.Web.Infrastructure;
using ServiceDesk.Web.Models;
using Xunit;

namespace ServiceDesk.IntegrationTests.Controllers
{
    public class TicketsApiControllerTests
    {
        [Fact]
        public async Task Post_Is_Successful()
        {
            var loggerStub = new Mock<ILogger<TicketsApiController>>();
            
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(nameof(Post_Is_Successful)).Options;
            var dbContext = new ApplicationDbContext(options);
            var controller = new TicketsApiController(dbContext, new InMemoryEmailSender(), loggerStub.Object);

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
            createdTicket.Should().BeEquivalentTo(input, x => x.ExcludingMissingMembers());
        }
    }
}
