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
    public class StockSellServiceTest
    {
        private readonly StockSellService _stockSellService;

        private readonly ISellOrderRepository _sellOrderRepository;

        private readonly Mock<IBuyOrderRepository> _buyOrderRepositoryMock;
        private readonly Mock<ISellOrderRepository> _sellOrderRepositoryMock;

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;

        public StockSellServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();

            _buyOrderRepositoryMock = new Mock<IBuyOrderRepository>();
            _sellOrderRepositoryMock = new Mock<ISellOrderRepository>();

            _sellOrderRepository = _sellOrderRepositoryMock.Object;

            _stockSellService = new StockSellService(_sellOrderRepository);

            _testOutputHelper = testOutputHelper;
        }

        #region CreateSellOrder

        //When you supply SellOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async Task CreateSellOrder_NullSellOrderRequest_ToBeArgumentNullException()
        {
            //Arrange
            SellOrderRequest? sell_order_request = null;

            //Act
            Func<Task> action = async () => await _stockSellService.CreateSellOrder(sell_order_request);

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();

        }

        //When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw      ArgumentException.
        [Fact]
        public async Task CreateSellOrder_QuantityLessThanMinimal_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest sell_order_request = _fixture.Build<SellOrderRequest>()
                .With(s => s.Quantity, (uint)0)
                .With(s => s.Price, 500)
                .With(s => s.DateAndTimeOfOrder, DateTime.Parse("2005-01-01"))
                .Create();
            SellOrder sell_order = sell_order_request.ToSellOrder();

            _sellOrderRepositoryMock.Setup(s => s.AddSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sell_order);

            //Act
            Func<Task> action = async () => await _stockSellService.CreateSellOrder(sell_order_request);

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should      throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_QuantityMoreThanMaximum_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest sell_order_request = _fixture.Build<SellOrderRequest>()
                .With(s => s.Quantity, (uint)100001)
                .With(s => s.Price, 500)
                .With(s => s.DateAndTimeOfOrder, DateTime.Parse("2005-01-01"))
                .Create();
            SellOrder sell_order = sell_order_request.ToSellOrder();

            _sellOrderRepositoryMock.Setup(s => s.AddSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sell_order);

            //Act
            Func<Task> action = async () => await _stockSellService.CreateSellOrder(sell_order_request);

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw        ArgumentException.
        [Fact]
        public async Task CreateSellOrder_PriceLessThanMinimal_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest sell_order_request = _fixture.Build<SellOrderRequest>()
                .With(s => s.Quantity, (uint)500)
                .With(s => s.Price, 0)
                .With(s => s.DateAndTimeOfOrder, DateTime.Parse("2005-01-01"))
                .Create();
            SellOrder sell_order = sell_order_request.ToSellOrder();

            _sellOrderRepositoryMock.Setup(s => s.AddSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sell_order);

            //Act
            Func<Task> action = async () => await _stockSellService.CreateSellOrder(sell_order_request);

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw      ArgumentException.
        [Fact]
        public async Task CreateSellOrder_PriceMoreThanMaximum_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest sell_order_request = _fixture.Build<SellOrderRequest>()
                .With(s => s.Quantity, (uint)500)
                .With(s => s.Price, 10001)
                .With(s => s.DateAndTimeOfOrder, DateTime.Parse("2005-01-01"))
                .Create();
            SellOrder sell_order = sell_order_request.ToSellOrder();

            _sellOrderRepositoryMock.Setup(s => s.AddSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sell_order);

            //Act
            Func<Task> action = async () => await _stockSellService.CreateSellOrder(sell_order_request);

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should   throw  ArgumentException.
        [Fact]
        public async Task CreateSellOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest sell_order_request = _fixture.Build<SellOrderRequest>()
                .With(s => s.StockSymbol, null as string)
                .With(s => s.Quantity, (uint)500)
                .With(s => s.Price, 500)
                .With(s => s.DateAndTimeOfOrder, DateTime.Parse("2005-01-01"))
                .Create();
            SellOrder sell_order = sell_order_request.ToSellOrder();

            _sellOrderRepositoryMock.Setup(s => s.AddSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sell_order);

            //Act
            Func<Task> action = async () => await _stockSellService.CreateSellOrder(sell_order_request);

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it   should be  equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_InvalidDateAndTimeOfOrder_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest sell_order_request = _fixture.Build<SellOrderRequest>()
                .With(s => s.Quantity, (uint)500)
                .With(s => s.Price, 500)
                .With(s => s.DateAndTimeOfOrder, DateTime.Parse("1999-12-31"))
                .Create();
            SellOrder sell_order = sell_order_request.ToSellOrder();

            _sellOrderRepositoryMock.Setup(s => s.AddSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sell_order);

            //Act
            Func<Task> action = async () => await _stockSellService.CreateSellOrder(sell_order_request);

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID(guid).
        [Fact]
        public async Task CreateSellOrder_ValidData_ToBeSuccessful()
        {
            //Arrange
            SellOrderRequest sell_order_request = _fixture.Build<SellOrderRequest>()
                .With(s => s.Quantity, (uint)500)
                .With(s => s.Price, 500)
                .With(s => s.DateAndTimeOfOrder, DateTime.Parse("2005-01-01"))
                .Create();
            SellOrder sell_order = sell_order_request.ToSellOrder();
            SellOrderResponse sell_order_response_expected = sell_order.ToSellOrderResponse();

            _sellOrderRepositoryMock.Setup(s => s.AddSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sell_order);


            //Act
            SellOrderResponse sell_order_from_create = await _stockSellService.CreateSellOrder(sell_order_request);
            sell_order_response_expected.SellOrderID = sell_order_from_create.SellOrderID;

            //Assert
            sell_order_from_create.SellOrderID.Should().NotBe(Guid.Empty);
            sell_order_from_create.Should().Be(sell_order_response_expected);
        }

        #endregion

        #region GetSellOrders

        //When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task GetSellOrders_EmptyList_ToReturnEmptyList()
        {
            //Arrange
            List<SellOrder> sell_orders = new List<SellOrder>();

            _sellOrderRepositoryMock.Setup(b => b.GetSellOrders()).ReturnsAsync(sell_orders);

            //Act
            List<SellOrderResponse> sell_orders_from_get = await _stockSellService.GetSellOrders();

            //Assert
            sell_orders_from_get.Should().BeEmpty();
        }

        //When you first add few sell orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method; the returned list should contain all the same sell orders.
        [Fact]
        public async Task GetSellOrders_FewElements_ToBeSuccessful()
        {
            //Arrange
            SellOrder sell_order1 = _fixture.Build<SellOrder>()
                .With(s => s.Quantity, (uint)500)
                .With(s => s.Price, 500)
                .With(s => s.DateAndTimeOfOrder, DateTime.Parse("2005-01-01"))
                .Create();
            SellOrder sell_order2 = _fixture.Build<SellOrder>()
                .With(s => s.Quantity, (uint)700)
                .With(s => s.Price, 200)
                .With(s => s.DateAndTimeOfOrder, DateTime.Parse("2007-03-07"))
                .Create();
            SellOrder sell_order3 = _fixture.Build<SellOrder>()
                .With(s => s.Quantity, (uint)5)
                .With(s => s.Price, 2000)
                .With(s => s.DateAndTimeOfOrder, DateTime.Parse("2010-02-17"))
                .Create();

            List<SellOrder> sell_orders = new List<SellOrder> { sell_order1, sell_order2, sell_order3 };
            List<SellOrderResponse> sell_order_responses_expected = sell_orders.Select(b => b.ToSellOrderResponse()).ToList();

            _sellOrderRepositoryMock.Setup(s => s.GetSellOrders()).ReturnsAsync(sell_orders);

            _testOutputHelper.WriteLine("Sell order responses from add (expected)");
            foreach (var sell_order_response in sell_order_responses_expected)
            {
                _testOutputHelper.WriteLine(sell_order_response.ToString());
            }

            //Act
            List<SellOrderResponse> sell_order_responses_from_get = await _stockSellService.GetSellOrders();

            _testOutputHelper.WriteLine("Sell order responses from get (actual)");
            foreach (var sell_order_response_from_get in sell_order_responses_from_get)
            {
                _testOutputHelper.WriteLine(sell_order_response_from_get.ToString());
            }

            //Assert
            sell_order_responses_expected.Should().BeEquivalentTo(sell_order_responses_from_get);
        }

        #endregion
    }
}