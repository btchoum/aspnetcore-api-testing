using System;
using FluentAssertions;
using ServiceDesk.Web.ApiModels;
using ServiceDesk.Web.Data;
using Xunit;

namespace ServiceDesk.UnitTests.ApiModels
{
    public class TicketCreateDtoTests
    {
        [Fact]
        public void MapToTicket_Populates_All_Fields()
        {
            var dto = new TicketCreateDto
            {
                Title = $"Test Title {Guid.NewGuid()}",
                SubmitterName = $"Test SubmitterName {Guid.NewGuid()}",
                Details = $"Test Details {Guid.NewGuid()}",
                SubmitterEmail = $"test-email{Guid.NewGuid()}@example.com",
                Now = DateTime.Parse("2021-01-01")
            };

            var ticket = dto.MapToTicket();
            
            ticket.Should().BeEquivalentTo(dto, options => options.ExcludingMissingMembers());
        }
    }
}
