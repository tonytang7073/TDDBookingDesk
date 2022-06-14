using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingDesk.Domain;
using BookingDesk.Processor;
using BookingDesk.Web.Pages;
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



    }
}
