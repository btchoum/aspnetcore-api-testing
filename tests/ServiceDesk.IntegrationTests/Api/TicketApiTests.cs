using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ServiceDesk.Web;
using ServiceDesk.Web.ApiModels;
using Xunit;

namespace ServiceDesk.IntegrationTests
{
    public class TicketApiTests
            : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public TicketApiTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Post_Returns_Http_201_Created()
        {
            var client = _factory.CreateClient();

            var payload = GivenValidInput();
            var response = await client.PostAsJsonAsync("api/tickets", payload);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Post_And_Get_RoundTrip_Fields_Are_Saved()
        {
            var client = _factory.CreateClient();

            var payload = GivenValidInput();
            var postResponse = await client.PostAsJsonAsync("api/tickets", payload);
            postResponse.EnsureSuccessStatusCode();

            var response = await client.GetAsync(postResponse.Headers.Location?.ToString());
            response.EnsureSuccessStatusCode();

            var ticket = await response.Content.ReadFromJsonAsync<TicketDetailsDto>();
            
            Assert.NotNull(ticket);
            Assert.Equal(payload.Title, ticket.Title);
            Assert.Equal(payload.Details, ticket.Details);
            Assert.Equal(payload.SubmitterName, ticket.SubmitterName);
            Assert.Equal(payload.SubmitterEmail, ticket.SubmitterEmail);
            Assert.Equal("New", ticket.State);
            Assert.NotEqual(DateTime.MinValue, ticket.Created);
        }


        [Fact]
        public async Task Add_Comment()
        {
            var client = _factory.CreateClient();

            var payload = GivenValidInput();
            var postResponse = await client.PostAsJsonAsync("api/tickets", payload);
            postResponse.EnsureSuccessStatusCode();

            var location = $"{postResponse.Headers.Location}/comments";

            var commentPayload = new 
            {
                text = "New Comment"
            };
            var response = await client.PostAsJsonAsync(location, commentPayload);
            response.EnsureSuccessStatusCode();
        }

        private static TicketCreateDto GivenValidInput()
        {
            var payload = new TicketCreateDto
            {
                Title = "Test Title",
                Details = "Test Details",
                SubmitterEmail = "test@example.com",
                SubmitterName = "Test User"
            };
            return payload;
        }
    }
}
