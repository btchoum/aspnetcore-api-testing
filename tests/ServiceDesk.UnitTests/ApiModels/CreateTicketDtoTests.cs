using System;
using System.Collections.Generic;
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

    public class TicketDetailsDtoTests
    {
        [Fact]
        public void MapFromTicket_Populates_All_Fields()
        {
            var ticket = new Ticket
            {
                Id = 1,
                Title = $"Test Title-{Guid.NewGuid()}",
                Details = $"Test Details-{Guid.NewGuid()}",
                SubmitterName = $"Test SubmitterName-{Guid.NewGuid()}",
                SubmitterEmail = $"Test-{Guid.NewGuid()}@example.com",
                State = TicketState.Resolved,
                Created = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                Comments = new List<Comment>()
            };

            var dto = new TicketDetailsDto(ticket);
            
            dto.Should().BeEquivalentTo(ticket, 
                options => options.ExcludingMissingMembers().Excluding(t => t.State));
            dto.State.Should().Be("Resolved");
        }
    }
}
