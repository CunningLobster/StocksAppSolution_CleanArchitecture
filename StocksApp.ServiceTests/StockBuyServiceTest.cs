using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;
using System;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit.Abstractions;
using Xunit.Sdk;
using Entities;
using AutoFixture;
using FluentAssertions;
using Moq;
using RepositoryContracts;

namespace StocksAppTests.ServiceTests
{
    public class StockBuyServiceTest
    {
        private readonly StockBuyService _stockBuyService;

        private readonly IBuyOrderRepository _buyOrderRepository;

        private readonly Mock<IBuyOrderRepository> _buyOrderRepositoryMock;
        private readonly Mock<ISellOrderRepository> _sellOrderRepositoryMock;

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;

        public StockBuyServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();

            _buyOrderRepositoryMock = new Mock<IBuyOrderRepository>();
            _sellOrderRepositoryMock = new Mock<ISellOrderRepository>();

            _buyOrderRepository = _buyOrderRepositoryMock.Object;

            _stockBuyService = new StockBuyService(_buyOrderRepository);

            _testOutputHelper = testOutputHelper;
        }

        #region CreateBuyOrder

        //When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async Task CreateBuyOrder_NullBuyOrderRequest_ToBeArgumentNullException()
        {
            //Arrange
            BuyOrderRequest? buy_order_request = null;

            //Act
            Func<Task> action = async () => await _stockBuyService.CreateBuyOrder(buy_order_request);

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw       ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_QuantityLessThanMinimum_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest buy_order_request = _fixture.Build<BuyOrderRequest>()
                .With(b => b.Quantity, (uint)0)
                .With(b => b.Price, 500)
                .With(b => b.DateAndTimeOfOrder, DateTime.Parse("2005-01-01"))
                .Create();
            BuyOrder buy_order = buy_order_request.ToBuyOrder();

            _buyOrderRepositoryMock.Setup(s => s.AddBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buy_order);

            //Act
            Func<Task> action = async () => await _stockBuyService.CreateBuyOrder(buy_order_request);

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should   throw  ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_QuantityMoreThanMaximum_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest buy_order_request = _fixture.Build<BuyOrderRequest>()
                .With(b => b.Quantity, (uint)100001)
                .With(b => b.Price, 500)
                .With(b => b.DateAndTimeOfOrder, DateTime.Parse("2005-01-01"))
                .Create();
            BuyOrder buy_order = buy_order_request.ToBuyOrder();

            _buyOrderRepositoryMock.Setup(s => s.AddBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buy_order);

            //Act
            Func<Task> action = async () => await _stockBuyService.CreateBuyOrder(buy_order_request);

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw      ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_PriceLessThanMinimum_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest buy_order_request = _fixture.Build<BuyOrderRequest>()
                .With(b => b.Quantity, (uint)500)
                .With(b => b.Price, 0)
                .With(b => b.DateAndTimeOfOrder, DateTime.Parse("2005-01-01"))
                .Create();
            BuyOrder buy_order = buy_order_request.ToBuyOrder();

            _buyOrderRepositoryMock.Setup(s => s.AddBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buy_order);

            //Act
            Func<Task> action = async () => await _stockBuyService.CreateBuyOrder(buy_order_request);

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw   ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_PriceMoreThanMaximum_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest buy_order_request = _fixture.Build<BuyOrderRequest>()
                .With(b => b.Quantity, (uint)500)
                .With(b => b.Price, 10001)
                .With(b => b.DateAndTimeOfOrder, DateTime.Parse("2005-01-01"))
                .Create();
            BuyOrder buy_order = buy_order_request.ToBuyOrder();

            _buyOrderRepositoryMock.Setup(s => s.AddBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buy_order);

            //Act
            Func<Task> action = async () => await _stockBuyService.CreateBuyOrder(buy_order_request);

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should   throw  ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest buy_order_request = _fixture.Build<BuyOrderRequest>()
                .With(b => b.StockSymbol, null as string)
                .With(b => b.Quantity, (uint)500)
                .With(b => b.Price, 500)
                .With(b => b.DateAndTimeOfOrder, DateTime.Parse("2005-01-01"))
                .Create();
            BuyOrder buy_order = buy_order_request.ToBuyOrder();

            _buyOrderRepositoryMock.Setup(s => s.AddBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buy_order);

            //Act
            Func<Task> action = async () => await _stockBuyService.CreateBuyOrder(buy_order_request);

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it   should be  equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_InvalidDateAndTimeOfOrder_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest buy_order_request = _fixture.Build<BuyOrderRequest>()
                .With(b => b.Quantity, (uint)500)
                .With(b => b.Price, 500)
                .With(b => b.DateAndTimeOfOrder, DateTime.Parse("1999-12-31"))
                .Create();
            BuyOrder buy_order = buy_order_request.ToBuyOrder();

            _buyOrderRepositoryMock.Setup(s => s.AddBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buy_order);

            //Act
            Func<Task> action = async () => await _stockBuyService.CreateBuyOrder(buy_order_request);

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID(guid).
        [Fact]
        public async Task CreateBuyOrder_ValidData_ToBeSuccessful()
        {
            //Arrange
            BuyOrderRequest buy_order_request = _fixture.Build<BuyOrderRequest>()
                .With(b => b.Quantity, (uint)500)
                .With(b => b.Price, 500)
                .With(b => b.DateAndTimeOfOrder, DateTime.Parse("2005-01-01"))
                .Create();
            BuyOrder buy_order = buy_order_request.ToBuyOrder();
            BuyOrderResponse buy_order_response_expected = buy_order.ToBuyOrderResponse();

            _buyOrderRepositoryMock.Setup(s => s.AddBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buy_order);

            //Act
            BuyOrderResponse buy_order_response_from_create = await _stockBuyService.CreateBuyOrder(buy_order_request);
            buy_order_response_expected.BuyOrderID = buy_order_response_from_create.BuyOrderID;

            //Assert
            buy_order_response_from_create.BuyOrderID.Should().NotBe(Guid.Empty);
            buy_order_response_from_create.Should().Be(buy_order_response_expected);
        }

        #endregion

        #region GetBuyOrders

        //When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task GetBuyOrders_EmptyList_ToReturnEmptyList()
        {
            //Arrange
            List<BuyOrder> buy_orders = new List<BuyOrder>();

            _buyOrderRepositoryMock.Setup(b => b.GetBuyOrders()).ReturnsAsync(buy_orders);

            //Act
            List<BuyOrderResponse> buy_orders_from_get = await _stockBuyService.GetBuyOrders();

            //Assert
            buy_orders_from_get.Should().BeEmpty();
        }

        //When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public async Task GetBuyOrders_FewElements_ToBeSuccessful()
        {
            //Arrange
            BuyOrder buy_order1 = _fixture.Build<BuyOrder>()
                .With(b => b.Quantity, (uint)500)
                .With(b => b.Price, 500)
                .With(b => b.DateAndTimeOfOrder, DateTime.Parse("2005-01-01"))
                .Create();
            BuyOrder buy_order2 = _fixture.Build<BuyOrder>()
                .With(b => b.Quantity, (uint)700)
                .With(b => b.Price, 200)
                .With(b => b.DateAndTimeOfOrder, DateTime.Parse("2007-03-07"))
                .Create();
            BuyOrder buy_ordert3 = _fixture.Build<BuyOrder>()
                .With(b => b.Quantity, (uint)5)
                .With(b => b.Price, 2000)
                .With(b => b.DateAndTimeOfOrder, DateTime.Parse("2010-02-17"))
                .Create();

            List<BuyOrder> buy_orders = new List<BuyOrder> { buy_order1, buy_order2, buy_ordert3 };
            List<BuyOrderResponse> buy_order_responses_expected = buy_orders.Select(b => b.ToBuyOrderResponse()).ToList();

            _testOutputHelper.WriteLine("Buy order responses from add (expected)");
            foreach (var buy_order_response in buy_order_responses_expected)
            {
                _testOutputHelper.WriteLine(buy_order_response.ToString());
            }

            _buyOrderRepositoryMock.Setup(b => b.GetBuyOrders()).ReturnsAsync(buy_orders);

            //Act
            List<BuyOrderResponse> buy_order_responses_from_get = await _stockBuyService.GetBuyOrders();

            _testOutputHelper.WriteLine("Buy order responses from get (actual)");
            foreach (var buy_order_response_from_get in buy_order_responses_from_get)
            {
                _testOutputHelper.WriteLine(buy_order_response_from_get.ToString());
            }

            //Assert
            buy_order_responses_expected.Should().BeEquivalentTo(buy_order_responses_from_get);
        }

        #endregion

    }
}