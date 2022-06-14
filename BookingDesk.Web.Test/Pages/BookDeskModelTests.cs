using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingDesk.Domain;
using BookingDesk.Processor;
using BookingDesk.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;

namespace BookingDesk.Web.Test.Pages
{
    public class BookDeskModelTests
    {
        private readonly Mock<IDeskBookingRequestProcess> _processMock;
        private readonly BookDeskModel _bookDeskModel;
        private readonly BookingResult _deskBookingResult;

        public BookDeskModelTests()
        {
            _processMock = new Mock<IDeskBookingRequestProcess>();
            _bookDeskModel = new BookDeskModel(_processMock.Object)
            {
                BookingRequest = new BookingRequest()
            };

            _deskBookingResult = new BookingResult()
            {
                ResultCode = BookingResultCode.Success
            };

            _processMock.Setup(x => x.BookDesk(_bookDeskModel.BookingRequest)).Returns(_deskBookingResult);


        }


        [Theory]
        [InlineData(1, true)]
        [InlineData(0, false)]
        public void ShouldCallBookDeskMethodOfProcessIfModelIsValid(int expectedcalls, bool isModelValid)
        {
            if (!isModelValid)
            {
                _bookDeskModel.ModelState.AddModelError("AnyKey", "AnErrorMessage");
            }

            //act
            _bookDeskModel.OnPost();

            //assert
            _processMock.Verify(x => x.BookDesk(_bookDeskModel.BookingRequest), Times.Exactly(expectedcalls));

        }

        [Fact]
        public void ShouldAddModelErrorIfNoDeskIsAvailable()
        {
            //arrange
            _deskBookingResult.ResultCode=BookingResultCode.NoDeskAvailable;

            //act
            _bookDeskModel.OnPost();

            //assert
            var modelState =
            Assert.Contains("BookingRequest.Date", _bookDeskModel.ModelState);

            var modelError = Assert.Single(modelState.Errors);
            Assert.Equal("No desk available for selected date", modelError.ErrorMessage);

        }
        [Theory]
        [InlineData(typeof(PageResult), false, null)]
        [InlineData(typeof(PageResult), true, BookingResultCode.NoDeskAvailable)]
        [InlineData(typeof(RedirectToPageResult), true, BookingResultCode.Success)]
        public void ShouldReturnExpectedActionResult(Type expectedActionResultType, bool isModelValid, BookingResultCode? bookingResultCode)
        {
            //arrange
            if (!isModelValid)
            {
                _bookDeskModel.ModelState.AddModelError("AnyKey", "AnyError");
            }

            if (bookingResultCode.HasValue)
            {
                _deskBookingResult.ResultCode = bookingResultCode.Value;
            }

            //act
            IActionResult actionResult = _bookDeskModel.OnPost();

            //assert
            Assert.IsType(expectedActionResultType, actionResult);


        }

        [Fact]
        public void ShouldRedirectToBookDeskConfirmationPage()
        {
            // Arrange
            _deskBookingResult.ResultCode = BookingResultCode.Success;
            _deskBookingResult.BookingId = new Guid("A047343E-A17E-49C9-A8B9-9BB3A4C92BD2");
            _deskBookingResult.Firstname = "Thomas";
            _deskBookingResult.Date = new DateTime(2023, 1, 28);

            // Act
            IActionResult actionResult = _bookDeskModel.OnPost();

            // Assert
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(actionResult);
            Assert.Equal("BookDeskConfirmation", redirectToPageResult.PageName);

            IDictionary<string, object> routeValues = redirectToPageResult.RouteValues;
            Assert.Equal(3, routeValues.Count);

            var deskBookingId = Assert.Contains("BookingId", routeValues);
            Assert.Equal(_deskBookingResult.BookingId, deskBookingId);

            var firstName = Assert.Contains("FirstName", routeValues);
            Assert.Equal(_deskBookingResult.Firstname, firstName);

            var date = Assert.Contains("Date", routeValues);
            Assert.Equal(_deskBookingResult.Date, date);
        }

    }
}
