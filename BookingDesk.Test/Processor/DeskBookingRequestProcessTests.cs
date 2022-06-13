using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingDesk.Datainterface;
using BookingDesk.Domain;
using BookingDesk.Processor;
using Moq;

namespace BookingDesk.Test.Processor
{
    public class DeskBookingRequestProcessTests
    {
        private readonly BookingRequest _request;
        private readonly Mock<IDeskBookRepo> _deskbookingRepoMock;
        private readonly Mock<IDeskRepo> _deskRespMock;
        private readonly DeskBookingRequestProcess _process;
        private readonly List<Desk> _availableDesk;
        public DeskBookingRequestProcessTests()
        {
            _request = new BookingRequest()
            {
                Firstname = "Tony",
                Lastname = "Tang",
                Email = "tt@tt.com",
                Date = new DateTime(2022, 06, 10),
            };

            _availableDesk = new List<Desk>() { new Desk() { Id = new Guid("5995CBF3-0F43-4034-8EF5-246E76DBE8CF") } };

            _deskbookingRepoMock = new Mock<IDeskBookRepo>();
            _deskRespMock = new Mock<IDeskRepo>();
            _deskRespMock.Setup(x => x.GetDesksByDate(_request.Date))
                .Returns(_availableDesk);

            this._process = new DeskBookingRequestProcess(_deskbookingRepoMock.Object, _deskRespMock.Object);
        }

        [Fact]
        public void ShouldReturnBookingResultWithBookingRequest()
        {
            

            var result = _process.BookDesk(_request);
            Assert.NotNull(result);
            Assert.Equal(result.Firstname, _request.Firstname);
            Assert.Equal(result.Lastname, _request.Lastname);
            Assert.Equal(result.Email, _request.Email);
            Assert.Equal(result.Date, _request.Date);

        }

        [Fact]
        public void ShouldThrowExceptionIfRequestIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _process.BookDesk(null));

            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public void ShouldSaveDeskBooking()
        {
            DeskBook SavedDeskBooking = null;
            _deskbookingRepoMock.Setup(x => x.Save(It.IsAny<DeskBook>()))
                .Callback<DeskBook>(deskBooking =>
                {
                    SavedDeskBooking = deskBooking;
                });

            _process.BookDesk(_request);

            _deskbookingRepoMock.Verify(x=>x.Save(It.IsAny<DeskBook>()), Times.Once);
            Assert.NotNull(SavedDeskBooking);
            Assert.Equal(_request.Firstname, SavedDeskBooking.Firstname);
            Assert.Equal(_request.Lastname, SavedDeskBooking.Lastname);
            Assert.Equal(_request.Email, SavedDeskBooking.Email);
            Assert.Equal(_request.Date, SavedDeskBooking.Date);
            Assert.Equal(_availableDesk.First().Id, SavedDeskBooking.DeskId);

        }
        [Fact]
        public void ShouldNotSaveDeskBookingIfNoDeskAvailable()
        {
            _availableDesk.Clear();

            _process.BookDesk(_request);

            _deskbookingRepoMock.Verify(x=>x.Save(It.IsAny<DeskBook>()), Times.Never);


        }

        [Theory]
        [InlineData(BookingResultCode.NoDeskAvailable, false)]
        [InlineData(BookingResultCode.Success, true)]
        public void ShouldReturnCorretResultCode(BookingResultCode resultCode, bool isDeskAvailable)
        {
            if (!isDeskAvailable)
            {
                _availableDesk.Clear();
            }

            var result= _process.BookDesk(_request);

            Assert.Equal(result.ResultCode, resultCode);

        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShouldReturnExpectedBookingId(bool isDeskAvailable)
        {
                DeskBook savedDeskBooking = null;
            if (!isDeskAvailable) { _availableDesk.Clear(); }
            else
            {

                _deskbookingRepoMock.Setup(x => x.Save(It.IsAny<DeskBook>()))
                    .Callback<DeskBook>(dbook =>
                    {
                        savedDeskBooking = dbook;
                    });
            }

            var result = _process.BookDesk(_request);

            Guid expectedBookingId = (savedDeskBooking == null) ? Guid.Empty : savedDeskBooking.Id;
            Assert.Equal(result.BookingId, expectedBookingId);


        }

    }
}
